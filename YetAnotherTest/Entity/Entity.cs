using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YetAnotherTest
{
    public class Entity : IEntity
    {
        public Vector2 Position;
        public Vector2 PreviousPosition;
        public Texture2D texture;
        public delegate void EntityMoved(Entity entity);
        public event EntityMoved Moved;

        public void Move(Vector2 moveVector)
        {
            PreviousPosition = Position;
            Position += moveVector;
            Moved(this);
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, new Vector2(Position.X * Tile.TileWidth, Position.Y * Tile.TileHeight));
        }

        public virtual void Update(GameTime time)
        {

        }
    }
}
