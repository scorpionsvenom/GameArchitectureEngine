using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna;

namespace GameArchitectureEngine
{
    public class GameObjectBase
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

        public virtual void LoadContent()
        {
        }

        public virtual void Reset(Vector2 position)
        {
        }

        public virtual void Update(GameTime gameTime)
        {
        }

        public virtual void Draw()
        {
        }
    }
}
