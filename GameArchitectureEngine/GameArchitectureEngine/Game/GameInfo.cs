using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameArchitectureEngine
{
    /// <summary>
    /// lab 02
    /// </summary>
    public class GameInfo
    {
        private static GameInfo mInstance = null;
        //public static long GameObjectID = 0;

        public static GameInfo Instance
        {
            get
            {
                if (mInstance == null)
                    mInstance = new GameInfo();
                return mInstance;
            }

            set { mInstance = value; }
        }

        //public Dictionary<long, GameObjectBase> gameObjectList;
        public GameObjectBase GameObjectBase;

        //private long getNextID()
        //{
        //    return ++GameObjectID;
        //}
    }
}
