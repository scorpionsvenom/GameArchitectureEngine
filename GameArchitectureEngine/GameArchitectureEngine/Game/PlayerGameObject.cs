using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameArchitectureEngine
{
    public class PlayerGameObject : GameObjectBase
    {
        private int health;
        public int Health
        {
            get { return health; }
        }

        private int maxHealth;
        public int MaxHealth
        {
            get { return maxHealth; }
        }

        public Collidable Collidable;

        private ResourceManager Resources;

        private Vector2 velocity;
        private Animation walkAnimation;
        private AnimationPlayer sprite;

        private SpriteEffects flip = SpriteEffects.None;

        private bool isAlive = true;

        private Vector2 lastMouseLocation = Vector2.Zero;
        private float stoppingDistance = 3.0f;

        public Vector2 Velocity
        {
            get { return velocity; }
            set { velocity = value; }
        }

        private float speed = 5.0f;        

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
            //TODO: set position from serialised object
            Position = new Vector2(120f, 200f);
            
            
            //Reset(Position);
        }

        public override void Initialise()
        {
            health = 50;
            maxHealth = 100;
        }

        public void LoadContent(ResourceManager resources)
        {
            //TODO: Keep local copy of resource manager, don't think this is a good approach
            Resources = resources;

            //TODO: Too specific, i think this class shouldn't need to know so many specifics, they should be passed in
            walkAnimation = new Animation(Resources.SpriteSheets["Sprites/Player/WalkSpriteSheet"],0.1f, true);
            sprite.PlayAnimation(walkAnimation);

            int width = (int)(walkAnimation.FrameWidth * 0.4f);
            int left = (walkAnimation.FrameWidth - width) / 2;
            int height = (int)(walkAnimation.FrameHeight * 0.8);
            int top = walkAnimation.FrameHeight - height;
            localBounds = new Rectangle(left, top, width, height);

            //Collidable = new Collidable();
            BoundingBox = BoundingRectangle;
            //sounds
        }

        public override void Reset(Vector2 position)
        {
            //TODO: fill in
        }

        public override void Update(GameTime gameTime)
        {
            //TODO: use idle animation if velocity is 0, or dead animation if dead
            if (isAlive)
                sprite.PlayAnimation(walkAnimation);
            //else

            if (Vector2.Distance(lastMouseLocation, Position) < stoppingDistance)
                velocity = Vector2.Zero;
            else
                Position += velocity;

            BoundingBox = BoundingRectangle;
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
                Rectangle safeArea = Resources.GraphicsDevice.Viewport.TitleSafeArea;
                
                if (IsVectorInsideWindow(mouseLocation, Resources.GraphicsDevice))
                {
                    lastMouseLocation = mouseLocation;

                    //calculate the direction in which to move
                    Vector2 direction = mouseLocation - Position;
                    direction.Normalize();

                    float distance = Vector2.Distance(Position, mouseLocation);
                    direction *= Math.Min(speed, distance);

                    Velocity = direction;
                }
            }
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
            
        }

        public void HealPlayer(int amount)
        {
            health = Math.Min(health + amount, maxHealth);
        }

        //TODO: should be utility function
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
