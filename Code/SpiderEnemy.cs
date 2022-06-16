using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Constants;


public class SpiderEnemy : BasicStats, IHitable
{
    public int wayCounter = 0;
    public int Speed { get; set; } = 40;
    public int Health { get; set; } = 5;


    public Direction direction = Direction.Up;

    public MonstersSpawns monstrSp;

    public const int wayLength = 18;

    public int EngagedYcoord = 0;
    
    

    public void DrawEnemy(int coordX, int coordY)
    {
        CleanOrWriteSymbol(coordX, coordY, @"/╲/\╭ºo8oº╮/\╱\");
    }

    public void AnimateEnemy()
    {
        if(wayCounter < wayLength - 1)
        {
            DrawEnemy(monstrSp.XUpSpawn, monstrSp.YUpSpawn + wayCounter);
            CleanOrWriteSymbol(monstrSp.XUpSpawn, monstrSp.YUpSpawn + wayCounter - 1, "      |        ");
            EngagedYcoord = monstrSp.YUpSpawn + wayCounter;
            if (wayCounter != wayLength-1)
                wayCounter++;
            
        }
    }
    public void ClearWeb(int xCoord, int yCoord, string space, int roofCoord)
    {
        for (int i = yCoord; i < roofCoord; i++)
        {
            CleanOrWriteSymbol(xCoord, i, space);
        }
    }
    

    public async void GetDamaged()
    {
        int yCoordSpider = monstrSp.YUpSpawn + wayCounter - 1;
        SetColor("Red");
        DrawEnemy(monstrSp.XUpSpawn, yCoordSpider);
        await Task.Run(()=> Health--);
        SetColor("White");
    }

    public new bool CheckOnHit(int bulletCoord, int enemyCoord)
    {
        return bulletCoord == enemyCoord - 1;
    }
    

}
