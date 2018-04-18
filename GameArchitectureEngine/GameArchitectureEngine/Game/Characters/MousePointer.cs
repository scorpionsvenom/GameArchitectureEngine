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

        public CommandManager commandManager;

        private int mapWidth;
        private int mapHeight;
        public Texture2D Icon;
        public MouseState mouseState;

        //For collision with enemy, allowing player to attack
        //private Rectangle boundingBox;
        private int rectDimensions = 10;
        private bool mouseClicked;

        public MousePointer(CommandManager commandManager)
        {
            this.commandManager = commandManager;
            this.mapWidth = mapWidth;
            this.mapHeight = mapHeight;

            //commandManager.m_Input.OnKeyPressed += MousePressed;
        }

        /// <summary>
        /// Can't override base class here because this class needs map dimensions for clamping the mouse coords.
        /// </summary>
        /// <param name="resources"></param>
        /// <param name="mapWidth"></param>
        /// <param name="mapHeight"></param>
        public override void LoadContent(ResourceManager resources)
        {
            Icon = resources.SpriteSheets["Sprites/Player/MouseIcon"];
            //BoundingBox = new Rectangle();
        }

        public override void Update(GameTime gameTime)
        {
            mouseState = Mouse.GetState();

            Vector2 mousePosition = new Vector2(mouseState.X, mouseState.Y);
            
            ////Clamp values
            //if (mousePosition.X < 0) mousePosition.X = 0;
            //if (mousePosition.Y < 0) mousePosition.Y = 0;
            //if (mousePosition.X > mapWidth) mousePosition.X = mapWidth;
            //if (mousePosition.Y > mapHeight) mousePosition.Y = mapHeight;

            Position = mousePosition;
            BoundingBox = new Rectangle((int)(Position.X - rectDimensions / 2), (int)(Position.Y - rectDimensions / 2), rectDimensions, rectDimensions);            
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Icon, Position, Color.White);
        }

        public override void Reset(Vector2 position)
        {
            throw new NotImplementedException();
        }

        public override bool CollisionTest(Collidable col)
        {
            if (col != null)
            {
                EnemyGameObject enemy = col as EnemyGameObject;
                if (enemy != null)
                {
                    OnCollisionWithEnemy(enemy);
                    return BoundingBox.Intersects(col.BoundingBox);
                }

                HealthPotionGameObject potion = col as HealthPotionGameObject;

                if (potion != null)
                {
                    return BoundingBox.Intersects(col.BoundingBox);
                }
            }

            return false;
        }

        public override void OnCollision(Collidable col)
        {
            EnemyGameObject enemy = col as EnemyGameObject;

            if (enemy != null && mouseClicked)
            {
                OnCollisionWithEnemy(col as EnemyGameObject);
            }
        }

        public void OnCollisionWithEnemy(EnemyGameObject enemy)
        {
            MouseState state = Mouse.GetState();

            if (state.LeftButton == ButtonState.Pressed)
            {
                SelectEnemy?.Invoke((object)enemy, new CollisionEventArgs(enemy.Position));
            }
        }        
    }
}
