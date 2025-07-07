using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Block_Sliding
{
    public class Block : GameObject
    {
        public enum BlockType
        {
            One,
            Two,
            Three,
            Four,
            Rock
        }
        public BlockType CurrentBlockType;
        public Vector2[] Pieces;

        public Block(Texture2D texture) : base(texture)
        {
            Pieces = new Vector2[4];
        }

        public override void Update(GameTime gameTime)
        {

            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i <= 3; i++)
            {
                spriteBatch.Draw(_texture, Pieces[i] + Position, null, Color.Lime, 0f, Vector2.Zero, Singleton._TILESIZE - 1, SpriteEffects.None, 0f);
            }

            base.Draw(spriteBatch);
        }

        public override void Reset()
        {
            CurrentBlockType = (BlockType)Singleton.Instance.Random.Next(5);

            switch (CurrentBlockType)
            {
                case BlockType.One:
                    Pieces[0] = new Vector2(0, 0);
                    break;
                case BlockType.Two:
                    Pieces[0] = new Vector2(0, 0);
                    Pieces[1] = new Vector2(Singleton._TILESIZE, 0);
                    break;
                case BlockType.Three:
                    Pieces[0] = new Vector2(0, 0);
                    Pieces[1] = new Vector2(Singleton._TILESIZE, 0);
                    Pieces[2] = new Vector2(Singleton._TILESIZE * 2, 0);
                    break;
                case BlockType.Four:
                    Pieces[0] = new Vector2(0, 0);
                    Pieces[1] = new Vector2(Singleton._TILESIZE, 0);
                    Pieces[2] = new Vector2(Singleton._TILESIZE * 2, 0);
                    Pieces[3] = new Vector2(Singleton._TILESIZE * 3, 0);
                    break;
                case BlockType.Rock:
                    Pieces[0] = new Vector2(0, 0);
                    break;
            }

            // int _tileCount = GetLength();

            // if (_tileCount >= 1 && _tileCount <= 4)
            // {
            //     _texture = Drawing.DogTextures[_tileCount];
            // }
            // else
            // {
            //     // กรณี Rock หรืออื่นๆ ถ้าอยากใช้ Texture อื่น ก็เปลี่ยนตรงนี้ได้
            //     _texture = Drawing.DogTextures[1];
            // }

            Velocity = new Vector2(10f, 1f);

            base.Reset();
        }

        

        public int GetLength()
        {
            return CurrentBlockType switch
            {
                BlockType.One => 1,
                BlockType.Two => 2,
                BlockType.Three => 3,
                BlockType.Four => 4,
                _ => 0
            };
        }
    }
}