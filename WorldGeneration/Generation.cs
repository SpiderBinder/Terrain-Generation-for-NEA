using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics;
using System.Text;
using System.Threading.Tasks;

namespace Terrain_Generation.WorldGeneration
{
    internal class Generation
    {
        private List<Chunk> _totalChunks;
        private Chunk[,] _loadedChunks;
        private Vector2 _previousPos = new Vector2(100, 100);
        private int _renderRadius;
        private int _chunkSize;
        private int _tileSize;
        // Temporary thingo (as the name implies)
        private Texture2D[] _tileTextures;
        private string[] _memoryInfo;

        public Generation(Texture2D[] tileTextures, int renderRadius, int chunkSize)
        {
            _totalChunks = new List<Chunk>();
            _chunkSize = chunkSize;
            _tileSize = tileTextures[0].Width;
            _renderRadius = renderRadius;
            _loadedChunks = new Chunk[renderRadius * 2, renderRadius * 2];
            _tileTextures = tileTextures;
        }

        // (Currently) Generates the height values for a given chunk and its position and fills in the chunk grid
        private Chunk GenerateChunk(Vector2 chunkPos)
        {
            Tile[,] tiles = new Tile[_chunkSize, _chunkSize];

            for (int i = 0; i < _chunkSize; i++)
            {
                int height = (int)GenEquation(chunkPos.X + i);

                int chunkHeight = (int)(height - (chunkPos.Y + _chunkSize));

                // TODO: Fix this shit plz
                for (int j = _chunkSize - 1; j >= chunkHeight && j >= 0; j--)
                {
                    int dirtHeight = (int)chunkPos.X % 3 == 0 ? 3 : 4;

                    if (j > (chunkHeight + dirtHeight)) { tiles[i, j] = new Tile(_tileTextures[2]); }
                    else if (j > chunkHeight) { tiles[i, j] = new Tile(_tileTextures[1]); }
                    else if (j == chunkHeight) 
                    {
                        if (height <= 0)
                        {
                            tiles[i, j] = new Tile(_tileTextures[3]);
                        }
                        else
                        {
                            tiles[i, j] = new Tile(_tileTextures[0]);
                        }
                    }
                }
            }

            return new Chunk(chunkPos, tiles, _chunkSize, _tileSize);
        }

        // Equation for generating elevation values, multiple Noise calls for varying scales
        private double GenEquation(float x)
        {
            return (float)(
                Noise(x, 1, 0.2) +
                Noise(x, 8, 2) +
                Noise(x, 30, 8) +
                Noise(x, 150, 35));
        }

        // 1D Perlin Noise Equation
        private double Noise(double x, double width, double height)
        {
            x /= width;
            return height * (
                Math.Sin(x) +
                Math.Sin(Math.E * x) +
                Math.Sin(Math.PI * x));
        }

        // Loads chunks surrounding the camera object
        private void LoadChunks(Vector2 chunkPos)
        {
            Vector2 position;

            // Goes through every position in '_loadedChunks' to add or generate a chunk
            for (int i = 0; i < _renderRadius * 2; i++)
            {
                for (int j = 0; j < _renderRadius * 2; j++)
                {
                    // Creating a ChunkID from X and Y coordinates to check for pregenerated chunks
                    position = chunkPos + new Vector2(
                        (i - _renderRadius) * _chunkSize,
                        (j - _renderRadius) * _chunkSize);

                    string x = Convert.ToString((int)position.X, 2);
                    x = "00000000000000000000000000000000".Substring(x.Length) + x;
                    string y = Convert.ToString((int)position.Y, 2);
                    y = "00000000000000000000000000000000".Substring(y.Length) + y;

                    Chunk tempChunk = new Chunk(Convert.ToInt64(x + y, 2));

                    // Checking for pregenerated chunks, loading them from chunk list or generating one if none exist
                    int num = _totalChunks.BinarySearch(tempChunk);
                    if (num > 0)
                    {
                        _loadedChunks[i, j] = _totalChunks[num];
                    }
                    else
                    {
                        _loadedChunks[i, j] = GenerateChunk(position);
                        _totalChunks.Add(_loadedChunks[i, j]);
                        _totalChunks.Sort();
                    }
                }
            }
        }

        // Checks for a substantial change in the camera's position and if detected calls 'LoadChunks' to create/load new chunks
        public void Update(Vector2 cameraPos)
        {
            // Rounds the camera position to the nearest multiple of '_chunkSize'
            cameraPos /= _tileSize;
            Vector2 chunkPos = cameraPos / _chunkSize;
            chunkPos.X = Convert.ToInt32(chunkPos.X);
            chunkPos.Y = Convert.ToInt32(chunkPos.Y);
            
            // Checks for a change in position
            if (chunkPos != _previousPos)
            {
                LoadChunks(chunkPos * _chunkSize);
            }

            foreach (Chunk chunk in _loadedChunks)
            {
                chunk.Update();
            }

            // Records chunk memory info
            _memoryInfo = new string[] {$"Total Chunks: {_totalChunks.Count}",
                $"Loaded Chunks: {_loadedChunks.Length}",
                $"Camera Position: {cameraPos}" };

            _previousPos = chunkPos;
        }

        // Calls the Draw method for every chunk object in '_loadedChunks'
        public void Draw(SpriteBatch spriteBatch, Vector2 position, SpriteFont spriteFont)
        {
            foreach (Chunk chunk in _loadedChunks)
            {
                chunk.Draw(spriteBatch);
            }

            for (int i = 0; i < _memoryInfo.Length; i++)
            {
                spriteBatch.DrawString(spriteFont, _memoryInfo[i], position + new Vector2(0, i * 20), Color.White);
            }
        }
    }
}
