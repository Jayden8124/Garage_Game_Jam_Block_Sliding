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
        public int GAMEWIDTH = 9;
        public int GAMEHEIGHT = 10;
        public Texture2D _rect;
        public int _TILESIZE = 60;

        // Utility variables
        public int Score;
        public long Timer;
        public Random Random;
        public int[,] GameBoard;
        

        // Game state
        public enum GameState
        {
            GameStart,
            GamePlaying,
            GameSelection,
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