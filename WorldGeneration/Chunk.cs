using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Terrain_Generation.WorldGeneration
{
    internal class Chunk : IComparable<Chunk>
    {
        public long ChunkID { get; }
        public Vector2 Coords;
        private Tile[,] _tiles;
        private int _tileSize;

        public Chunk(long chunkID)
        {
            ChunkID = chunkID;
        }
        public Chunk(Vector2 coords, Tile[,] tiles, int chunkSize, int tileSize)
        {
            Coords = coords;
            _tileSize = tileSize;
            _tiles = tiles;

            string x = Convert.ToString((int)coords.X, 2);
            x = "00000000000000000000000000000000".Substring(x.Length) + x;
            string y = Convert.ToString((int)coords.Y, 2);
            y = "00000000000000000000000000000000".Substring(y.Length) + y;

            ChunkID = Convert.ToInt64(x + y, 2);
        }

        public int CompareTo(Chunk chunk)
        {
            return ChunkID.CompareTo(chunk.ChunkID);
        }

        // TODO: Add functionality for multiple different tiles depending on surrounding tiles
        public void Update()
        {

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < _tiles.GetLength(0); i++)
            {
                for (int j = 0; j < _tiles.GetLength(1); j++)
                {
                    if (_tiles[i, j] != null)
                    {
                        _tiles[i, j].Draw(
                            spriteBatch,
                            new Vector2(Coords.X + i, Coords.Y + j) * _tileSize);
                    }
                }
            }
        }
    }
}
