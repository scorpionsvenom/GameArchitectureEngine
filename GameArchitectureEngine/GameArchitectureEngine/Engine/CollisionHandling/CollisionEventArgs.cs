using System;
using Microsoft.Xna.Framework;

namespace GameArchitectureEngine
{
    public class CollisionEventArgs : EventArgs
    {
        public readonly Vector2 PositionOfCollidingObject;

        public CollisionEventArgs(Vector2 position)
        {
            PositionOfCollidingObject = position;
        }
    }
}