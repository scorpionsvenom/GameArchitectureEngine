using System;

namespace GameArchitectureEngine
{
    public class CollisionEventArgs : EventArgs
    {
        public readonly bool isColliding;
        public bool wasCollidingLastFrame;

        public CollisionEventArgs(bool isColliding, bool wasColliding)
        {
            this.isColliding = isColliding;
            this.wasCollidingLastFrame = wasColliding;
        }
    }
}