using System;

public class Player
{
	private const int XCoord = 45;
	private const int YCoord = 15;

	private int Health { get; set; }
	private int Ammo { get; set; }
	private bool isDead { get; set; }
	
	public enum Diresction
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
			if (key == ConsoleKey.W)
            {
				ClearSpace(XCoord - 17, YCoord);
				DrawPlayer(XCoord-1, YCoord, XCoord , XCoord + 2, true,
					new DrawHero
					{
						OverHead2 = @"||",
						OverHead1 = @"oo",
						Head =   @"(▀ ͜ʖ▀)  \\",
						BodyNGun = @"|   | /",
						Legs = @"/ \"
					});
				Shooting(0,0, Diresction.up);
			}
			else if (key == ConsoleKey.A)
            {
				ClearSpace(XCoord-17, YCoord);
				DrawPlayer(XCoord-9, YCoord, XCoord - 5, XCoord+2, false,
					new DrawHero
					{
						Head = @"8======* (▀ ͜ʖ▀)",
						BodyNGun = @"\\ -- |   |",
						Legs = @"/ \"
					});
				Shooting(0,0,Diresction.left);
			}
            else if (key == ConsoleKey.D)
			{
				ClearSpace(XCoord - 17, YCoord);
				DrawPlayer(XCoord, YCoord, XCoord + 1, XCoord + 2, false,
					new DrawHero
					{
						Head = @"(▀ ͜ʖ▀) *======8",
						BodyNGun = @"|   | -- //",
						Legs = @"/ \"
					});
				Shooting(0,0, Diresction.right);
			}
		} while (isDead == false);
        
    }

	public void Shooting(int xCoord, int yCoord, Diresction direction)
    {
		//while () ... to do
    }

	private void DrawPlayer(int x1Coord, int y1Coord, int x2Coord, int x3Coord, bool hasOverHead, DrawHero drawhero)
    {
        if (hasOverHead)
        {
			Console.SetCursorPosition(x1Coord + 8, y1Coord - 1);
			Console.WriteLine(@drawhero.OverHead2);
			Console.SetCursorPosition(x1Coord + 8, y1Coord - 2);
			Console.WriteLine(@drawhero.OverHead2);
			Console.SetCursorPosition(x1Coord + 8, y1Coord - 3);
			Console.WriteLine(@drawhero.OverHead2);
			Console.SetCursorPosition(x1Coord + 8, y1Coord - 4);
			Console.WriteLine(@drawhero.OverHead1);
		}
		Console.SetCursorPosition(x1Coord, y1Coord);
		Console.WriteLine(@drawhero.Head);
		Console.SetCursorPosition(x2Coord, y1Coord + 1);
		Console.WriteLine(@drawhero.BodyNGun);
		Console.SetCursorPosition(x3Coord, y1Coord + 2);
		Console.WriteLine(@drawhero.Legs);
		Console.SetCursorPosition(XCoord, YCoord);
	}

	private void ClearSpace(int xCoord, int yCoord)
    {
		Console.SetCursorPosition(xCoord, yCoord);
		Console.Write("                                 ");
		Console.SetCursorPosition(xCoord, yCoord+1);
		Console.Write("                                ");
		Console.SetCursorPosition(xCoord, yCoord + 2);
		Console.Write("                 ");
		Console.SetCursorPosition(xCoord, yCoord - 1);
		Console.Write("                          ");
		Console.SetCursorPosition(xCoord, yCoord - 2);
		Console.Write("                          ");
		Console.SetCursorPosition(xCoord, yCoord - 3);
		Console.Write("                          ");
		Console.SetCursorPosition(xCoord, yCoord - 4);
		Console.Write("                          ");
	}

}

 public struct DrawHero
 {
	public string OverHead1 { get; set; }
	public string OverHead2 { get; set; }

	
	public string Head { get; set; }
	public string BodyNGun { get; set; }
	public string Legs { get; set; }
 }
