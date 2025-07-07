using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Block_Sliding;

public class MainScene : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    // GameObjects
    List<GameObject> _gameObjects;
    public int _numOjects;
    // Point _selectedTile;
    List<Point> _possibleClicked;

    // Drawing
    private Drawing _drawing;

    public MainScene()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        _graphics.PreferredBackBufferWidth = Singleton.SCREENWIDTH;
        _graphics.PreferredBackBufferHeight = Singleton.SCREENHEIGHT;
        _graphics.ApplyChanges();

        _gameObjects = new List<GameObject>();

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        // // TODO: use this.Content to load your game content here
        // Singleton.Instance._rect = new Texture2D(_graphics.GraphicsDevice, Singleton.Instance._TILESIZE, Singleton.Instance._TILESIZE);

        // Color[] data = new Color[Singleton.Instance._TILESIZE * Singleton.Instance._TILESIZE];
        // for (int i = 0; i < data.Length; ++i) data[i] = Color.White;
        // Singleton.Instance._rect.SetData(data);

        _possibleClicked = new List<Point>();

        Reset();
    }

    protected override void Update(GameTime gameTime)
    {
        Singleton.Instance.CurrentKey = Keyboard.GetState();
        Singleton.Instance.CurrentMouse = Mouse.GetState();

        _numOjects = _gameObjects.Count;

        switch (Singleton.Instance.CurrentGameState)
        {
            case Singleton.GameState.GameStart:
                if (IsButtonClicked(_drawing.PlayRect))
                {
                    Singleton.Instance.CurrentGameState = Singleton.GameState.GamePlaying;
                }
                else if (IsButtonClicked(_drawing.ExitRect))
                {
                    Exit();
                }
                break;

            case Singleton.GameState.GamePlaying:
                // // handle the pause
                if (Singleton.Instance.CurrentKey.IsKeyDown(Keys.Escape) && !Singleton.Instance.CurrentKey.Equals(Singleton.Instance.PreviousKey))
                {
                    Singleton.Instance.CurrentGameState = Singleton.GameState.GamePaused;
                }

                Singleton.Instance.Timer += gameTime.ElapsedGameTime.Ticks; // Timer Count in Seconds

                _possibleClicked.Clear();

                break;

            case Singleton.GameState.GameSelection:

                break;

            case Singleton.GameState.GamePaused:
                // handle unpause
                if (Singleton.Instance.CurrentKey.IsKeyDown(Keys.Escape) && !Singleton.Instance.CurrentKey.Equals(Singleton.Instance.PreviousKey))
                {
                    Singleton.Instance.CurrentGameState = Singleton.GameState.GamePlaying;
                }
                break;
            case Singleton.GameState.GameOver:
                // handle end game
                if (Singleton.Instance.CurrentKey.IsKeyDown(Keys.Space) && !Singleton.Instance.CurrentKey.Equals(Singleton.Instance.PreviousKey))
                {
                    Singleton.Instance.CurrentGameState = Singleton.GameState.GameStart;
                    Reset();
                }
                else if (IsButtonClicked(_drawing.BackRect))
                {
                    Singleton.Instance.CurrentGameState = Singleton.GameState.GameStart;
                }
                break;
        }

        Singleton.Instance.PreviousKey = Singleton.Instance.CurrentKey;

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.White);

        switch (Singleton.Instance.CurrentGameState)
        {
            case Singleton.GameState.GameStart:
                {
                    _drawing._DrawGameStart(_spriteBatch);
                }
                break;
            case Singleton.GameState.GamePlaying:
                {
                    _drawing._DrawGamePlaying(_spriteBatch);
                }
                break;
            case Singleton.GameState.GameSelection:
                {
                    _drawing._DrawGameSelection(_spriteBatch);
                }
                break;
            case Singleton.GameState.GamePaused:
                {
                    _drawing._DrawGamePaused(_spriteBatch);
                }
                break;
            case Singleton.GameState.GameOver:
                {
                    _drawing._DrawGameOver(_spriteBatch);
                }
                break;
        }

        _graphics.BeginDraw();

        base.Draw(gameTime);
    }

    protected void Reset()
    {
        Singleton.Instance.GameBoard = new int[Singleton.GAMEHEIGHT, Singleton.GAMEWIDTH];

        Singleton.Instance.Score = 0;
        // Singleton.Instance.Level = 1;
        // Singleton.Instance.LineDeleted = 0;
        Singleton.Instance.CurrentGameState = Singleton.GameState.GameStart;

    }

    protected bool CheckDeletableRow(int[] checkingRow)
    {
        for (int i = 0; i < checkingRow.Length; i++)
        {
            if (checkingRow[i] == 0)
                return false;
        }
        return true;
    }

    private bool IsButtonClicked(Rectangle buttonRect)
    {
        return Singleton.Instance.CurrentMouse.LeftButton == ButtonState.Pressed && Singleton.Instance.PreviousMouse.LeftButton == ButtonState.Released && buttonRect.Contains(Singleton.Instance.CurrentMouse.Position);
    }
}
