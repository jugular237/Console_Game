using System;

public class Player
{
	private const int XCoord = 45;
	private const int YCoord = 15;

	private int Health { get; set; }
	private int Ammo { get; set; }
	private bool isDead { get; set; }
	
	enum Diresction
    {
		up,
		left,
		right,
		none
    }

	public void AnimatePlayer()
    {
		Console.SetCursorPosition(XCoord, YCoord);
		do
		{
			ConsoleKeyInfo keyInfo = Console.ReadKey();
			ConsoleKey key = keyInfo.Key;
			Diresction direction=Diresction.none;
			if (key == ConsoleKey.W && direction != Diresction.up)
            {
				//to do
			}
			else if (key == ConsoleKey.A && direction != Diresction.left)
            {
				ClearSpace(XCoord, YCoord);
				Console.SetCursorPosition(XCoord, YCoord);
				DrawPlayer(XCoord-9, YCoord, XCoord - 5, XCoord+2,
					new DrawHero
					{
						Head = @"0======* (▀ ͜ʖ▀)",
						BodyNGun = @"\\ -- |   |",
						Legs = @"/ \"
					});
				direction = Diresction.left;
			}
            else if (key == ConsoleKey.D && direction != Diresction.right)
			{
				ClearSpace(XCoord - 17, YCoord);
				Console.SetCursorPosition(XCoord, YCoord);
				DrawPlayer(XCoord, YCoord, XCoord + 1, XCoord + 2,
					new DrawHero
					{
						Head = @"(▀ ͜ʖ▀) *======0",
						BodyNGun = @"|   | -- //",
						Legs = @"/ \"
					});
				direction = Diresction.right;
			}
		} while (isDead == false);
        
    }

	private void DrawPlayer(int x1Coord, int y1Coord, int x2Coord, int x3Coord, DrawHero drawhero)
    {
		Console.SetCursorPosition(x1Coord, y1Coord);
		Console.WriteLine(@drawhero.Head);
		Console.SetCursorPosition(x2Coord, y1Coord+1);
		Console.WriteLine(@drawhero.BodyNGun);
		Console.SetCursorPosition(x3Coord, y1Coord+2);
		Console.WriteLine(@drawhero.Legs);
		Console.SetCursorPosition(XCoord, YCoord);
	}

	private void ClearSpace(int xCoord, int yCoord)
    {
		Console.SetCursorPosition(xCoord, yCoord);
		Console.Write("                 ");
		Console.SetCursorPosition(xCoord, yCoord+1);
		Console.Write("                 ");
		Console.SetCursorPosition(xCoord, yCoord + 2);
		Console.Write("         ");
	}

}

 public struct DrawHero
 {
	public string Head { get; set; }
	public string BodyNGun { get; set; }
	public string Legs { get; set; }
 }
