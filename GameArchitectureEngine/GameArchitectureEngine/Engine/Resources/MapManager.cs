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
    /// <summary>
    /// Assumes a square grid map is passed in as a comma separated value file
    /// </summary>
    public class MapManager//: IDisposable
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
        
        /// <summary>
        /// The width of the map, used to determine level size
        /// </summary>
        public int width;

        ///<summary>
        /// The height of the map, used to determine level size
        /// </summary>
        public int height;
        ///<summary>
        /// Build the map
        /// </summary>
        public MapManager() 
        {
            mapTileTypes = new Dictionary<int, MapTileType>();
            usedValues = new List<int>();       
        }

        public void LoadContent(ResourceManager Resources)
        {
            resources = Resources;
        }

        public void Draw(Map map, Texture2D tileSheet, SpriteBatch spriteBatch)
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
                    //string earth = "" + enumMapTileType.Earth;

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
                    Texture2D texture = tileSheet;
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

        public void ClearMapTypes()
        {
            mapTileTypes.Clear();
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
