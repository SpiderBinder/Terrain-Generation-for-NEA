using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Terrain_Generation.PlayerLogic;
using Terrain_Generation.WorldGeneration;

namespace Terrain_Generation
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private Camera _camera;
        private SpriteFont _spriteFont;
        public static int ScreenHeight;
        public static int ScreenWidth;

        private Generation _generation;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            _graphics.PreferredBackBufferWidth = 1920;
            _graphics.PreferredBackBufferHeight = 1080;
            _graphics.IsFullScreen = false;
            _graphics.ApplyChanges();

            ScreenHeight = _graphics.PreferredBackBufferHeight;
            ScreenWidth = _graphics.PreferredBackBufferWidth;
        }

        protected override void Initialize()
        {
            _camera = new Camera();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            Texture2D[] tileTextures =
            {
                Content.Load<Texture2D>("GrassSprite"),
                Content.Load<Texture2D>("DirtSprite"),
                Content.Load<Texture2D>("StoneSprite"),
                Content.Load<Texture2D>("SnowedDirtSprite")
            };
            _generation = new Generation(tileTextures, 3, 32);

            _camera.Texture = Content.Load<Texture2D>("CrosshairSprite");
            _spriteFont = Content.Load<SpriteFont>("InfoDisplay");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            _camera.Update(gameTime);
            _generation.Update(_camera.Position);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin(transformMatrix: _camera.Transform);

            _generation.Draw(_spriteBatch,
                _camera.Position - new Vector2(ScreenWidth / 2, ScreenHeight / 2),
                _spriteFont);
            _camera.Draw(_spriteBatch);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}