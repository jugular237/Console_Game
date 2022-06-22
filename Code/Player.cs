using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using static Constants;


public sealed class Player:BasicStats, IHitable
{
    public const int XCoord = 45;
    public const int YCoord = 25;
    public int Health { get; set; } = 7;

    public Direction direction = Direction.None;

    ConfigClass conf = JsonConvert.DeserializeObject<ConfigClass>(File.ReadAllText(@"jsconfig.json"));

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
    public void DrawCreature()
    {
        direction = ReadMovement(direction);
        if (direction == Direction.Up)
        {
            ClearSpace(conf.LeftBorderPlayer, YCoord, conf.LeftBorderBox);
            DrawPlayer(true, new Coordinates(
                new int[] { conf.playerX1Up, conf.playerX2Up, conf.playerX3Up, conf.playerX4Up },
                new int[] { conf.playerY1Up, conf.playerY2Up, conf.playerY3Up }), 
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
            ClearSpace(conf.LeftBorderPlayer, YCoord, conf.LeftBorderBox);
            DrawPlayer(false, new Coordinates(
                new int[] { conf.playerX1Left, conf.playerX2Left, conf.playerX3Left },
                new int[] { conf.playerY1Left, conf.playerY2Left, conf.playerY3Left }),
                    new DrawHero
                    {
                        Head = @"8======* (▀ ͜ʖ▀)",
                        BodyNGun = @"\\ -- |   |",
                        Legs = @"/ \"
                    });
        }
        else if (direction == Direction.Right)
        {
            ClearSpace(conf.LeftBorderPlayer, YCoord, conf.LeftBorderBox);
            DrawPlayer(false, new Coordinates(
                new int[] { conf.playerX1Right, conf.playerX2Right, conf.playerX3Right },
                new int[] { conf.playerY1Right, conf.playerY2Right, conf.playerY3Right }), 
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

    
    public void DrawPlayer(bool hasOverHead, Coordinates coords, DrawHero drawhero)
    {
        if (hasOverHead)
        {
            for(int i = 1; i < overheadLines ; i++)
            {
                if(i == overheadLines - 1)
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
        string[] spaces = { new String(' ', space1Lngth), new String(' ', space1Lngth), new String(' ', space1Lngth),
            new String(' ', space1Lngth),  new String(' ', space2Lngth) , new String(' ', space1Lngth), new String(' ', space3Lngth)};
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
            new int[] { conf.X1Box, conf.X2Box, conf.X3Box, conf.X4Box },
            new int[] { conf.Y1Box, conf.Y2Box, conf.Y3Box, conf.Y4Box }));
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

    
}



public struct DrawHero
{
    public string OverHead1 { get; set; }
    public string OverHead2 { get; set; }

    public string Head { get; set; }
    public string BodyNGun { get; set; }
    public string Legs { get; set; }
}
