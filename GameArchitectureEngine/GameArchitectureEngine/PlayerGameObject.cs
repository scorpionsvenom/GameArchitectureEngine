using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameArchitectureEngine
{
    public class PlayerGameObject: GameObjectBase
    {
        private ResourceManager Resources;

        private Vector2 velocity;
        private Animation walkAnimation;
        private AnimationPlayer sprite;

        private SpriteEffects flip = SpriteEffects.None;

        private bool isAlive = true;

        public Vector2 Velocity
        {
            get { return velocity; }
            set { velocity = value; }
        }

        private float speed = 5.0f;

        private const float MoveAcceleration = 12000.0f;
        private const float MaxMoveSpeed = 1500.0f;
        private const float DragFactor = 0.4f;

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

        public PlayerGameObject()
        {
            Position = new Vector2(120f, 200f);
            //Reset(Position);
        }

        public override void Initialise()
        {
            
        }

        public void LoadContent(ResourceManager resources)
        {
            //Keep local copy of resource manager, don't think this is a good approach
            Resources = resources;

            //Too specific, i think this class shouldn't need to know so many specifics, they should be passed in
            walkAnimation = new Animation(Resources.SpriteSheets["Sprites/Player/WalkSpriteSheet"],0.1f, true);
            sprite.PlayAnimation(walkAnimation);

            int width = (int)(walkAnimation.FrameWidth * 0.4f);
            int left = (walkAnimation.FrameWidth - width) / 2;
            int height = (int)(walkAnimation.FrameHeight * 0.8);
            int top = walkAnimation.FrameHeight - height;
            localBounds = new Rectangle(left, top, width, height);

            //sounds
        }

        public override void Reset(Vector2 position)
        {

        }

        public override void Update(GameTime gameTime)
        {
            if (isAlive)
                sprite.PlayAnimation(walkAnimation);
            //else
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (Velocity.X > 0)
                flip = SpriteEffects.FlipHorizontally;
            if (Velocity.X < 0)
                flip = SpriteEffects.None;

            sprite.Draw(gameTime, spriteBatch, Position, flip);
        }

        public void Move(Vector2 velocityChange)
        {
            velocity = velocityChange;
        }
        
        public void MoveTowards(eButtonState buttonState, Vector2 mouseLocation)
        {
            if (buttonState == eButtonState.DOWN)
            {
                Console.WriteLine("Move towards called");
                Vector2 playerLocation = Position;

                Console.WriteLine("playerLocation {0}", playerLocation);

                Rectangle safeArea = Resources.GraphicsDevice.Viewport.TitleSafeArea;
                
                if (IsVectorInsideWindow(mouseLocation, Resources.GraphicsDevice))
                {
                    //calculate the direction in which to move
                    Vector2 direction = playerLocation - mouseLocation;
                    Console.WriteLine("direction {0}", direction);
                    direction.Normalize();
                    Console.WriteLine("direction {0}", direction);
                    direction *= speed;

                    playerLocation += direction;
                    Console.WriteLine("new playerLocation: {0}", playerLocation);
                    Position = playerLocation;
                }
            }
        }

        //should be utility function
        ///<summary https://stackoverflow.com/questions/7173256/check-if-mouse-is-inside-the-game-window>
        ///check if mouse coords are inside the window
        /// </summary>
        bool IsVectorInsideWindow(Vector2 vectorToCheck, GraphicsDevice graphicsDevice)
        {
            Point pos = new Point((int)vectorToCheck.X, (int)vectorToCheck.Y);

            return graphicsDevice.Viewport.Bounds.Contains(pos);
        }
    }
}
