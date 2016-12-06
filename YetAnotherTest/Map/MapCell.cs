using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace YetAnotherTest
{
    [Serializable]
    public class MapCell
    {
        public int tile;

        List<IMapObject> objects = new List<IMapObject>();

        public MapCell()
        {

        }
        public MapCell(int tile)
        {
            this.tile = tile;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
           // spriteBatch.Draw()
        }
    }
}
