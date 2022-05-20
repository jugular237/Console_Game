using System;

public class Player
{
	private const int XCoord = 45;
	private const int YCoord = 15;

	private int Health { get; set; }
	private int Ammo { get; set; }
	enum Diresction
    {
		up,
		left,
		right
    }
	public void DrawPlayer()
    {
		Console.SetCursorPosition(XCoord, YCoord);
		ConsoleKeyInfo keyInfo = Console.ReadKey();
		ConsoleKey key = keyInfo.Key;
		Diresction direction = Diresction.right;
		
		do
		{
			if(key == ConsoleKey.W && direction != Diresction.up)
            {
				Console.Write(@"(-_+)
					      /\");
				direction = Diresction.up;
			}
			else if (key == ConsoleKey.A && direction != Diresction.left)
            {
				Console.Write(@"(-_+)
					       ̿̿ ̿̿ ̿'̿'\̵͇̿̿\/
					      /\");
				direction = Diresction.left;

			}
            else if (key == ConsoleKey.D && direction != Diresction.right)
			{
				Console.Write(@"(-_+)
					      \==╦╦═─
					      /\");
				direction = Diresction.right;
			}
		} while (key != ConsoleKey.Enter);
        
    }

}
