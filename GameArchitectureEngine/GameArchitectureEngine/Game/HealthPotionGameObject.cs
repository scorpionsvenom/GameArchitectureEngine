using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameArchitectureEngine
{
    public class HealthPotionGameObject : GameObjectBase
    {
        public Collidable Collidable;
        private Animation displayAnimation;
        private AnimationPlayer sprite;

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

        public HealthPotionGameObject(Vector2 position)
        {            
            Position = position;
        }

        public void LoadContent(Texture2D texture)
        {
            displayAnimation = new Animation(texture, 0.5f, true);
            //sprite = new AnimationPlayer();
            sprite.PlayAnimation(displayAnimation);

            int width = (int)(displayAnimation.FrameWidth * 0.4f);
            int left = (displayAnimation.FrameWidth - width) / 2;
            int height = (int)(displayAnimation.FrameHeight * 0.8);
            int top = displayAnimation.FrameHeight - height;
            localBounds = new Rectangle(left, top, width, height);

            //Collidable = new Collidable();
            BoundingBox = new Rectangle(BoundingRectangle.X, BoundingRectangle.Y, BoundingRectangle.Width, BoundingRectangle.Height);
        }

        public void Update(GameTime gameTime)
        {

        }

        public void Draw(GameTime gameTime, SpriteBatch sprBatch)
        {
            sprite.Draw(gameTime, sprBatch, Position, SpriteEffects.None);

        }

        public override bool CollisionTest(Collidable col)
        {
            if (col != null)
            {
                return BoundingBox.Intersects(col.BoundingBox);
            }

            return false;
        }

        public override void OnCollision(Collidable col)
        {
            PlayerGameObject player = col as PlayerGameObject;

            if (player != null)
            {
                //TODO: fire event to heal player
                player.HealPlayer(50); 

                flagForRemoval = true;
            }
        }
    }
}
