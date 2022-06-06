using System;
using System.Threading.Tasks;
using System.Threading;

public sealed class Player:BasicStats, IHitable
{
    public const int XCoord = 45;
    public const int YCoord = 25;
    private const int LeftBorderPlayer = XCoord - 5;
    private const int LeftBorderBox = XCoord - 9;

    public int Health { get; set; } = 10;

    public Direction direction;

    private static Player PlayerInstance;
    private Player()
    {

    }

    public static Player GetInstance()
    {
        if(PlayerInstance == null)
            PlayerInstance = new Player();
        return PlayerInstance;
    }
    public override void DrawCreature()
    {
        direction = ReadMovement(direction);
        if (direction == Direction.Up)
        {
            ClearSpace(LeftBorderPlayer, YCoord, LeftBorderBox);
            DrawPlayer(true, new Coordinates(
                new int[] { XCoord - 1, XCoord, XCoord + 2, XCoord+6},
                new int[] { YCoord, YCoord + 1, YCoord + 2 }), 
                    new DrawHero
                    {
                        OverHead2 = @"||",
                        OverHead1 = @"oo",
                        Head = @"(▀ ͜ʖ▀)  \\",
                        BodyNGun = @"|   | /",
                        Legs = @"/ \"
                    });
        }
        else if (direction == Direction.Left)
        {
            ClearSpace(LeftBorderPlayer, YCoord, LeftBorderBox);
            DrawPlayer(false, new Coordinates(
                new int[] { XCoord - 9, XCoord - 5, XCoord + 2, },
                new int[] { YCoord, YCoord + 1, YCoord + 2 }),
                    new DrawHero
                    {
                        Head = @"8======* (▀ ͜ʖ▀)",
                        BodyNGun = @"\\ -- |   |",
                        Legs = @"/ \"
                    });
        }
        else if (direction == Direction.Right)
        {
            ClearSpace(LeftBorderPlayer, YCoord, LeftBorderBox);
            DrawPlayer(false, new Coordinates(
                new int[] { XCoord, XCoord + 1, XCoord + 2 },
                new int[] {YCoord, YCoord + 1, YCoord + 2 }), 
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
            currentDirection = Direction.Up;
        else if (key == ConsoleKey.LeftArrow)
            currentDirection = Direction.Left;
        else if (key == ConsoleKey.RightArrow)
            currentDirection = Direction.Right;
        return currentDirection;
    }

    public static void CleanOrWriteSymbol(int coordx, int coordy, string symb)
    {
        Console.SetCursorPosition(coordx, coordy);
        Console.Write(symb);
    }
    private void DrawPlayer(bool hasOverHead, Coordinates coords, DrawHero drawhero)
    {
        if (hasOverHead)
        {
            for(int i = 1; i< 5; i++)
            {
                if(i == 4)
                    CleanOrWriteSymbol(coords.xCoords[3], coords.yCoords[0] - i, @drawhero.OverHead1);
                else
                CleanOrWriteSymbol(coords.xCoords[3], coords.yCoords[0] - i, @drawhero.OverHead2);
            }
        }
        CleanOrWriteSymbol(coords.xCoords[0], coords.yCoords[0], @drawhero.Head);
        CleanOrWriteSymbol(coords.xCoords[1], coords.yCoords[1], @drawhero.BodyNGun);
        CleanOrWriteSymbol(coords.xCoords[2], coords.yCoords[2], @drawhero.Legs);
    }

    private void ClearSpace(int xCoord, int yCoord, int leftBordBox)
    {
        int counter = 4;
        string[] spaces = { new String(' ', 18), new String(' ', 18), new String(' ', 18),
            new String(' ', 18),  new String(' ', 25) , new String(' ', 18), new String(' ', 8)};
        foreach(var space in spaces)
        {
            if (counter == 0)
                CleanOrWriteSymbol(leftBordBox, yCoord, space);
            else
                CleanOrWriteSymbol(xCoord, yCoord - counter, space);
            counter--;
        }
    }

    public async void GetDamaged()
    {
        ChangeColor();
        await Task.Run(() => Health--);
    }
    public void ChangeColor()
    {
        SetColor("Red");
        DrawBox(new Coordinates(
            new int[] { XCoord - 9, XCoord + 16, XCoord - 1, XCoord + 11 },
            new int[] { YCoord + 2, YCoord - 5, YCoord, YCoord - 1 }));
        SetColor("White");
    }
    public void DrawBox(Coordinates coords)
    {
        
        for (int i = coords.yCoords[0]; i > coords.yCoords[1]; i--)
        {
            if (i == coords.yCoords[2] || i == coords.yCoords[3])
                continue;
            Console.SetCursorPosition(coords.xCoords[0], i);
            Console.Write('|');
            Console.SetCursorPosition(coords.xCoords[1], i);
            Console.Write('|');
        }
        for (int i = coords.xCoords[0]; i < coords.xCoords[1]; i++)
        {
            if (i > coords.xCoords[2] && i < coords.xCoords[3]) 
                continue;
            Console.SetCursorPosition(i, coords.yCoords[1]);
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
