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
        static Player player = Player.GetInstance();

        static MonstersSpawns mSpawn;

        static Queue<Enemy1> enemies1 = new Queue<Enemy1>();

        static Random random = new Random();

        private const int FieldSizeX = 95;
        private const int FieldSizeY = 30;
        private const int frameRate = 14;

        private const int XCoordPlayer = Player.XCoord;
        private const int YCoordPlayer = Player.YCoord;

        private static int BulletcounterLeft = 0;
        private static int BulletcounterRight = 0;
        private static int BulletcounterUp = 0;

        static bool bulletOnLeftWay, bulletOnUpWay, bulletOnRightWay;

        static BasicStats.Direction direction = BasicStats.Direction.Up;

        static int monsterRate=0;
        static bool bulletUpDestroyed = false;
        public static bool[] YcoordEngaged = new bool[Enemy1.wayLength+3];


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
            SpawnerEnemy1();
            Thread.Sleep(frameRate);
        }

        static  void SpawnerEnemy1()
        {
            if (random.Next(0, 200) == 3)
                enemies1.Enqueue(new Enemy1());
            foreach (var enemy1 in enemies1)
                if (enemy1.isDead == false) 
                    InitializeEnemy(enemy1);
        }
        static void AnimateBullet(Coordinates coords)
        {
            if ((player.direction == BasicStats.Direction.Left || bulletOnLeftWay) && BulletcounterLeft < coords.xCoords[0])
            {
                bulletOnLeftWay = true;
                CleanOrWriteBullet(coords.xCoords[0] - BulletcounterLeft, coords.yCoords[0], "-");
                CleanOrWriteBullet((coords.xCoords[0] + 1) - BulletcounterLeft, coords.yCoords[0], " ");
                if (BulletcounterLeft == coords.xCoords[0] - 1)
                {
                    BulletcounterLeft = 0;
                    CleanOrWriteBullet(1, coords.yCoords[0], " ");
                    bulletOnLeftWay = false;
                }
                else
                    BulletcounterLeft += 1;
            }
            if ((player.direction == BasicStats.Direction.Right || bulletOnRightWay) && BulletcounterRight + coords.xCoords[1] < FieldSizeX - 1)
            {
                bulletOnRightWay = true;
                CleanOrWriteBullet(coords.xCoords[1] + BulletcounterRight, coords.yCoords[1], "-");
                CleanOrWriteBullet((coords.xCoords[1] - 1) + BulletcounterRight, coords.yCoords[1], " ");
                if (BulletcounterRight + coords.xCoords[1] == FieldSizeX - 2)
                {
                    BulletcounterRight = 0;
                    CleanOrWriteBullet(FieldSizeX - 2, coords.yCoords[1], " ");
                    bulletOnRightWay = false;
                }
                else
                    BulletcounterRight += 1;
            }
            if ((player.direction == BasicStats.Direction.Up || bulletOnUpWay) && BulletcounterUp < coords.yCoords[2])
            {
                if (bulletUpDestroyed)
                {
                    DestroyBullet(ref bulletUpDestroyed, coords.yCoords[2] - 1, ref BulletcounterUp);
                }
                bulletOnUpWay = true;
                CleanOrWriteBullet(coords.xCoords[2], coords.yCoords[2] - BulletcounterUp, "'");
                CleanOrWriteBullet(coords.xCoords[2], (coords.yCoords[2] + 1) - BulletcounterUp, " ");
                if (BulletcounterUp == coords.yCoords[2] - 1)
                {
                    BulletcounterUp = 0;
                    CleanOrWriteBullet(coords.xCoords[2], 1, " ");
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
        static void InitializeEnemy(Enemy1 enemy1)
        {
            if (enemy1.Health > 0)
            {
                if (player.CheckOnHit(mSpawn.YUpSpawn + enemy1.wayCounter, FieldSizeY - 10) && enemy1.isAttacking)
                {
                    player.GetDamaged();
                }
                enemy1.isAttacking = false;
                if (enemy1.CheckOnHit(20 - BulletcounterUp, mSpawn.YUpSpawn + enemy1.wayCounter))
                {
                    enemy1.GetDamaged();
                    bulletUpDestroyed = true;
                }
                if (monsterRate % 20 == 0 && YcoordEngaged[enemy1.EngagedYcoord + 1] == false && enemy1.reachedBox == false)
                {
                    enemy1.AnimateEnemy(direction); 
                    YcoordEngaged[enemy1.EngagedYcoord] = true;
                    YcoordEngaged[enemy1.EngagedYcoord - 1] = false;
                }
                else if (monsterRate % 20 == 0 && enemy1.reachedBox == true)
                    enemy1.isAttacking = true;
                monsterRate++;
            }
            else
            {
                Enemy1Die(enemy1);
            }
        }
        
        static void Enemy1Die(Enemy1 enemy1)
        {
            if (enemy1.reachedBox) 
                CleanOrWriteBullet(mSpawn.XUpSpawn, FieldSizeY - 11, "                       ");
            else 
                enemy1.CleanOrWriteSymbol(mSpawn.XUpSpawn, mSpawn.YUpSpawn + enemy1.wayCounter - 1, "                               ");
            YcoordEngaged[enemy1.EngagedYcoord] = false;
            enemy1.isDead = true;
        }
        
        static void InitializePlayer()
        {
            if (player.Health > 0)
            {
                player.SetColor("White");
                player.DrawBox(new Coordinates(
                    new int[] {XCoordPlayer - 9, XCoordPlayer + 16, XCoordPlayer - 1, XCoordPlayer + 11  }, 
                    new int[] { FieldSizeY - 3, FieldSizeY - 10,FieldSizeY - 5, FieldSizeY - 6}));
                player.DrawCreature();
                AnimateBullet(new Coordinates(
                    new int[] { 34, 62, XCoordPlayer + 8},
                    new int[] { YCoordPlayer, YCoordPlayer, 19}));
            }
            else
            {
                player.isDead = true;
            }
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
        
        }

        
    }
}
