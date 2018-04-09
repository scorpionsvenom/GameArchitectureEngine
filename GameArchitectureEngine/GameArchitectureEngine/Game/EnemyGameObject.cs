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

        private float speed = 2.0f;
        private int range = 200;
        private int multiplier = 4;

        public Collidable Collidable;

        private Rectangle sightRange;

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

        private FSM fsm;

        private GameObjectBase player;

        public EnemyGameObject(Vector2 position, GameObjectBase player)
        {
            Position = position;
            this.player = player;
        }

        public void LoadContent(Texture2D texture)
        {
            idleAnimation = new Animation(texture, 0.5f, true);
            sprite.PlayAnimation(idleAnimation);

            int width = (int)(idleAnimation.FrameWidth * 0.4f);
            int left = (idleAnimation.FrameWidth - width) / 2;
            int height = (int)(idleAnimation.FrameHeight * 0.8);
            int top = idleAnimation.FrameHeight - height;
            localBounds = new Rectangle(left, top, width, height);

            //Collidable = new Collidable();
            BoundingBox = new Rectangle(BoundingRectangle.X, BoundingRectangle.Y, BoundingRectangle.Width, BoundingRectangle.Height);

            sightRange = new Rectangle((int)Position.X - range, (int)Position.Y - range, range * 2, range * 2);
        }

        public override void Update(GameTime gameTime)
        {
            sightRange = new Rectangle((int)Position.X - range, (int)Position.Y - range, range* 2, range * 2);
            MoveToward(player, gameTime);            
        }

        public void Draw(GameTime gameTime, SpriteBatch sprBatch)
        {
            sprite.Draw(gameTime, sprBatch, Position, SpriteEffects.None);            
        }

        public void MoveToward(GameObjectBase obj, GameTime gameTime)
        {
            if (obj.BoundingBox.Intersects(sightRange))
            {
                float elapsedTime = (float)gameTime.ElapsedGameTime.TotalMilliseconds;

                Vector2 direction = obj.Position - Position;

                direction.Normalize();

                Position += (direction * speed);
            }
        }        
    }
}
