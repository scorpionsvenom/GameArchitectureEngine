﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameArchitectureEngine
{
    public class PotionGameObject : GameObjectBase
    {
        public Collidable Collidable;
        protected Animation displayAnimation;
        protected AnimationPlayer sprite;

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

        //public PotionGameObject(Vector2 position)
        //{            
        //    Position = position;
        //}

        public override void Initialise()
        {
            
        }

        public override void LoadContent(ResourceManager resources)
        {
            int width = (int)(displayAnimation.FrameWidth * 0.4f);
            int left = (displayAnimation.FrameWidth - width) / 2;
            int height = (int)(displayAnimation.FrameHeight * 0.8);
            int top = displayAnimation.FrameHeight - height;
            localBounds = new Rectangle(left, top, width, height);
            
            BoundingBox = new Rectangle(BoundingRectangle.X, BoundingRectangle.Y, BoundingRectangle.Width, BoundingRectangle.Height);
        }

        public override void Update(GameTime gameTime)
        {

        }

        public override void Draw(GameTime gameTime, SpriteBatch sprBatch)
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

        //public override void OnCollision(Collidable col)
        //{
        //    PlayerGameObject player = col as PlayerGameObject;

        //    if (player != null)
        //    {                
        //        player.OnCollisionWithPotion(this);
        //        flagForRemoval = true;
        //    }
        //}

        public override void Reset(Vector2 position)
        {
            throw new NotImplementedException();
        }

        //public void OnHealPlayer()
        //{
        //    HealPlayer?.Invoke(this, new CollisionEventArgs(Position));
        //}
    }

    public class StrengthPotion : PotionGameObject
    {
        //...embolden? really?
        public event CollisionHandler EmboldenPlayer;

        public StrengthPotion(Vector2 position)
        {
            Position = position;
        }

        public override void LoadContent(ResourceManager resources)
        {
            displayAnimation = new Animation(resources.SpriteSheets["Sprites/Powerups/AttackBoostPotion"], 0.5f, true);
            sprite.PlayAnimation(displayAnimation);

            base.LoadContent(resources);
        }

        public void OnEmboldenPlayer()
        {
            EmboldenPlayer?.Invoke(this, new CollisionEventArgs(Position));
        }
    }

    public class HealthPotion : PotionGameObject
    {
        public event CollisionHandler HealPlayer;

        public HealthPotion(Vector2 position)
        {
            Position = position;
        }

        public override void LoadContent(ResourceManager resources)
        {
            displayAnimation = new Animation(resources.SpriteSheets["Sprites/Powerups/Potion"], 0.5f, true);
            sprite.PlayAnimation(displayAnimation);

            base.LoadContent(resources);
        }

        public void OnHealPlayer()
        {
            HealPlayer?.Invoke(this, new CollisionEventArgs(Position));
        }
    }
}
