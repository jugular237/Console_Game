using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class Enemy1 : BasicStats, IHitable
{
    public int wayCounter = 0;
    public int Speed { get; set; } = 40;
    public int Health { get; set; } = 5;

    public Direction direction = Direction.Up;

    public MonstersSpawns monstrSp;

    public const int wayLength = 18;

    public int EngagedYcoord = 0;
    
    public override void DrawCreature()
    {
        
        if(direction == Direction.Right)
        {
            DrawEnemy(monstrSp.XLeftSpawn, monstrSp.YLeftSpawn);
        }
        else if(direction == Direction.Left)
        {
            DrawEnemy(monstrSp.XRightSpawn, monstrSp.YRightSpawn);
        }
        else if(direction == Direction.Up)
        {
            DrawEnemy(monstrSp.XUpSpawn, monstrSp.YUpSpawn);
        }
    }

    public void DrawEnemy(int coordX, int coordY)
    {
        CleanOrWriteSymbol(coordX, coordY, @"/╲/\╭ºo8oº╮/\╱\");
    }

    public void AnimateEnemy(Direction directn)
    {

        if(directn == Direction.Up && wayCounter < wayLength-1)
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
    public bool CheckOnHit(int bulletCoord, int enemyCoord)
    {
        return bulletCoord <= enemyCoord - 1;
    }

    public async void GetDamaged()
    {
        int yCoordSpider = monstrSp.YUpSpawn + wayCounter - 1;
        SetColor("Red");
        DrawEnemy(monstrSp.XUpSpawn, yCoordSpider);
        await Task.Run(()=> Health--);
    }

    public void CleanOrWriteSymbol(int coordx, int coordy, string symb)
    {
        Console.SetCursorPosition(coordx, coordy);
        Console.Write(symb);
    }
    
}
