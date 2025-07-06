using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Block_Sliding
{
    class Block : GameObject
    {
        Vector2 _tick;
        public bool IsHitGround;
        public bool IsDead;
        public enum BlockType
        {
            One,
            Two,
            Three,
            Four,
            Rock
        }
        public BlockType CurrentBlockType;
        public Vector2[,] Pieces;
        
        public Block(Texture2D texture) : base(texture)
        {
            Pieces = new Vector2[4, 4];
            IsHitGround = false;
            IsDead = false;
        }

        // public override void Update(GameTime gameTime)
        // {
           
        //     // base.Update(gameTime);
        // }

        public override void Draw(SpriteBatch spriteBatch)
        {
            
            base.Draw(spriteBatch);
        }

        public override void Reset()
        {
            

            base.Reset();
        }

        // private bool CheckHit()
        // {

        // }

        // private bool CheckDead()
        // {

        // }
    }
}