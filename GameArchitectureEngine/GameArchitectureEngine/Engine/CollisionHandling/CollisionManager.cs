using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameArchitectureEngine
{
    public class CollisionManager
    {
        private List<Collidable> collidables = new List<Collidable>();
        private HashSet<Collision> collisions = new HashSet<Collision>(new CollisionComparer());
        
        public void AddCollidable(Collidable col)
        {
            collidables.Add(col);            
        }

        public void Update()
        {
            UpdateCollisions();
            ResolveCollisions();
        }

        private void UpdateCollisions()
        {
            if (collisions.Count > 0)
            {
                collisions.Clear();
            }

            for (int i = 0; i < collidables.Count; i++)
            {
                for (int j = 0; j < collidables.Count; j++)
                {
                    Collidable collidable1 = collidables[i];
                    Collidable collidable2 = collidables[j];

                    if (!collidable1.Equals(collidable2))
                    {
                        if (collidable1.CollisionTest(collidable2))
                        {
                            collisions.Add(new Collision(collidable1, collidable2));
                        }
                    }
                }
            }
        }

        private void ResolveCollisions()
        {
            foreach (Collision col in collisions)
                col.Resolve();
        }

        public void RemoveCollidable(List<Collidable> toRemove)
        {
            for (int i = 0; i < toRemove.Count; i++)
            {
                for (int j = 0; j < collidables.Count; j++)
                {
                    if (toRemove[i].Equals(collidables[j]))
                    {
                        collidables.RemoveAt(j);
                    }
                }
            }
        }
    }
}
