using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using static Constants;


namespace Console_Game
{
    class Program
    {
        static Player player = Player.GetInstance();

        static MonstersSpawns mSpawn;

        static Queue<Enemy1> enemies1 = new Queue<Enemy1>();
        static List<int> spawnIntervals = new List<int>() { 80 };

        static Random random = new Random();

        private const int FieldSizeX = 95;
        private const int FieldSizeY = 30;
        
        private const int XCoordPlayer = Player.XCoord;
        private const int YCoordPlayer = Player.YCoord;

        private static int BulletcounterLeft = 0;
        private static int BulletcounterRight = 0;
        private static int BulletcounterUp = 0;
        private static int BulletSkipUpCounter = 0;

        static bool bulletOnLeftWay;
        static bool bulletOnUpWay;
        static bool bulletOnRightWay;
        static bool bulletKilled = false;

        static BasicStats.Direction direction = BasicStats.Direction.Up;

        static int monsterRate = 0;
        static bool bulletUpDestroyed = false;
        public static bool[] YcoordEngaged = new bool[Enemy1.wayLength + 3];


        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.Unicode;
            InitializeField();

            while (!player.isDead)
                Update();
            Console.WriteLine("YOU LOST");
            Console.ReadKey();
        }


        static void Update()
        {
            SpawnerEnemy1();
            InitializePlayer();
            BulletKilled();
            Thread.Sleep(frameRate);
        }
        static void BulletKilled()
        {
            if (bulletKilled)
            {
                bulletKilled = false;
            }
        }

