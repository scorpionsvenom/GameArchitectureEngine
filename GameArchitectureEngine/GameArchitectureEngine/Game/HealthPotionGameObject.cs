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

        public HealthPotionGameObject(Vector2 position)
        {
            Collidable = new Collidable((int)Position.X, (int)Position.Y, sprite.Animation.FrameWidth, sprite.Animation.FrameHeight);
            Position = position;
        }

        public void LoadContent(Texture2D texture)
        {
            displayAnimation = new Animation(texture, 0.5f, true);
            sprite = new AnimationPlayer();
            sprite.PlayAnimation(displayAnimation);
        }

        public void Update(GameTime gameTime)
        {

        }

        public void Draw(GameTime gameTime, SpriteBatch sprBatch)
        {
            sprite.Draw(gameTime, sprBatch, Position, SpriteEffects.None);
        }
    }
}
