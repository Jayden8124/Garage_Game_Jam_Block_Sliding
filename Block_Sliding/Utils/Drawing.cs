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
        public Texture2D ButtonUp { get; private set; }
        public Texture2D GameOver { get; private set; }
        public Texture2D Pause { get; private set; }
        public Texture2D Setting { get; private set; }
        public Texture2D Volume { get; private set; }
        public Texture2D freezeSkill { get; private set; }
        public Texture2D howToPlay { get; private set; }

        // Textures Object
        public static Texture2D[] DogTextures { get; private set; }
        public static readonly Dictionary<Block.BlockType, Rectangle> DogSourceRects = new Dictionary<Block.BlockType, Rectangle>();


        // Button Rectangles
        public Rectangle PlayRect { get; private set; }
        public Rectangle ExitRect { get; private set; }
        public Rectangle ButtonUpRect { get; private set; }
        public Rectangle SettingRect { get; private set; }
        public Rectangle FreezeSkillRect { get; private set; }
        public Rectangle VolumeRect { get; private set; }
        public Rectangle HowToPlayRect { get; private set; }

        public Drawing(GraphicsDevice graphicsDevice)
        {
            GraphicsDevice = graphicsDevice;

            // Button Rectangles for Contain Mouse Destination
            PlayRect = new Rectangle(540, 420, 200, 80);
            ExitRect = new Rectangle(540, 530, 200, 80);
            ButtonUpRect = new Rectangle(962, 500, 200, 140);
            SettingRect = new Rectangle(1171, 60, 60, 60);
            VolumeRect = new Rectangle(1101, 60, 60, 60);
            FreezeSkillRect = new Rectangle(962, 350, 200, 140);
            HowToPlayRect = new Rectangle(1171, 60, 60, 60);

            DogSourceRects[Block.BlockType.One] = new Rectangle(183, 267, 657, 485);
            DogSourceRects[Block.BlockType.Two] = new Rectangle(128, 201, 1164, 553);
            DogSourceRects[Block.BlockType.Three] = new Rectangle(110, 121, 1288, 723);
            DogSourceRects[Block.BlockType.Four] = new Rectangle(97, 172, 918, 349);
            DogSourceRects[Block.BlockType.Rock] = new Rectangle(6, 10, 50, 43);
        }

        public void LoadContent(ContentManager Content)
        {
            // Load Textures
            MenuBackgroundText = Content.Load<Texture2D>("12");
            MainBackgroundText = Content.Load<Texture2D>("bg");
            IconBackgroundText = Content.Load<Texture2D>("logo");
            ButtonText = Content.Load<Texture2D>("button");
            UIText = Content.Load<Texture2D>("ui");
            ButtonUp = Content.Load<Texture2D>("button_up");
            GameOver = Content.Load<Texture2D>("game_over");
            Pause = Content.Load<Texture2D>("pause");
            Setting = Content.Load<Texture2D>("setting");
            freezeSkill = Content.Load<Texture2D>("freeze");
            Volume = Content.Load<Texture2D>("volume");
            howToPlay = Content.Load<Texture2D>("how_to_play");

            // Dog Object Textures
            DogTextures = new Texture2D[6];
            DogTextures[1] = Content.Load<Texture2D>("dog1");
            DogTextures[2] = Content.Load<Texture2D>("dog2");
            DogTextures[3] = Content.Load<Texture2D>("dog3");
            DogTextures[4] = Content.Load<Texture2D>("dog4");
            DogTextures[5] = Content.Load<Texture2D>("rock");

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

            _spriteBatch.Draw(MenuBackgroundText, new Rectangle(0, 0, Singleton.SCREENWIDTH, Singleton.SCREENHEIGHT), Color.White * 0.65f); // Background

            _spriteBatch.End();

            // Layer 2: User Interface
            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            _spriteBatch.Draw(IconBackgroundText, new Rectangle(317, -36, 730, 487), new Rectangle(0, 0, 1536, 1024), Color.White); // Icon Background
            _spriteBatch.Draw(ButtonText, PlayRect, new Rectangle(221, 82, 640, 248), Color.White); // Play Button
            _spriteBatch.Draw(ButtonText, ExitRect, new Rectangle(220, 392, 641, 248), Color.White); // Exit Button
            _spriteBatch.Draw(howToPlay, HowToPlayRect, new Rectangle(167, 401, 690, 680), Color.White); // How to Play Button

            _spriteBatch.End();
        }

        public void _DrawGamePlaying(SpriteBatch _spriteBatch, List<GameObject> _gameObjects, int _numOjects)
        {
            // Layer 1: Background
            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            _spriteBatch.Draw(MainBackgroundText, new Rectangle(0, 0, Singleton.SCREENWIDTH, Singleton.SCREENHEIGHT), Color.White); // Background

            _spriteBatch.End();

            // Layer 2: User Interface
            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            _spriteBatch.Draw(UIText, new Rectangle(70, 60, 200, 80), new Rectangle(81, 149, 910, 340), Color.White);
            _spriteBatch.Draw(UIText, new Rectangle(70, 180, 200, 80), new Rectangle(81, 548, 910, 360), Color.White);

            _spriteBatch.Draw(ButtonUp, ButtonUpRect, new Rectangle(147, 219, 787, 636), Color.White);
            _spriteBatch.Draw(Setting, SettingRect, new Rectangle(141, 165, 438, 382), Color.White);
            if (!Singleton.Instance.AudioManager.IsMuted())
                _spriteBatch.Draw(Volume, VolumeRect, new Rectangle(82, 158, 432, 405), Color.White);
            else
                _spriteBatch.Draw(Volume, VolumeRect, new Rectangle(566, 158, 432, 405), Color.White);
            _spriteBatch.Draw(freezeSkill, FreezeSkillRect, new Rectangle(142, 192, 745, 632), Color.White);

            _spriteBatch.End();

            // Layer 3: Text
            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            string timeText = String.Format($"{(int)Singleton.Instance.Timer % 60:00}");
            Vector2 timeSize = Font.MeasureString(timeText);
            _spriteBatch.DrawString(Font, timeText, new Vector2(210 - timeSize.X * 0.5f, 100 - timeSize.Y * 0.5f), Color.Black);

            string scoreText = Singleton.Instance.Score.ToString();
            Vector2 textSize = Font.MeasureString(scoreText);
            _spriteBatch.DrawString(Font, scoreText, new Vector2(215 - textSize.X * 0.5f, 220 - textSize.Y * 0.5f), Color.Black);

            _spriteBatch.End();

            // Layer 4: Game Board
            int offsetX = (Singleton.SCREENWIDTH - (Singleton.GAMEWIDTH * Singleton._TILESIZE)) / 2;
            int offsetY = (Singleton.SCREENHEIGHT - (Singleton.GAMEHEIGHT * Singleton._TILESIZE)) / 2;

            _spriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: Matrix.CreateTranslation(offsetX, offsetY, 0f));

            // Draw Board Tiles
            for (int y = 0; y < Singleton.GAMEHEIGHT; y++)
            {
                for (int x = 0; x < Singleton.GAMEWIDTH; x++)
                {
                    Rectangle rect = new Rectangle(x * Singleton._TILESIZE, y * Singleton._TILESIZE, Singleton._TILESIZE, Singleton._TILESIZE);

                    // ใช้ลายหมากรุก: คาราเมลอ่อน-เข้ม
                    Color lightCaramel = new Color(248, 209, 146); // คาราเมลอ่อน
                    Color darkCaramel = new Color(124, 75, 24);    // คาราเมลเข้ม

                    Color baseColor = ((x + y) % 2 == 0) ? lightCaramel : darkCaramel;

                    int cell = Singleton.Instance.GameBoard[y, x];

                    // ถ้าช่องมีบล็อก อาจใส่สีอีกแบบให้เด่นขึ้น
                    Color fill = (cell == 0) ? baseColor : baseColor;

                    if (y == Singleton.GAMEHEIGHT - 1) fill = darkCaramel;
                    _spriteBatch.Draw(Singleton.Instance._Rect, rect, fill);
                }
            }

            foreach (Point p in Singleton.Instance.PossibleClicked)
            {
                _spriteBatch.Draw(Singleton.Instance._Rect, new Vector2(Singleton._TILESIZE * p.X, Singleton._TILESIZE * p.Y), null, Color.Gold * 0.5f, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            }

            for (int i = 0; i < _numOjects; i++)
            {
                _gameObjects[i].Draw(_spriteBatch);
            }

            DrawRectangleWithOutline(_spriteBatch, Singleton.Instance._Rect, new Rectangle(0, 0, 540, 600), Color.SaddleBrown, 5);
            DrawRectangleWithOutline(_spriteBatch, Singleton.Instance._Rect, new Rectangle(0, 540, 540, 60), Color.Black, 5);

            _spriteBatch.End();
        }


        public void _DrawWaitingForSelect(SpriteBatch _spriteBatch, List<GameObject> _gameObjects, int _numOjects)
        {
            // Layer 1: Background
            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            _spriteBatch.Draw(MainBackgroundText, new Rectangle(0, 0, Singleton.SCREENWIDTH, Singleton.SCREENHEIGHT), Color.White); // Background

            _spriteBatch.End();

            // Layer 2: User Interface
            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            _spriteBatch.Draw(UIText, new Rectangle(70, 60, 200, 80), new Rectangle(81, 149, 910, 340), Color.White);
            _spriteBatch.Draw(UIText, new Rectangle(70, 180, 200, 80), new Rectangle(81, 548, 910, 360), Color.White);

            _spriteBatch.Draw(ButtonUp, ButtonUpRect, new Rectangle(147, 219, 787, 636), Color.White);
            _spriteBatch.Draw(Setting, SettingRect, new Rectangle(141, 165, 438, 382), Color.White);
            if (!Singleton.Instance.AudioManager.IsMuted())
                _spriteBatch.Draw(Volume, VolumeRect, new Rectangle(82, 158, 432, 405), Color.White);
            else
                _spriteBatch.Draw(Volume, VolumeRect, new Rectangle(566, 158, 432, 405), Color.White);

            _spriteBatch.Draw(freezeSkill, FreezeSkillRect, new Rectangle(142, 192, 745, 632), Color.White);
            _spriteBatch.End();

            // Layer 3: Text
            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            string timeText = String.Format($"{(int)Singleton.Instance.Timer % 60:0}");
            Vector2 timeSize = Font.MeasureString(timeText);
            _spriteBatch.DrawString(Font, timeText, new Vector2(210 - timeSize.X * 0.5f, 100 - timeSize.Y * 0.5f), Color.Black);

            string scoreText = Singleton.Instance.Score.ToString();
            Vector2 textSize = Font.MeasureString(scoreText);
            _spriteBatch.DrawString(Font, scoreText, new Vector2(215 - textSize.X * 0.5f, 220 - textSize.Y * 0.5f), Color.Black);

            _spriteBatch.End();

            // Layer 4: Game Board
            int offsetX = (Singleton.SCREENWIDTH - (Singleton.GAMEWIDTH * Singleton._TILESIZE)) / 2;
            int offsetY = (Singleton.SCREENHEIGHT - (Singleton.GAMEHEIGHT * Singleton._TILESIZE)) / 2;

            _spriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: Matrix.CreateTranslation(offsetX, offsetY, 0f));
            // Draw Board Tiles
            for (int y = 0; y < Singleton.GAMEHEIGHT; y++)
            {
                for (int x = 0; x < Singleton.GAMEWIDTH; x++)
                {
                    Rectangle rect = new Rectangle(x * Singleton._TILESIZE, y * Singleton._TILESIZE, Singleton._TILESIZE, Singleton._TILESIZE);

                    // ใช้ลายหมากรุก: คาราเมลอ่อน-เข้ม
                    Color lightCaramel = new Color(248, 209, 146); // คาราเมลอ่อน
                    Color darkCaramel = new Color(124, 75, 24);    // คาราเมลเข้ม

                    Color baseColor = ((x + y) % 2 == 0) ? lightCaramel : darkCaramel;

                    int cell = Singleton.Instance.GameBoard[y, x];

                    // ถ้าช่องมีบล็อก อาจใส่สีอีกแบบให้เด่นขึ้น
                    Color fill = (cell == 0) ? baseColor : baseColor;

                    if (y == Singleton.GAMEHEIGHT - 1) fill = darkCaramel;

                    _spriteBatch.Draw(Singleton.Instance._Rect, rect, fill);
                }
            }

            _spriteBatch.End();

            _spriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: Matrix.CreateTranslation(offsetX, offsetY, 0f));

            foreach (Point p in Singleton.Instance.PossibleClicked)
            {
                _spriteBatch.Draw(Singleton.Instance._Rect, new Vector2(Singleton._TILESIZE * p.X, Singleton._TILESIZE * p.Y), null, Color.Gold * 0.5f, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            }

            for (int i = 0; i < _numOjects; i++)
            {
                _gameObjects[i].Draw(_spriteBatch);
            }

            DrawRectangleWithOutline(_spriteBatch, Singleton.Instance._Rect, new Rectangle(0, 0, 540, 600), Color.SaddleBrown, 5);
            DrawRectangleWithOutline(_spriteBatch, Singleton.Instance._Rect, new Rectangle(0, 540, 540, 60), Color.Black, 5);

            _spriteBatch.End();
        }
        public void _DrawTileSelected(SpriteBatch _spriteBatch, List<GameObject> _gameObjects, int _numOjects)
        {
            // Layer 1: Background
            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            _spriteBatch.Draw(MainBackgroundText, new Rectangle(0, 0, Singleton.SCREENWIDTH, Singleton.SCREENHEIGHT), Color.White); // Background

            _spriteBatch.End();

            // Layer 2: User Interface
            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            _spriteBatch.Draw(UIText, new Rectangle(70, 60, 200, 80), new Rectangle(81, 149, 910, 340), Color.White);
            _spriteBatch.Draw(UIText, new Rectangle(70, 180, 200, 80), new Rectangle(81, 548, 910, 360), Color.White);

            _spriteBatch.Draw(ButtonUp, ButtonUpRect, new Rectangle(147, 219, 787, 636), Color.White);
            _spriteBatch.Draw(Setting, SettingRect, new Rectangle(141, 165, 438, 382), Color.White);
            if (!Singleton.Instance.AudioManager.IsMuted())
                _spriteBatch.Draw(Volume, VolumeRect, new Rectangle(82, 158, 432, 405), Color.White);
            else
                _spriteBatch.Draw(Volume, VolumeRect, new Rectangle(566, 158, 432, 405), Color.White);
            _spriteBatch.Draw(freezeSkill, FreezeSkillRect, new Rectangle(142, 192, 745, 632), Color.White);

            _spriteBatch.End();

            // Layer 3: Text
            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            string timeText = String.Format($"{(int)Singleton.Instance.Timer % 60:00}");
            Vector2 timeSize = Font.MeasureString(timeText);
            _spriteBatch.DrawString(Font, timeText, new Vector2(210 - timeSize.X * 0.5f, 100 - timeSize.Y * 0.5f), Color.Black);

            string scoreText = Singleton.Instance.Score.ToString();
            Vector2 textSize = Font.MeasureString(scoreText);
            _spriteBatch.DrawString(Font, scoreText, new Vector2(215 - textSize.X * 0.5f, 220 - textSize.Y * 0.5f), Color.Black);

            _spriteBatch.End();

            // Layer 4: Game Board
            int offsetX = (Singleton.SCREENWIDTH - (Singleton.GAMEWIDTH * Singleton._TILESIZE)) / 2;
            int offsetY = (Singleton.SCREENHEIGHT - (Singleton.GAMEHEIGHT * Singleton._TILESIZE)) / 2;

            _spriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: Matrix.CreateTranslation(offsetX, offsetY, 0f));
            // Draw Board Tiles
            for (int y = 0; y < Singleton.GAMEHEIGHT; y++)
            {
                for (int x = 0; x < Singleton.GAMEWIDTH; x++)
                {
                    Rectangle rect = new Rectangle(x * Singleton._TILESIZE, y * Singleton._TILESIZE, Singleton._TILESIZE, Singleton._TILESIZE);

                    // ใช้ลายหมากรุก: คาราเมลอ่อน-เข้ม
                    Color lightCaramel = new Color(248, 209, 146); // คาราเมลอ่อน
                    Color darkCaramel = new Color(124, 75, 24);    // คาราเมลเข้ม

                    Color baseColor = ((x + y) % 2 == 0) ? lightCaramel : darkCaramel;

                    int cell = Singleton.Instance.GameBoard[y, x];

                    // ถ้าช่องมีบล็อก อาจใส่สีอีกแบบให้เด่นขึ้น
                    Color fill = (cell == 0) ? baseColor : baseColor;
                    if (y == Singleton.GAMEHEIGHT - 1) fill = darkCaramel;

                    _spriteBatch.Draw(Singleton.Instance._Rect, rect, fill);
                }
            }

            foreach (Point p in Singleton.Instance.PossibleClicked)
            {
                _spriteBatch.Draw(Singleton.Instance._Rect, new Vector2(Singleton._TILESIZE * p.X, Singleton._TILESIZE * p.Y), null, Color.LawnGreen * 0.7f, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            }

            Block clickedBlock = Singleton.Instance.BlockMap[Singleton.Instance.SelectedTile.Y, Singleton.Instance.SelectedTile.X];
            int blockLength = clickedBlock.GetLength();
            int row = Singleton.Instance.SelectedTile.Y;

            // หาหัวจริงของ block
            int headX = Singleton.Instance.SelectedTile.X;
            while (headX > 0 && Singleton.Instance.BlockMap[row, headX - 1] == clickedBlock)
                headX--;

            // วาดทั้ง block เป็นสีแดง
            for (int i = 0; i < blockLength; i++)
            {
                int drawX = headX + i;
                _spriteBatch.Draw(
                    Singleton.Instance._Rect,
                    new Vector2(Singleton._TILESIZE * drawX, Singleton._TILESIZE * row),
                    null,
                    Color.SandyBrown,
                    0f,
                    Vector2.Zero,
                    1f,
                    SpriteEffects.None,
                    0f
                );
            }

            for (int i = 0; i < _numOjects; i++)
            {
                _gameObjects[i].Draw(_spriteBatch);
            }

            DrawRectangleWithOutline(_spriteBatch, Singleton.Instance._Rect, new Rectangle(0, 0, 540, 600), Color.SaddleBrown, 5);
            DrawRectangleWithOutline(_spriteBatch, Singleton.Instance._Rect, new Rectangle(0, 540, 540, 60), Color.Black, 5);


            _spriteBatch.End();
        }

        public void _DrawGameTurnEnded(SpriteBatch _spriteBatch, List<GameObject> _gameObjects, int _numOjects)
        {
            // Layer 1: Background
            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            _spriteBatch.Draw(MainBackgroundText, new Rectangle(0, 0, Singleton.SCREENWIDTH, Singleton.SCREENHEIGHT), Color.White); // Background

            _spriteBatch.End();

            // Layer 2: User Interface
            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            _spriteBatch.Draw(UIText, new Rectangle(70, 60, 200, 80), new Rectangle(81, 149, 910, 340), Color.White);
            _spriteBatch.Draw(UIText, new Rectangle(70, 180, 200, 80), new Rectangle(81, 548, 910, 360), Color.White);

            _spriteBatch.Draw(ButtonUp, ButtonUpRect, new Rectangle(147, 219, 787, 636), Color.White);
            _spriteBatch.Draw(Setting, SettingRect, new Rectangle(141, 165, 438, 382), Color.White);
            if (!Singleton.Instance.AudioManager.IsMuted())
                _spriteBatch.Draw(Volume, VolumeRect, new Rectangle(82, 158, 432, 405), Color.White);
            else
                _spriteBatch.Draw(Volume, VolumeRect, new Rectangle(566, 158, 432, 405), Color.White);
            _spriteBatch.Draw(freezeSkill, FreezeSkillRect, new Rectangle(142, 192, 745, 632), Color.White);

            _spriteBatch.End();

            // Layer 3: Text
            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            string timeText = String.Format($"{(int)Singleton.Instance.Timer % 60:00}");
            Vector2 timeSize = Font.MeasureString(timeText);
            _spriteBatch.DrawString(Font, timeText, new Vector2(210 - timeSize.X * 0.5f, 100 - timeSize.Y * 0.5f), Color.Black);

            string scoreText = Singleton.Instance.Score.ToString();
            Vector2 textSize = Font.MeasureString(scoreText);
            _spriteBatch.DrawString(Font, scoreText, new Vector2(215 - textSize.X * 0.5f, 220 - textSize.Y * 0.5f), Color.Black);

            _spriteBatch.End();

            // Layer 4: Game Board
            int offsetX = (Singleton.SCREENWIDTH - (Singleton.GAMEWIDTH * Singleton._TILESIZE)) / 2;
            int offsetY = (Singleton.SCREENHEIGHT - (Singleton.GAMEHEIGHT * Singleton._TILESIZE)) / 2;

            _spriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: Matrix.CreateTranslation(offsetX, offsetY, 0f));
            // Draw Board Tiles
            for (int y = 0; y < Singleton.GAMEHEIGHT; y++)
            {
                for (int x = 0; x < Singleton.GAMEWIDTH; x++)
                {
                    Rectangle rect = new Rectangle(x * Singleton._TILESIZE, y * Singleton._TILESIZE, Singleton._TILESIZE, Singleton._TILESIZE);

                    // ใช้ลายหมากรุก: คาราเมลอ่อน-เข้ม
                    Color lightCaramel = new Color(248, 209, 146); // คาราเมลอ่อน
                    Color darkCaramel = new Color(124, 75, 24);    // คาราเมลเข้ม

                    Color baseColor = ((x + y) % 2 == 0) ? lightCaramel : darkCaramel;

                    int cell = Singleton.Instance.GameBoard[y, x];

                    // ถ้าช่องมีบล็อก อาจใส่สีอีกแบบให้เด่นขึ้น
                    Color fill = (cell == 0) ? baseColor : baseColor;
                    if (y == Singleton.GAMEHEIGHT - 1) fill = darkCaramel;

                    _spriteBatch.Draw(Singleton.Instance._Rect, rect, fill);
                }
            }
            foreach (Point p in Singleton.Instance.PossibleClicked)
            {
                _spriteBatch.Draw(Singleton.Instance._Rect, new Vector2(Singleton._TILESIZE * p.X, Singleton._TILESIZE * p.Y), null, Color.Gold * 0.5f, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            }

            for (int i = 0; i < _numOjects; i++)
            {
                _gameObjects[i].Draw(_spriteBatch);
            }
            DrawRectangleWithOutline(_spriteBatch, Singleton.Instance._Rect, new Rectangle(0, 0, 540, 600), Color.SaddleBrown, 5);
            DrawRectangleWithOutline(_spriteBatch, Singleton.Instance._Rect, new Rectangle(0, 540, 540, 60), Color.Black, 5);


            _spriteBatch.End();
        }
        public void _DrawGamePaused(SpriteBatch _spriteBatch, List<GameObject> _gameObjects, int _numOjects)
        {
            // Layer 1: Background
            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            _spriteBatch.Draw(MainBackgroundText, new Rectangle(0, 0, Singleton.SCREENWIDTH, Singleton.SCREENHEIGHT), Color.White); // Background

            _spriteBatch.End();

            // Layer 2: User Interface
            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            _spriteBatch.Draw(UIText, new Rectangle(70, 60, 200, 80), new Rectangle(81, 149, 910, 340), Color.White);
            _spriteBatch.Draw(UIText, new Rectangle(70, 180, 200, 80), new Rectangle(81, 548, 910, 360), Color.White);

            _spriteBatch.Draw(ButtonUp, ButtonUpRect, new Rectangle(147, 219, 787, 636), Color.White);
            _spriteBatch.Draw(Setting, SettingRect, new Rectangle(141, 165, 438, 382), Color.White);
            if (!Singleton.Instance.AudioManager.IsMuted())
                _spriteBatch.Draw(Volume, VolumeRect, new Rectangle(82, 158, 432, 405), Color.White);
            else
                _spriteBatch.Draw(Volume, VolumeRect, new Rectangle(566, 158, 432, 405), Color.White);
            _spriteBatch.Draw(freezeSkill, FreezeSkillRect, new Rectangle(142, 192, 745, 632), Color.White);

            _spriteBatch.End();

            // Layer 3: Text
            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            string timeText = String.Format($"{(int)Singleton.Instance.Timer % 60:00}");
            Vector2 timeSize = Font.MeasureString(timeText);
            _spriteBatch.DrawString(Font, timeText, new Vector2(210 - timeSize.X * 0.5f, 100 - timeSize.Y * 0.5f), Color.Black);

            string scoreText = Singleton.Instance.Score.ToString();
            Vector2 textSize = Font.MeasureString(scoreText);
            _spriteBatch.DrawString(Font, scoreText, new Vector2(215 - textSize.X * 0.5f, 220 - textSize.Y * 0.5f), Color.Black);

            _spriteBatch.End();

            // Layer 4: Game Board
            int offsetX = (Singleton.SCREENWIDTH - (Singleton.GAMEWIDTH * Singleton._TILESIZE)) / 2;
            int offsetY = (Singleton.SCREENHEIGHT - (Singleton.GAMEHEIGHT * Singleton._TILESIZE)) / 2;

            _spriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: Matrix.CreateTranslation(offsetX, offsetY, 0f));
            // Draw Board Tiles
            for (int y = 0; y < Singleton.GAMEHEIGHT; y++)
            {
                for (int x = 0; x < Singleton.GAMEWIDTH; x++)
                {
                    Rectangle rect = new Rectangle(x * Singleton._TILESIZE, y * Singleton._TILESIZE, Singleton._TILESIZE, Singleton._TILESIZE);

                    // ใช้ลายหมากรุก: คาราเมลอ่อน-เข้ม
                    Color lightCaramel = new Color(248, 209, 146); // คาราเมลอ่อน
                    Color darkCaramel = new Color(124, 75, 24);    // คาราเมลเข้ม

                    Color baseColor = ((x + y) % 2 == 0) ? lightCaramel : darkCaramel;

                    int cell = Singleton.Instance.GameBoard[y, x];

                    // ถ้าช่องมีบล็อก อาจใส่สีอีกแบบให้เด่นขึ้น
                    Color fill = (cell == 0) ? baseColor : baseColor;

                    if (y == Singleton.GAMEHEIGHT - 1) fill = darkCaramel;

                    _spriteBatch.Draw(Singleton.Instance._Rect, rect, fill);
                }
            }

            foreach (Point p in Singleton.Instance.PossibleClicked)
            {
                _spriteBatch.Draw(Singleton.Instance._Rect, new Vector2(Singleton._TILESIZE * p.X, Singleton._TILESIZE * p.Y), null, Color.Gold * 0.5f, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            }

            for (int i = 0; i < _numOjects; i++)
            {
                _gameObjects[i].Draw(_spriteBatch);
            }

            DrawRectangleWithOutline(_spriteBatch, Singleton.Instance._Rect, new Rectangle(0, 0, 540, 600), Color.SaddleBrown, 5);
            DrawRectangleWithOutline(_spriteBatch, Singleton.Instance._Rect, new Rectangle(0, 540, 540, 60), Color.Black, 5);

            _spriteBatch.Draw(Pause, new Rectangle(Singleton._TILESIZE * (Singleton.GAMEWIDTH / 2) - 100, Singleton._TILESIZE * (Singleton.GAMEHEIGHT / 2) - 110, 260, 100), new Rectangle(32, 325, 960, 330), Color.White);

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

            _spriteBatch.Draw(UIText, new Rectangle(70, 60, 200, 80), new Rectangle(81, 149, 910, 340), Color.White);
            _spriteBatch.Draw(UIText, new Rectangle(70, 180, 200, 80), new Rectangle(81, 548, 910, 360), Color.White);

            _spriteBatch.Draw(ButtonUp, ButtonUpRect, new Rectangle(147, 219, 787, 636), Color.White);
            _spriteBatch.Draw(Setting, SettingRect, new Rectangle(141, 165, 438, 382), Color.White);
            if (!Singleton.Instance.AudioManager.IsMuted())
                _spriteBatch.Draw(Volume, VolumeRect, new Rectangle(82, 158, 432, 405), Color.White);
            else
                _spriteBatch.Draw(Volume, VolumeRect, new Rectangle(566, 158, 432, 405), Color.White);

            _spriteBatch.End();

            // Layer 3: Text
            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            string timeText = String.Format($"{(int)Singleton.Instance.Timer % 60:00}");
            Vector2 timeSize = Font.MeasureString(timeText);
            _spriteBatch.DrawString(Font, timeText, new Vector2(210 - timeSize.X * 0.5f, 100 - timeSize.Y * 0.5f), Color.Black);

            string scoreText = Singleton.Instance.Score.ToString();
            Vector2 textSize = Font.MeasureString(scoreText);
            _spriteBatch.DrawString(Font, scoreText, new Vector2(215 - textSize.X * 0.5f, 220 - textSize.Y * 0.5f), Color.Black);

            _spriteBatch.End();

            // Layer 4: Game Board
            int offsetX = (Singleton.SCREENWIDTH - (Singleton.GAMEWIDTH * Singleton._TILESIZE)) / 2;
            int offsetY = (Singleton.SCREENHEIGHT - (Singleton.GAMEHEIGHT * Singleton._TILESIZE)) / 2;

            _spriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: Matrix.CreateTranslation(offsetX, offsetY, 0f));
            // Draw Board Tiles
            for (int y = 0; y < Singleton.GAMEHEIGHT; y++)
            {
                for (int x = 0; x < Singleton.GAMEWIDTH; x++)
                {
                    Rectangle rect = new Rectangle(x * Singleton._TILESIZE, y * Singleton._TILESIZE, Singleton._TILESIZE, Singleton._TILESIZE);

                    // ใช้ลายหมากรุก: คาราเมลอ่อน-เข้ม
                    Color lightCaramel = new Color(248, 209, 146); // คาราเมลอ่อน
                    Color darkCaramel = new Color(124, 75, 24);    // คาราเมลเข้ม

                    Color baseColor = ((x + y) % 2 == 0) ? lightCaramel : darkCaramel;

                    _spriteBatch.Draw(Singleton.Instance._Rect, rect, baseColor);
                }
            }

            DrawRectangleWithOutline(_spriteBatch, Singleton.Instance._Rect, new Rectangle(0, 0, 540, 600), Color.SaddleBrown, 5);
            _spriteBatch.Draw(GameOver, new Rectangle(Singleton._TILESIZE * (Singleton.GAMEWIDTH / 2) - 150, Singleton._TILESIZE * (Singleton.GAMEHEIGHT / 2) - 60, 360, 120), new Rectangle(45, 270, 986, 297), Color.White);
            _spriteBatch.End();
        }

        public void Update(GameTime gameTime)
        {

        }

        public void DrawRectangleWithOutline(SpriteBatch spriteBatch, Texture2D pixel, Rectangle rect, Color outlineColor, int outlineThickness) // Use to draw outline of grid
        {
            spriteBatch.Draw(pixel, new Rectangle(rect.X, rect.Y, rect.Width, outlineThickness), outlineColor);
            spriteBatch.Draw(pixel, new Rectangle(rect.X, rect.Y, outlineThickness, rect.Height), outlineColor);
            spriteBatch.Draw(pixel, new Rectangle(rect.X + rect.Width - outlineThickness, rect.Y, outlineThickness, rect.Height), outlineColor);
            spriteBatch.Draw(pixel, new Rectangle(rect.X, rect.Y + rect.Height - outlineThickness, rect.Width, outlineThickness), outlineColor);
        }

        public void DrawLine(SpriteBatch spriteBatch, Texture2D pixel, Vector2 start, Vector2 end, Color color, int thickness)
        {
            Vector2 direction = end - start;
            float length = direction.Length();
            if (length == 0) return; // Avoid division by zero

            direction.Normalize();
            Vector2 perpendicular = new Vector2(-direction.Y, direction.X);

            Rectangle rect = new Rectangle((int)start.X, (int)start.Y, (int)(length), thickness);
            spriteBatch.Draw(pixel, rect, null, color, (float)Math.Atan2(direction.Y, direction.X), Vector2.Zero, SpriteEffects.None, 0f);
        }

        public void _DrawHowToPlay(SpriteBatch _spriteBatch)
        {
            const int panelSize = 600;
            const int leftIndent = 20;             // indent for non-header lines
            float lineHeight = Font.LineSpacing;

            // center panel on screen
            int offsetX = (Singleton.SCREENWIDTH - panelSize) / 2;
            int offsetY = (Singleton.SCREENHEIGHT - panelSize) / 2;

            _spriteBatch.Begin(
                samplerState: SamplerState.PointClamp,
                transformMatrix: Matrix.CreateTranslation(offsetX, offsetY, 0f)
            );

            // background panel
            _spriteBatch.Draw(
                MenuBackgroundText,
                new Rectangle(0, 0, panelSize, panelSize),
                Color.White
            );

            string[] lines = new[]
            {
        "How to Play",    // header
        "",
        "- Slide dog blocks (1-4 tiles) left or right.",
        "- Every move or 20s, a new row appears.",
        "- Rock blocks cannot be moved.",
        "- Clear full rows to earn 9 points.",
        "- Game over when blocks reach the top.",
        "",
        "Special Skill - Time Reset:",
        "- Resets the 20-second timer.",
        "- Costs -10 points per use.",
        "",
        "Controls:",
        "- R         : Restart game",
        "- Spacebar : Shift row up",
        "- P         : Pause / Resume",
        "- M         : Mute / Unmute",
        "- Mouse     : Select blocks, press buttons"
    };

            // start drawing just a bit below top edge
            float y = 20f;

            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];
                Vector2 size = Font.MeasureString(line);

                float x;
                if (i == 0)
                {
                    // center only the first line
                    x = (panelSize - size.X) / 2f;
                }
                else
                {
                    // left-align all other lines
                    x = leftIndent;
                }

                _spriteBatch.DrawString(Font, line, new Vector2(x, y), Color.Black);
                y += lineHeight;
            }

            _spriteBatch.End();
        }
    }
}