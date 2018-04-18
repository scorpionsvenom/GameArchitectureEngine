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
        public event CollisionHandler DamagePlayer;

        public enum EnemyState
        {
            Idle = 0,
            Flee,
            Chase,
            Attack,
            Die,
        }
        public EnemyState state = EnemyState.Idle;


        public const float ChaseSpeed = 88.0f;
        public const float WanderSpeed = 48.0f;
        public const float FleeSpeed = 96.0f;

        private float speed = 64.0f;
        public float Speed
        {
            get { return speed; }
            set { speed = value; }
        }

        public FSM fsm;
        public IdleState idle;
        public FleeState flee;
        public ChaseState chase;
        public AttackState attack;
        public DieState die;

        GameTime gameTime;

        Animation idleAnimation;

        internal void Die()
        {
            speed = 0.0f;
            isAlive = false;
            canAttack = false;
        }

        Animation attackAnimation;
        Animation dieAnimation;

        AnimationPlayer sprite;

        private int health;
        public int Health
        {
            get { return health; }
            set { health = value; }
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
        public bool IsAlive
        {
            get { return isAlive; }
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

        public override void LoadContent(ResourceManager resources)
        {
            fsm = new FSM(this);

            idle = new IdleState();
            flee = new FleeState();
            chase = new ChaseState();
            attack = new AttackState();
            die = new DieState();
            
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

            idle.AddTransitions(new Transition(die, () => !isAlive));
            flee.AddTransitions(new Transition(die, () => !isAlive));
            chase.AddTransitions(new Transition(die, () => !isAlive));
            attack.AddTransitions(new Transition(die, () => !isAlive));

            fsm.AddState(idle);            
            fsm.AddState(flee);
            fsm.AddState(chase);
            fsm.AddState(die);

            fsm.Initialise("Idle");

            idleAnimation = new Animation(resources.SpriteSheets["Sprites/Enemies/EnemyIdle"], 0.5f, true);
            attackAnimation = new Animation(resources.SpriteSheets["Sprites/Enemies/Attack"], 0.25f, true);
            dieAnimation = new Animation(resources.SpriteSheets["Sprites/Enemies/EnemyDie"], 0.5f, false);

            int width = (int)(idleAnimation.FrameWidth * 0.4f);
            int left = (idleAnimation.FrameWidth - width) / 2;
            int height = (int)(idleAnimation.FrameHeight * 0.8);
            int top = idleAnimation.FrameHeight - height;
            localBounds = new Rectangle(left, top, width, height);

            attackRangeRectangle = new Rectangle((int)Position.X - (attackAnimation.FrameWidth / 2), (int)Position.Y - attackAnimation.FrameHeight, attackAnimation.FrameWidth, attackAnimation.FrameHeight);
        }

        public override void Update(GameTime gameTime)
        {
            isAlive = (health > 0);

            if (isAlive)
            {
                sprite.PlayAnimation(idleAnimation);

                this.gameTime = gameTime;

                isWeak = (Health < MaxHealth / 2);
                playerSeen = (Vector2.Distance(Position, player.Position) < SightRange);

                attackRangeRectangle = new Rectangle((int)Position.X - (attackAnimation.FrameWidth / 2), (int)Position.Y - attackAnimation.FrameHeight, attackAnimation.FrameWidth, attackAnimation.FrameHeight);

                canAttack = (attackRangeRectangle.Intersects(player.BoundingBox));

                Position += Velocity;

                BoundingBox = new Rectangle((int)Position.X - idleAnimation.FrameWidth / 2, (int)Position.Y - idleAnimation.FrameWidth / 2, idleAnimation.FrameWidth, idleAnimation.FrameWidth);
            }
            else
            {
                sprite.PlayAnimation(dieAnimation);
                speed = 0;
                Velocity = Vector2.Zero;
            }

            fsm.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch sprBatch)
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
            sprite.PlayAnimation(attackAnimation);
            Velocity = Vector2.Zero;
            OnDamagePlayer();
        }

        public void OnDamagePlayer()
        {
            DamagePlayer?.Invoke(this, new CollisionEventArgs(Position));
        }

        public override void Reset(Vector2 position)
        {
            throw new NotImplementedException();
        }

        public override bool CollisionTest(Collidable col)
        {
            if (col != null)
            {
                //OnCollision(col);
                return BoundingBox.Intersects(col.BoundingBox);                
            }

            return false;
        }

        public override void OnCollision(Collidable col)
        {
            EnemyGameObject enemy = col as EnemyGameObject;

            if (enemy != null && isAlive)
            {
                Point enemyCentre = enemy.BoundingBox.Center;
                Point centre = BoundingBox.Center;

                Vector2 enemyCentreVector = new Vector2(enemyCentre.X, enemyCentre.Y);
                Vector2 centreVector = new Vector2(centre.X, centre.Y);

                Vector2 collisionNormal = Vector2.Normalize(enemyCentreVector - centreVector);

                float distance = Vector2.Distance(enemyCentreVector, centreVector);

                float penetrationDepth = ((enemy.BoundingBox.Width / 2 + BoundingBox.Width / 2) - distance) * 0.08f;

                Position += (-collisionNormal * penetrationDepth);
            }
        }
    }
}
