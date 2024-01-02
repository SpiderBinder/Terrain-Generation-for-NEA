using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Terrain_Generation.PlayerLogic
{
    // Credit to Oyyou's Monogame Tutorial series
    // Specifically the section on camera mechanics in monogame for this section
    internal class Camera
    {
        public Matrix Transform { get; private set; }
        public Vector2 Position;
        public Texture2D Texture;
        private float _speed;

        public Camera()
        {
            Position = new Vector2();
            _speed = 0.5f;
        }

        public void Update(GameTime gameTime)
        {
            Vector2 velocity = new Vector2();

            if (Keyboard.GetState().IsKeyDown(Keys.W)) { velocity.Y = -_speed; }
            if (Keyboard.GetState().IsKeyDown(Keys.S)) { velocity.Y = _speed; }
            if (Keyboard.GetState().IsKeyDown(Keys.A)) { velocity.X = -_speed; }
            if (Keyboard.GetState().IsKeyDown(Keys.D)) { velocity.X = _speed; }

            velocity *= gameTime.ElapsedGameTime.Milliseconds;
            Position += velocity;

            Matrix pos = Matrix.CreateTranslation(
                -this.Position.X,
                -this.Position.Y,
                0);

            Matrix offset = Matrix.CreateTranslation(
                Game1.ScreenWidth / 2,
                Game1.ScreenHeight / 2,
                0);

            Transform = pos * offset;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                Texture,
                Position - new Vector2(Texture.Width / 2, Texture.Height / 2),
                Color.White);
        }
    }
}
