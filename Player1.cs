//using System;
//using System.Threading.Tasks;
//using System.Threading;

//public class Player1
//{
//	private const int XCoord = 45;
//	private const int YCoord = 20;

//	private int Health { get; set; }
//	private int Ammo { get; set; }
//	private bool isDead { get; set; }

//	public enum Direction
//	{
//		none,
//		up,
//		left,
//		right,
//	}

//	private Direction direction1;

//	Mutex mutexObj = new Mutex(true);

//	object locker = new object();
//	public void AnimatePlayer()
//	{
//		Console.SetCursorPosition(XCoord, YCoord);
//		do
//		{
//            lock (locker) { 
//			ConsoleKeyInfo keyInfo = Console.ReadKey();
//			ConsoleKey key = keyInfo.Key;
//				if (key == ConsoleKey.UpArrow)
//				{
//					direction1 = Direction.up;
//					ClearSpace(XCoord - 17, YCoord);
//					DrawPlayer(XCoord - 1, YCoord, XCoord, XCoord + 2, true,
//						new DrawHero1
//						{
//							OverHead2 = @"||",
//							OverHead1 = @"oo",
//							Head = @"(▀ ͜ʖ▀)  \\",
//							BodyNGun = @"|   | /",
//							Legs = @"/ \"
//						});
//				}
//				else if (key == ConsoleKey.LeftArrow)
//				{
//					direction1 = Direction.left;
//					ClearSpace(XCoord - 17, YCoord);
//					DrawPlayer(XCoord - 9, YCoord, XCoord - 5, XCoord + 2, false,
//						new DrawHero1
//						{
//							Head = @"8======* (▀ ͜ʖ▀)",
//							BodyNGun = @"\\ -- |   |",
//							Legs = @"/ \"
//						});
//					lock (locker)
//					{
//						for (int i = XCoord - 10; i > 0; i--)
//						{
//							Console.SetCursorPosition(i, YCoord);
//							Console.Write("-");

//							Thread.Sleep(25);
//							Console.SetCursorPosition(i, YCoord);
//							Console.Write(" ");
//							if (i == 3) Console.SetCursorPosition(XCoord, YCoord);
//						}
//					}

//				}
//				else if (key == ConsoleKey.RightArrow)
//				{
//					direction1 = Direction.right;
//					ClearSpace(XCoord - 17, YCoord);
//					DrawPlayer(XCoord, YCoord, XCoord + 1, XCoord + 2, false,
//						new DrawHero1
//						{
//							Head = @"(▀ ͜ʖ▀) *======8",
//							BodyNGun = @"|   | -- //",
//							Legs = @"/ \"
//						});
//					lock (locker)
//					{
//						for (int i = XCoord + 16; i < 94; i++)
//						{
//							Console.SetCursorPosition(i, YCoord);
//							Console.Write("-");
//							Thread.Sleep(25);
//							Console.SetCursorPosition(i, YCoord);
//							Console.Write(" ");
//							if (i == 91) Console.SetCursorPosition(XCoord, YCoord);
//						}
//					}
//				}
//			}
//		} while (isDead == false);

//	}
	
//	public void Shooting(object mainthread)
//	{

//		lock (locker)
//		{
//			if (direction1 == Direction.left)
//				for (int i = XCoord - 10; i > 0; i--)
//				{
//					Console.SetCursorPosition(i, YCoord);
//					Console.Write("-");
				
//					Thread.Sleep(25);
//					Console.SetCursorPosition(i, YCoord);
//					Console.Write(" ");
//					if (i == 3) Console.SetCursorPosition(XCoord, YCoord);

//				}
//			else if (direction1 == Direction.right)
//				for (int i = XCoord + 16; i < 94; i++)
//				{
//					Console.SetCursorPosition(i, YCoord);
//					Console.Write("-");
					
//					Thread.Sleep(25);
//					Console.SetCursorPosition(i, YCoord);
//					Console.Write(" ");
//					if (i == 91) Console.SetCursorPosition(XCoord, YCoord);
//				}
//			else if (direction1 == Direction.up)
//				for (int i = YCoord - 5; i > 0; i--)
//				{
//					Console.SetCursorPosition(XCoord + 8, i);
//					Console.Write("|");
					
//					Thread.Sleep(50);
//					Console.SetCursorPosition(XCoord + 8, i);
//					Console.Write(" ");
//					if (i == 3) Console.SetCursorPosition(XCoord, YCoord);
//				}

//		}
//	}
//	private void ClearSpace(Coordinates coords)
//	{
//		Console.SetCursorPosition(coords.X1, coords.Y1);
//		Console.Write("  ");
//		Console.SetCursorPosition(coords.X2, coords.Y2);
//		Console.Write("  ");
//		Console.SetCursorPosition(coords.X3, coords.Y3);
//		Console.Write("  ");
//	}

//	private void DrawPlayer(int x1Coord, int y1Coord, int x2Coord, int x3Coord, bool hasOverHead, DrawHero1 drawhero)
//	{
//		if (hasOverHead)
//		{
//			Console.SetCursorPosition(x1Coord + 8, y1Coord - 1);
//			Console.WriteLine(@drawhero.OverHead2);
//			Console.SetCursorPosition(x1Coord + 8, y1Coord - 2);
//			Console.WriteLine(@drawhero.OverHead2);
//			Console.SetCursorPosition(x1Coord + 8, y1Coord - 3);
//			Console.WriteLine(@drawhero.OverHead2);
//			Console.SetCursorPosition(x1Coord + 8, y1Coord - 4);
//			Console.WriteLine(@drawhero.OverHead1);
//		}
//		Console.SetCursorPosition(x1Coord, y1Coord);
//		Console.WriteLine(@drawhero.Head);
//		Console.SetCursorPosition(x2Coord, y1Coord + 1);
//		Console.WriteLine(@drawhero.BodyNGun);
//		Console.SetCursorPosition(x3Coord, y1Coord + 2);
//		Console.WriteLine(@drawhero.Legs);
//		Console.SetCursorPosition(XCoord, YCoord);
//	}

//	private void ClearSpace(int xCoord, int yCoord)
//	{
//		Console.SetCursorPosition(xCoord, yCoord);
//		Console.Write("                                 ");
//		Console.SetCursorPosition(xCoord, yCoord + 1);
//		Console.Write("                                ");
//		Console.SetCursorPosition(xCoord, yCoord + 2);
//		Console.Write("                 ");
//		Console.SetCursorPosition(xCoord, yCoord - 1);
//		Console.Write("                          ");
//		Console.SetCursorPosition(xCoord, yCoord - 2);
//		Console.Write("                          ");
//		Console.SetCursorPosition(xCoord, yCoord - 3);
//		Console.Write("                          ");
//		Console.SetCursorPosition(xCoord, yCoord - 4);
//		Console.Write("                          ");
//	}

//}

//public struct DrawHero1
//{
//	public string OverHead1 { get; set; }
//	public string OverHead2 { get; set; }


//	public string Head { get; set; }
//	public string BodyNGun { get; set; }
//	public string Legs { get; set; }
//}
