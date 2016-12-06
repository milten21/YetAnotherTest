using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Timers;

namespace YetAnotherTest
{
    public class Character: Entity
    {
        public GraphicsDevice device;

        public enum MovingDirection
        {
            Down = 0,
            Left = 1,
            Right = 2,
            Up = 3
        }
        public enum CharacterState
        {
            Idle = 1,
            Moving = 0,
            Moving2 = 2
        }

        bool isMoving = false;
        public MovingDirection Direction;
        public CharacterState State;
        private int MovingFor = 0;

        public int CharacterWidth = 32;
        public int CharacterHeight = 32;
        public Rectangle CurrentFrame { get; set; }

        public Character(Texture2D texture)
        {
            this.texture = texture;
            this.Position = new Vector2(0, 0);
            Direction = MovingDirection.Down;
            State = CharacterState.Idle;
            CurrentFrame = GetSourceRectangle(Direction, State);
            Timer frameTick = new Timer(200);
            frameTick.Elapsed += frameTick_Elapsed;
            frameTick.Start();
        }

        private void frameTick_Elapsed(object sender, ElapsedEventArgs e)
        {
            State = State == CharacterState.Moving ? CharacterState.Moving2 : CharacterState.Moving;
        }

        private Rectangle GetSourceRectangle(MovingDirection direction, CharacterState state)
        {
            return new Rectangle((int)state * CharacterWidth, (int)direction * CharacterHeight, CharacterWidth, CharacterHeight);
        }
        
        public void HandleInput(KeyboardState state, GameTime time)
        {
            float speed = 0.1f;

            if (state.IsKeyDown(Keys.W))
            {
                Direction = MovingDirection.Up;
                isMoving = true;
                Move(new Vector2(0, -speed));
            }
            if (state.IsKeyDown(Keys.A))
            {
                Direction = MovingDirection.Left;
                isMoving = true;
                Move(new Vector2(-speed, 0));
            }
            if (state.IsKeyDown(Keys.S))
            {
                Direction = MovingDirection.Down;
                isMoving = true;
                Move(new Vector2(0, speed));
            }
            if (state.IsKeyDown(Keys.D))
            {
                Direction = MovingDirection.Right;
                isMoving = true;
                Move(new Vector2(speed, 0));
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Rectangle rec = GetSourceRectangle(Direction, State);
            spriteBatch.Draw(texture, Position * 32, null, rec);
        }

        public override void Update(GameTime time)
        {
            if (isMoving)
            {
                if (State == CharacterState.Idle)
                    State = CharacterState.Moving;
                CurrentFrame = GetSourceRectangle(Direction, State);
                MovingFor++;
                if (MovingFor == 10)
                {
                    isMoving = false;
                    MovingFor = 0;
                }

            }
            else
            {
                State = CharacterState.Idle;
            }
        }
    }
}
