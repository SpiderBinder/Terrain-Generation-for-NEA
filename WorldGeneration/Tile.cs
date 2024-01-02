using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Terrain_Generation.WorldGeneration
{
    internal class Tile
    {
        private Texture2D[] _textures;
        private int _currentTexture;
        private bool _modular;

        public Tile(Texture2D texture)
        {
            _textures = new Texture2D[] { texture };
            _currentTexture = 0;
            _modular = false;
        }
        public Tile(Texture2D[] textures)
        {
            _textures = textures;
            _currentTexture = 0;
            _modular = true;
        }

        public void Update()
        {
            if (_modular)
            {

            }
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            spriteBatch.Draw(
                _textures[_currentTexture],
                position,
                Color.White);
        }
    }
}
