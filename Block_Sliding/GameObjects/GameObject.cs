using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Dog_Sliding
{
    public class GameObject : ICloneable
    {
        protected Texture2D _texture;
        public Vector2 Position;
        public float Rotation;
        public Vector2 Scale;
        public Vector2 Velocity;
        public string Name; 
        public bool IsActive; 
        public Rectangle Viewport; 

        public Rectangle Rectangle
        {
            get { return new Rectangle((int)Position.X, (int)Position.Y, Viewport.Width, Viewport.Height); }
        }

        public GameObject()
        {
            Position = Vector2.Zero;
            Scale = Vector2.One;
            Rotation = 0f;
            IsActive = true;
        }

        public GameObject(Texture2D texture)
        {
            _texture = texture;
            Position = Vector2.Zero;
            Scale = Vector2.One;
            Rotation = 0f;
            IsActive = true;
        }
        
        public virtual void Update(GameTime gameTime)
        {

        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {

        }

        public virtual void Reset()
        {

        }

        public virtual object Clone()
        {
            return this.MemberwiseClone();
        }
        
    }
}
