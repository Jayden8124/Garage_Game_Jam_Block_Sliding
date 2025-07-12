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
    public int _numObjects;

    // Drawing
    private Drawing _drawing;

    private bool _pendingShiftAfterClear;

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
        Singleton.Instance.CurrentGameState = Singleton.GameState.GameStart;
    }

    protected override void Update(GameTime gameTime)
    {
        Singleton.Instance.CurrentKey = Keyboard.GetState();
        Singleton.Instance.CurrentMouse = Mouse.GetState();

        foreach (var obj in _gameObjects)
            obj.Update(gameTime);

        if (IsAnyAnimationRunning()) // if animation playing will skip
            return;

        _gameObjects.RemoveAll(o => o is Block b && !b.IsClearing && b.Scale.X <= 0f);
        _gameObjects.RemoveAll(o => o is Block b && b.Position.Y < 0 && !b.IsMoving);

        _numObjects = _gameObjects.Count;


        if (Singleton.Instance.CurrentGameState == Singleton.GameState.GamePaused ||
            Singleton.Instance.CurrentGameState == Singleton.GameState.GameStart)
        {
            Singleton.Instance.Timer += 0;
        }
        else
        {
            Singleton.Instance.Timer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        _numObjects = _gameObjects.Count;

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

            // case Singleton.GameState.GamePlaying:
            //     if (Singleton.Instance.CurrentGameState == Singleton.GameState.GameOver)
            //     {
            //         Console.WriteLine("[GAME OVER] Game Over due to shift up.");
            //         return;
            //     }

            //     if (Singleton.Instance.Timer < 0f)
            //     {
            //         ShiftRowsUp();
            //         Singleton.Instance.Timer = 20.8f;
            //         Singleton.Instance.CurrentGameState = Singleton.GameState.GamePlaying;
            //     }

            //     while (true)
            //     {
            //         // Block Drop
            //         for (int y = Singleton.GAMEHEIGHT - 2; y >= 0; y--)
            //         {
            //             for (int x = 0; x < Singleton.GAMEWIDTH; x++)
            //             {
            //                 TryDropBlockDown(new Point(x, y));
            //             }
            //         }

            //         bool cleared = CheckAndClearFullRows();
            //         if (cleared)
            //         {
            //             ShiftRowsUp(); // Shift rows when turn ends   
            //         }
            //         else
            //         {
            //             break;
            //         }
            //     }

            //     // Find all possible clicked tiles before change state to WaitingForSelection
            //     Singleton.Instance.PossibleClicked.Clear();
            //     HashSet<Point> possibleClicked = new HashSet<Point>();

            //     for (int i = 0; i < Singleton.GAMEWIDTH; i++)
            //     {
            //         for (int j = 0; j < Singleton.GAMEHEIGHT - 1; j++)
            //         {
            //             List<Point> result = FindPossibleClickedTiles(new Point(i, j));
            //             foreach (Point pt in result)
            //             {
            //                 possibleClicked.Add(pt);
            //             }
            //         }
            //     }
            //     Singleton.Instance.PossibleClicked = possibleClicked.ToList();

            //     // Console.WriteLine("[INIT] BlockMap Initialized");
            //     // for (int y = 0; y < Singleton.GAMEHEIGHT; y++)
            //     // {
            //     //     string line = "";
            //     //     for (int x = 0; x < Singleton.GAMEWIDTH; x++)
            //     //     {
            //     //         if (Singleton.Instance.BlockMap[y, x] != null)
            //     //             line += Singleton.Instance.BlockMap[y, x].CurrentBlockType + " ";
            //     //         else
            //     //             line += "null ";
            //     //     }
            //     //     Console.WriteLine($"Y={y}: {line}");
            //     // }

            //     // Console.WriteLine("[INIT] GameBoard Initialized");
            //     // for (int y = 0; y < Singleton.GAMEHEIGHT; y++)
            //     // {
            //     //     string line = "";
            //     //     for (int x = 0; x < Singleton.GAMEWIDTH; x++)
            //     //     {
            //     //         line += Singleton.Instance.GameBoard[y, x] + " ";
            //     //     }
            //     //     Console.WriteLine($"Y={y}: {line}");
            //     // }

            //     Singleton.Instance.CurrentGameState = Singleton.GameState.GameWaitingForSelection;

            //     break;
            // case Singleton.GameState.GamePlaying:
            //     // 1) If we have a pending shift from a clear, wait for all clears to finish…
            //     if (_pendingShiftAfterClear)
            //     {
            //         if (IsAnyAnimationRunning()) return;
            //         ShiftRowsUp();
            //         _pendingShiftAfterClear = false;
            //         return;
            //     }

            //     // 2) Timer‐driven automatic shift
            //     if (Singleton.Instance.Timer < 0f)
            //     {
            //         ShiftRowsUp();
            //         Singleton.Instance.Timer = 20.8f;
            //         return; // let the shift animation play before doing anything else
            //     }

            //     // 3) Single drop pass
            //     for (int y = Singleton.GAMEHEIGHT - 2; y >= 0; y--)
            //         for (int x = 0; x < Singleton.GAMEWIDTH; x++)
            //             TryDropBlockDown(new Point(x, y));
            //     if (IsAnyAnimationRunning()) return;

            //     // 4) Single clear pass (if any full rows)
            //     if (CheckAndClearFullRows())
            //     {
            //         // clear animations now running; defer the shift
            //         _pendingShiftAfterClear = true;
            //         return;
            //     }

            //     // 5) No more drop/clear to do—proceed to find possible clicks
            //     Singleton.Instance.PossibleClicked.Clear();
            //     var possible = new HashSet<Point>();
            //     for (int i = 0; i < Singleton.GAMEWIDTH; i++)
            //         for (int j = 0; j < Singleton.GAMEHEIGHT - 1; j++)
            //             foreach (var pt in FindPossibleClickedTiles(new Point(i, j)))
            //                 possible.Add(pt);
            //     Singleton.Instance.PossibleClicked = possible.ToList();
            //     Singleton.Instance.CurrentGameState = Singleton.GameState.GameWaitingForSelection;
            //     break;

            // case Singleton.GameState.GamePlaying:
            //     // 1) If we have a pending shift from a clear, wait for all clears to finish…
            //     if (_pendingShiftAfterClear)
            //     {
            //         if (IsAnyAnimationRunning()) return;
            //         ShiftRowsUp();
            //         _pendingShiftAfterClear = false;
            //         return;
            //     }

            //     // 2) Timer‐driven automatic shift
            //     if (Singleton.Instance.Timer < 0f)
            //     {
            //         ShiftRowsUp();
            //         Singleton.Instance.Timer = 20.8f;
            //         return; // let the shift animation play before doing anything else
            //     }

            //     // 3) Single drop pass
            //     for (int y = Singleton.GAMEHEIGHT - 2; y >= 0; y--)
            //         for (int x = 0; x < Singleton.GAMEWIDTH; x++)
            //             TryDropBlockDown(new Point(x, y));
            //     if (IsAnyAnimationRunning()) return;

            //     // 4) Single clear pass (if any full rows)
            //     if (CheckAndClearFullRows())
            //     {
            //         // clear animations now running; defer the shift
            //         _pendingShiftAfterClear = true;
            //         return;
            //     }

            //     // 5) No more drop/clear to do—proceed to find possible clicks
            //     Singleton.Instance.PossibleClicked.Clear();
            //     var possible = new HashSet<Point>();
            //     for (int i = 0; i < Singleton.GAMEWIDTH; i++)
            //         for (int j = 0; j < Singleton.GAMEHEIGHT - 1; j++)
            //             foreach (var pt in FindPossibleClickedTiles(new Point(i, j)))
            //                 possible.Add(pt);
            //     Singleton.Instance.PossibleClicked = possible.ToList();
            //     Singleton.Instance.CurrentGameState = Singleton.GameState.GameWaitingForSelection;
            //     break;
            case Singleton.GameState.GamePlaying:
                // 1) ถ้ามี pending shift จาก clear รอบก่อน ให้รอให้ clear จบแล้วค่อย shift

                while (true)
                {
                    // 3a) Drop ทุกบล็อกลงล่าง
                    for (int y = Singleton.GAMEHEIGHT - 2; y >= 0; y--)
                        for (int x = 0; x < Singleton.GAMEWIDTH; x++)
                            TryDropBlockDown(new Point(x, y));
                    // ถ้ายังมีบล็อกกำลังเคลื่อน ให้รอ animation จบก่อน
                    if (IsAnyAnimationRunning())
                        return;

                    // 3b) ตรวจสอบและลบแถวเต็มทีละแถว
                    if (CheckAndClearFullRows())
                    {
                        // เจอแถวเต็ม รอบนี้สั่ง clear animation แล้วรอให้มันจบ
                        _pendingShiftAfterClear = true;
                        return;
                    }

                    // ถ้าไม่มีแถวให้ clear อีก → ออกจาก loop
                    break;
                }

                if (_pendingShiftAfterClear)
                {
                    if (IsAnyAnimationRunning())
                        return;
                    ShiftRowsUp();
                    _pendingShiftAfterClear = false;
                    return;
                }

                // 2) ถ้า Timer หมด ให้ shift ทันที แล้วรอ animation จบ
                if (Singleton.Instance.Timer < 0f)
                {
                    ShiftRowsUp();
                    Singleton.Instance.Timer = 20.8f;
                    return;
                }

                // 3) Loop ทำ Drop → Clear จนไม่มีแถวให้ลบอีก


                // 4) ไม่มีอะไรจะ drop/clear แล้ว → เปลี่ยนไป WaitingForSelection
                Singleton.Instance.PossibleClicked.Clear();
                var possible = new HashSet<Point>();
                for (int i = 0; i < Singleton.GAMEWIDTH; i++)
                    for (int j = 0; j < Singleton.GAMEHEIGHT - 1; j++)
                        foreach (var pt in FindPossibleClickedTiles(new Point(i, j)))
                            possible.Add(pt);
                Singleton.Instance.PossibleClicked = possible.ToList();
                Singleton.Instance.CurrentGameState = Singleton.GameState.GameWaitingForSelection;
                break;

            case Singleton.GameState.GameWaitingForSelection:
                Singleton.Instance.CurrentMouse = Mouse.GetState();

                // Pause
                if (Singleton.Instance.CurrentKey.IsKeyDown(Keys.Escape) && !Singleton.Instance.CurrentKey.Equals(Singleton.Instance.PreviousKey))
                {
                    Singleton.Instance.CurrentGameState = Singleton.GameState.GamePaused;
                }

                if (Singleton.Instance.Timer < 0f)
                {
                    ShiftRowsUp();
                    Singleton.Instance.Timer = 20.8f;
                    Singleton.Instance.CurrentGameState = Singleton.GameState.GamePlaying;
                }

                // Check if the mouse is clicked on a button up
                if (IsButtonClicked(_drawing.ButtonUpRect))
                {
                    Singleton.Instance.Timer = 20.8f; // Reset timer for next turn

                    ShiftRowsUp();

                    if (Singleton.Instance.CurrentGameState == Singleton.GameState.GameOver) return;

                    Singleton.Instance.CurrentGameState = Singleton.GameState.GamePlaying;
                }

                if (IsButtonClicked(_drawing.SettingRect))
                {
                    Singleton.Instance.CurrentGameState = Singleton.GameState.GameStart;
                }

                if (IsButtonClicked(_drawing.VolumeRect))
                {
                    Console.WriteLine("[VOLUME] Volume button clicked.");
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
                        Singleton.Instance.PossibleClicked.AddRange(FindAvailableSpaces(Singleton.Instance.SelectedTile));
                        Singleton.Instance.PreviousMouse = Singleton.Instance.CurrentMouse;
                        Singleton.Instance.CurrentGameState = Singleton.GameState.GameTileSelected;
                    }
                }
                break;
            case Singleton.GameState.GameTileSelected:
                Singleton.Instance.CurrentMouse = Mouse.GetState();

                // Pause
                if (Singleton.Instance.CurrentKey.IsKeyDown(Keys.Escape) && !Singleton.Instance.CurrentKey.Equals(Singleton.Instance.PreviousKey))
                {
                    Singleton.Instance.CurrentGameState = Singleton.GameState.GamePaused;
                }

                if (Singleton.Instance.Timer < 0f)
                {
                    ShiftRowsUp();
                    Singleton.Instance.Timer = 20.8f;
                    Singleton.Instance.CurrentGameState = Singleton.GameState.GamePlaying;
                }

                // Check if the mouse is clicked on a button up
                if (IsButtonClicked(_drawing.ButtonUpRect))
                {
                    Singleton.Instance.Timer = 20.8f; // Reset timer for next turn

                    ShiftRowsUp();

                    if (Singleton.Instance.CurrentGameState == Singleton.GameState.GameOver)
                        return;

                    Singleton.Instance.CurrentGameState = Singleton.GameState.GamePlaying;
                }

                if (IsButtonClicked(_drawing.SettingRect))
                {
                    Singleton.Instance.CurrentGameState = Singleton.GameState.GameStart;
                }

                if (IsButtonClicked(_drawing.VolumeRect))
                {
                    Console.WriteLine("[VOLUME] Volume button clicked.");
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

                        // Find the head of the block
                        int headX = oldPos.X;
                        while (headX > 0 && Singleton.Instance.BlockMap[y, headX - 1] == selectedBlock)
                            headX--;

                        int tailX = headX + blockLength - 1;

                        // Calculate and Selected new head position
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
                            newHeadX = headX;
                        }

                        // Check boundaries
                        if (newHeadX < 0 || newHeadX + blockLength - 1 >= Singleton.GAMEWIDTH)
                            return;

                        // Find blank space in GameBoard and BlockMap
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

                        // Clear old position
                        for (int i = 0; i < blockLength; i++)
                        {
                            Singleton.Instance.GameBoard[y, headX + i] = 0;
                            Singleton.Instance.BlockMap[y, headX + i] = null;
                        }

                        // Place the block in the new position
                        for (int i = 0; i < blockLength; i++)
                        {
                            Singleton.Instance.GameBoard[y, newHeadX + i] = 1;
                            Singleton.Instance.BlockMap[y, newHeadX + i] = selectedBlock;
                        }

                        var newPos = new Vector2(newHeadX * Singleton._TILESIZE, y * Singleton._TILESIZE);
                        selectedBlock.AnimationMove(newPos, 2f);

                        Singleton.Instance.PossibleClicked.Clear();
                        Singleton.Instance.CurrentGameState = Singleton.GameState.GameTurnEnded;
                    }
                }
                break;
            case Singleton.GameState.GameTurnEnded:

                Singleton.Instance.Timer = 20.8f; // Reset timer for next turn

                // while (true)
                // {
                //     for (int y = Singleton.GAMEHEIGHT - 2; y >= 0; y--)
                //     {
                //         for (int x = 0; x < Singleton.GAMEWIDTH; x++)
                //         {
                //             TryDropBlockDown(new Point(x, y));
                //         }
                //     }

                //     bool cleared = CheckAndClearFullRows();
                //     if (!cleared) break;
                // }

                while (true)
                {
                    // 1) DROP PHASE: รันการ drop ให้ครบกระดาน
                    for (int y = Singleton.GAMEHEIGHT - 2; y >= 0; y--)
                        for (int x = 0; x < Singleton.GAMEWIDTH; x++)
                            TryDropBlockDown(new Point(x, y));

                    // รอให้การ drop animation จบก่อน
                    if (IsAnyAnimationRunning())
                        return;

                    // 2) CLEAR PHASE: ลบแถวเต็มทีละแถว
                    bool cleared = CheckAndClearFullRows();
                    if (cleared)
                    {
                        // ถ้ามีการลบแถว ให้รอ clear animation จบก่อน
                        return;
                    }

                    // ถ้าไม่มีแถวให้ลบอีก → ออกจาก loop
                    break;
                }

                ShiftRowsUp(); // Shift rows when turn ends
                if (Singleton.Instance.CurrentGameState == Singleton.GameState.GameOver)
                {
                    // Console.WriteLine("[GAME OVER] Game Over due to shift up.");
                    return;
                }

                Singleton.Instance.CurrentGameState = Singleton.GameState.GamePlaying;

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
                    Singleton.Instance.CurrentGameState = Singleton.GameState.GamePlaying;
                    Reset();
                }
                break;
        }

        Singleton.Instance.PreviousMouse = Singleton.Instance.CurrentMouse;
        Singleton.Instance.PreviousKey = Singleton.Instance.CurrentKey;

        Console.WriteLine($"[GAME STATE] {Singleton.Instance.CurrentGameState}");

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
                    _drawing._DrawGamePlaying(_spriteBatch, _gameObjects, _numObjects);
                }
                break;
            case Singleton.GameState.GameWaitingForSelection:
                {
                    _drawing._DrawWaitingForSelect(_spriteBatch, _gameObjects, _numObjects);
                }
                break;
            case Singleton.GameState.GameTileSelected:
                {
                    _drawing._DrawTileSelected(_spriteBatch, _gameObjects, _numObjects);
                }
                break;
            case Singleton.GameState.GameTurnEnded:
                {
                    _drawing._DrawGameTurnEnded(_spriteBatch, _gameObjects, _numObjects);
                }
                break;
            case Singleton.GameState.GamePaused:
                {
                    _drawing._DrawGamePaused(_spriteBatch, _gameObjects, _numObjects);
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
        Singleton.Instance.Timer = 20.8f;

        _pendingShiftAfterClear = false;
        CreateRow(3);
    }

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

        // Find the head and tail of the block
        int headX = blockPosition.X;
        while (headX > 0 && Singleton.Instance.BlockMap[y, headX - 1] == selectedBlock)
            headX--;
        int tailX = headX + blockLength - 1;

        // Check if the block is at the top row
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

        // Check if the block is at the bottom row
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

    private bool CheckAndClearFullRows()
    {
        bool anyCleared = false;

        for (int y = 0; y < Singleton.GAMEHEIGHT - 1; y++)
        {
            bool fullRow = Enumerable.Range(0, Singleton.GAMEWIDTH)
                            .All(x => Singleton.Instance.BlockMap[y, x] != null);
            if (!fullRow) continue;

            anyCleared = true;
            var blocksToRemove = new HashSet<Block>();
            for (int x = 0; x < Singleton.GAMEWIDTH; x++)
                blocksToRemove.Add(Singleton.Instance.BlockMap[y, x]);

            // Update Score
            foreach (var b in blocksToRemove)
            {
                if (b.CurrentBlockType != Block.BlockType.Rock)
                    Singleton.Instance.Score += (int)b.CurrentBlockType + 1;
            }
            // Clear GameBoard and BlockMap
            for (int x = 0; x < Singleton.GAMEWIDTH; x++)
            {
                Singleton.Instance.GameBoard[y, x] = 0;
                Singleton.Instance.BlockMap[y, x] = null;
            }

            foreach (var b in blocksToRemove)
                b.AnimationClear(1f);

            y--;
            _numObjects = _gameObjects.Count;
            return true;
        }
        return anyCleared;
    }


    protected List<Point> FindPossibleClickedTiles(Point mousePosition)
    {
        List<Point> clickedTiles = new List<Point>();

        // Not a block
        if (Singleton.Instance.GameBoard[mousePosition.Y, mousePosition.X] != 1)
            return clickedTiles;

        // Not found the block or is a rock
        Block clickedBlock = Singleton.Instance.BlockMap[mousePosition.Y, mousePosition.X];
        if (clickedBlock == null || clickedBlock.CurrentBlockType == Block.BlockType.Rock)
            return clickedTiles;

        int y = mousePosition.Y;
        int x = mousePosition.X;

        // find headX of block
        while (x > 0 && Singleton.Instance.BlockMap[y, x - 1] == clickedBlock)
            x--;

        int headX = x;
        int blockLength = clickedBlock.GetLength();

        // query all position of block
        for (int i = 0; i < blockLength; i++)
        {
            Point pt = new Point(headX + i, y);
            clickedTiles.Add(pt);
            // Console.WriteLine($"[CLICK] Block at ({pt.X}, {pt.Y})");
        }

        return clickedTiles;
    }
    private bool IsGameOverByShift() // Use for check gameover
    {
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

        // Move up data in GameBoard and BlockMap
        for (int y = 0; y < Singleton.GAMEHEIGHT - 1; y++)
        {
            for (int x = 0; x < Singleton.GAMEWIDTH; x++)
            {
                Singleton.Instance.GameBoard[y, x] = Singleton.Instance.GameBoard[y + 1, x];
                Singleton.Instance.BlockMap[y, x] = Singleton.Instance.BlockMap[y + 1, x];
            }
        }

        // Call animation for each block to move up
        foreach (var obj in _gameObjects.OfType<Block>())
        {
            var block = obj;
            // Vector2 target = block.Position - new Vector2(0, Singleton._TILESIZE);
            Vector2 target = new Vector2(block.Position.X, block.Position.Y - Singleton._TILESIZE);
            // obj.Position = new Vector2(obj.Position.X, obj.Position.Y - Singleton._TILESIZE);
            block.AnimationMove(target, 2.5f);
        }
        CreateRow(1);
    }

    protected void CreateRow(int rowCount)
    {
        for (int k = 0; k < rowCount; k++)
        {
            int row = Singleton.GAMEHEIGHT - rowCount + k;
            int col = 0;

            while (col < Singleton.GAMEWIDTH)
            {
                // Random
                var selected = GetWeightedRandomBlockType();

                //  Calculate Length of Block
                int length = selected switch
                {
                    Block.BlockType.One => 1,
                    Block.BlockType.Two => 2,
                    Block.BlockType.Three => 3,
                    Block.BlockType.Four => 4,
                    Block.BlockType.Rock => 1,
                    _ => 1  // Blank
                };

                // If out of bounds, new Random
                if (col + length > Singleton.GAMEWIDTH)
                    continue;

                // In case of null (Blank)
                if (selected == null)
                {
                    Singleton.Instance.GameBoard[row, col] = 0;
                    Singleton.Instance.BlockMap[row, col] = null;
                    col += 1;
                }
                else
                {
                    // Create blocktype
                    var blockType = selected.Value;
                    Texture2D tex = blockType == Block.BlockType.Rock
                        ? Drawing.DogTextures[5]
                        : Drawing.DogTextures[(int)blockType + 1];

                    var block = new Block(tex)
                    {
                        CurrentBlockType = blockType,
                        Position = new Vector2(col * Singleton._TILESIZE, row * Singleton._TILESIZE)
                    };
                    _gameObjects.Add(block);

                    // Set the block pieces
                    for (int i = 0; i < length; i++)
                    {
                        block.Pieces[i] = new Vector2(i * Singleton._TILESIZE, 0);
                        Singleton.Instance.GameBoard[row, col + i] = 1;
                        Singleton.Instance.BlockMap[row, col + i] = block;
                    }
                    col += length;
                }
            }
        }
    }

    private void TryDropBlockDown(Point blockPosition)
    {
        int x = blockPosition.X;
        int y = blockPosition.Y;

        // Out of bounds check
        if (x < 0 || x >= Singleton.GAMEWIDTH || y < 0 || y >= Singleton.GAMEHEIGHT - 1)
            return;

        Block block = Singleton.Instance.BlockMap[y, x];
        if (block == null)
            return;

        int length = block.GetLength();

        // Find the head of the block
        int headX = x;
        while (headX > 0 && Singleton.Instance.BlockMap[y, headX - 1] == block)
            headX--;

        int tailX = headX + length - 1;

        // Find the lowest position the block can drop to
        int dropY = y;
        while (dropY + 1 < Singleton.GAMEHEIGHT - 1) // the last row is reserved for new blocks
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

        // Can't move down
        if (dropY == y)
            return;

        // Clear old position
        for (int i = 0; i < length; i++)
        {
            Singleton.Instance.GameBoard[y, headX + i] = 0;
            Singleton.Instance.BlockMap[y, headX + i] = null;
        }

        // Place the block in the new position
        for (int i = 0; i < length; i++)
        {
            Singleton.Instance.GameBoard[dropY, headX + i] = 1;
            Singleton.Instance.BlockMap[dropY, headX + i] = block;
        }

        block.AnimationMove(new Vector2(headX * Singleton._TILESIZE, dropY * Singleton._TILESIZE), 2.5f);
    }

    private bool IsAnyAnimationRunning()
    {
        return _gameObjects.OfType<Block>().Any(b => b.IsMoving || b.IsClearing);
    }
    private Block.BlockType? GetWeightedRandomBlockType()
    {
        // Weight for each block type (percentage change)
        var options = new List<(Block.BlockType? type, int weight)> {
        (Block.BlockType.One,   30),
        (Block.BlockType.Two,   20),
        (Block.BlockType.Three, 15),
        (Block.BlockType.Four,   10),
        (Block.BlockType.Rock,   5),
        (null,                 20) // null => Blank
    };

        int totalWeight = options.Sum(o => o.weight);
        int r = Singleton.Instance.Random.Next(totalWeight);
        int cumulative = 0;

        foreach (var opt in options)
        {
            cumulative += opt.weight;
            if (r < cumulative)
                return opt.type;
        }
        return null;
    }
}