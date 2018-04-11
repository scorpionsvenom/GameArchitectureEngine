using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameArchitectureEngine
{
    public class CollisionListener
    {
        public event CollisionEventHandler<CollisionEventArgs> OnCollision = delegate { };
        public event CollisionEventHandler<CollisionEventArgs> OnExitCollision = delegate { };

        public CollisionListener()
        {
        }

        public void Update()
        {
            FireCollisionEvents();
        }

        private void FireCollisionEvents()
        {

        }
    }
}
