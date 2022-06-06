using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class Enemy1 : BasicStats, IHitable
{
    public int wayCounter = 0;
    public int Speed { get; set; } = 20;
    public int Health { get; set; } = 5;

    public Direction direction = Direction.Up;

    public MonstersSpawns monstrSp;

    public const int wayLength = 18;

    public int EngagedYcoord = 0;
    public bool reachedBox = false;
    
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

        if(directn == Direction.Up && wayCounter < wayLength)
        {
            DrawEnemy(monstrSp.XUpSpawn, monstrSp.YUpSpawn + wayCounter);
            CleanOrWriteSymbol(monstrSp.XUpSpawn, monstrSp.YUpSpawn + wayCounter - 1, "      |          ");
            EngagedYcoord = monstrSp.YUpSpawn + wayCounter;
            if (wayCounter != wayLength - 1)
                wayCounter++;
            else
                reachedBox = true;
        }
    }

    public bool CheckOnHit(int bulletCoord, int enemyCoord)
    {
        return bulletCoord == enemyCoord - 1;
    }

    public void GetDamaged()
    {
        SetColor("Red");
        if(!reachedBox)
            DrawEnemy(monstrSp.XUpSpawn, monstrSp.YUpSpawn + wayCounter - 1);
        else
            DrawEnemy(monstrSp.XUpSpawn, monstrSp.YUpSpawn + wayCounter);
        Health--;
    }

    public void CleanOrWriteSymbol(int coordx, int coordy, string symb)
    {
        Console.SetCursorPosition(coordx, coordy);
        Console.Write(symb);
    }
    public void CheckOnEnemyInFront(int xCoord, int yCoord)
    {

    }
}
