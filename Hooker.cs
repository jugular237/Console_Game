using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Constants;

public class Hooker : BasicStats, IHitable
{
    public int wayCounter = 0;
    public int Speed { get; set; } = 50;
    public int Health { get; set; } = 10;

    public MonstersSpawns monstrSp;

    public string[] hookerLines = new string[]
    {
      @"   _    _",
      @"  /╰-  -╯\",
      @"   \ ++ /",
      @" //|.  .|\\",
      @"//|      |\____)",
      @"* |      | ",
      @"  --------",
      @"  |  /\  |",
      @"   ||  ||",
      @"   |_   |__"
    };

    public const int wayLength = XCoord - 13;

    public int EngagedXcoord = 0;


    public override void DrawCreature()
    {


    }
    public void AnimateEnemy(Direction directn)
    {
        if (wayCounter < wayLength - 1)
        {
            int xCoordHooker = monstrSp.XLeftSpawn + wayCounter - 1;
            DrawEnemy(xCoordHooker + 1, YBottomBorder, hookerLines);
            ClearSpace(xCoordHooker, YBottomBorder - hookerHight);
            EngagedXcoord = monstrSp.XLeftSpawn + wayCounter;
            if (wayCounter != wayLength - 1)
                wayCounter++;
        }
    }

    private void ClearSpace(int xCoord, int yCoord)
    {
        string space = " ";
        for(int i = 0; i < hookerHight; i++)
        {
            CleanOrWriteSymbol(xCoord, yCoord+i, space);
        }
    }
    public async void GetDamaged()
    {
        int xCoordZombie = monstrSp.XLeftSpawn + wayCounter - 1;
        SetColor("Red");
        DrawEnemy(xCoordZombie, YBottomBorder, hookerLines);
        await Task.Run(() => Health--);
        SetColor("White");
    }

    public void DrawEnemy(int coordX, int coordY, string[] hookerLines)
    {
        for(int i = 0; i < hookerHight; i++)
            CleanOrWriteSymbol(coordX, coordY - hookerHight + i, hookerLines[i]);
    }



    
}