        static  void SpawnerEnemy1()
        {
            if (random.Next(0, spawnIntervals[0]) == Constants.spawnNumber )
                enemies1.Enqueue(new Enemy1());
            IEnumerator<Enemy1> enumerator = enemies1.GetEnumerator();
            while (enumerator.MoveNext())
            {
                if (!enumerator.Current.isDead)
                    InitializeEnemy(enumerator.Current);
            }
        }
        static void AnimateBullet(Coordinates coords)
        {
            bool checkOnLeft = player.direction == BasicStats.Direction.Left;
            bool checkOnRight = player.direction == BasicStats.Direction.Right;
            bool checkOnUp = player.direction == BasicStats.Direction.Up;
            bool bulletOnLeftEnd = BulletcounterLeft < coords.xCoords[0];
            bool bulletOnRightEnd = BulletcounterRight + coords.xCoords[1] < bulletRightRange;
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
                if (BulletcounterRight + coords.xCoords[1] == bulletRightRange-1)
                {
                    BulletcounterRight = 0;
                    CleanOrWriteBullet(bulletRightRange-1, coords.yCoords[1], " ");
                    bulletOnRightWay = false;
                }
                else
                    BulletcounterRight++;
            }
            if ((checkOnUp || bulletOnUpWay) && bulletOnUpEnd)
            {
                bulletOnUpWay = true;
                AnimateBulletUp(coords);
                BulletHit();
            }
        }
        static void AnimateBulletUp(Coordinates coords)
        {
            if (bulletUpDestroyed)
                BulletDestroyed(ref BulletcounterUp, ref BulletSkipUpCounter, ref bulletOnUpWay, ref bulletUpDestroyed, coords);
            else if (!bulletUpDestroyed)
                BulletOnWay(ref BulletcounterUp, ref BulletSkipUpCounter, ref bulletOnUpWay, coords);
        }
        static void BulletDestroyed(ref int bullCounter, ref int skipCounter, ref bool onWay,
            ref bool destroyed, Coordinates coords)
        {
            bullCounter = 0;
            if (skipCounter == coords.yCoords[2] - 1)
            {
                skipCounter = 0;
                onWay = false;
                destroyed = false;
            }
            if (onWay)
                skipCounter++;
        }
        static void BulletOnWay(ref int bullCounter, ref int skipCounter, ref bool onWay, Coordinates coords)
        {
            CleanOrWriteBullet(coords.xCoords[2], coords.yCoords[2] - bullCounter, "'");
            CleanOrWriteBullet(coords.xCoords[2], (coords.yCoords[2] + 1) - bullCounter, " ");
            if (bullCounter == coords.yCoords[2] - 1)
            {
                bullCounter = 0;
                CleanOrWriteBullet(coords.xCoords[2], YTopBorder, " ");
                onWay = false;
                skipCounter = 0;
            }
            if (onWay)
            {
                bullCounter++;
                skipCounter++;
            }
        }
        static void CleanOrWriteBullet(int coordx, int coordy, string symb)
        {
            Console.SetCursorPosition(coordx, coordy);
            Console.Write(symb);
        }  
        static void BulletHit()
        {
            if(enemies1.Count > 0)
            {
                Enemy1 firstSpider = enemies1.Peek();
                int bulletCoord = YBoxRoof - BulletcounterUp + 1;
                int spiderCoord = mSpawn.YUpSpawn + firstSpider.wayCounter;
                if (firstSpider.CheckOnHit(bulletCoord, spiderCoord))
                {
                    firstSpider.GetDamaged();
                    if (firstSpider.Health <= 0)
                    {
                        Enemy1Die(firstSpider);
                        enemies1.Dequeue();
                    }

                }
            }
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
                if (enemy1Turn && !YcoordEngaged[enemy1.EngagedYcoord + 1])
                {
                    enemy1.AnimateEnemy(direction); 
                    YcoordEngaged[enemy1.EngagedYcoord] = true;
                    YcoordEngaged[enemy1.EngagedYcoord - 1] = false;
                }
                else if (enemy1Turn)
                    enemy1.isAttacking = true;
                monsterRate++;
            }
            else
                Enemy1Die(enemy1);
        }
        
        static void Enemy1Die(Enemy1 enemy1)
        {
            int spiderCoord = mSpawn.YUpSpawn + enemy1.wayCounter - 1;
            enemy1.CleanOrWriteSymbol(mSpawn.XUpSpawn, spiderCoord, new String(' ', clearSpiderLngth));
            enemy1.ClearWeb(mSpawn.XUpSpawn, spiderCoord, new String(' ', clearWebLngth), YBoxRoof);
            YcoordEngaged[enemy1.EngagedYcoord] = false;
            enemy1.isDead = true;
            bulletKilled = true;
        }
        
        static void InitializePlayer()
        {
            if (player.Health > 0)
            {
                player.SetColor("White");
                player.DrawBox(new Coordinates(
                    new int[] {leftBorderBox, rightBorderBox, X1roofHole, X2roofHole}, 
                    new int[] { bottomBorderBox, topBorderBox, Y2WallHole, Y1WallHole }));
                player.DrawCreature();
                AnimateBullet(new Coordinates(
                    new int[] { XbulletLeftcoord, XbulletRightcoord, XbulletUpcoord },
                    new int[] { YbulletLeftcoord, YbulletRightcoord, YbulletUpcoord}));
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
            player.DrawPlayer(false, new Coordinates(
                new int[] { XCoordPlayer, XCoordPlayer + 1, XCoordPlayer + 2 },
                new int[] { YCoordPlayer, YCoordPlayer + 1, YCoordPlayer + 2 }),
                    new DrawHero
                    {
                        Head = @"(▀ ͜ʖ▀) *======8",
                        BodyNGun = @"|   | -- //",
                        Legs = @"/ \"
                    });
        }

        static void DrawBorders()
        {
            for(int i = 0; i < FieldSizeY; i++)
            {
                for (int j = 0; j < FieldSizeX; j++)
                {
                    if (i == YTopBorder || i == bottomBorder)
                    {
                        Console.SetCursorPosition(j, i);
                        Console.Write('-');
                    }
                    else if (j == 0 || j == XRightBorder)
                    {
                        Console.SetCursorPosition(j, i);
                        Console.Write('|');
                    }
                }
                 
            }
            
        }

        
    }
}
