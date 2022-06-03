using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;


namespace Console_Game
{
    class Program 
    {
        static Player player = new Player();

        static Enemy1 enemy1 = new Enemy1();

        static MonstersSpawns mSpawn;

        private const int FieldSizeX = 95;
        private const int FieldSizeY = 30;
        private const int frameRate = 14;

        private const int XCoordPlayer = Player.XCoord;
        private const int YCoordPlayer = Player.YCoord;

        private static int BulletcounterLeft = 0;
        private static int BulletcounterRight = 0;
        private static int BulletcounterUp = 0;

        static bool bulletOnLeftWay, bulletOnUpWay, bulletOnRightWay;

        static BasicStats.Direction direction = BasicStats.Direction.up;

        static int monsterRate=0;
        static bool bulletUpDestroyed = false;

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.Unicode;
            InitializeField();
            while(player.isDead==false)
                Update();
            Console.WriteLine("YOU LOST");
            Console.ReadKey();
        }

        static void Update()
        {
            InitializePlayer();
            if (enemy1.isDead == false)
                InitializeEnemy();
            Thread.Sleep(frameRate);
        }

        
        static void AnimateBullet(Coordinates coords)
        {
            if ((player.direction == BasicStats.Direction.left || bulletOnLeftWay) && BulletcounterLeft < coords.X1)
            {
                bulletOnLeftWay = true;
                CleanOrWriteBullet(coords.X1 - BulletcounterLeft, coords.Y1, "-");
                CleanOrWriteBullet((coords.X1 + 1) - BulletcounterLeft, coords.Y1, " ");
                if (BulletcounterLeft == coords.X1-1)
                {
                    BulletcounterLeft = 0;
                    CleanOrWriteBullet(1, coords.Y1, " ");
                    bulletOnLeftWay = false;
                }
                else
                    BulletcounterLeft += 1;
            }
            if ((player.direction == BasicStats.Direction.right || bulletOnRightWay) && BulletcounterRight + coords.X2 < FieldSizeX - 1)
            {
                bulletOnRightWay = true;
                CleanOrWriteBullet(coords.X2 + BulletcounterRight, coords.Y2, "-");
                CleanOrWriteBullet((coords.X2 - 1) + BulletcounterRight, coords.Y2, " ");
                if (BulletcounterRight + coords.X2 == FieldSizeX - 2)
                {
                    BulletcounterRight = 0;
                    CleanOrWriteBullet(FieldSizeX - 2, coords.Y2, " ");
                    bulletOnRightWay = false;
                }
                else
                    BulletcounterRight += 1;
            }
            if ((player.direction == BasicStats.Direction.up || bulletOnUpWay) && BulletcounterUp < coords.Y3)
            {
                if (bulletUpDestroyed)
                {
                    DestroyBullet(ref bulletUpDestroyed, coords.Y3 - 1, ref BulletcounterUp);
                }
                bulletOnUpWay = true;
                CleanOrWriteBullet(coords.X3, coords.Y3 - BulletcounterUp, "'");
                CleanOrWriteBullet(coords.X3, (coords.Y3 + 1) - BulletcounterUp, " ");
                if (BulletcounterUp == coords.Y3 - 1)
                {
                    BulletcounterUp = 0;
                    CleanOrWriteBullet(coords.X3, 1, " ");
                    bulletOnUpWay = false;
                }
                else
                    BulletcounterUp += 1;
            }

        }

        static void DestroyBullet(ref bool bulletDestroyed, int coord, ref int bulletCounter)
        {
           bulletDestroyed = false;
           bulletCounter = coord;
        }
        static void CleanOrWriteBullet(int coordx, int coordy, string symb)
        {
            Console.SetCursorPosition(coordx, coordy);
            Console.Write(symb);
        }  
        static void InitializeEnemy()
        {
            if (enemy1.Health > 0)
            {
                if (enemy1.CheckOnHit(20 - BulletcounterUp, mSpawn.YUpSpawn + enemy1.wayCounter))
                {
                    enemy1.GetDamaged();
                    bulletUpDestroyed = true;
                }
                if (monsterRate % 20 == 0)
                    enemy1.AnimateEnemy(direction);
                monsterRate++;
            }
            else
            {
                enemy1.CleanOrWriteSymbol(mSpawn.XUpSpawn, mSpawn.YUpSpawn + enemy1.wayCounter - 1, "                               ");
                for (int i = 21 - BulletcounterUp; i > 0; i--)
                {
                    enemy1.CleanOrWriteSymbol(mSpawn.XUpSpawn, i, "        ");
                }
                enemy1.isDead = true;
            }
        }

        static void InitializePlayer()
        {
            player.SetColor("White");
            player.DrawCreature();
            AnimateBullet(new Coordinates(34, YCoordPlayer, 62, YCoordPlayer, XCoordPlayer + 8, 19));
        }

        static void InitializeField()
        {
            Console.SetWindowSize(FieldSizeX, FieldSizeY);
            Console.SetBufferSize(FieldSizeX, FieldSizeY);
            Console.CursorVisible = false;
            DrawBorders();
        }

        static void DrawBorders()
        {
            for(int i = 0; i < FieldSizeY; i++)
            {
                for (int j = 0; j < FieldSizeX; j++)
                {
                    if (i == 1 || i == FieldSizeY - 1)
                    {
                        Console.SetCursorPosition(j, i);
                        Console.Write('-');
                    }
                    else if (j == 0 || j == FieldSizeX - 1)
                    {
                        Console.SetCursorPosition(j, i);
                        Console.Write('|');
                    }
                }
                
            }

            for(int i = FieldSizeY-3; i>FieldSizeY - 10; i--)
            {
                Console.SetCursorPosition(XCoordPlayer - 12, i);
                Console.Write('|');
                Console.SetCursorPosition(XCoordPlayer + 12, i);
                Console.Write('|');
            }
            for(int i = XCoordPlayer - 12; i< XCoordPlayer + 12; i++)
            {
                Console.SetCursorPosition(i, FieldSizeY - 10);
                Console.Write('-');
            }
                
        }
    }
}
