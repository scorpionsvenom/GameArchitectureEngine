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
        public event CollisionHandler SelectEnemy;

        public Texture2D Icon;
        public MouseState mouseState;

        //For collision with enemy, allowing player to attack
        private Rectangle boundingBox;
        private int rectDimensions = 2;

        public override void LoadContent(ResourceManager resources)
        {
            Icon = resources.SpriteSheets["Sprites/Player/MouseIcon"];
            boundingBox = new Rectangle();
        }

        public override void Update(GameTime gameTime)
        {
            mouseState = Mouse.GetState();

            Vector2 mousePosition = new Vector2(mouseState.X, mouseState.Y);

            Position = mousePosition;

            boundingBox.X = (int)(Position.X - rectDimensions / 2);
            boundingBox.Y = (int)(Position.Y - rectDimensions / 2);
            boundingBox.Width = rectDimensions;
            boundingBox.Height = rectDimensions;


        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Icon, Position, Color.White);
        }

        public override void Reset(Vector2 position)
        {
            throw new NotImplementedException();
        }

        public void OnCollisionWithEnemy(EnemyGameObject enemy)
        {
            SelectEnemy?.Invoke((object)enemy, new CollisionEventArgs(enemy.Position));
        }
    }
}
