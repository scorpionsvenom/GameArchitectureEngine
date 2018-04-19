using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna;
using Microsoft.Xna.Framework.Graphics;

namespace GameArchitectureEngine
{
    //TODO: make better use of this inheritance
    public abstract class GameObjectBase : Collidable
    {
        protected Vector2 position;
        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        protected Vector2 velocity;
        public Vector2 Velocity
        {
            get { return velocity; }
            set { velocity = value; }
        }


        protected float rotation;
        public float Rotation
        {
            get { return rotation; }
            set { rotation = value; }
        }

        protected string type;
        public string Type
        {
            get { return type; }
            set { type = value; }
        }

        public float Depth
        {
            get { return Position.Y; }
            set { Depth = value; }
        }

        public abstract void Initialise();

        public abstract void LoadContent(ResourceManager Resources);

        public abstract void Reset(Vector2 position);

        public abstract void Update(GameTime gameTime);

        public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch);
    }
}
