namespace mergeblox
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int width;

            if (args.Length > 0)
            {
                if (!int.TryParse(args[0], out width))
                {
                    width = 5;
                }
            } else
            {
                width = 5;
            }

            Game game = new(width:width);

            while (game.Running)
            {
                Console.Clear();
                Console.WriteLine(string.Format("Score: {0}pts\n", game.Score));
                DrawGrid(game.Tiles, game.Width);
                Console.WriteLine("\nWASD/Arrows = Move\nEsc = Exit\nR = Reset");

                ConsoleKey key = Console.ReadKey().Key;

                switch (key)
                {
                    case ConsoleKey.W:
                    case ConsoleKey.UpArrow:
                        game.Move(Direction.UP);
                        break;
                    case ConsoleKey.A:
                    case ConsoleKey.LeftArrow:
                        game.Move(Direction.LEFT);
                        break;
                    case ConsoleKey.S:
                    case ConsoleKey.DownArrow:
                        game.Move(Direction.DOWN);
                        break;
                    case ConsoleKey.D:
                    case ConsoleKey.RightArrow:
                        game.Move(Direction.RIGHT);
                        break;
                    case ConsoleKey.R:
                        game.Reset();
                        break;
                    case ConsoleKey.Q:
                        Tile.Rotate(game.Tiles);
                        break;
                    case ConsoleKey.Escape:
                        game.Running = false;
                        break;
                }
            }

            Console.Clear();
            Console.WriteLine(string.Format("GAME OVER\n"));
            DrawGrid(game.Tiles, game.Width);
            Console.WriteLine(string.Format("\nYour final score: {0}pts.\n\n[PRESS ENTER TO EXIT]", game.Score));

            while (Console.ReadKey(true).Key != ConsoleKey.Enter)
            {
                Thread.Sleep(100);
            }
        }

        static void DrawGrid(Tile[] tiles, int width = 4)
        {
            for (int y = 0; y < width; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    int val = Tile.At(tiles, x, y, width).Value;
                    if (val == -1)
                    {
                        Console.Write("    ");
                    }
                    else
                    {
                        Console.Write(string.Format("[{0:D2}]", val + 1));
                    }
                }

                Console.WriteLine();
            }
        }
    }
}
