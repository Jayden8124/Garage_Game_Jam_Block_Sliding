using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Block_Sliding
{
    public class Singleton
    {
        // Size of the screen
        public const int SCREENWIDTH = 1280;
        public const int SCREENHEIGHT = 720;

        // Rectacngle Size
        public const int _TILESIZE = 60;
        public const int GAMEWIDTH = 9;
        public const int GAMEHEIGHT = 10;

        // Utility variables
        public int Score;
        public long Timer;
        public Random Random;
        public int[,] GameBoard;
        public Block[,] BlockMap;
        public Point _selectedTile;
        public List<Point> _possibleClicked;
        public Point _clickedPos;
        public Texture2D _rect;

        // Game state
        public enum GameState
        {
            GameStart,
            GamePlaying,
            WaitingForSelection,
            TileSelected,
            TurnEnded,
            GamePaused,
            GameOver
        }
        public GameState CurrentGameState;

        // Input State
        public KeyboardState PreviousKey, CurrentKey;
        public MouseState PreviousMouse, CurrentMouse;


        // Singleton instance
        private static Singleton instance;
        private Singleton() { }
        public static Singleton Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Singleton();
                }
                return instance;
            }
        }
    }
}