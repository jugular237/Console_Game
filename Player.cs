using System;
using System.Threading;

public class Player:BasicStats, IHitable
{
    public const int XCoord = 45;
    public const int YCoord = 25;

    public int Health { get; set; } = 10;

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
        CleanOrWriteSymbol(xCoord+4, yCoord + 1, "                  ");
        CleanOrWriteSymbol(xCoord + 4, yCoord + 2, "        ");
        CleanOrWriteSymbol(xCoord + 4, yCoord - 1, "                  ");
        CleanOrWriteSymbol(xCoord + 4, yCoord - 2, "                  ");
        CleanOrWriteSymbol(xCoord + 4, yCoord - 3, "                  ");
        CleanOrWriteSymbol(xCoord + 4, yCoord - 4, "                  ");
    }

    public void GetDamaged()
    {
        SetColor("Red");
        DrawBox(new Coordinates(
                x1: XCoord - 9, y1: YCoord + 2,
                x2: XCoord + 16, y2: YCoord -5,
                x3: XCoord - 1, y3: YCoord,
                x4: XCoord + 11, y4: YCoord -1));
        Health--;
        SetColor("White");
    }

    public void DrawBox(Coordinates coords)
    {
        for (int i = coords.Y1; i > coords.Y2; i--)
        {
            if (i == coords.Y3 || i == coords.Y4) continue;
            Console.SetCursorPosition(coords.X1, i);
            Console.Write('|');
            Console.SetCursorPosition(coords.X2, i);
            Console.Write('|');
        }
        for (int i = coords.X1; i < coords.X2; i++)
        {
            if (i > coords.X3 && i < coords.X4) continue;
            Console.SetCursorPosition(i, coords.Y2);
            Console.Write('-');
        }
    }

    public bool CheckOnHit(int enemyCoord, int playerBoxCoord)
    {
        return enemyCoord + 1 == playerBoxCoord;
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
