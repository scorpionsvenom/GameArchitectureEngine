using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameArchitectureEngine
{
    public class Tree : GameObjectBase
    {
        private Animation treeAnimation;
        private AnimationPlayer sprite;

        public Tree(Vector2 position)
        {
            this.Position = position;
        }

        public override void Initialise()
        { 
        }

        public override void LoadContent(ResourceManager Resources)
        {
            treeAnimation = new Animation(Resources.SpriteSheets["Sprites/Environment/Tree"], 1.0f, false);
            sprite.PlayAnimation(treeAnimation);
        }

        public override void Update(GameTime gameTime)
        {
        }

        public override void Reset(Vector2 position)
        {           
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            sprite.Draw(gameTime, spriteBatch, Position, SpriteEffects.None);
        }
       
    }

    public class Rock : GameObjectBase
    {
        private Animation rockAnimation;
        private AnimationPlayer sprite;

        public Rock(Vector2 position)
        {
            this.Position = position;
        }

        public override void Initialise()
        {
        }

        public override void LoadContent(ResourceManager Resources)
        {
            rockAnimation = new Animation(Resources.SpriteSheets["Sprites/Environment/Rock"], 1.0f, false);
            sprite.PlayAnimation(rockAnimation);
        }

        public override void Update(GameTime gameTime)
        {
        }

        public override void Reset(Vector2 position)
        {
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            sprite.Draw(gameTime, spriteBatch, Position, SpriteEffects.None);
        }

    }
}
