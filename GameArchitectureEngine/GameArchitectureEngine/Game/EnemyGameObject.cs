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
        public FSM fsm;
        public IdleState idle;
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
        //public Vector2 Direction
        //{
        //    get { return direction; }
        //    set { direction = value; }
        //}

        private int sightRange = 200;
        public int SightRange
        {
            get { return sightRange; }
        }

        private int attackRange = 32;
        public int AttackRange
        {
            get { return attackRange; }            
        }

        public Collidable Collidable;

        private Rectangle sightRangeRectangle;

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

        public EnemyGameObject(Vector2 position, GameObjectBase player)
        {
            Position = position;
            this.player = player;
        }

        public void LoadContent(Texture2D texture)
        {
            fsm = new FSM(this);
            idle = new IdleState();
            fsm.AddState(idle);
            fsm.Initialise("Idle");
            idleAnimation = new Animation(texture, 0.5f, true);
            sprite.PlayAnimation(idleAnimation);

            int width = (int)(idleAnimation.FrameWidth * 0.4f);
            int left = (idleAnimation.FrameWidth - width) / 2;
            int height = (int)(idleAnimation.FrameHeight * 0.8);
            int top = idleAnimation.FrameHeight - height;
            localBounds = new Rectangle(left, top, width, height);

            //Collidable = new Collidable();
            BoundingBox = new Rectangle(BoundingRectangle.X, BoundingRectangle.Y, BoundingRectangle.Width, BoundingRectangle.Height);

            sightRangeRectangle = new Rectangle((int)Position.X - SightRange, (int)Position.Y - SightRange, SightRange * 2, SightRange * 2);
        }

        public override void Update(GameTime gameTime)
        {
            this.gameTime = gameTime;
            fsm.Update(gameTime);
            sightRangeRectangle = new Rectangle((int)Position.X - SightRange, (int)Position.Y - SightRange, SightRange * 2, SightRange * 2);
            Position += Velocity;
            //MoveToward(player, gameTime);            
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
            if (obj.BoundingBox.Intersects(sightRangeRectangle))
            {
                float elapsedTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

                Vector2 directionToChase = obj.Position - Position;

                directionToChase.Normalize();
                Velocity = directionToChase * speed * elapsedTime;
                direction = directionToChase;
            }
        }


        public void SetRandomDirection()
        {
            float randomXDirection = Utilities.RandomFloatInRange();
            float randomYDirection = Utilities.RandomFloatInRange();

            direction.X += randomXDirection * Speed;
            direction.Y += randomYDirection * Speed;
            direction.Normalize();

            Velocity = direction * speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        
    }
}
