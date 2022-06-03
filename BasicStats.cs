using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public abstract class BasicStats
{
    
    public bool isDead { get; set; }

    protected int Speed { get; set; }

    public enum Direction
    {
        none,
        up,
        left,
        right,
    }
    public abstract void DrawCreature();

    public virtual void SetColor(string color)
    {
        Type type = typeof(ConsoleColor);
        Console.ForegroundColor = (ConsoleColor)Enum.Parse(type, color);
    }
}
