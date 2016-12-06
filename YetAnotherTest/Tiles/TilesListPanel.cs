using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace YetAnotherTest
{
    public class TilesListPanel
    {
        public static int margin = 4;
        static Rectangle rectangle;
        static int totalHeight;
        static int totalWidth;
        static int TilesInLine;
        public static int GetTile(Point mouse)
        {

            if (mouse.X - rectangle.X < 0)
                return -1;
            return (((mouse.X - rectangle.X) / totalWidth) + (mouse.Y / totalHeight) * TilesInLine);
        }

        static void Fill(SpriteBatch spriteBatch)
        {
            totalHeight = margin + Tile.TileHeight;
            totalWidth = margin + Tile.TileWidth;
            TilesInLine = rectangle.Width / totalWidth;
            int currentHeight = rectangle.Y + margin;
            int currentWidth = rectangle.X + margin;
              
            for (int i = 1; i <= Tile.Tiles.Count; i++)
            {
                Rectangle tileRect = new Rectangle(currentWidth, currentHeight, Tile.TileWidth, Tile.TileHeight);
                spriteBatch.Draw(Tile.Tiles[i-1], tileRect, Color.White);

                if (i % TilesInLine == 0)
                {
                    currentHeight += Tile.TileHeight + margin;
                    currentWidth = rectangle.X + margin;
                    continue;
                }
                currentWidth += Tile.TileWidth + margin;
            }
        }

        public static void Draw(SpriteBatch spriteBatch, GraphicsDevice graphics)
        {
            Texture2D pixel = new Texture2D(graphics, 1, 1);
            Color[] colorData = {  
                        Color.White,   
                    };
            pixel.SetData<Color>(colorData);

            rectangle = new Rectangle(Game1.ScreenWidth - Game1.ScreenWidth / 3, 0, Game1.ScreenWidth / 3, Game1.ScreenHeight);
            Color color = Color.Black;
            color.A = 50;
            spriteBatch.Draw(pixel, rectangle, color);
            Fill(spriteBatch);
        }
    }
}
