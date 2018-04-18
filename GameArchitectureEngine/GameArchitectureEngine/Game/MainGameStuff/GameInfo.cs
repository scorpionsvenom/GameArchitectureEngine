using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace GameArchitectureEngine
{
    

    public class PlayerInfo
    {
        public Vector2 Position = Vector2.Zero;
        public int Health = 0;
    }

    public class EnemyInfo
    {
        public Vector2 Position = Vector2.Zero;
        public int Health = 0;
    }

    public class HealthPotionInfo
    {
        public Vector2 Position = Vector2.Zero;
    }

    /// <summary>
    /// Creates a singleton instance of gameinfo
    /// which is loaded from a save file
    /// </summary>
    public class GameInfo
    {
        private static GameInfo mInstance = null;

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

        public PlayerInfo PlayerInfo;
        public List<EnemyInfo> EnemyInfoArray;        
        public List<HealthPotionInfo> HealthPotionInfoArray;
    }
}
