using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public abstract class BasicStats
{
    public bool isAttacking { get; set; }
    public bool isDead { get; set; }

    protected int Speed { get; set; }

    public enum Direction
    {
        None,
        Up,
        Left,
        Right,
    }
    public abstract void DrawCreature();

    public virtual void SetColor(string color)
    {
        Type type = typeof(ConsoleColor);
        Console.ForegroundColor = (ConsoleColor)Enum.Parse(type, color);
    }
}
