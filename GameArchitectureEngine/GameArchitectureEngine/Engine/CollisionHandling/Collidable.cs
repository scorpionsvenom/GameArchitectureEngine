using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace GameArchitectureEngine
{
    public class Collidable
    {
        public bool flagForRemoval;

        private Rectangle boundingBox;
        public Rectangle BoundingBox
        {
            get { return boundingBox; }
            set { boundingBox = value; }
        }        

        public virtual bool CollisionTest(Collidable col)
        {
            return false;
        }
        //public bool CollisionTest(Collidable col)
        //{
        //    if (col != null)
        //    {
        //        return BoundingBox.Intersects(col.BoundingBox);
        //    }

        //    return false;
        //}
        public virtual void OnCollision(Collidable col)
        {

        }
    }    
}
