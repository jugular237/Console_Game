using System;

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

    public void DrawBulletUp(int coordX, int coordY, int bullCounter, string bullt)
    {
        CleanOrWriteSymbol(coordX, coordY - bullCounter, bullt);
        CleanOrWriteSymbol(coordX, (coordY + 1) - bullCounter, " ");
    }

    public void DrawBulletLeft(int coordX, int coordY, int bullCounter, string bullt)
    {
        CleanOrWriteSymbol(coordX - bullCounter, coordY, bullt);
        CleanOrWriteSymbol(coordX + 1 - bullCounter, coordY, " ");
    }

    public void DrawBulletRight(int coordX, int coordY, int bullCounter, string bullt)
    {
        CleanOrWriteSymbol(coordX + bullCounter, coordY, bullt);
        CleanOrWriteSymbol(coordX - 1 + bullCounter, coordY, " ");
    }


}
