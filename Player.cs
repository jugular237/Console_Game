using System;
using System.Threading;

public class Player:BasicStats
{
    private const int XCoord = 45;
    private const int YCoord = 20;
    
    private Direction direction;
    
    public override void AnimateCreature()
    {
        Console.SetCursorPosition(XCoord, YCoord);
        do
        {
            
            ConsoleKeyInfo keyInfo = Console.ReadKey();
            ConsoleKey key = keyInfo.Key;
            if (key == ConsoleKey.UpArrow)
            {
                direction = Direction.up;
                ClearSpace(XCoord - 17, YCoord);
                DrawPlayer(XCoord - 1, YCoord, XCoord, XCoord + 2, true,
                    new DrawHero
                    {
                        OverHead2 = @"||",
                        OverHead1 = @"oo",
                        Head = @"(▀ ͜ʖ▀)  \\",
                        BodyNGun = @"|   | /",
                        Legs = @"/ \"
                    });
               
                Thread thread1 = new Thread(Shooting);
                thread1.Start();
            }
            else if (key == ConsoleKey.LeftArrow)
            {
                direction = Direction.left;
                ClearSpace(XCoord - 17, YCoord);
                DrawPlayer(XCoord - 9, YCoord, XCoord - 5, XCoord + 2, false,
                    new DrawHero
                    {
                        Head = @"8======* (▀ ͜ʖ▀)",
                        BodyNGun = @"\\ -- |   |",
                        Legs = @"/ \"
                    });
                
                Thread thread2 = new Thread(Shooting);
                thread2.Start();
            }
            else if (key == ConsoleKey.RightArrow)
            {
                direction = Direction.right;
                ClearSpace(XCoord - 17, YCoord);
                DrawPlayer(XCoord, YCoord, XCoord + 1, XCoord + 2, false,
                    new DrawHero
                    {
                        Head = @"(▀ ͜ʖ▀) *======8",
                        BodyNGun = @"|   | -- //",
                        Legs = @"/ \"
                    });
                
                Thread thread3 = new Thread(Shooting);
                thread3.Start();
            }
        } while (isDead == false);

    }
    object locker = new object();
    public void Shooting()
    {
        lock (locker)
        {
            if (direction == Direction.left)
                for (int i = XCoord - 12; i > 0; i--)
                {
                    Console.SetCursorPosition(i, YCoord);
                    Console.Write("-");
                    Thread.Sleep(18);
                    Console.SetCursorPosition(i, YCoord);
                    Console.Write(" ");
                    if (i == 3) Console.SetCursorPosition(XCoord, YCoord);
                }
            else if (direction == Direction.right)
                for (int i = XCoord + 16; i < 92; i++)
                {
                    Console.SetCursorPosition(i, YCoord);
                    Console.Write("-");
                    Thread.Sleep(18);
                    Console.SetCursorPosition(i, YCoord);
                    Console.Write(" ");
                    if (i == 90) Console.SetCursorPosition(XCoord, YCoord);
                }
            else if (direction == Direction.up)
                for (int i = YCoord - 5; i > 0; i--)
                {
                    Console.SetCursorPosition(XCoord + 8, i);
                    Console.Write("|");
                    Thread.Sleep(22);
                    Console.SetCursorPosition(XCoord + 8, i);
                    Console.Write(" ");
                    if (i == 3) Console.SetCursorPosition(XCoord, YCoord);
                }
        }
    }
    

    private void DrawPlayer(int x1Coord, int y1Coord, int x2Coord, int x3Coord, bool hasOverHead, DrawHero drawhero)
    {
        lock (locker)
        {
            if (hasOverHead)
            {
                Console.SetCursorPosition(x1Coord + 8, y1Coord - 1);
                Console.WriteLine(@drawhero.OverHead2);
                Console.SetCursorPosition(x1Coord + 8, y1Coord - 2);
                Console.WriteLine(@drawhero.OverHead2);
                Console.SetCursorPosition(x1Coord + 8, y1Coord - 3);
                Console.WriteLine(@drawhero.OverHead2);
                Console.SetCursorPosition(x1Coord + 8, y1Coord - 4);
                Console.WriteLine(@drawhero.OverHead1);
            }
            Console.SetCursorPosition(x1Coord, y1Coord);
            Console.WriteLine(@drawhero.Head);
            Console.SetCursorPosition(x2Coord, y1Coord + 1);
            Console.WriteLine(@drawhero.BodyNGun);
            Console.SetCursorPosition(x3Coord, y1Coord + 2);
            Console.WriteLine(@drawhero.Legs);
            Console.SetCursorPosition(XCoord, YCoord);
        }
    }

    private void ClearSpace(int xCoord, int yCoord)
    {
        lock (locker)
        {
            Console.SetCursorPosition(xCoord, yCoord);
            Console.Write("                                 ");
            Console.SetCursorPosition(xCoord, yCoord + 1);
            Console.Write("                                ");
            Console.SetCursorPosition(xCoord, yCoord + 2);
            Console.Write("                 ");
            Console.SetCursorPosition(xCoord, yCoord - 1);
            Console.Write("                          ");
            Console.SetCursorPosition(xCoord, yCoord - 2);
            Console.Write("                          ");
            Console.SetCursorPosition(xCoord, yCoord - 3);
            Console.Write("                          ");
            Console.SetCursorPosition(xCoord, yCoord - 4);
            Console.Write("                          ");
        }
    }

}

public struct DrawHero
{
    public string OverHead1 { get; set; }
    public string OverHead2 { get; set; }


    public string Head { get; set; }
    public string BodyNGun { get; set; }
    public string Legs { get; set; }
}
