using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using YetAnotherTest.Camera;

namespace YetAnotherTest
{

    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SingleKeyStroke singleStroke;

        Map myMap = Map.Load("Content/start.tmap");
        
        ICamera2D Cam;
        SpriteFont andy;

        public static int ScreenWidth;
        public static int ScreenHeight;

        bool debug = false;
        bool mapEditing = false;
        bool canPlaceTile = true;
        int currentTile = 0;
        public static bool shooting = false;

        Character hero;
        Fireball fireball;
        Texture2D ball;
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            this.IsMouseVisible = true;
            ScreenWidth = this.graphics.PreferredBackBufferWidth;
            ScreenHeight = this.graphics.PreferredBackBufferHeight;
            Cam = new Camera2D(new Vector2(ScreenWidth, ScreenHeight));
        }

        protected override void Initialize()
        {
            singleStroke = new SingleKeyStroke(this);
            singleStroke.KeyDown += (o, e) =>
            {
                switch (e.Key)
                {
                    case Keys.LeftControl: shooting = true; break;
                    case Keys.F2: mapEditing = !mapEditing; break;
                    case Keys.F3: debug = !debug; break;
                    case Keys.F5: myMap.SaveMap(); break;
                    case Keys.F6: System.Windows.Forms.OpenFileDialog dialog = new System.Windows.Forms.OpenFileDialog(); dialog.Filter = "Tile map|*.tmap";
                        dialog.ShowDialog(); myMap = Map.Load(dialog.FileName); break;
                }
            };
            Components.Add(singleStroke); 

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Tile.TileSetTexture = Content.Load<Texture2D>("Tiles");
            Tile.LoadTiles(GraphicsDevice);
            andy = Content.Load<SpriteFont>("Fonts/Andy");
            hero = new Character(Content.Load<Texture2D>("Character"));
            hero.device = GraphicsDevice;
            myMap.AddEntity(hero);
            ball = Content.Load<Texture2D>("Fireball");
        }

        protected override void UnloadContent()
        {
            Tile.TileSetTexture.Dispose();
            foreach (Texture2D tile in Tile.Tiles)
                tile.Dispose();
        }

        int previousScrollValue = 0;
        Vector2 MouseCoords;
        MouseState lastMouseState;
        MouseState currentMouseState;
        bool isPlacing = false;
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            
            KeyboardState state = Keyboard.GetState();
            int xMax = (int)(myMap.Width) - (ScreenWidth/2);
            int yMax = (int)(myMap.Height) - (ScreenHeight/2);

            hero.HandleInput(state, gameTime);            

            MouseState mouse = Mouse.GetState();
            if (mouse.ScrollWheelValue < previousScrollValue)
                Cam.Scale = MathHelper.Clamp(Cam.Scale - 0.08f, 0.5f, 1.2f);
            if (mouse.ScrollWheelValue > previousScrollValue)
                Cam.Scale = MathHelper.Clamp(Cam.Scale + 0.08f, 0.5f, 1.2f);
            if (mouse.MiddleButton == ButtonState.Pressed)
                Cam.Scale = 1f;


            MouseCoords = Vector2.Transform(new Vector2(mouse.X, mouse.Y), Matrix.Invert(Cam.GetCameraTransformation(GraphicsDevice)));
            
            previousScrollValue = mouse.ScrollWheelValue;

            lastMouseState = currentMouseState;

            currentMouseState = Mouse.GetState();
            
            if (lastMouseState.LeftButton == ButtonState.Released && currentMouseState.LeftButton == ButtonState.Pressed && mapEditing)
            {
                int tileID = TilesListPanel.GetTile(new Point(currentMouseState.X, currentMouseState.Y));
                if (tileID < Tile.Tiles.Count + 1 && tileID != -1)
                {
                    currentTile = tileID;
                    canPlaceTile = false;
                }
                if (canPlaceTile)
                    isPlacing = true;
                canPlaceTile = true;
            }

            try
            { 
                if (currentMouseState.LeftButton == ButtonState.Pressed && isPlacing && currentTile != 31)
                myMap.Rows[(int)MouseCoords.Y / 32].Columns[(int)MouseCoords.X / 32].tile = currentTile;
            }
            catch { }
            if (currentMouseState.LeftButton == ButtonState.Released && isPlacing)
                isPlacing = false;

            Cam.CenterOn(hero);
            myMap.Update(gameTime);
            base.Update(gameTime);
        }

        public void Fire(SpriteBatch spriteBatch)
        {
            fireball = new Fireball(ball);
            Vector2 ballPos = hero.Position * 32;
            fireball.Parent = hero;
            switch (hero.Direction)
            {
                case Character.MovingDirection.Down: fireball.Velocity = new Vector2(0, 2); ballPos.Y += 32; break;
                case Character.MovingDirection.Left: fireball.Velocity = new Vector2(-2, 0);  break;
                case Character.MovingDirection.Right: fireball.Velocity = new Vector2(2, 0); break;
                case Character.MovingDirection.Up: fireball.Velocity = new Vector2(0, -2); ballPos.X += 32; break;
            }
            
            fireball.Position = ballPos;
            myMap.AddEntity(fireball);
            //myMap.RemoveEntity(fireball);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin(SpriteSortMode.BackToFront,
                        BlendState.AlphaBlend,
                        null,
                        null,
                        null,
                        null,
                        Cam.GetCameraTransformation(GraphicsDevice));

            int x = (int)MouseCoords.X / 32;
            int y = (int)MouseCoords.Y / 32;

            for (int row = 0; row < myMap.Rows.Count; row++)
            {
                for (int cell = 0; cell < myMap.Rows[row].Columns.Count; cell++)
                {
                    var mapCell = myMap.Rows[row].Columns[cell];
                    spriteBatch.Draw(Tile.Tiles[mapCell.tile],
                        new Vector2((cell * Tile.TileWidth), (row * Tile.TileHeight)));

                    foreach (var entity in myMap.Entities)
                        entity.Draw(spriteBatch);

                    if (mapEditing && currentTile != 31)
                        spriteBatch.Draw(Tile.Tiles[currentTile], new Vector2(x, y) * 32, Color.White * 0.5f);
                    if (mapEditing && debug)
                        spriteBatch.Draw(Tile.Tiles[31], new Vector2(x, y) * 32);
                    if (shooting)
                    {
                        Fire(spriteBatch);
                        shooting = false;
                    }
                }
            }

            spriteBatch.End();

            // Map editing overlay                    
            if (mapEditing)
            {
                spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.LinearClamp, null, null);
                TilesListPanel.Draw(spriteBatch, graphics.GraphicsDevice);
                spriteBatch.End(); 
            }

            // Debug Overlay
            if (debug)
            {
                spriteBatch.Begin();
                spriteBatch.DrawString(andy, "Character Pos: " + hero.Position, new Vector2(10, 385), Color.Black);
                spriteBatch.DrawString(andy, "Mouse Tile Pos: " + MouseCoords / 32, new Vector2(10, 400), Color.Black);
                spriteBatch.DrawString(andy, "Camera Zoom: {" + Cam.Scale + " }", new Vector2(10, 415), Color.Black);
                spriteBatch.DrawString(andy, "Previous Mouse Scroll Value: {" + previousScrollValue + " }", new Vector2(10, 430), Color.Black);
                spriteBatch.DrawString(andy, "Screen resolution: { Height: " + ScreenHeight + ", Width: " + ScreenWidth + " }", new Vector2(10, 445), Color.Black);
                spriteBatch.DrawString(andy, "Current Camera Position: " + Cam.Min.ToString(), new Vector2(10, 460), Color.Black);
                spriteBatch.End();
            }
            base.Draw(gameTime);
        }
    }
}
