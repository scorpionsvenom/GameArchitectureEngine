using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace GameArchitectureEngine
{
    public class Collidable
    {        
        private Rectangle boundingBox;
        public Rectangle BoundingBox
        {
            get { return boundingBox; }
        }

        public Collidable(int x, int y, int width, int height)
        {
            boundingBox = new Rectangle(x, y, width, height);
        }

        public bool CollisionTest(Collidable col)
        {
            if (col != null)
            {
                return BoundingBox.Intersects(col.BoundingBox);
            }

            return false;
        }
        public void OnCollision(Collidable col)
        {

        }
    }    
}
