using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameArchitectureEngine
{
    public class EnemyGameObject : GameObjectBase
    {
        public delegate void DamagePlayerHandler(object sender, CollisionEventArgs e);
        public event DamagePlayerHandler DamagePlayer;
        public bool previouslyColliding = false;
        public bool currentlyColliding = false;

        public FSM fsm;
        public IdleState idle;
        public FleeState flee;
        public ChaseState chase;
        public AttackState attack;

        GameTime gameTime;

        Animation idleAnimation;
        Animation walkingAnimation;
        Animation attackAnimation;
        Animation dieAnimation;

        AnimationPlayer sprite;

        private int health;
        public int Health
        {
            get { return health; }
        }

        private int maxHealth;
        public int MaxHealth
        {
            get { return maxHealth; }
        }

        private bool isWeak = false;
        public bool IsWeak
        {
            get { return isWeak; }
        }

        private Vector2 velocity;
        public Vector2 Velocity
        {
            get { return velocity; }
            set { velocity = value; }
        }
        private SpriteEffects flip = SpriteEffects.None;

        bool isAlive = true;

        private float speed = 64.0f;
        public float Speed
        {
            get { return speed; }
            set { speed = value; }
        }

        private Vector2 direction;
        public Vector2 Direction
        {
            get { return direction; }
            set { direction = value; }
        }

        private bool playerSeen = false;
        public bool PlayerSeen
        {
            get { return playerSeen; }
        }

        private int sightRange = 200;
        public int SightRange
        {
            get { return sightRange; }
        }

        private bool canAttack;
        public bool CanAttack
        {
            get { return canAttack; }
        }

        private int attackRange = 32;
        public int AttackRange
        {
            get { return attackRange; }            
        }

        public Collidable Collidable;

        private Rectangle attackRangeRectangle;

        private Rectangle localBounds;

        public Rectangle BoundingRectangle
        {
            get
            {
                int left = (int)Math.Round(Position.X - sprite.Origin.X) + localBounds.X;
                int top = (int)Math.Round(Position.Y - sprite.Origin.Y) + localBounds.Y;

                return new Rectangle(left, top, localBounds.Width, localBounds.Height);
            }
        }
        
        private GameObjectBase player;
        public GameObjectBase Player
        {
            get { return player; }
        }

        public EnemyGameObject(Vector2 position, GameObjectBase player, int health, int maxHealth)
        {
            this.health = health;
            this.maxHealth = maxHealth;

            Position = position;
            this.player = player;
        }

        public void LoadContent(ResourceManager resources)
        {
            fsm = new FSM(this);

            idle = new IdleState();
            flee = new FleeState();
            chase = new ChaseState();
            attack = new AttackState();
            
            idle.AddTransitions(new Transition(flee, () => (PlayerSeen && isWeak)));
            flee.AddTransitions(new Transition(idle, () => !PlayerSeen));

            idle.AddTransitions(new Transition(chase, () => (PlayerSeen && !isWeak)));
            chase.AddTransitions(new Transition(idle, () => (!PlayerSeen)));

            flee.AddTransitions(new Transition(chase, () => (PlayerSeen && !isWeak)));
            chase.AddTransitions(new Transition(flee, () => (PlayerSeen && isWeak)));

            idle.AddTransitions(new Transition(attack, () => (CanAttack && !isWeak)));
            attack.AddTransitions(new Transition(idle, () => (!CanAttack && !PlayerSeen)));

            chase.AddTransitions(new Transition(attack, () => (CanAttack && !isWeak)));
            attack.AddTransitions(new Transition(chase, () => (!CanAttack && PlayerSeen)));

            fsm.AddState(idle);            
            fsm.AddState(flee);

            fsm.Initialise("Idle");

            idleAnimation = new Animation(resources.SpriteSheets["Sprites/Enemies/EnemyIdle"], 0.5f, true);
            attackAnimation = new Animation(resources.SpriteSheets["Sprites/Enemies/Attack"], 0.25f, true);

            sprite.PlayAnimation(idleAnimation);

            int width = (int)(idleAnimation.FrameWidth * 0.4f);
            int left = (idleAnimation.FrameWidth - width) / 2;
            int height = (int)(idleAnimation.FrameHeight * 0.8);
            int top = idleAnimation.FrameHeight - height;
            localBounds = new Rectangle(left, top, width, height);
                        
            BoundingBox = new Rectangle(BoundingRectangle.X, BoundingRectangle.Y, BoundingRectangle.Width, BoundingRectangle.Height);

            attackRangeRectangle = new Rectangle((int)Position.X - (attackAnimation.FrameWidth / 2), (int)Position.Y - attackAnimation.FrameHeight, attackAnimation.FrameWidth, attackAnimation.FrameHeight);
        }

        public override void Update(GameTime gameTime)
        {
            this.gameTime = gameTime;

            isWeak = (Health < MaxHealth / 2);
            playerSeen = (Vector2.Distance(Position, player.Position) < SightRange);

            fsm.Update(gameTime);

            attackRangeRectangle = new Rectangle((int)Position.X - (attackAnimation.FrameWidth / 2), (int)Position.Y - attackAnimation.FrameHeight, attackAnimation.FrameWidth, attackAnimation.FrameHeight);

            canAttack = (attackRangeRectangle.Intersects(player.BoundingBox));

            Position += Velocity;          
        }

        public void Draw(GameTime gameTime, SpriteBatch sprBatch)
        {
            if (Velocity.X > 0)
                flip = SpriteEffects.FlipHorizontally;
            if (Velocity.X < 0)
                flip = SpriteEffects.None;

            sprite.Draw(gameTime, sprBatch, Position, flip);
        }

        public void MoveToward(GameObjectBase obj, GameTime gameTime)
        {
            if (sprite.Animation != idleAnimation) sprite.PlayAnimation(idleAnimation);

            float elapsedTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            Vector2 directionToChase = obj.Position - Position;

            directionToChase.Normalize();
            Velocity = directionToChase * speed * elapsedTime;
            direction = directionToChase;
        }


        public void SetRandomDirection()
        {
            if (sprite.Animation != idleAnimation) sprite.PlayAnimation(idleAnimation);

            float randomXDirection = Utilities.RandomFloatInRange();
            float randomYDirection = Utilities.RandomFloatInRange();

            direction.X += randomXDirection * Speed;
            direction.Y += randomYDirection * Speed;
            direction.Normalize();

            Velocity = direction * speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        public void Attack()
        {
            Console.WriteLine("Attack!");
            sprite.PlayAnimation(attackAnimation);
            Velocity = Vector2.Zero;

            //if (!previouslyColliding && currentlyColliding)
                OnDamagePlayer();
        }

        public void OnDamagePlayer()
        {
            DamagePlayer?.Invoke(this, new CollisionEventArgs(Position));
        }
    }
}
