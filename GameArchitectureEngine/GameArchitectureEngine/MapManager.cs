using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.IO;

namespace GameArchitectureEngine
{
    public class MapManager : IDisposable
    {
        private Tile[,] tiles;
        private Texture2D[] layers;

        //Layer upon which to draw entities
        private const int EntityLayer = 2;

        private Vector2 start;
        private Point exit = InvalidPosition;
        private static readonly Point InvalidPosition = new Point(-1, -1);

        //Map state
        private Random random = new Random(1); //testing value

        private FileLoader fileLoader;

        private bool reachedMapPortal;

        public bool ReachedMapPortal
        {
            get { return reachedMapPortal; }
        }

        private ResourceManager resources;

        public ResourceManager Resources
        {
            get { return resources; }
        }

        ///<summary>
        /// Build the map
        /// </summary>
        public MapManager(ResourceManager resources)
        {
            this.resources = resources;

            fileLoader = new FileLoader()
        }

    }
}
