using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;

namespace GameArchitectureEngine
{
    public class MapManager //: IDisposable
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
        }


        public void Draw(Map map, SpriteBatch spriteBatch)
        {
            List<string[]> mapList = map.MapList;

            for (int i = 0; i < mapList.Count; i++)
            {
                for (int j = 0; j < mapList[i].Length; j++)
                {
                    int x, y, width, height;
                    width = height = 64;
                    x = y = 0;

                    string value = mapList[i][j];
                    string earth = "" + MapTileType.Earth;

                    Vector2 origin = Vector2.Zero;

                    switch (int.Parse(value))
                    {
                        case (int)MapTileType.Earth:
                            x = y = 0;
                            break;
                        case (int)MapTileType.Grass:
                            x = 64; y = 0;
                            break;
                        case (int)MapTileType.Water:
                            x = 0; y = 64;
                            break;
                        case (int)MapTileType.Mountain:
                            x = y = 64;
                            break;
                        default:
                            x = y = 0;
                            break;
                    }

                    Rectangle source = new Rectangle(x, y, width, height);
                    Vector2 position = new Vector2(j * 64, i * 64);

                    //TODO this resource should be passed in by caller, not stored internally. Not generic enough
                    Texture2D texture = Resources.TileSheets[@"TileSheet/Terrains"];
                    //TODO too many magic numbers...details should be passed in.
                    spriteBatch.Draw(texture, position, source, Color.White, 0.0f, origin, 1.0f, SpriteEffects.None, .8f);
                }
            }
        }
    }
}
