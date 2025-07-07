using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Block_Sliding
{
    public class Drawing
    {
        // Graphics Device
        public GraphicsDevice GraphicsDevice { get; private set; }

        // Font
        public SpriteFont Font { get; private set; }

        // Textures
        public Texture2D MenuBackgroundText { get; private set; }
        public Texture2D MainBackgroundText { get; private set; }
        public Texture2D IconBackgroundText { get; private set; }
        public Texture2D PlayText { get; private set; }
        public Texture2D ExitText { get; private set; }
        public Texture2D BackText { get; private set; }

        // Textures Object
        public static Texture2D[] DogTextures { get; private set; } 

        // Button Rectangles
        public Rectangle PlayRect { get; private set; }
        public Rectangle ExitRect { get; private set; }
        public Rectangle BackRect { get; private set; }

        public Drawing(GraphicsDevice graphicsDevice)
        {
            GraphicsDevice = graphicsDevice;

            // Button Rectangles for Contain Mouse Destination
            PlayRect = new Rectangle(540, 320, 200, 80);
            ExitRect = new Rectangle(540, 470, 200, 80);
            BackRect = new Rectangle(/* 540, 470, 200, 80 Change the size of button */);
        }

        public void LoadContent(ContentManager Content)
        {
            // Load Textures
            // MenuBackgroundText = Content.Load<Texture2D>("?");
            // MainBackgroundText = Content.Load<Texture2D>("?");
            // IconBackgroundText = Content.Load<Texture2D>("?");
            // PlayText = Content.Load<Texture2D>("?");
            // ExitText = Content.Load<Texture2D>("?");
            // BackText = Content.Load<Texture2D>("?");

            DogTextures = new Texture2D[5];
            DogTextures[1] = Content.Load<Texture2D>("Dog1");
            DogTextures[2] = Content.Load<Texture2D>("Dog2");
            DogTextures[3] = Content.Load<Texture2D>("Dog3");
            DogTextures[4] = Content.Load<Texture2D>("Dog4");

            // Font
            Font = Content.Load<SpriteFont>("GameFont");

            // {   // Load Content
                Singleton.Instance._rect = new Texture2D(this.GraphicsDevice, 20, 20);
                Color[] data = new Color[20 * 20];
                for (int i = 0; i < data.Length; i++) data[i] = Color.White;
                Singleton.Instance._rect.SetData(data);
            // }
        }

        public void _DrawGameStart(SpriteBatch _spriteBatch)
        {
            // Layer 1: Background
            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            _spriteBatch.Draw(MenuBackgroundText, new Rectangle(0, 0, Singleton.SCREENWIDTH, Singleton.SCREENHEIGHT), Color.White); // Background

            _spriteBatch.End();

            // Layer 2: User Interface
            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            _spriteBatch.Draw(IconBackgroundText, new Rectangle(/* คำแหน่งของภาพ Sprite */), Color.White); // Icon Background
            _spriteBatch.Draw(PlayText, PlayRect, new Rectangle(/* ตำแหน่งของภาพ Sprite */), Color.White); // Play Button
            _spriteBatch.Draw(ExitText, ExitRect, new Rectangle(/* ตำแหน่งของภาพ Sprite */), Color.White); // Exit Button

            _spriteBatch.End();
        }

        public void _DrawGamePlaying(SpriteBatch _spriteBatch)
        {
            // Layer 1: Background
            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            // _spriteBatch.Draw(MainBackgroundText, new Rectangle(0, 0, Singleton.SCREENWIDTH, Singleton.SCREENHEIGHT), Color.White); // Background

            _spriteBatch.End();

            // Layer 2: User Interface
            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            // DrawGrid(_spriteBatch);
            _spriteBatch.DrawString(Font, Singleton.Instance.Score.ToString(), new Vector2(/* 1050, 55  Position of Score */), Color.White);
            _spriteBatch.DrawString(Font, (Singleton.Instance.Timer / 10000000).ToString(), new Vector2(/* 1050, 95 Position of Timer */), Color.White);

            _spriteBatch.End();
        }
        
        public void _DrawWaitingForSelect(SpriteBatch _spriteBatch)
        {
             // Layer 1: Background
            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            //_spriteBatch.Draw(MainBackgroundText, new Rectangle(0, 0, Singleton.SCREENWIDTH, Singleton.SCREENHEIGHT), Color.White); // Background

            _spriteBatch.End();

            // Layer 2: User Interface
            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            foreach (Point p in Singleton.Instance._possibleClicked)
            {
                _spriteBatch.Draw(Singleton.Instance._rect, new Vector2(Singleton._TILESIZE * p.X, Singleton._TILESIZE * p.Y), null, Color.DarkGreen, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            }

            // draw tiles
            

            _spriteBatch.End();
        }
        public void _DrawTileSelected(SpriteBatch _spriteBatch)
        {
            // Layer 1: Background
            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            // _spriteBatch.Draw(MainBackgroundText, new Rectangle(0, 0, Singleton.SCREENWIDTH, Singleton.SCREENHEIGHT), Color.White); // Background

            _spriteBatch.End();

            // Layer 2: User Interface
            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            // block
            Block clickedBlock = Singleton.Instance.BlockMap[Singleton.Instance._selectedTile.Y, Singleton.Instance._selectedTile.X];
            int blockLength = clickedBlock.GetLength();
            // draw selected tile
            _spriteBatch.Draw(Singleton.Instance._rect, new Vector2(Singleton._TILESIZE * Singleton.Instance._selectedTile.X * blockLength, Singleton._TILESIZE * Singleton.Instance._selectedTile.Y), null, Color.Blue, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);

            //draw possible move
            foreach (Point v in Singleton.Instance._possibleClicked)
            {
                _spriteBatch.Draw(Singleton.Instance._rect, new Vector2(Singleton._TILESIZE * v.X, Singleton._TILESIZE * v.Y), null, Color.LimeGreen, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            }

            // draw tiles

            _spriteBatch.End();
        }

        public void _DrawGamePaused(SpriteBatch _spriteBatch)
        {
            // Layer 1: Background
            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            _spriteBatch.Draw(MainBackgroundText, new Rectangle(0, 0, Singleton.SCREENWIDTH, Singleton.SCREENHEIGHT), Color.White); // Background

            _spriteBatch.End();

            // Layer 2: User Interface
            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            // _spriteBatch.Draw(PlayText, PlayRect, new Rectangle(/* ตำแหน่งของภาพ Sprite */), Color.White); // Resume Button
            // _spriteBatch.Draw(ExitText, ExitRect, new Rectangle(/* ตำแหน่งของภาพ Sprite */), Color.White); // Exit Button

            _spriteBatch.End();
        }

        public void _DrawGameOver(SpriteBatch _spriteBatch)
        {
            // Layer 1: Background
            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            _spriteBatch.Draw(MainBackgroundText, new Rectangle(0, 0, Singleton.SCREENWIDTH, Singleton.SCREENHEIGHT), Color.White); // Background

            _spriteBatch.End();

            // Layer 2: User Interface
            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            // _spriteBatch.Draw(ExitText, ExitRect, new Rectangle(/* ตำแหน่งของภาพ Sprite */), Color.White); // Back to Main Menu Button

            _spriteBatch.End();
        }

        public void Update(GameTime gameTime)
        {

        }

        // public void DrawGrid(SpriteBatch _spriteBatch) // รอ Texture มาวาด Grid
        // {
        //     for (int row = 0; row < Singleton.GAMEHEIGHT; row++)
        //     {
        //         for (int col = 0; col < Singleton.GAMEWIDTH; col++)
        //         {
        //             Rectangle _grid = new Rectangle(col * Singleton._TILESIZE, row * Singleton._TILESIZE, Singleton._TILESIZE, Singleton._TILESIZE);

        //             _spriteBatch.Draw(DogTextures[1], _grid, Color.White);
        //         }
        //     }
        // }

        public void DrawRectangleWithOutline(SpriteBatch spriteBatch, Texture2D pixel, Rectangle rect, Color outlineColor, int outlineThickness) // Use to draw outline of grid
        {
            spriteBatch.Draw(pixel, new Rectangle(rect.X, rect.Y, rect.Width, outlineThickness), outlineColor);
            spriteBatch.Draw(pixel, new Rectangle(rect.X, rect.Y + 5, outlineThickness, rect.Height), outlineColor);
            spriteBatch.Draw(pixel, new Rectangle(rect.X + rect.Width - outlineThickness, rect.Y + 5, outlineThickness, rect.Height), outlineColor);
        }
    }
}
