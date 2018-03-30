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
