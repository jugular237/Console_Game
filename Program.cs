using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Console_Game
{
    class Program
    {
        private const int FieldSizeX = 95;
        private const int FieldSizeY = 25;
        static void Main(string[] args)
        {
            InitializeField();
            InitializePlayer();
            Console.ReadKey();
        }

        private static void InitializePlayer()
        {
            Player player = new Player();
            player.DrawPlayer();
        }

        private static void InitializeField()
        {
            Console.SetWindowSize(FieldSizeX, FieldSizeY);
            Console.SetBufferSize(FieldSizeX, FieldSizeY);
            Console.CursorVisible = false;
            DrawBorders();
        }

        private static void DrawBorders()
        {
            for(int i = 0; i < FieldSizeY; i++)
            {
                for (int j = 0; j < FieldSizeX; j++)
                {
                    if (i == 1 || i == FieldSizeY - 1)
                    {
                        Console.SetCursorPosition(j, i);
                        Console.Write('-');
                    }
                    else if (j == 0 || j == FieldSizeX - 1)
                    {
                        Console.SetCursorPosition(j, i);
                        Console.Write('|');
                    }
                }
                
            }
                
        }
    }
}
