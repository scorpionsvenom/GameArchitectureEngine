using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameArchitectureEngine
{
    public class Animation
    {
        private Texture2D texture;
        private float frameTime;
        private bool isLooping;

        public Texture2D Texture
        {
            get { return texture; }
        }

        public float FrameTime
        {
            get { return frameTime; }
        }

        public bool IsLooping
        {
            get { return isLooping; }
        }

        public int FrameCount
        {
            get { return Texture.Width / FrameWidth; }
        }

        public int FrameWidth
        {
            get { return Texture.Height; }
        }

        //This is not generic enough. What if we compress all sprites onto a single sheet? We need to be able to specify offsets in both dimensions
        //as well as widths and heights
        public int FrameHeight
        {
            get { return Texture.Height; }
        }

        /// <summary>
        /// This assumes a single animation contained horizontally in one spritesheet
        /// </summary>
        /// <param name="texture"></param>
        /// <param name="frameTime"></param>
        /// <param name="isLooping"></param>
        /// <param name="frameCount"></param>
        public Animation(Texture2D texture, float frameTime, bool isLooping)
        {
            this.texture = texture;
            this.frameTime = frameTime;
            this.isLooping = isLooping;
        }
    }
}
