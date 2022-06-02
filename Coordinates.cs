using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


	public struct Coordinates
	{
		public int X1 { get; set; }
		public int X2 { get; set; }
		public int X3 { get; set; }
		public int X4 { get; set; }

		public int Y1 { get; set; }
		public int Y2 { get; set; }
		public int Y3 { get; set; }
		public int Y4 { get; set; }

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

