using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YetAnotherTest
{
    public class Fireball: Entity, IDaughterEntity
    {
        public Vector2 Velocity;

        public Entity Parent { get;set; }

        public Fireball(Texture2D texture)
        {
            this.texture = texture;
        }

        public override void Update(GameTime time)
        {
            Position += Velocity;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            
            Vector2 Down = new Vector2(0, 2);
            Vector2 Left =  new Vector2(-2, 0);
            Vector2 Right = new Vector2(2, 0); 
            Vector2 Top = new Vector2(0, -2);

            if (Velocity == Down)
                spriteBatch.Draw(texture, new Rectangle((int)Position.X, (int)Position.Y, 32, 32), null, Color.White, (float)Math.PI / 2, new Vector2(texture.Width, texture.Height), SpriteEffects.None, 0f);
            if (Velocity == Left)
                spriteBatch.Draw(texture, new Rectangle((int)Position.X, (int)Position.Y, 32, 32), null, Color.White, (float)Math.PI, new Vector2(texture.Width, texture.Height), SpriteEffects.None, 0f);
            if (Velocity == Right)
                spriteBatch.Draw(texture, new Rectangle((int)Position.X, (int)Position.Y, 32, 32), Color.White);
            if (Velocity == Top)
                spriteBatch.Draw(texture, new Rectangle((int)Position.X, (int)Position.Y, 32, 32), null, Color.White, (float)-Math.PI/2, new Vector2(texture.Width, texture.Height), SpriteEffects.None, 0f);
        }
    }
}
