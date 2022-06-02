using System;



	public struct Coordinates
	{
	public readonly int X1;
	public readonly int X2;
	public readonly int X3;
	public readonly int X4;

	public readonly int Y1;
	public readonly int Y2;
	public readonly int Y3;
	public readonly int Y4;

        public Coordinates(int x1=0, int y1 = 0, int x2=0, int y2=0,
							int x3=0, int y3=0, int x4=0, int y4=0)
        {
			X1 = x1;
			X2 = x2;
			X3 = x3;
			X4 = x4;
			Y1 = y1;
			Y2 = y2;
			Y3 = y3;
			Y4 = y4;
        }
	}

