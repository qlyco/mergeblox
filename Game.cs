/*
 * Game.cs : The main implementation of the game
 * 
 * This class provides everything that's needed to build the game.
 * It allows for custom seed, and customizable grid sizes.
 * Manipulate the instance of this class to operate the game.
 * Any front-end/client simply need to provide a way to let the user
 * call the Move, and Reset method to make them play the game.
 */

namespace mergeblox
{
    // Direction enum is used to tell the Move method which way to shift the tiles
    public enum Direction
    {
        LEFT, UP, RIGHT, DOWN
    }

    public class Game
    {
        internal static Random randomizer = new();
        
        private readonly int _width;
        private readonly Tile[] _tiles;
        private int _score = 0;
        private bool _running = false;

        public Tile[] Tiles => _tiles;
        public int Score { get => _score; set => _score = value; }
        public bool Running { get => _running; set => _running = value; }
        public int Width { get => _width; }

        // By default, use random seed and 5x5 grid
        public Game(int seed = -1, int width = 5) {
            if (seed <= 0)
            {
                randomizer = new Random();
            } else
            {
                randomizer = new Random(seed);
            }

            _width = width;

            _tiles = new Tile[width * width];

            Setup();
            Reset();
        }

        // Setup the grid, automatically called
        private void Setup()
        {
            for (int i = 0; i < _tiles.Length; i++)
            {
                _tiles[i] = new Tile();
            }
        }

        // Reset the board to a new state. Useful for restarting the game
        public void Reset()
        {
            _running = true;

            Tile.Clear(_tiles, true);

            _score = 0;

            Tile.AddTile(_tiles);

            if (randomizer.Next(2) == 1)
            {
                Tile.AddTile(_tiles);
            }
        }

        // Move all the tiles in a direction. Accepts which direction to move the tiles
        public void Move(Direction dir)
        {
            if (!_running)
            {
                return;
            }

            MoveInfo info;

            switch(dir)
            {
                case Direction.LEFT:
                    info = Tile.Shift(_tiles, width:_width);
                    break;
                case Direction.UP:
                    Tile.Rotate(_tiles, width:_width);
                    info = Tile.Shift(_tiles, width: _width);
                    Tile.Rotate(_tiles, amount:3, width: _width);
                    break;
                case Direction.RIGHT:
                    Tile.Rotate(_tiles, amount:2, width: _width);
                    info = Tile.Shift(_tiles, width: _width);
                    Tile.Rotate(_tiles, amount:2, width: _width);
                    break;
                case Direction.DOWN:
                    Tile.Rotate(_tiles, amount:3, width: _width);
                    info = Tile.Shift(_tiles, width: _width);
                    Tile.Rotate(_tiles, width: _width);
                    break;
                default:
                    info = new();
                    break;
            }

            _score += info.Score;

            // If the move shifted/merged some tiles, spawn new tiles
            if (info.Shifted)
            {
                Tile.AddTile(_tiles);
            }

            // Check if there's any valid moves. End the game if it isn't
            if (!Tile.HasMove(_tiles, width: _width))
            {
                _running = false;
            }
        }
    }
}
