using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameArchitectureEngine
{
    enum TileCollisionType
    {
        Passable = 0,
        Impassable = 1,
        Platform = 2,
    }

    enum enumMapTileType
    {
        //Earth = 0,
        //Grass = 1,
        //Water = 2,
        //Mountain = 3,
        //PlayerStart = 4,
        //EnemyStart = 5,
        //Exit = 6,
        GrassTL = 0,
        GrassT = 1,
        GrassTR = 2,
        EarthTL = 3,
        EarthT = 4,
        EarthTR = 5,
        GrassL = 6,
        GrassM = 7,
        GrassR = 8,
        EarthL = 9,
        EarthM = 10,
        EarthR = 11,
        GrassBL = 12,
        GrassB = 13,
        GrassBR = 14,
        EarthBL = 15,
        EarthB = 16,
        EarthBR = 17,
    }

    class Tile
    {
        public Texture2D Texture;
        public TileCollisionType Collision;

        public const int Width = 40;
        public const int Height = 32;

        public static readonly Vector2 Size = new Vector2(Width, Height);

        public Tile(Texture2D texture, TileCollisionType collision)
        {
            Texture = texture;
            Collision = collision;
        }
    }
}
