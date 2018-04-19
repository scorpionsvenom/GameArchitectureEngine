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
        
        ///<summary https://stackoverflow.com/questions/7173256/check-if-mouse-is-inside-the-game-window>
        ///check if mouse coords are inside the window
        /// </summary>
        public static bool IsVectorInsideWindow(Vector2 vectorToCheck, GraphicsDevice graphicsDevice)
        {
            Point pos = new Point((int)vectorToCheck.X, (int)vectorToCheck.Y);

            return graphicsDevice.Viewport.Bounds.Contains(pos);
        }
        
        public static List<GameObjectBase> SelectionSortList(List<GameObjectBase> listToSort)
        {
            int min_index = 0;

            for (int i = 0; i < listToSort.Count - 1; i++)
            {
                min_index = i;

                for (int j = i + i; j < listToSort.Count; j++)
                {
                    if (listToSort[j].Depth < listToSort[min_index].Depth)
                        min_index = j;
                }

                GameObjectBase a = listToSort[i];
                GameObjectBase b = listToSort[min_index];
                
                listToSort[i] = b;
                listToSort[min_index] = a;
            }

            return listToSort;
        } 

        private static void Swap(ref GameObjectBase a, ref GameObjectBase b)
        {
            GameObjectBase temp = a;
            a = b;
            b = temp;
        }
    }
}
