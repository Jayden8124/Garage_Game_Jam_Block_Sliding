using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;
using System;

namespace Dog_Sliding
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
        public Texture2D ButtonText { get; private set; }
        public Texture2D UIText { get; private set; }

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
            PlayRect = new Rectangle(540, 420, 200, 80);
            ExitRect = new Rectangle(540, 530, 200, 80);
            BackRect = new Rectangle(/* 540, 470, 200, 80 Change the size of button */);
        }

        public void LoadContent(ContentManager Content)
        {
            // Load Textures
            // MenuBackgroundText = Content.Load<Texture2D>("?");
            MainBackgroundText = Content.Load<Texture2D>("bg");
            IconBackgroundText = Content.Load<Texture2D>("logo");
            ButtonText = Content.Load<Texture2D>("button");
            UIText = Content.Load<Texture2D>("ui");

            // Dog Object Textures
            DogTextures = new Texture2D[5];
            DogTextures[1] = Content.Load<Texture2D>("Dog1");
            DogTextures[2] = Content.Load<Texture2D>("Dog2");
            DogTextures[3] = Content.Load<Texture2D>("Dog3");
            DogTextures[4] = Content.Load<Texture2D>("Dog4");

            // Font
            Font = Content.Load<SpriteFont>("GameFont");

            // Create a Rectangle Textures
            Singleton.Instance._Rect = new Texture2D(this.GraphicsDevice, Singleton._TILESIZE, Singleton._TILESIZE);

            Color[] data = new Color[Singleton._TILESIZE * Singleton._TILESIZE];
            for (int i = 0; i < data.Length; i++) data[i] = Color.White;
            Singleton.Instance._Rect.SetData(data);
        }

        public void _DrawGameStart(SpriteBatch _spriteBatch)
        {
            // Layer 1: Background
            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            // _spriteBatch.Draw(MenuBackgroundText, new Rectangle(0, 0, Singleton.SCREENWIDTH, Singleton.SCREENHEIGHT), Color.White); // Background

            _spriteBatch.End();

            // Layer 2: User Interface
            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            _spriteBatch.Draw(IconBackgroundText, new Rectangle(317, -36, 730, 487), new Rectangle(0, 0, 1536, 1024), Color.White); // Icon Background
            _spriteBatch.Draw(ButtonText, PlayRect, new Rectangle(221, 82, 640, 248), Color.White); // Play Button
            _spriteBatch.Draw(ButtonText, ExitRect, new Rectangle(220, 392, 641, 248), Color.White); // Exit Button

            _spriteBatch.End();
        }

        public void _DrawGamePlaying(SpriteBatch _spriteBatch, List<GameObject> _gameObjects, int _numOjects)
        {
            // Layer 1: Background
            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            _spriteBatch.Draw(MainBackgroundText, new Rectangle(0, 0, Singleton.SCREENWIDTH, Singleton.SCREENHEIGHT), Color.White); // Background

            _spriteBatch.End();


            // Layer 2: User Interface
            int offsetX = (Singleton.SCREENWIDTH - (Singleton.GAMEWIDTH * Singleton._TILESIZE)) / 2;
            int offsetY = (Singleton.SCREENHEIGHT - (Singleton.GAMEHEIGHT * Singleton._TILESIZE)) / 2;

            _spriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: Matrix.CreateTranslation(offsetX, offsetY, 0f));

            // Draw Board Tiles
            for (int y = 0; y < Singleton.GAMEHEIGHT; y++)
            {
                for (int x = 0; x < Singleton.GAMEWIDTH; x++)
                {
                    // คำนวณขนาดและตำแหน่งของเซลล์
                    Rectangle rect = new Rectangle(x * Singleton._TILESIZE, y * Singleton._TILESIZE, Singleton._TILESIZE, Singleton._TILESIZE);

                    // เลือกสีตามค่าใน GameBoard
                    int cell = Singleton.Instance.GameBoard[y, x];
                    Color fill = (cell == 0) ? Color.CornflowerBlue : Color.Red;

                    // วาดสี่เหลี่ยมทึบ
                    _spriteBatch.Draw(Singleton.Instance._Rect, rect, fill);

                    // วาดขอบ (ใช้เมธ็อดช่วยวาด outline ที่มีอยู่แล้ว)
                    DrawRectangleWithOutline(_spriteBatch, Singleton.Instance._Rect, rect, Color.Black, 1);
                }
            }

            for (int i = 0; i < _numOjects; i++)
            {
                _gameObjects[i].Draw(_spriteBatch);
            }

            _spriteBatch.End();
        }


        public void _DrawWaitingForSelect(SpriteBatch _spriteBatch, List<GameObject> _gameObjects, int _numOjects)
        {
            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            _spriteBatch.Draw(MainBackgroundText, new Rectangle(0, 0, Singleton.SCREENWIDTH, Singleton.SCREENHEIGHT), Color.White); // Background

            _spriteBatch.End();

            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            _spriteBatch.Draw(UIText, new Rectangle(70, 60, 200, 80), new Rectangle(81, 149, 910, 340), Color.White); 
            _spriteBatch.Draw(UIText, new Rectangle(70, 180, 200, 80), new Rectangle(81, 548, 910, 360), Color.White); 

            _spriteBatch.End();


            int offsetX = (Singleton.SCREENWIDTH - (Singleton.GAMEWIDTH * Singleton._TILESIZE)) / 2;
            int offsetY = (Singleton.SCREENHEIGHT - (Singleton.GAMEHEIGHT * Singleton._TILESIZE)) / 2;

            _spriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: Matrix.CreateTranslation(offsetX, offsetY, 0f));

            // Draw Board Tiles
            for (int y = 0; y < Singleton.GAMEHEIGHT; y++)
            {
                for (int x = 0; x < Singleton.GAMEWIDTH; x++)
                {
                    // คำนวณขนาดและตำแหน่งของเซลล์
                    Rectangle rect = new Rectangle(x * Singleton._TILESIZE, y * Singleton._TILESIZE, Singleton._TILESIZE, Singleton._TILESIZE);

                    // เลือกสีตามค่าใน GameBoard
                    int cell = Singleton.Instance.GameBoard[y, x];
                    Color fill = (cell == 0) ? Color.CornflowerBlue : Color.Red;

                    // วาดสี่เหลี่ยมทึบ
                    _spriteBatch.Draw(Singleton.Instance._Rect, rect, fill);

                    // วาดขอบ (ใช้เมธ็อดช่วยวาด outline ที่มีอยู่แล้ว)
                    DrawRectangleWithOutline(_spriteBatch, Singleton.Instance._Rect, rect, Color.Black, 1);
                }
            }

            foreach (Point p in Singleton.Instance.PossibleClicked)
            {
                _spriteBatch.Draw(Singleton.Instance._Rect, new Vector2(Singleton._TILESIZE * p.X, Singleton._TILESIZE * p.Y), null, Color.DarkGreen * 0.5f, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            }
            
            for (int i = 0; i < _numOjects; i++)
            {
                _gameObjects[i].Draw(_spriteBatch);
            }

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
            Block clickedBlock = Singleton.Instance.BlockMap[Singleton.Instance.SelectedTile.Y, Singleton.Instance.SelectedTile.X];
            int blockLength = clickedBlock.GetLength();
            // draw selected tile
            _spriteBatch.Draw(Singleton.Instance._Rect, new Vector2(Singleton._TILESIZE * Singleton.Instance.SelectedTile.X * blockLength, Singleton._TILESIZE * Singleton.Instance.SelectedTile.Y), null, Color.Blue, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);

            //draw possible move
            foreach (Point v in Singleton.Instance.PossibleClicked)
            {
                _spriteBatch.Draw(Singleton.Instance._Rect, new Vector2(Singleton._TILESIZE * v.X, Singleton._TILESIZE * v.Y), null, Color.LimeGreen, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
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

            // _spriteBatch.Draw(ButtonText, PlayRect, new Rectangle(/* ตำแหน่งของภาพ Sprite */), Color.White); // Resume Button
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

        public void DrawRectangleWithOutline(SpriteBatch spriteBatch, Texture2D pixel, Rectangle rect, Color outlineColor, int outlineThickness) // Use to draw outline of grid
        {
            spriteBatch.Draw(pixel, new Rectangle(rect.X, rect.Y, rect.Width, outlineThickness), outlineColor);
            spriteBatch.Draw(pixel, new Rectangle(rect.X, rect.Y + 5, outlineThickness, rect.Height), outlineColor);
            spriteBatch.Draw(pixel, new Rectangle(rect.X + rect.Width - outlineThickness, rect.Y + 5, outlineThickness, rect.Height), outlineColor);
        }
    }
}
