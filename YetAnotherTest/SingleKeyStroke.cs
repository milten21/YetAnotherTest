using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YetAnotherTest
{
    public class SingleKeyStroke : GameComponent
    {
        public event EventHandler<KeyDownEventArgs> KeyDown = delegate { };

        private int pressedKeys;
        public SingleKeyStroke(Game game) : base(game) { }

        public override void Update(GameTime gameTime)
        {
            var pressed = Keyboard.GetState().GetPressedKeys();
            var keys = 0;
            foreach (var key in pressed)
            {
                var num = 1 << (int)key;
                keys |= num;
                if ((pressedKeys & num) != num)
                {
                    KeyDown(this, new KeyDownEventArgs(key));
                }
            }
            pressedKeys = keys;
            base.Update(gameTime);
        }
    } 

    public class KeyDownEventArgs: EventArgs
    {
        public Keys Key;
        public KeyDownEventArgs(Keys key)
        {
            Key = key;
        }
        public KeyDownEventArgs(object sender, Keys key)
        {
            Key = key;
        }
    }
}
