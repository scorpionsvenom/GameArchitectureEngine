using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameArchitectureEngine
{
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
