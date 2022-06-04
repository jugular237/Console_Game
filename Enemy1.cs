using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class Enemy1 : BasicStats, IHitable
{
    public int wayCounter = 0;
    public int Health { get; set; } = 5;

    public Direction direction = Direction.up;

    public MonstersSpawns monstrSp;
    public override void DrawCreature()
    {
        
        if(direction == Direction.right)
        {
            DrawEnemy(monstrSp.XLeftSpawn, monstrSp.YLeftSpawn);
        }
        else if(direction == Direction.left)
        {
            DrawEnemy(monstrSp.XRightSpawn, monstrSp.YRightSpawn);
        }
        else if(direction == Direction.up)
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

        if(directn == Direction.up && wayCounter < 17)
        {
            DrawEnemy(monstrSp.XUpSpawn, monstrSp.YUpSpawn + wayCounter);
            CleanOrWriteSymbol(monstrSp.XUpSpawn, monstrSp.YUpSpawn + wayCounter - 1, "      |          ");
            if (wayCounter != 18)
                wayCounter++;
        }
    }

    public bool CheckOnHit(int bulletCoord, int enemyCoord)
    {
        return bulletCoord == enemyCoord - 1;
    }

    public void GetDamaged()
    {
        SetColor("Red");
        DrawEnemy(monstrSp.XUpSpawn, monstrSp.YUpSpawn + wayCounter - 1);
        Health--;
    }

    public void CleanOrWriteSymbol(int coordx, int coordy, string symb)
    {
        Console.SetCursorPosition(coordx, coordy);
        Console.Write(symb);
    }
}
