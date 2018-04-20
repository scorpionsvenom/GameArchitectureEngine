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
        
        public Texture2D Icon;

        public MouseState mouseState;

        //For collision with enemy, allowing player to attack
        //private Rectangle boundingBox;
        private int rectDimensions = 10;
        private bool mouseClicked;

        public PlayerGameObject player { get; private set; }

        public MousePointer(CommandManager commandManager, PlayerGameObject player)
        {
            this.commandManager = commandManager;

            this.player = player;

            //commandManager.m_Input.OnKeyPressed += MousePressed;
        }

        public override void Initialise()
        {            
        }

        /// <summary>
        /// 
        /// </summary>
        public override void LoadContent(ResourceManager resources)
        {
            Icon = resources.SpriteSheets["Sprites/Player/MouseIcon"];
        }

        public override void Update(GameTime gameTime)
        {
            mouseState = Mouse.GetState();

            Vector2 mousePosition = new Vector2(mouseState.X, mouseState.Y);
            
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
                    return BoundingBox.Intersects(col.BoundingBox);
                }

                PotionGameObject potion = col as PotionGameObject;

                if (potion != null)
                {
                    return BoundingBox.Intersects(col.BoundingBox);
                }
            }

            return false;
        }

        public override void OnCollision(Collidable col)
        {
            //EnemyGameObject enemy = col as EnemyGameObject;

            if (col != null)
            {
                //return BoundingBox.Intersects(col.BoundingBox);
                OnCollisionWithEntity(col as GameObjectBase);                
            }
        }

        public void OnCollisionWithEntity(GameObjectBase entity)
        {
            MouseState state = Mouse.GetState();

            if (entity != null && state.LeftButton == ButtonState.Pressed)
            {
                EnemyGameObject enemy = entity as EnemyGameObject;

                if (enemy != null)
                {
                    if (enemy.IsAlive)
                    {
                        player.CanAttack = true;
                        player.target = enemy;                        
                    }
                    return;
                }

                PotionGameObject potion = entity as PotionGameObject;
                
                if (potion != null)
                {
                    player.target = potion;
                    player.retrieveItem = true;
                    return;                    
                }
                //SelectEnemy?.Invoke((object)enemy, new CollisionEventArgs(enemy.Position));
            }
        }

        //public void MouseSelectEntity(object sender)//, CollisionEventArgs e)
        //{
        //    MouseState state = Mouse.GetState();

        //    if (state.LeftButton == ButtonState.Pressed)
        //    {
        //        EnemyGameObject enemy = sender as EnemyGameObject;

        //        if (enemy != null)
        //        {
        //            if (enemy.IsAlive)
        //            {
        //                player.CanAttack = true;
        //                player.target = enemy;
        //                return;
        //            }
        //        }

        //        HealthPotionGameObject potion = sender as HealthPotionGameObject;

        //        //MouseState state = Mouse.GetState();

        //        if (potion != null)
        //        {
        //            player.target = potion;
        //            //TODO: move player towards selected item
        //            //player.MoveTowards(state, new Vector2(state.X, state.Y));
        //            return;
        //        }
        //    }
        //}
        

        //public void OnSelectEnemy(eButtonState buttonState, Vector2 mouseLocation)
        //{
        //    if (buttonState == eButtonState.PRESSED)
        //    {
                               
        //    }
        //}
    }
}
