using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YetAnotherTest
{
    public static class Tile
    {
        public static readonly int TileWidth = 32;
        public static readonly int TileHeight = 32;

        public static Texture2D TileSetTexture;
        public static readonly List<Texture2D> Tiles = new List<Texture2D>();
        public static Rectangle GetSourceRectangle(int tileIndex)
        {
            int tileY = tileIndex / (TileSetTexture.Width / TileWidth);
            int tileX = tileIndex % (TileSetTexture.Width / TileWidth);

            return new Rectangle(tileX * TileWidth, tileY * TileHeight, TileWidth, TileHeight);
        }

        public static void LoadTiles(GraphicsDevice device)
        {
            int framesCount = TileSetTexture.Width / TileWidth * TileSetTexture.Height / TileHeight;
 
            for (int i = 0; i < framesCount; i++)
            {
                Rectangle current = GetSourceRectangle(i);
                Texture2D frame = new Texture2D(device, current.Width, current.Height);
                Color[] data = new Color[current.Width * current.Height];
                TileSetTexture.GetData(0, current, data, 0, data.Length);
                frame.SetData(data);
                Tiles.Add(frame);
            }
            
        }
    }
}
