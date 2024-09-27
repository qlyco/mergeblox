/*
 * Program.cs : Entry point for the program
 * 
 * This is a CLI version of mergeblox. This can be a reference on how to implement a custom
 * front-end/client for the game.
 */

namespace mergeblox
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int width;

            // Check if the user sets a custom grid size. Defaults to 5x5
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

            // Game loop
            while (game.Running)
            {
                // Render current game state
                Console.Clear();
                Console.WriteLine(string.Format("Score: {0}pts\n", game.Score));
                DrawGrid(game.Tiles, game.Width);
                Console.WriteLine("\nWASD/Arrows = Move\nEsc = Exit\nR = Reset");

                // Handle input & update the game state
                ConsoleKey key = Console.ReadKey(true).Key;

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
                        Tile.Rotate(game.Tiles, width:game.Width);
                        break;
                    case ConsoleKey.Escape:
                        game.Running = false;
                        break;
                }
            }

            // End screen
            Console.Clear();
            Console.WriteLine(string.Format("GAME OVER\n"));
            DrawGrid(game.Tiles, game.Width);
            Console.WriteLine(string.Format("\nYour final score: {0}pts.\n\n[PRESS ENTER TO EXIT]", game.Score));

            // Wait for user to exit
            while (Console.ReadKey(true).Key != ConsoleKey.Enter)
            {
                Thread.Sleep(100);
            }
        }

        // This method draws the entire grid into the console with correct formatting
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
