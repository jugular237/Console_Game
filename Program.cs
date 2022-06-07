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
        static List<int> spawnIntervals = new List<int>() {200 };

        static Random random = new Random();

        private const int FieldSizeX = 95;
        private const int FieldSizeY = 30;
        private const int frameRate = 14;
        private const int spawnNumber = 3;
        private const int XLeftBorder = 1;
        private const int XRightBorder = FieldSizeX - 2;
        private const int YTopBorder = 1;
        private const int YBottomBorder = FieldSizeY - 2;
        private const int YBoxRoof = FieldSizeY - 10;

        private const int XCoordPlayer = Player.XCoord;
        private const int YCoordPlayer = Player.YCoord;

        private static int BulletcounterLeft = 0;
        private static int BulletcounterRight = 0;
        private static int BulletcounterUp = 0;
        private static int BulletSkipCounter = 0;

        static bool bulletOnLeftWay;
        static bool bulletOnUpWay; 
        static bool bulletOnRightWay;

        static BasicStats.Direction direction = BasicStats.Direction.Up;

        static int monsterRate=0;
        static bool bulletUpDestroyed = false;
        public static bool[] YcoordEngaged = new bool[Enemy1.wayLength+3];
        

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.Unicode;
            InitializeField();
            while(!player.isDead)
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
            if (random.Next(0, spawnIntervals[0]) == spawnNumber )
                enemies1.Enqueue(new Enemy1());
            foreach (var enemy1 in enemies1)
                if (enemy1.isDead == false) 
                    InitializeEnemy(enemy1);
        }
        static void AnimateBullet(Coordinates coords)
        {
            bool checkOnLeft = player.direction == BasicStats.Direction.Left;
            bool checkOnRight = player.direction == BasicStats.Direction.Right;
            bool checkOnUp = player.direction == BasicStats.Direction.Up;
            bool bulletOnLeftEnd = BulletcounterLeft < coords.xCoords[0];
            bool bulletOnRightEnd = BulletcounterRight + coords.xCoords[1] < FieldSizeX - 1;
            bool bulletOnUpEnd = BulletcounterUp < coords.yCoords[2];
            if ((checkOnLeft || bulletOnLeftWay) && bulletOnLeftEnd)
            {
                bulletOnLeftWay = true;
                CleanOrWriteBullet(coords.xCoords[0] - BulletcounterLeft, coords.yCoords[0], "-");
                CleanOrWriteBullet((coords.xCoords[0] + 1) - BulletcounterLeft, coords.yCoords[0], " ");
                if (BulletcounterLeft == coords.xCoords[0] - 1)
                {
                    BulletcounterLeft = 0;
                    CleanOrWriteBullet(XLeftBorder, coords.yCoords[0], " ");
                    bulletOnLeftWay = false;
                }
                else
                    BulletcounterLeft++;
            }
            if ((checkOnRight || bulletOnRightWay) && bulletOnRightEnd)
            {
                bulletOnRightWay = true;
                CleanOrWriteBullet(coords.xCoords[1] + BulletcounterRight, coords.yCoords[1], "-");
                CleanOrWriteBullet((coords.xCoords[1] - 1) + BulletcounterRight, coords.yCoords[1], " ");
                if (BulletcounterRight + coords.xCoords[1] == XRightBorder)
                {
                    BulletcounterRight = 0;
                    CleanOrWriteBullet(XRightBorder, coords.yCoords[1], " ");
                    bulletOnRightWay = false;
                }
                else
                    BulletcounterRight++;
            }
            if ((checkOnUp || bulletOnUpWay) && bulletOnUpEnd)
            {
                bulletOnUpWay = true;
                if (bulletUpDestroyed)
                {
                    BulletcounterUp = 0;
                    if (BulletSkipCounter == coords.yCoords[2] - 1)
                    {
                        BulletSkipCounter = 0;
                        bulletOnUpWay = false;
                        bulletUpDestroyed = false;
                    }
                    if (bulletOnUpWay)
                        BulletSkipCounter++;
                }
                else if (!bulletUpDestroyed)
                {
                    CleanOrWriteBullet(coords.xCoords[2], coords.yCoords[2] - BulletcounterUp, "'");
                    CleanOrWriteBullet(coords.xCoords[2], (coords.yCoords[2] + 1) - BulletcounterUp, " ");
                    if (BulletcounterUp == coords.yCoords[2] - 1)
                    {
                        BulletcounterUp = 0;
                        CleanOrWriteBullet(coords.xCoords[2], YTopBorder, " ");
                        bulletOnUpWay = false;
                        BulletSkipCounter = 0;
                    }
                    if (bulletOnUpWay)
                    {
                        BulletcounterUp++;
                        BulletSkipCounter++;
                    }
                }
            }

        }

        static void CleanOrWriteBullet(int coordx, int coordy, string symb)
        {
            Console.SetCursorPosition(coordx, coordy);
            Console.Write(symb);
        }  
        static void InitializeEnemy(Enemy1 enemy1)
        {
            enemy1.isAttacking = false;
            bool playerUnderHit = player.CheckOnHit(mSpawn.YUpSpawn + enemy1.wayCounter, YBoxRoof);
            bool enemyUnderHit = enemy1.CheckOnHit(YBoxRoof - BulletcounterUp, mSpawn.YUpSpawn + enemy1.wayCounter);
            bool enemy1Turn = monsterRate % enemy1.Speed == 0;
            if (enemy1.Health > 0)
            {
                if (playerUnderHit && enemy1Turn)
                {
                    player.GetDamaged();
                }
                if (enemyUnderHit)
                {
                    enemy1.GetDamaged();
                    bulletUpDestroyed = true;
                }
                if (enemy1Turn && !YcoordEngaged[enemy1.EngagedYcoord + 1] && !enemy1.reachedBox)
                {
                    enemy1.AnimateEnemy(direction); 
                    YcoordEngaged[enemy1.EngagedYcoord] = true;
                    YcoordEngaged[enemy1.EngagedYcoord - 1] = false;
                }
                else if (enemy1Turn && enemy1.reachedBox)
                    enemy1.isAttacking = true;
                monsterRate++;
            }
            else
                Enemy1Die(enemy1);
        }
        
        static void Enemy1Die(Enemy1 enemy1)
        {
            if (enemy1.reachedBox) 
                CleanOrWriteBullet(mSpawn.XUpSpawn, YBoxRoof-1, new String(' ', 23));
            else 
                enemy1.CleanOrWriteSymbol(mSpawn.XUpSpawn, mSpawn.YUpSpawn + enemy1.wayCounter - 1, new String(' ', 23));
            YcoordEngaged[enemy1.EngagedYcoord] = false;
            enemy1.isDead = true;
        }
        
        static void InitializePlayer()
        {
            if (player.Health > 0)
            {
                player.SetColor("White");
                player.DrawBox(new Coordinates(
                    new int[] {XCoordPlayer - 9, XCoordPlayer + 16, XCoordPlayer - 1, XCoordPlayer + 11}, 
                    new int[] { FieldSizeY - 3, FieldSizeY - 10,FieldSizeY - 5, FieldSizeY - 6}));
                player.DrawCreature();
                AnimateBullet(new Coordinates(
                    new int[] { 34, 62, XCoordPlayer + 8},
                    new int[] { YCoordPlayer, YCoordPlayer, 19}));
            }
            else
                player.isDead = true;
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
