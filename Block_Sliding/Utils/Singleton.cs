using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Dog_Sliding
{
    public class Singleton
    {
        // Size of the screen
        public const int SCREENWIDTH = 1280;
        public const int SCREENHEIGHT = 720;

        // Game Size
        public const int _TILESIZE = 60;
        public const int GAMEWIDTH = 9;
        public const int GAMEHEIGHT = 10;

        // Utility variables
        public int Score;
        public float Timer;
        public Random Random;
    
        // Game Set Up
        public int[,] GameBoard; // use for mapping 0 (null) , 1 (block)
        public Block[,] BlockMap;
        public Point SelectedTile;
        public List<Point> PossibleClicked;
        public Point ClickedPos;
        public Texture2D _Rect;

        // Game state
        public enum GameState
        {
            GameStart,
            GamePlaying,
            GameWaitingForSelection,
            TileSelected,
            GameTurnEnded,
            GamePaused,
            GameOver
        }
        public GameState CurrentGameState;

        // Input State
        public KeyboardState PreviousKey, CurrentKey;
        public MouseState PreviousMouse, CurrentMouse;

        public Audio AudioManager;

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