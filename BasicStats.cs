using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public abstract class BasicStats
{
    public int Health { get; set; }
    public bool isDead { get; set; }

    public int Speed { get; set; }

    public enum Direction
    {
        none,
        up,
        left,
        right,
    }
    public abstract void AnimateCreature();
}
