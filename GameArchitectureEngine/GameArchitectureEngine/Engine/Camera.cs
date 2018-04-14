using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameArchitectureEngine
{
    public class Camera
    {
        private Vector2 position;
        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        private int width;
        public int Width
        {
            get { return width; }
            set { width = value; }
        }

        private int height;
        public int Height
        {
            get { return height; }
            set { height = value; }
        }

        private int levelWidth;
        public int LevelWidth
        {
            get { return levelWidth; }
            set { levelWidth = value; }
        }

        private int levelHeight;
        public int LevelHeight
        {
            get { return levelHeight; }
            set { levelHeight = value; }
        }

        /// <summary>
        /// Initialises the camera viewport's centre position, the width and height of the view port and the dimensions of the level
        /// </summary>
        /// <param name="centrePosition"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="levelWidth"></param>
        /// <param name="levelHeight"></param>
        public Camera(Vector2 centrePosition, int width, int height, int levelWidth, int levelHeight)
        {
            this.position = centrePosition;
            this.width = width;
            this.height = height;
            this.levelWidth = levelWidth;
            this.levelHeight = levelHeight;
        }

        /// <summary>
        /// Pass in the centre coordinates, and this will ensure the camera stays inside the level boundaries
        /// </summary>
        /// <param name="midPointX"></param>
        /// <param name="midPointY"></param>
        public void Update(int midPointX, int midPointY)
        {
            int camX = 0;
            int camY = 0;

            int testLeftX = midPointX - width / 2;
            int testRightX = midPointX + width / 2;
            int testUpY = midPointY - width / 2;
            int testDownY = midPointY + width / 2;

            if (testLeftX > 0 && testRightX < levelWidth)
                camX = (int)CalculatePosition(midPointX, midPointY).X;
            else if (testLeftX < 0)
                camX = 0;
            else if (testRightX > levelWidth)
                camX = levelWidth - width / 2;

            if (testUpY > 0 && testDownY < levelHeight)
                camY = (int)CalculatePosition(midPointX, midPointY).X;
            else if (testUpY < 0)
                camY = 0;
            else if (testDownY > levelHeight)
                camY = levelHeight - height / 2;

            position = new Vector2(camX, camY);
        }

        private Vector2 CalculatePosition(int midPointX, int midPointY)
        {
            return new Vector2(midPointX - width / 2, midPointY - height / 2);
        }
    }
}
