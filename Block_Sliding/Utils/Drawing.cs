using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Block_Sliding
{
    public class Drawing
    {
        // Graphics Device
        public GraphicsDevice GraphicsDevice { get; private set; }

        // Font
        public SpriteFont Font { get; private set; }

        // Textures
        public Texture2D Background1 { get; private set; }
        public Texture2D Background2 { get; private set; }
        
        public Drawing(GraphicsDevice graphicsDevice)
        {
            GraphicsDevice = graphicsDevice;

            // // Button Rectangles for Contain Mouse Destination
            // MenuPlay = new Rectangle(565, 410, 165, 98);
            // MenuExit = new Rectangle(565, 524, 165, 98);
            // Setting = new Rectangle(1175, 53, 35, 35);
            // PauseExit = new Rectangle(565, 364, 167, 100);
            // PauseResume = new Rectangle(565, 250, 167, 100);
        }

        public void LoadContent(ContentManager Content)
        {
            // {   // Main Menu
            //     MenuBackground = Content.Load<Texture2D>("bg_menu");
            //     MenuIcon = Content.Load<Texture2D>("icon_game");
            //     MenuButton = Content.Load<Texture2D>("menu");
            // }

            // {   // Font
            //     Font = Content.Load<SpriteFont>("game_font");
            // }

            // {   // Load Content
            //     Singleton.Instance._rect = new Texture2D(this.GraphicsDevice, 20, 20);
            //     Color[] data = new Color[20 * 20];
            //     for (int i = 0; i < data.Length; i++) data[i] = Color.White;
            //     Singleton.Instance._rect.SetData(data);
            // }
        }

        public void _DrawGameStart(SpriteBatch _spriteBatch)
        {
            // Layer 1: Background
            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            _spriteBatch.End();

            // Layer 2: Map & Camera
            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            _spriteBatch.End();
        }

        public void _DrawGamePlaying(SpriteBatch _spriteBatch)
        {
            // Layer 1: Background
            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            _spriteBatch.End();

            // Layer 2: Map & Camera
            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            _spriteBatch.End();
        }

        public void _DrawGameSelection(SpriteBatch _spriteBatch)
        {
            // Layer 1: Background
            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            _spriteBatch.End();

            // Layer 2: Map & Camera
            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            _spriteBatch.End();
        }

        public void _DrawGameOver(SpriteBatch _spriteBatch)
        {
            // Layer 1: Background
            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            _spriteBatch.End();

            // Layer 2: Map & Camera
            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            _spriteBatch.End();
        }

        public void Update(GameTime gameTime)
        {

        }
    }
}
