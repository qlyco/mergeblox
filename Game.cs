using System.ComponentModel;

namespace mergeblox
{
    public enum Direction
    {
        LEFT, UP, RIGHT, DOWN
    }

    public class Game
    {
        public static Random randomizer = new();
        
        private int _width;
        private Tile[] _tiles;
        private int _score = 0;
        private bool _running = false;

        public Tile[] Tiles => _tiles;
        public int Score { get => _score; set => _score = value; }
        public bool Running { get => _running; set => _running = value; }
        public int Width { get => _width; }

        public Game(int seed = -1, int width = 4) {
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

        private void Setup()
        {
            for (int i = 0; i < _tiles.Length; i++)
            {
                _tiles[i] = new Tile();
            }
        }

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

            if (info.Shifted)
            {
                Tile.AddTile(_tiles);
            }

            if (!Tile.HasMove(_tiles, width: _width))
            {
                _running = false;
            }
        }
    }
}
