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
        private Dictionary<int, MapTileType> mapTileTypes;
        private List<int> usedValues;

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
        public MapManager(ResourceManager resources) //TODO remove resources?
        {
            mapTileTypes = new Dictionary<int, MapTileType>();
            usedValues = new List<int>();
            this.resources = resources;            
        }


        public void Draw(Map map, SpriteBatch spriteBatch)
        {
            List<string[]> mapList = map.MapList;


            //TODO column or row major?
            for (int i = 0; i < mapList.Count; i++)
            {
                for (int j = 0; j < mapList[i].Length; j++)
                {
                    int x, y, width, height;
                    width = height = 64;
                    x = y = 0;

                    string value = mapList[i][j];
                    string earth = "" + enumMapTileType.Earth;

                    Vector2 origin = Vector2.Zero;
                    
                    int intValue = int.Parse(value);
                    MapTileType mapTileType;
                                        
                    //If value is not in the dictionary, throw an exception so the user fixes the map or adds the value
                    if (mapTileTypes.ContainsKey(intValue))
                    {
                        mapTileType = mapTileTypes[intValue];
                    }
                    else
                    {
                        //TODO handle exception: better than a default value, as what if this doesn't exist, AND, really the dev should be responsible for being correct here!
                        //throw new KeyNotFoundException("The map value was invalid");
                        mapTileType = mapTileTypes[0];
                    }

                    Rectangle source = new Rectangle(mapTileType.XOffset, mapTileType.YOffset, width, height);

                    Vector2 position = new Vector2(j * 64, i * 64);

                    //TODO this resource should be passed in by caller, not stored internally. Not generic enough
                    Texture2D texture = Resources.TileSheets[@"TileSheet/0"];
                    //TODO too many magic numbers...details should be passed in.
                    spriteBatch.Draw(texture, position, source, Color.White, 0.0f, origin, 1.0f, SpriteEffects.None, .8f);
                }
            }
        }

        public void AddMapTileTypes(string name, int mapValue, int xOffset, int yOffset)
        {
            MapTileType mapTileType = new MapTileType();
            mapTileType.Name = name;
            mapTileType.MapValue = mapValue;
            mapTileType.XOffset = xOffset;
            mapTileType.YOffset = yOffset;

            mapTileTypes.Add(mapValue, mapTileType);

            usedValues.Add(mapValue);            
        }

        struct MapTileType
        {
            public string Name;
            public int MapValue;
            public int XOffset;
            public int YOffset;
        }
    }
}
