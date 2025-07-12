using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Dog_Sliding
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
        private Vector2 _startPosition;
        private Vector2 _targetPosition;
        private float _moveElapsed, _moveDuration;
        private float _clearElapsed, _clearDuration;
        public bool IsMoving { get; private set; }
        public bool IsClearing { get; private set; }

        public Block(Texture2D texture) : base(texture)
        {
            Pieces = new Vector2[4];
        }

        public override void Update(GameTime gameTime)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (IsMoving)
            {
                _moveElapsed += dt;
                float raw = Math.Min(_moveElapsed / _moveDuration, 1f);
                float t = MathHelper.SmoothStep(0, 1, raw);
                Position = Vector2.Lerp(_startPosition, _targetPosition, t);
                if (raw >= 1f) IsMoving = false;
            }
            else if (IsClearing)
            {
                _clearElapsed += dt;
                float t = Math.Min(_clearElapsed / _clearDuration, 1f);
                Scale = new Vector2(1f - t, 1f - t);

                if (t >= 1f)
                    IsClearing = false;
            }

            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            int length = GetLength();
            Vector2 origin = new Vector2(0, 0);
            Rectangle dest = new Rectangle(
                (int)Position.X,
                (int)Position.Y,
                (int)(Singleton._TILESIZE * length * Scale.X),
                (int)(Singleton._TILESIZE * Scale.Y)
            );
            spriteBatch.Draw(_texture, dest, Color.White);
            base.Draw(spriteBatch);
        }

        public override void Reset()
        {
            CurrentBlockType = (BlockType)Singleton.Instance.Random.Next(5);

            for (int i = 0; i < 4; i++)
                Pieces[i] = Vector2.Zero;

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
                BlockType.Rock => 1,
                _ => 0
            };
        }

        public void AnimationMove(Vector2 newTarget, float duration) // Animation Move Block
        {
            _startPosition = Position;
            _targetPosition = newTarget;
            _moveDuration = duration;
            _moveElapsed = 0f;
            IsMoving = true;
        }

        public void AnimationClear(float duration) // Animation Clear Block
        {
            _clearDuration = duration;
            _clearElapsed = 0f;
            IsClearing = true;
        }
    }
}