using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameArchitectureEngine
{
    public static class Utilities
    {
        private static Random random = new Random();

        public static float RandomFloatInRange()
        {
            return (float)(random.NextDouble() - random.NextDouble());
        }

        public static int Rand()
        {
            return random.Next();
        }
        
        public static bool IsVectorInsideWindow(Vector2 vectorToCheck, GraphicsDevice graphicsDevice)
        {
            Point pos = new Point((int)vectorToCheck.X, (int)vectorToCheck.Y);

            return graphicsDevice.Viewport.Bounds.Contains(pos);
        }        
    }
}
