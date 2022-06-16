using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public abstract class BasicStats
{
    public bool isAttacking { get; set; }
    public bool isDead { get; set; }


    public enum Direction
    {
        None,
        Up,
        Left,
        Right,
    }
    

    public virtual void SetColor(string color)
    {
        Type type = typeof(ConsoleColor);
        Console.ForegroundColor = (ConsoleColor)Enum.Parse(type, color);
    }

    public virtual void CleanOrWriteSymbol(int coordx, int coordy, string symb)
    {
        Console.SetCursorPosition(coordx, coordy);
        Console.Write(symb);
    }

    public bool CheckOnHit(int bulletCoord, int enemyCoord)
    {
        return bulletCoord == enemyCoord - 1;
    }
}
