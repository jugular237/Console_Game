using System;
using static Constants;
using static BasicStats;
using System.Diagnostics;


namespace Console_Game
{
    class Program
    {
        static Player player = Player.GetInstance();
        static ResultsMenu resMenu = new ResultsMenu();
        static Stopwatch sw = new Stopwatch();

        
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.Unicode;
            BeginTrial(new Game());
        }

        static void BeginTrial(Game game)
        {
            sw.Restart();
            game.StartGame();
            sw.Stop();
            AftermatchResults();
        }
        static void AftermatchResults()
        {
            Console.Clear();
            Console.WriteLine("YOU LOST");
            long timeResult = sw.ElapsedMilliseconds / 1000;
            resMenu.WriteNewRecords(Game.killsCounter, timeResult);
            ConsoleKey key = Console.ReadKey(true).Key;
            if (key == ConsoleKey.Spacebar)
                RestartGame();
        }
        static void RestartGame()
        {
            Console.Clear();
            player.Health = 5;
            player.isDead = false;
            Game.killsCounter = 0;
            player.direction = Direction.None;
            BeginTrial(new Game());
        }
    }
}
