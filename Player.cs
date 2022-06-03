using System;
using System.Threading;

public class Player:BasicStats
{
    public const int XCoord = 45;
    public const int YCoord = 25;
    
    public Direction direction;
    
    public override void DrawCreature()
    {
        direction = ReadMovement(direction);
        if (direction == Direction.up)
        {
            ClearSpace(XCoord - 9, YCoord);
            DrawPlayer(XCoord - 1, YCoord, XCoord, XCoord + 2, true,
                    new DrawHero
                    {
                        OverHead2 = @"||",
                        OverHead1 = @"oo",
                        Head = @"(▀ ͜ʖ▀)  \\",
                        BodyNGun = @"|   | /",
                        Legs = @"/ \"
                    });
        }
        else if (direction == Direction.left)
        {
            ClearSpace(XCoord - 9, YCoord);
            DrawPlayer(XCoord - 9, YCoord, XCoord - 5, XCoord + 2, false,
                    new DrawHero
                    {
                        Head = @"8======* (▀ ͜ʖ▀)",
                        BodyNGun = @"\\ -- |   |",
                        Legs = @"/ \"
                    });
        }
        else if (direction == Direction.right)
        {
            ClearSpace(XCoord - 9, YCoord);
            DrawPlayer(XCoord, YCoord, XCoord + 1, XCoord + 2, false,
                    new DrawHero
                    {
                        Head = @"(▀ ͜ʖ▀) *======8",
                        BodyNGun = @"|   | -- //",
                        Legs = @"/ \"
                    });
        }
    }
    public Direction ReadMovement(Direction currentDirection)
    {
        if (!Console.KeyAvailable)
            return currentDirection;
        ConsoleKey key = Console.ReadKey(true).Key;
        if (key == ConsoleKey.UpArrow)
            currentDirection = Direction.up;
        else if (key == ConsoleKey.LeftArrow)
            currentDirection = Direction.left;
        else if (key == ConsoleKey.RightArrow)
            currentDirection = Direction.right;
        return currentDirection;
    }

    public static void CleanOrWriteSymbol(int coordx, int coordy, string symb)
    {
        Console.SetCursorPosition(coordx, coordy);
        Console.Write(symb);
    }
    private void DrawPlayer(int x1Coord, int y1Coord, int x2Coord, int x3Coord, bool hasOverHead, DrawHero drawhero)
    {
        if (hasOverHead)
        {
           CleanOrWriteSymbol(x1Coord + 8, y1Coord - 1, @drawhero.OverHead2);
           CleanOrWriteSymbol(x1Coord + 8, y1Coord - 2, @drawhero.OverHead2);
           CleanOrWriteSymbol(x1Coord + 8, y1Coord - 3, @drawhero.OverHead2);
           CleanOrWriteSymbol(x1Coord + 8, y1Coord - 4, @drawhero.OverHead1);
        }
        CleanOrWriteSymbol(x1Coord, y1Coord, @drawhero.Head);
        CleanOrWriteSymbol(x2Coord, y1Coord + 1, @drawhero.BodyNGun);
        CleanOrWriteSymbol(x3Coord, y1Coord + 2, @drawhero.Legs);
    }

    private void ClearSpace(int xCoord, int yCoord)
    {
        CleanOrWriteSymbol(xCoord, yCoord, "                         ");
        CleanOrWriteSymbol(xCoord, yCoord + 1, "                      ");
        CleanOrWriteSymbol(xCoord, yCoord + 2, "           ");
        CleanOrWriteSymbol(xCoord, yCoord - 1, "                  ");
        CleanOrWriteSymbol(xCoord, yCoord - 2, "                  ");
        CleanOrWriteSymbol(xCoord, yCoord - 3, "                  ");
        CleanOrWriteSymbol(xCoord, yCoord - 4, "                  ");
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
