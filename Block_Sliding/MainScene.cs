using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Dog_Sliding;

public class MainScene : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    // GameObjects
    List<GameObject> _gameObjects;
    public int _numOjects;

    // Drawing
    private Drawing _drawing;

    private float _rowTimer = 0f; // Timer for shifting rows down

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

        this.Window.Title = "Dog Sliding";

        _gameObjects = new List<GameObject>();
        _drawing = new Drawing(GraphicsDevice);

        Singleton.Instance.PossibleClicked = new List<Point>();
        Singleton.Instance.Random = new System.Random();

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        _drawing.LoadContent(Content);

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

                // _rowTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                // if (_rowTimer >= 5f)
                // {
                //     _rowTimer -= 5f;
                //     ShiftRowsUp();    // เรียกเมธ็อดใหม่
                // }
                
                ShiftRowsUp();
                if (Singleton.Instance.CurrentGameState == Singleton.GameState.GameOver)
                {
                    Console.WriteLine("[GAME OVER] Game Over due to shift up.");
                    return;
                }

                // ตรวจสอบแถวเต็มและลบแถวที่เต็ม
                // CheckAndClearFullRows();

                // ดรอปบล็อกลงก่อนเข้าสู่ WaitingForSelection
                for (int y = Singleton.GAMEHEIGHT - 2; y >= 0; y--)
                {
                    for (int x = 0; x < Singleton.GAMEWIDTH; x++)
                    {
                        TryDropBlockDown(new Point(x, y));
                    }
                }

                Singleton.Instance.PossibleClicked.Clear();

                for (int i = 0; i < Singleton.GAMEWIDTH; i++)
                {
                    for (int j = 0; j < Singleton.GAMEHEIGHT - 1; j++) // skip predict
                    {
                        if (FindPossibleClickedTiles(new Point(i, j)).Count != 0)
                        {
                            Singleton.Instance.PossibleClicked.Add(new Point(i, j));
                        }
                    }
                }

                // check dead
                // if(dead) Singleton.Instance.CurrentGameState = Singleton.GameState.GameOver;
                HashSet<Point> possibleClicked = new HashSet<Point>();

                for (int i = 0; i < Singleton.GAMEWIDTH; i++)
                {
                    for (int j = 0; j < Singleton.GAMEHEIGHT - 1; j++)
                    {
                        List<Point> result = FindPossibleClickedTiles(new Point(i, j));
                        foreach (Point pt in result)
                        {
                            possibleClicked.Add(pt); // ป้องกันซ้ำ
                        }
                    }
                }
                Singleton.Instance.PossibleClicked = possibleClicked.ToList();
                // Console.WriteLine("[INIT] GameBoard Initialized");
                // for (int y = 0; y < Singleton.GAMEHEIGHT; y++)
                // {
                //     string line = "";
                //     for (int x = 0; x < Singleton.GAMEWIDTH; x++)
                //         line += Singleton.Instance.GameBoard[y, x] + " ";
                //     Console.WriteLine($"Y={y}: {line}");
                // }
                Console.WriteLine("[INIT] BlockMap Initialized");
                for (int y = 0; y < Singleton.GAMEHEIGHT; y++)
                {
                    string line = "";
                    for (int x = 0; x < Singleton.GAMEWIDTH; x++)
                    {
                        if (Singleton.Instance.BlockMap[y, x] != null)
                            line += Singleton.Instance.BlockMap[y, x].CurrentBlockType + " ";
                        else
                            line += "null ";
                    }
                    Console.WriteLine($"Y={y}: {line}");
                }
                // foreach (var item in Singleton.Instance.PossibleClicked)
                // {
                //     Console.WriteLine($"[POSSIBLE] Clickable tile at ({item.X}, {item.Y})");
                // }
                Singleton.Instance.CurrentGameState = Singleton.GameState.WaitingForSelection;

                break;

            case Singleton.GameState.WaitingForSelection:
                Singleton.Instance.CurrentMouse = Mouse.GetState();

                // Check if the mouse is clicked on a button up
                if (IsButtonClicked(_drawing.ButtonUpRect))
                {
                    ShiftRowsUp();
                    Singleton.Instance.CurrentGameState = Singleton.GameState.GamePlaying;
                }

                if (Singleton.Instance.CurrentMouse.LeftButton == ButtonState.Pressed &&
                    Singleton.Instance.PreviousMouse.LeftButton == ButtonState.Released)
                {
                    int xPos = Singleton.Instance.CurrentMouse.X / Singleton._TILESIZE - 6; // shift to center
                    int yPos = Singleton.Instance.CurrentMouse.Y / Singleton._TILESIZE - 1; // shift to center

                    Singleton.Instance.ClickedPos = new Point(xPos, yPos);
                    if (Singleton.Instance.PossibleClicked.Contains(Singleton.Instance.ClickedPos))
                    {
                        Singleton.Instance.PossibleClicked.Clear();
                        Singleton.Instance.SelectedTile = Singleton.Instance.ClickedPos;
                        // Console.WriteLine($"[SELECT] Tile selected at ({Singleton.Instance.SelectedTile.X}, {Singleton.Instance.SelectedTile.Y})");
                        Singleton.Instance.PossibleClicked.AddRange(FindAvailableSpaces(Singleton.Instance.SelectedTile));
                        // foreach (var item in Singleton.Instance.PossibleClicked)
                        // {
                        //     Console.WriteLine($"[POSSIBLE] Clickable tile at ({item.X}, {item.Y})");
                        // }
                        Singleton.Instance.PreviousMouse = Singleton.Instance.CurrentMouse;
                        Singleton.Instance.CurrentGameState = Singleton.GameState.TileSelected;
                    }
                }
                break;
            case Singleton.GameState.TileSelected:
                Singleton.Instance.CurrentMouse = Mouse.GetState();

                // Check if the mouse is clicked on a button up
                if (IsButtonClicked(_drawing.ButtonUpRect))
                {
                    ShiftRowsUp();
                    Singleton.Instance.CurrentGameState = Singleton.GameState.GamePlaying;
                }

                if (Singleton.Instance.CurrentMouse.LeftButton == ButtonState.Pressed &&
                    Singleton.Instance.PreviousMouse.LeftButton == ButtonState.Released)
                {
                    int xPos = Singleton.Instance.CurrentMouse.X / Singleton._TILESIZE - 6; // shift to center
                    int yPos = Singleton.Instance.CurrentMouse.Y / Singleton._TILESIZE - 1; // shift to center

                    Singleton.Instance.ClickedPos = new Point(xPos, yPos);

                    if (!Singleton.Instance.PossibleClicked.Contains(Singleton.Instance.ClickedPos) && Singleton.Instance.ClickedPos != Singleton.Instance.SelectedTile)
                    {
                        //invalid moves
                        Singleton.Instance.PossibleClicked.Clear();
                        Singleton.Instance.CurrentGameState = Singleton.GameState.GamePlaying;
                    }
                    else if (Singleton.Instance.PossibleClicked.Contains(Singleton.Instance.ClickedPos))
                    {
                        Point emptyPos = Singleton.Instance.ClickedPos;
                        Point oldPos = Singleton.Instance.SelectedTile;
                        Block selectedBlock = Singleton.Instance.BlockMap[oldPos.Y, oldPos.X];
                        if (selectedBlock == null)
                            return;

                        int blockLength = selectedBlock.GetLength();
                        int y = oldPos.Y;

                        // หาหัวบล็อกเดิม
                        int headX = oldPos.X;
                        while (headX > 0 && Singleton.Instance.BlockMap[y, headX - 1] == selectedBlock)
                            headX--;

                        int tailX = headX + blockLength - 1;

                        // คำนวณตำแหน่งหัวบล็อกใหม่ ตามตำแหน่งช่องว่างที่เลือก
                        int newHeadX;
                        if (emptyPos.X < headX)
                        {
                            newHeadX = emptyPos.X;
                        }
                        else if (emptyPos.X > tailX)
                        {
                            newHeadX = emptyPos.X - blockLength + 1;
                        }
                        else
                        {
                            // ช่องว่างอยู่ในบล็อกเดิม ไม่ควรเกิดขึ้น แต่กันไว้
                            newHeadX = headX;
                        }

                        // ตรวจขอบเขต
                        if (newHeadX < 0 || newHeadX + blockLength - 1 >= Singleton.GAMEWIDTH)
                            return;

                        // ตรวจสอบพื้นที่ว่างใน GameBoard และ BlockMap
                        bool canMove = true;
                        for (int i = 0; i < blockLength; i++)
                        {
                            int x = newHeadX + i;
                            if (Singleton.Instance.GameBoard[y, x] != 0 &&
                                Singleton.Instance.BlockMap[y, x] != selectedBlock)
                            {
                                canMove = false;
                                break;
                            }
                        }

                        if (!canMove)
                        {
                            Console.WriteLine("[BLOCKED] Can't move block to occupied space.");
                            return;
                        }

                        // เคลียร์ตำแหน่งบล็อกเก่า
                        for (int i = 0; i < blockLength; i++)
                        {
                            Singleton.Instance.GameBoard[y, headX + i] = 0;
                            Singleton.Instance.BlockMap[y, headX + i] = null;
                        }

                        // วางบล็อกในตำแหน่งใหม่
                        for (int i = 0; i < blockLength; i++)
                        {
                            Singleton.Instance.GameBoard[y, newHeadX + i] = 1;
                            Singleton.Instance.BlockMap[y, newHeadX + i] = selectedBlock;
                        }
                        // อัปเดตตำแหน่งวาดใหม่ของ block (เปลี่ยนตำแหน่งบนจอ)
                        selectedBlock.Position = new Vector2(
                            newHeadX * Singleton._TILESIZE,
                            y * Singleton._TILESIZE
                        );

                        // เคลียร์สถานะ และกลับไปสถานะเล่นเกม
                        Singleton.Instance.PossibleClicked.Clear();
                        Singleton.Instance.CurrentGameState = Singleton.GameState.GamePlaying;
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

        Singleton.Instance.PreviousMouse = Singleton.Instance.CurrentMouse;
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
                    _drawing._DrawGamePlaying(_spriteBatch, _gameObjects, _numOjects);
                }
                break;
            case Singleton.GameState.WaitingForSelection:
                {
                    _drawing._DrawWaitingForSelect(_spriteBatch, _gameObjects, _numOjects);
                }
                break;
            case Singleton.GameState.TileSelected:
                {
                    _drawing._DrawTileSelected(_spriteBatch, _gameObjects, _numOjects);
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
        _gameObjects.Clear();

        Singleton.Instance.GameBoard = new int[Singleton.GAMEHEIGHT, Singleton.GAMEWIDTH];
        Singleton.Instance.BlockMap = new Block[Singleton.GAMEHEIGHT, Singleton.GAMEWIDTH];
        Singleton.Instance.Score = 0;
        Singleton.Instance.CurrentGameState = Singleton.GameState.GameStart;

        CreateRow(1);
    }

    // protected bool CheckDeletableRow(int[] checkingRow)
    // {
    //     for (int i = 0; i < checkingRow.Length; i++)
    //     {
    //         if (checkingRow[i] == 0)
    //             return false;
    //     }
    //     return true;
    // }

    private bool IsButtonClicked(Rectangle buttonRect)
    {
        return Singleton.Instance.CurrentMouse.LeftButton == ButtonState.Pressed && Singleton.Instance.PreviousMouse.LeftButton == ButtonState.Released && buttonRect.Contains(Singleton.Instance.CurrentMouse.Position);
    }

    public List<Point> FindAvailableSpaces(Point blockPosition)
    {
        List<Point> availableSpaces = new List<Point>();

        Block selectedBlock = Singleton.Instance.BlockMap[blockPosition.Y, blockPosition.X];
        if (selectedBlock == null)
            return availableSpaces;

        int blockLength = selectedBlock.GetLength();
        int y = blockPosition.Y;

        // หาหัวจริงของ block
        int headX = blockPosition.X;
        while (headX > 0 && Singleton.Instance.BlockMap[y, headX - 1] == selectedBlock)
            headX--;
        int tailX = headX + blockLength - 1;

        // === ตรวจซ้าย ===
        for (int offset = 1; headX - offset >= 0; offset++)
        {
            int newHeadX = headX - offset;
            bool canMove = true;

            for (int i = 0; i < blockLength; i++)
            {
                int x = newHeadX + i;
                if (x < 0 || x >= Singleton.GAMEWIDTH ||
                    (Singleton.Instance.BlockMap[y, x] != null &&
                     Singleton.Instance.BlockMap[y, x] != selectedBlock))
                {
                    canMove = false;
                    break;
                }
            }

            if (canMove)
            {
                if (Singleton.Instance.BlockMap[y, newHeadX] == null)
                    availableSpaces.Add(new Point(newHeadX, y));
            }
            else break;
        }

        // === ตรวจขวา ===
        for (int offset = 1; tailX + offset < Singleton.GAMEWIDTH; offset++)
        {
            int newTailX = tailX + offset;
            int newHeadX = newTailX - blockLength + 1;
            bool canMove = true;

            for (int i = 0; i < blockLength; i++)
            {
                int x = newHeadX + i;
                if (x >= Singleton.GAMEWIDTH ||
                    (Singleton.Instance.BlockMap[y, x] != null &&
                     Singleton.Instance.BlockMap[y, x] != selectedBlock))
                {
                    canMove = false;
                    break;
                }
            }

            if (canMove)
            {
                if (Singleton.Instance.BlockMap[y, newTailX] == null)
                    availableSpaces.Add(new Point(newTailX, y));
            }
            else break;
        }

        return availableSpaces;
    }



    protected List<Point> FindPossibleClickedTiles(Point mousePosition)
    {
        List<Point> clickedTiles = new List<Point>();

        // ไม่ใช่ block
        if (Singleton.Instance.GameBoard[mousePosition.Y, mousePosition.X] != 1)
            return clickedTiles;

        // ไม่พบ block หรือเป็น Rock
        Block clickedBlock = Singleton.Instance.BlockMap[mousePosition.Y, mousePosition.X];
        if (clickedBlock == null || clickedBlock.CurrentBlockType == Block.BlockType.Rock)
            return clickedTiles;

        int y = mousePosition.Y;
        int x = mousePosition.X;

        // หา headX โดยเดินถอยหลังไปจนเจอจุดเริ่มต้นของ block
        while (x > 0 && Singleton.Instance.BlockMap[y, x - 1] == clickedBlock)
            x--;

        int headX = x;
        int blockLength = clickedBlock.GetLength();

        // ดึงตำแหน่งทั้งหมดของ block
        for (int i = 0; i < blockLength; i++)
        {
            Point pt = new Point(headX + i, y);
            clickedTiles.Add(pt);
            // Console.WriteLine($"[CLICK] Block at ({pt.X}, {pt.Y})");
        }

        return clickedTiles;
    }
    private bool IsGameOverByShift()
    {
        // ถ้ามีบล็อกอยู่ในแถวบนสุดก่อนเลื่อน -> เกมโอเวอร์
        for (int x = 0; x < Singleton.GAMEWIDTH; x++)
        {
            if (Singleton.Instance.GameBoard[0, x] == 1)
            {
                return true;
            }
        }
        return false;
    }

    private void ShiftRowsUp()
    {
        if (IsGameOverByShift())
        {
            Singleton.Instance.CurrentGameState = Singleton.GameState.GameOver;
            Console.WriteLine("[GAME OVER] Cannot shift up: top row is full.");
            return;
        }

        // 1) เลื่อนข้อมูลใน GameBoard และ BlockMap ขึ้น
        for (int y = 0; y < Singleton.GAMEHEIGHT - 1; y++)
        {
            for (int x = 0; x < Singleton.GAMEWIDTH; x++)
            {
                Singleton.Instance.GameBoard[y, x] = Singleton.Instance.GameBoard[y + 1, x];
                Singleton.Instance.BlockMap[y, x] = Singleton.Instance.BlockMap[y + 1, x];
            }
        }

        // 2) เลื่อนตำแหน่ง GameObject ขึ้นทางกายภาพ
        foreach (var obj in _gameObjects)
            obj.Position = new Vector2(obj.Position.X, obj.Position.Y - Singleton._TILESIZE);

        // 3) ลบอ็อบเจ็กต์ที่หลุดขอบบน (Y < 0)
        _gameObjects.RemoveAll(o => o.Position.Y < 0);

        // 4) เติมแถวใหม่ที่ด้านล่าง
        CreateRow(1);
    }

    protected void CreateRow(int rowCount)
    {
        for (int k = 0; k < rowCount; k++)
        {
            int row = Singleton.GAMEHEIGHT - rowCount + k;

            List<int> sequence;
            bool hasEmpty;
            do
            {
                sequence = new List<int>();
                hasEmpty = false;
                int sum = 0;

                while (sum < Singleton.GAMEWIDTH)
                {
                    int rand = Singleton.Instance.Random.Next(5); // 0–4
                    if (rand == 0)
                    {
                        sequence.Add(0);
                        sum++;
                        hasEmpty = true;
                    }
                    else if (sum + rand <= Singleton.GAMEWIDTH)
                    {
                        sequence.Add(rand);
                        sum += rand;
                    }
                }
            } while (!hasEmpty);

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
                    var blockType = (Block.BlockType)(seg - 1);
                    var texture = Drawing.DogTextures[seg];
                    var block = new Block(texture)
                    {
                        CurrentBlockType = blockType,
                        Position = new Vector2(col * Singleton._TILESIZE, row * Singleton._TILESIZE)
                    };
                    _gameObjects.Add(block);

                    // กำหนดตำแหน่งแต่ละชิ้น
                    for (int i = 0; i < seg; i++)
                        block.Pieces[i] = new Vector2(i * Singleton._TILESIZE, 0);

                    // เติมลง GameBoard + BlockMap
                    for (int i = 0; i < seg; i++)
                    {
                        Singleton.Instance.GameBoard[row, col + i] = 1;
                        Singleton.Instance.BlockMap[row, col + i] = block;
                    }
                    col += seg;
                }
            }
        }
    }
    private void TryDropBlockDown(Point blockPosition)
    {
        int x = blockPosition.X;
        int y = blockPosition.Y;

        // ป้องกันเลยขอบเขต
        if (x < 0 || x >= Singleton.GAMEWIDTH || y < 0 || y >= Singleton.GAMEHEIGHT - 1)
            return;

        Block block = Singleton.Instance.BlockMap[y, x];
        if (block == null)
            return;

        int length = block.GetLength();

        // หา head ของ block
        int headX = x;
        while (headX > 0 && Singleton.Instance.BlockMap[y, headX - 1] == block)
            headX--;

        int tailX = headX + length - 1;

        // หาจำนวนช่องว่างด้านล่าง
        int dropY = y;
        while (dropY + 1 < Singleton.GAMEHEIGHT - 1) // ห้ามเกินแถวรองสุดท้าย
        {
            bool canDrop = true;
            for (int i = 0; i < length; i++)
            {
                int curX = headX + i;
                if (Singleton.Instance.BlockMap[dropY + 1, curX] != null)
                {
                    canDrop = false;
                    break;
                }
            }

            if (!canDrop)
                break;

            dropY++;
        }

        // ถ้าขยับไม่ได้ก็ไม่ต้องทำอะไร
        if (dropY == y)
            return;

        // เคลียร์ตำแหน่งเก่า
        for (int i = 0; i < length; i++)
        {
            Singleton.Instance.GameBoard[y, headX + i] = 0;
            Singleton.Instance.BlockMap[y, headX + i] = null;
        }

        // วางตำแหน่งใหม่
        for (int i = 0; i < length; i++)
        {
            Singleton.Instance.GameBoard[dropY, headX + i] = 1;
            Singleton.Instance.BlockMap[dropY, headX + i] = block;
        }

        // อัปเดตตำแหน่งบนจอ
        block.Position = new Vector2(headX * Singleton._TILESIZE, dropY * Singleton._TILESIZE);
    }

}
