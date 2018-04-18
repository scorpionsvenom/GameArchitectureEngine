//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace GameArchitectureEngine
//{
//    public delegate void CollisionEventHandler(object sender, EventArgs e);

//    public class CollisionEventManager
//    {
//        public event CollisionEventHandler OnCollisionEnterEvent;
//        public event CollisionEventHandler OnCollisionExitEvent;

//        private List<Collidable> collidables = new List<Collidable>();
//        private HashSet<Collision> collisions = new HashSet<Collision>(new CollisionComparer());

//        public void OnCollisionEnter(EventArgs e)
//        {
//            OnCollisionEnterEvent?.Invoke(this, e);
//        }

//        public void OnCollisionExit(EventArgs e)
//        {
//            OnCollisionExitEvent?.Invoke(this, e);
//        }

//        public void init()
//        {
//            foreach (Collision col in collisions)
//            {
//                //OnCollisionEnterEvent += col.Resolve;
//                //OnCollisionExitEvent += col.ResolveExit;
//            }
//        }

//        public void AddCollidable(Collidable col)
//        {
//            collidables.Add(col);            
//        }

//        public void Update()
//        {
//            UpdateCollisions();
//            ResolveCollisions();
//        }

//        private void UpdateCollisions()
//        {
//            for (int i = 0; i < collidables.Count; i++)
//            {
//                for (int j = 0; j < collidables.Count; j++)
//                {
//                    Collidable collidable1 = collidables[i];
//                    Collidable collidable2 = collidables[j];

//                    if (!collidable1.Equals(collidable2))
//                    {
//                        if (collidable1.CollisionTest(collidable2))
//                        {
//                            collisions.Add(new Collision(collidable1, collidable2));
//                        }
//                    }
//                }
//            }
//        }

//        private void ResolveCollisions()
//        {
//            foreach (Collision col in collisions)
//                col.Resolve();
//        }

//        public void RemoveCollidable(List<Collidable> toRemove)
//        {
//            for (int i = 0; i < toRemove.Count; i++)
//            {
//                for (int j = 0; j < collidables.Count; j++)
//                {
//                    if (toRemove[i].Equals(collidables[j]))
//                    {
//                        collidables.RemoveAt(j);
//                    }
//                }
//            }
//        }
//    }
//}
