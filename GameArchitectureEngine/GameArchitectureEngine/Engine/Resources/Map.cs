using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameArchitectureEngine
{
    /// <summary>
    /// Assumes the map file is a grid of comma separated values.
    /// </summary>
    public class Map
    {
        private List<string[]> mapList;

        public List<string[]> MapList
        {
            get { return mapList; }
        }

        public Map(List<string[]> mapList)
        {
            this.mapList = mapList;
        }        
    }
}
