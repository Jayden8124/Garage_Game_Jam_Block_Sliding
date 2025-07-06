using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Block_Sliding
{
    public class BubbleGrid : GameObject
    {
        public int Row;
        public int Col;

        public BubbleGrid(Texture2D texture) : base(texture)
        {
        }

        public override void Update(GameTime gameTime, System.Collections.Generic.List<GameObject> gameObjects)
        {
            base.Update(gameTime, gameObjects);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, Position, Viewport, Color.White);
            base.Draw(spriteBatch);
        }
    }
}
