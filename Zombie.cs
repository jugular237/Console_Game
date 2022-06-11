using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Constants;

public class Zombie : BasicStats, IHitable
{
    public int wayCounter = 0;
    public int Speed { get; set; } = 15;
    public int Health { get; set; } = 5;

    public MonstersSpawns monstrSp;

    public const int wayLength = XCoord - 13;

    public int EngagedXcoord = 0;

    
    public override void DrawCreature()
    {
       
        
    }
    public void AnimateEnemy(Direction directn)
    {
        if (wayCounter < wayLength - 1)
        {
            int xCoordZombie = monstrSp.XLeftSpawn + wayCounter - 1;
            DrawEnemy(xCoordZombie+1, monstrSp.YLeftSpawn,
                new DrawZombie
                {
                    Head = @"(+_+)",
                    Body = @"| |--",
                    Legs = @"/ \"
                });
            ClearSpace(xCoordZombie, monstrSp.YLeftSpawn-1);
            EngagedXcoord = monstrSp.XLeftSpawn + wayCounter;
            if (wayCounter != wayLength - 1)
                wayCounter++;

        }
    }

    private void ClearSpace(int xCoord, int yCoord)
    {
        string[] spaces = { new String(' ', 1), new String(' ', 1 ), new String(' ', 1)};
        foreach (var space in spaces)
        {
            CleanOrWriteSymbol(xCoord, yCoord, space);
            yCoord++;
        }
    } 
    public async void GetDamaged()
    {
        int xCoordZombie = monstrSp.XLeftSpawn + wayCounter - 1;
        SetColor("Red");
        DrawEnemy(xCoordZombie, monstrSp.YLeftSpawn,
                new DrawZombie 
                {
                    Head = @"(+_+)",
                    Body = @"|  |--",
                    Legs = @"/ \"
                });
        await Task.Run(() => Health--);
        SetColor("White");
    }

    public void DrawEnemy(int coordX, int coordY, DrawZombie drZombie)
    {
        CleanOrWriteSymbol(coordX, coordY - 1, drZombie.Head);
        CleanOrWriteSymbol(coordX, coordY, drZombie.Body);
        CleanOrWriteSymbol(coordX, coordY + 1, drZombie.Legs);
    }

    

    public struct DrawZombie
    {

        public string Head { get; set; }
        public string Body { get; set; }
        public string Legs { get; set; }
    }
}
