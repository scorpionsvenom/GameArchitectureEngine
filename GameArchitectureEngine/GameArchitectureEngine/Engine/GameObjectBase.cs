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
        private Vector2 position;
        private float rotation;
        private string type;

        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        public float Rotation
        {
            get { return rotation; }
            set { rotation = value; }
        }

        public string Type
        {
            get { return type; }
            set { type = value; }
        }

        public virtual void Initialise()
        {
        }

        public abstract void LoadContent(ResourceManager Resources);

        public abstract void Reset(Vector2 position);

        public abstract void Update(GameTime gameTime);

        public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch);
    }
}
