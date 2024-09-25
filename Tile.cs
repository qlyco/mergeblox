namespace mergeblox
{
    public class Tile
    {
        private int _value = -1;
        private bool _merged;

        public int Value { get => _value; set => _value = value; }
        public bool Merged { get => _merged; set => _merged = value; }

        private static void RotateTile(Tile[] tiles, int width = 4)
        {
            List<Tile> rotated = [];

            for (int x = width - 1; x >= 0; x--)
            {
                for (int y = 0; y < width; y++)
                {
                    rotated.Add(tiles[x + y * width]);
                }
            }

            rotated.ToArray().CopyTo(tiles, 0);
        }

        internal static void Rotate(Tile[] tiles, int amount = 1, int width = 4)
        {
            for (int i = 0; i < amount; i++)
            {
                RotateTile(tiles, width:width);
            }
        }

        internal static void Clear(Tile[] tiles, bool all = false)
        {
            foreach (Tile tile in tiles)
            {
                if (all)
                {
                    tile.Value = -1;
                }

                tile.Merged = false;
            }
        }

        public static Tile At(Tile[] tiles, int x, int y, int width = 4)
        {
            return tiles[x + y * width];
        }

        public static bool HasMove(Tile[] tiles, int width = 4)
        {
            bool canMove = false;

            for (int y = 0; y < width; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    int cur = At(tiles, x, y, width).Value;

                    if (cur == -1)
                    {
                        canMove = true;
                    } else
                    {
                        if (y > 0)
                        {
                            canMove = canMove || At(tiles, x, y - 1, width).Value == cur;
                        }

                        if (x > 0)
                        {
                            canMove = canMove || At(tiles, x - 1, y, width).Value == cur;
                        }

                        if (x + 1 < width)
                        {
                            canMove = canMove || At(tiles, x + 1, y, width).Value == cur;
                        }

                        if (y + 1 < width)
                        {
                            canMove = canMove || At(tiles, x, y + 1, width).Value == cur;
                        }
                    }

                    if (canMove)
                    {
                        break;
                    }
                }
            }

            return canMove;
        }

        internal static MoveInfo Shift(Tile[] tiles, int width = 4)
        {
            MoveInfo info = new();

            for (int y = 0; y < width; y++)
            {
                for (int x = 1; x < width; x++)
                {
                    Move(tiles, x, y, info, width:width);
                }
            }

            Clear(tiles);

            return info;
        }

        private static MoveInfo Move(Tile[] tiles, int x, int y, MoveInfo info, int width = 4)
        {
            const int MAX_SCORE = 12;

            if (x == 0)
            {
                return info;
            } else
            {
                Tile cur = At(tiles, x, y, width);
                Tile prev = At(tiles, x - 1, y, width);

                if (prev.Value == -1 && cur.Value != -1)
                {
                    prev.Value = cur.Value;
                    prev.Merged = cur.Merged;
                    cur.Value = -1;
                    cur.Merged = false;

                    info.Shifted = true;
                } else if (prev.Value == cur.Value && (!prev.Merged && !cur.Merged) && prev.Value != -1)
                {
                    prev.Value = prev.Value + 1 >= MAX_SCORE ? -1 : prev.Value + 1;
                    prev.Merged = prev.Value != -1;
                    info.Score += prev.Value != -1 ? prev.Value * 2 : MAX_SCORE * 2;
                    cur.Value = -1;
                    cur.Merged = false;

                    info.Shifted = true;
                }

                return Move(tiles, x - 1, y, info, width);
            }
        }

        internal static void AddTile(Tile[] tiles)
        {
            List<Tile> empty = [];

            foreach (Tile tile in tiles)
            {
                if (tile.Value == -1)
                {
                    empty.Add(tile);
                }
            }

            int idx = Game.randomizer.Next(empty.Count);
            empty[idx].Value = Game.randomizer.Next(2);
        }
    }
}
