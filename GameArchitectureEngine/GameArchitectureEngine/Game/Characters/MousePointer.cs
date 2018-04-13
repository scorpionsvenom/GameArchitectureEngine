using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GameArchitectureEngine
{
    public class MousePointer : GameObjectBase
    {
        public Texture2D Icon;
        public MouseState mouseState;
        public Vector2 Position;

        public override void LoadContent(ResourceManager resources)
        {
            Icon = resources.SpriteSheets["Sprites/Player/MouseIcon"];
        }

        public override void Update(GameTime gameTime)
        {
            mouseState = Mouse.GetState();

            Position.X = mouseState.X;
            Position.Y = mouseState.Y;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Icon, Position, Color.White);
        }

        public override void Reset(Vector2 position)
        {
            throw new NotImplementedException();
        }
    }
}
