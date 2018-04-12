using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GameArchitectureEngine
{
    public class MousePointer
    {
        public Texture2D Icon;
        public MouseState mouseState;
        public Vector2 Position;

        public void LoadContent(ResourceManager resources)
        {
            Icon = resources.SpriteSheets["Sprites/Player/MouseIcon"];
        }

        public void Update()
        {
            mouseState = Mouse.GetState();

            Position.X = mouseState.X;
            Position.Y = mouseState.Y;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Icon, Position, Color.White);
        }
    }
}
