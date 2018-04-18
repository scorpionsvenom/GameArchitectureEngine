﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace GameArchitectureEngine
{
    public class PlayerGameObject : GameObjectBase
    {
        public event CollisionWithSpecificObjectHandler DamageEnemy;
        public event CollisionWithSpecificObjectHandler CollideWithPotion;
        public event CollisionWithSpecificObjectHandler CollideWithEnemy;

        FSM fsm;

        WalkState walkState;
        PlayerAttackState attackState;
        PlayerDieState dieState;
        
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

        public Collidable Collidable;

        private ResourceManager Resources;

        private Vector2 velocity;
        private Animation walkAnimation;
        private Animation attackAnimation;
        private Animation death;
        private AnimationPlayer sprite;

        private SpriteEffects flip = SpriteEffects.None;

        private bool isAlive = true;
        public bool IsAlive
        {
            get { return isAlive; }
            set { isAlive = value; }
        }

        private Vector2 lastMouseLocation = Vector2.Zero;
        private float stoppingDistance = 3.0f;

        public Vector2 Velocity
        {
            get { return velocity; }
            set { velocity = value; }
        }

        GameTime gameTime;

        private const float maxSpeed = 160.0f;
        public float MaxSpeed
        {
            get { return maxSpeed; }
        }

        private float speed = maxSpeed;        
        public float Speed
        {
            get { return speed; }
            set { speed = value; }
        }

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

        private bool canAttack = false;
        public bool CanAttack
        {
            get { return canAttack; }
            set { canAttack = value; }
        }

        private bool isCloseEnoughToAttack = false;

        public PlayerGameObject()
        {
            //TODO: set position from serialised object
            Position = new Vector2(0.0f, 0.0f);
        }

        public override void Initialise()
        {
            fsm = new FSM(this);

            walkState = new WalkState();
            attackState = new PlayerAttackState();
            dieState = new PlayerDieState();

            walkState.AddTransitions(new Transition(attackState, () => (canAttack && isCloseEnoughToAttack)));
            attackState.AddTransitions(new Transition(walkState, () => isCloseEnoughToAttack));

            walkState.AddTransitions(new Transition(dieState, () => !isAlive));
            attackState.AddTransitions(new Transition(dieState, () => !isAlive));

            fsm.AddState(walkState);
            fsm.AddState(attackState);
            fsm.AddState(dieState);

            fsm.Initialise("Walk");

            health = 50;
            maxHealth = 200;
            IsAlive = true;
        }

        public override void LoadContent(ResourceManager resources)
        {
            gameTime = new GameTime();
            //TODO: Keep local copy of resource manager, don't think this is a good approach
            Resources = resources;

            //TODO: Too specific, i think this class shouldn't need to know so many specifics, they should be passed in
            walkAnimation = new Animation(Resources.SpriteSheets["Sprites/Player/WalkSpriteSheet"],0.25f, true);
            attackAnimation = new Animation(Resources.SpriteSheets["Sprites/Player/Attack"], 0.25f, true);
            death = new Animation(Resources.SpriteSheets["Sprites/Player/Death"], 0.25f, false);
            sprite.PlayAnimation(walkAnimation);

            int width = (int)(walkAnimation.FrameWidth * 0.4f);
            int left = (walkAnimation.FrameWidth - width) / 2;
            int height = (int)(walkAnimation.FrameHeight * 0.8);
            int top = walkAnimation.FrameHeight - height;
            localBounds = new Rectangle(left, top, width, height);

            //Collidable = new Collidable();
            BoundingBox = BoundingRectangle;
            //sounds
        }

        public override void Reset(Vector2 position)
        {
            //TODO: fill in
        }

        public override void Update(GameTime gameTime)
        {
            this.gameTime = gameTime;

            isAlive = !(health <= 0);

            if (isAlive)
            {
                sprite.PlayAnimation(walkAnimation);

                if (Vector2.Distance(lastMouseLocation, Position) < stoppingDistance)
                    velocity = Vector2.Zero;
                else
                    Position += velocity;

                BoundingBox = BoundingRectangle;
            }
            else
            {
                sprite.PlayAnimation(death);
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (Velocity.X > 0)
                flip = SpriteEffects.FlipHorizontally;
            if (Velocity.X < 0)
                flip = SpriteEffects.None;

            sprite.Draw(gameTime, spriteBatch, Position, flip);
        }
        
        public void MoveTowards(eButtonState buttonState, Vector2 mouseLocation)
        {
            if (buttonState == eButtonState.DOWN)
            {
                if (gameTime == null)
                    return;

                float elapsedTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
                
                if (Utilities.IsVectorInsideWindow(mouseLocation, Resources.GraphicsDevice))
                {
                    lastMouseLocation = mouseLocation;

                    //calculate the direction in which to move
                    Vector2 direction = mouseLocation - Position;
                    direction.Normalize();

                    //float distance = Vector2.Distance(Position, mouseLocation);
                    direction = direction * speed * elapsedTime;

                    Velocity = direction;
                }
                else
                {
                    //lastMouseLocation = 
                }
            }
        }


        internal void Die()
        {
            speed = 0.0f;
            sprite.PlayAnimation(death);
            isAlive = false;
        }

        public override bool CollisionTest(Collidable col)
        {
            if (col != null)
            {
                OnCollision(col);
                return BoundingBox.Intersects(col.BoundingBox);
            }

            return false;
        }

        public override void OnCollision(Collidable col)
        {
            EnemyGameObject enemy = col as EnemyGameObject;

            if (enemy != null)
            {

            }
        }

        public void HealPlayer(int amount)
        {
            health = Math.Min(health + amount, maxHealth);
        }

        public void HurtPlayer(int amount)
        {
            health = Math.Max(0, health - amount);
        }

        public void Attack()
        {
            sprite.PlayAnimation(attackAnimation);
            Velocity = Vector2.Zero;
            //OnDamageEnemy();
        }

        public void OnDamageEnemy(EnemyGameObject enemy)
        {
            DamageEnemy?.Invoke(this, enemy, new CollisionEventArgs(Position));
        }

        public void OnCollisionWithPotion(HealthPotionGameObject potion)
        {
            CollideWithPotion?.Invoke(this, potion, new CollisionEventArgs(Position));
        }

        public void OnCollisionWithEnemy(EnemyGameObject enemy)
        {
            if (canAttack)
                CollideWithEnemy?.Invoke(this, enemy, new CollisionEventArgs(Position));
        }
    }
}
