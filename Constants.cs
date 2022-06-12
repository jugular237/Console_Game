using System;


public class Constants
{
    private const int XCoordPlayer = Player.XCoord;
    private const int YCoordPlayer = Player.YCoord;

    public const int XCoord = 45;
    public const int YCoord = 25;
    public const int FieldSizeX = 95;
    public const int FieldSizeY = 30;

    public const int frameRate = 13;
    public const int spawnNumber = 3;
    public const int spawnNumber1 = 5;

    public const int XLeftBorder = 1;
    public const int XRightBorder = FieldSizeX - 1;
    public const int YTopBorder = 1;
    public const int YBottomBorder = FieldSizeY - 2;
    public const int YBoxRoof = FieldSizeY - 10;

    public const int bulletRightRange = XRightBorder - 1;
    public const int bottomBorder = FieldSizeY - 1;
    public const int leftBorderBox = XCoordPlayer - 9;
    public const int rightBorderBox = XCoordPlayer + 16;

    public const int X1roofHole = XCoordPlayer - 1;
    public const int X2roofHole = XCoordPlayer + 10;

    public const int bottomBorderBox = FieldSizeY - 3;
    public const int topBorderBox = FieldSizeY - 11;

    public const int Y1WallHole = FieldSizeY - 6;
    public const int Y2WallHole = FieldSizeY - 5;

    public const int XbulletLeftcoord = 34;
    public const int XbulletRightcoord = 62;
    public const int XbulletUpcoord = XCoordPlayer + 8;
    public const int YbulletLeftcoord = YCoordPlayer;
    public const int YbulletRightcoord = YCoordPlayer;
    public const int YbulletUpcoord = 19;

    public const int clearSpiderLngth= 23;
    public const int clearWebLngth = 7;

    public const int clearZombieLngth = 6;

    public const int LeftBorderPlayer = XCoord - 5;
    public const int LeftBorderBox = XCoord - 9;

    public const int playerX1Up = XCoord - 1;
    public const int playerX2Up = XCoord;
    public const int playerX3Up = XCoord + 2;
    public const int playerX4Up = XCoord + 6;

    public const int playerY1Up = YCoord;
    public const int playerY2Up = YCoord + 1;
    public const int playerY3Up = YCoord + 2;

    public const int playerX1Left = XCoord - 9;
    public const int playerX2Left = XCoord - 5;
    public const int playerX3Left = XCoord + 2;
 
    public const int playerY1Left = YCoord;
    public const int playerY2Left = YCoord + 1;
    public const int playerY3Left = YCoord + 2;

    public const int playerX1Right = XCoord;
    public const int playerX2Right = XCoord + 1;
    public const int playerX3Right = XCoord + 2;

    public const int playerY1Right = YCoord;
    public const int playerY2Right = YCoord + 1;
    public const int playerY3Right = YCoord + 2;

    public const int overheadLines = 5;
    public const int space1Lngth = 18;
    public const int space2Lngth = 25;
    public const int space3Lngth = 8;

    public const int zSpace1Lngth = 5;
    public const int zSpace2Lngth = 6;
    public const int zSpace3Lngth = 3;

    public const int X1Box = XCoord - 9;
    public const int X2Box = XCoord + 16;
    public const int X3Box = XCoord - 1;
    public const int X4Box = XCoord + 11;

    public const int Y1Box = YCoord + 2;
    public const int Y2Box = YCoord - 6;
    public const int Y3Box = YCoord;
    public const int Y4Box = YCoord - 1;

    public const int zombieX1Right = 2;
   
    public const int zombieY1Right = YCoordPlayer - 1;
    public const int zombieY2Right = YCoordPlayer;
    public const int zombieY3Right = YCoordPlayer + 1;

    public const int blinkRate = 5;

    public const int zombieLngth = 5;
}
