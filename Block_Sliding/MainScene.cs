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

        _drawing = new Drawing(GraphicsDevice);

        _gameObjects = new List<GameObject>();

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        // // TODO: use this.Content to load your game content here

        _drawing.LoadContent(Content);
        Singleton.Instance._possibleClicked = new List<Point>();
        Singleton.Instance.Random = new System.Random();
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

                Singleton.Instance._possibleClicked.Clear();

                InitializeStartingRows();

                for (int i = 0; i < Singleton.GAMEWIDTH; i++)
                {
                    for (int j = 0; j < Singleton.GAMEHEIGHT - 1; j++) // skip predict
                    {
                        if (FindPossibleClickedTiles(new Point(i, j)).Count != 0)
                        {
                            Singleton.Instance._possibleClicked.Add(new Point(i, j));
                        }
                    }
                }

                // check dead
                // if(dead) Singleton.Instance.CurrentGameState = Singleton.GameState.GameOver;

                // Singleton.Instance.CurrentGameState = Singleton.GameState.WaitingForSelection;

                break;

            case Singleton.GameState.WaitingForSelection:
                Singleton.Instance.CurrentMouse = Mouse.GetState();

                if (Singleton.Instance.CurrentMouse.LeftButton == ButtonState.Pressed &&
                    Singleton.Instance.PreviousMouse.LeftButton == ButtonState.Released)
                {
                    int xPos = Singleton.Instance.CurrentMouse.X / Singleton._TILESIZE;
                    int yPos = Singleton.Instance.CurrentMouse.Y / Singleton._TILESIZE;

                    Singleton.Instance._clickedPos = new Point(xPos, yPos);
                    if (Singleton.Instance._possibleClicked.Contains(Singleton.Instance._clickedPos))
                    {
                        Singleton.Instance._possibleClicked.Clear();
                        Singleton.Instance._selectedTile = Singleton.Instance._clickedPos;
                        Singleton.Instance._possibleClicked.AddRange(FindPossibleClickedTiles(Singleton.Instance._selectedTile));
                        Singleton.Instance.CurrentGameState = Singleton.GameState.TileSelected;
                    }
                }

                break;
            case Singleton.GameState.TileSelected:
                Singleton.Instance.CurrentMouse = Mouse.GetState();

                if (Singleton.Instance.CurrentMouse.LeftButton == ButtonState.Pressed &&
                    Singleton.Instance.PreviousMouse.LeftButton == ButtonState.Released)
                {
                    int xPos = Singleton.Instance.CurrentMouse.X / Singleton._TILESIZE;
                    int yPos = Singleton.Instance.CurrentMouse.Y / Singleton._TILESIZE;

                    Singleton.Instance._clickedPos = new Point(xPos, yPos);

                    if (!Singleton.Instance._possibleClicked.Contains(Singleton.Instance._clickedPos) && Singleton.Instance._clickedPos != Singleton.Instance._selectedTile)
                    {
                        //invalid moves
                        Singleton.Instance._possibleClicked.Clear();
                        Singleton.Instance.CurrentGameState = Singleton.GameState.GamePlaying;
                    }
                    else if (Singleton.Instance._possibleClicked.Contains(Singleton.Instance._clickedPos))
                    {
                        // Singleton.Instance.GameBoard[Singleton.Instance._clickedPos.Y, Singleton.Instance._clickedPos.X] = Singleton.Instance.GameBoard[Singleton.Instance._selectedTile.Y, Singleton.Instance._selectedTile.X];
                        // Singleton.Instance.GameBoard[Singleton.Instance._selectedTile.Y, Singleton.Instance._selectedTile.X] = 0;
                        Block b = Singleton.Instance.BlockMap[Singleton.Instance._selectedTile.Y, Singleton.Instance._selectedTile.X];

                        foreach (Vector2 p in b.Pieces) { continue; }

                    }
                }
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
            case Singleton.GameState.WaitingForSelection:
                {
                    _drawing._DrawWaitingForSelect(_spriteBatch);
                }
                break;
            case Singleton.GameState.TileSelected:
                {
                    _drawing._DrawTileSelected(_spriteBatch);
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
        Singleton.Instance.BlockMap = new Block[Singleton.GAMEHEIGHT, Singleton.GAMEWIDTH];

        Singleton.Instance.Score = 0;
        // Singleton.Instance.Level = 1;
        // Singleton.Instance.LineDeleted = 0;
        Singleton.Instance.CurrentGameState = Singleton.GameState.GamePlaying;

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

    protected List<Point> FindPossibleClickedTiles(Point mousePosition)
    {
        List<Point> clickedTiles = new List<Point>();
        if (Singleton.Instance.GameBoard[mousePosition.Y, mousePosition.X] == 1)
        {
            Block clickedBlock = Singleton.Instance.BlockMap[mousePosition.Y, mousePosition.X];

            if (clickedBlock == null || clickedBlock.CurrentBlockType == Block.BlockType.Rock)
                return clickedTiles;

            int blockLength = clickedBlock.GetLength();

            for (int x = 0; x <= Singleton.GAMEWIDTH - blockLength; x++)
            {
                bool canPlace = true;
                for (int i = 0; i < blockLength; i++)
                {
                    int checkX = x + i;
                    int y = mousePosition.Y;

                    if (Singleton.Instance.GameBoard[y, checkX] != 0)
                    {
                        canPlace = false;
                        break;
                    }
                }

                if (canPlace)
                    clickedTiles.Add(new Point(x, mousePosition.Y));
            }
        }
        return clickedTiles;
    }

    public void InitializeStartingRows()
    {

        int initialRows = 2;

        for (int row = 0; row < initialRows; row++)
        {
            List<int> sequence;
            bool hasEmpty;

            do
            {
                sequence = new List<int>();
                hasEmpty = false;
                int sum = 0;
                while (sum < Singleton.GAMEWIDTH)
                {
                    int rand = Singleton.Instance.Random.Next(5);    // 0–4
                    if (rand == 0)
                    {
                        sequence.Add(0);
                        sum += 1;
                        hasEmpty = true;
                    }
                    else if (sum + rand <= Singleton.GAMEWIDTH)
                    {
                        sequence.Add(rand);
                        sum += rand;
                    }
                }
            }
            while (!hasEmpty);

            // นำ sequence ที่ได้มาเติมลง GameBoard กับ BlockMap
            int col = 0;
            foreach (var seg in sequence)
            {
                if (seg == 0)
                {
                    // ช่องว่าง
                    Singleton.Instance.GameBoard[row, col] = 0;
                    Singleton.Instance.BlockMap[row, col] = null;
                    col++;
                }
                else
                {
                    // สร้างบล็อกความยาว seg
                    Block.BlockType blockType = (Block.BlockType)(seg - 1);
                    Texture2D tex = Drawing.DogTextures[seg];   // DogTextures[1] = size1, [2] = size2 ฯลฯ
                    var block = new Block(tex)
                    {
                        CurrentBlockType = blockType,
                        Position = new Vector2(col * Singleton._TILESIZE, row * Singleton._TILESIZE),
                    };

                    // // กำหนด offset ของแต่ละชิ้นย่อย
                    // for (int i = 0; i < seg; i++)
                    //     block.Pieces[i] = new Vector2(i * Singleton._TILESIZE, 0);

                    // เติมลงทั้งเซลล์
                    for (int i = 0; i < seg; i++)
                    {
                        Singleton.Instance.GameBoard[row, col + i] = 1;
                        Singleton.Instance.BlockMap[row, col + i] = (i == 0) ? block : null;
                    }

                    col += seg;
                }
            }
        }
    }

}
