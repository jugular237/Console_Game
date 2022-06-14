using System;
using System.Collections.Generic;
using System.Threading;
using static Constants;
using static BasicStats;
using System.Diagnostics;


namespace Console_Game
{
    class Program
    {
        static Player player = Player.GetInstance();
        static ResultsMenu resMenu = new ResultsMenu();

        static MonstersSpawns mSpawn;

        static Stopwatch sw = new Stopwatch();

        static Queue<SpiderEnemy> SpiderEnemies = new Queue<SpiderEnemy>();
        static Queue<Zombie> Zombies = new Queue<Zombie>();
        static Queue<Hooker> Hooker = new Queue<Hooker>();

        static List<int> startSpawnIntervals = new List<int>() { 400, 300, 800 };
        static List<int> minSpawnIntervals = new List<int>() { 120, 150, 400 };

        static Random random = new Random();

        private const int FieldSizeX = 95;
        private const int FieldSizeY = 30;
        
        private const int XCoordPlayer = Player.XCoord;
        private const int YCoordPlayer = Player.YCoord;

        private static int BulletcounterLeft = 0;
        private static int BulletcounterRight = 0;
        private static int BulletcounterUp = 0;
        private static int BulletSkipUpCounter = 0;
        private static int BulletSkipLeftCounter = 0;
        private static int BulletSkipRightCounter = 0;

        static bool bulletOnLeftWay;
        static bool bulletOnUpWay;
        static bool bulletOnRightWay;

        static int WhiteDelay = 0;

        static int spiderRate = 0;
        static int zombieRate = 0;
        static int hookerRate = 0;
        static bool bulletUpDestroyed = false;
        static bool bulletLeftDestroyed = false;
        static bool bulletRightDestroyed = false;
        static int killsCounter = 0;

        public static bool[] YcoordEngaged = new bool[SpiderEnemy.wayLength + 3];
        public static bool[] XcoordEngaged = new bool[FieldSizeX];

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.Unicode;
            sw.Start();
            InitializeField();
            while (!player.isDead)
                Update();
            sw.Stop();
            Console.Clear();
            Console.WriteLine("YOU LOST");
            resMenu.WriteNewRecords(killsCounter, sw.ElapsedMilliseconds/1000);
            Console.ReadKey();
        }


        static void Update()
        {
            SpawnerSpiders();
            SpawnerHooker();
            SpawnerZombies();
            InitializePlayer();
            Thread.Sleep(frameRate);
        }
        

        static  void SpawnerSpiders()
        {
            if (random.Next(0, startSpawnIntervals[0]) == spawnNumber )
                SpiderEnemies.Enqueue(new SpiderEnemy());
            IEnumerator<SpiderEnemy> enumerator = SpiderEnemies.GetEnumerator();
            while (enumerator.MoveNext())
            {
                if (!enumerator.Current.isDead)
                    InitializeSpider(enumerator.Current);
            }
        }

        static void SpawnerZombies()
        {
            if (FreeSpawner(zombieLngth) && random.Next(0, startSpawnIntervals[1]) == spawnNumber1)
                Zombies.Enqueue(new Zombie());
            IEnumerator<Zombie> enumerator = Zombies.GetEnumerator();
            while (enumerator.MoveNext())
            {
                if (!enumerator.Current.isDead)
                    InitializeZombie(enumerator.Current);
            }
        }
        static bool FreeSpawner(int length)
        {
            for(int i =0; i<length; i++)
            {
                if (!XcoordEngaged[i])
                    return true;
            }
            return false;
        }
        static void SpawnerHooker()
        {
            if (FreeSpawner(hookerLngth) && random.Next(0, startSpawnIntervals[2]) == spawnNumber2)
                Hooker.Enqueue(new Hooker());
            IEnumerator<Hooker> enumerator = Hooker.GetEnumerator();
            while (enumerator.MoveNext())
            {
                if (!enumerator.Current.isDead)
                    InitializeHooker(enumerator.Current);
            }
        }

        static void AnimateBullet(Coordinates coords)
        {
            bool checkOnLeft = player.direction == Direction.Left;
            bool checkOnRight = player.direction == Direction.Right;
            bool checkOnUp = player.direction == Direction.Up;
            bool bulletOnLeftEnd = BulletcounterLeft < coords.xCoords[0];
            bool bulletOnRightEnd = BulletcounterRight + coords.xCoords[1] < bulletRightRange;
            bool bulletOnUpEnd = BulletcounterUp < coords.yCoords[2];
            if ((checkOnLeft || bulletOnLeftWay) && bulletOnLeftEnd)
            {
                bulletOnLeftWay = true;
                AnimateBulletLeft(coords);
                BulletHitHooker();
            }
            if ((checkOnRight || bulletOnRightWay) && bulletOnRightEnd)
            {
                bulletOnRightWay = true;
                AnimateBulletRight(coords);
                BulletHitZombie();
            }
            if ((checkOnUp || bulletOnUpWay) && bulletOnUpEnd)
            {
                bulletOnUpWay = true;
                AnimateBulletUp(coords);
                BulletHitSpider();
            }
        }
        static void AnimateBulletUp(Coordinates coords)
        {
            var direct = Direction.Up;
            if (bulletUpDestroyed)
                BulletDestroyed(ref BulletcounterUp, ref BulletSkipUpCounter, ref bulletOnUpWay, ref bulletUpDestroyed, coords.yCoords[2] - 1);
            else if (!bulletUpDestroyed)
                BulletOnWay(ref BulletcounterUp, ref BulletSkipUpCounter, ref bulletOnUpWay, coords, "'", direct);
        }
        static void AnimateBulletLeft(Coordinates coords)
        {
            var direct = Direction.Left;
            if (bulletLeftDestroyed)
                BulletDestroyed(ref BulletcounterLeft, ref BulletSkipLeftCounter, ref bulletOnLeftWay, ref bulletLeftDestroyed, coords.xCoords[0]);
            else if (!bulletLeftDestroyed)
                BulletOnWay(ref BulletcounterLeft, ref BulletSkipLeftCounter, ref bulletOnLeftWay, coords, "-", direct);
        }

        static void AnimateBulletRight(Coordinates coords)
        {
            var direct = Direction.Right;
            if (bulletRightDestroyed)
                BulletDestroyed(ref BulletcounterRight, ref BulletSkipRightCounter, ref bulletOnRightWay, ref bulletRightDestroyed, coords.xCoords[1]);
            else if (!bulletRightDestroyed)
                BulletOnWay(ref BulletcounterRight, ref BulletSkipRightCounter, ref bulletOnRightWay, coords, "-", direct);
        }
        static void BulletDestroyed(ref int bullCounter, ref int skipCounter, ref bool onWay,
            ref bool destroyed, int border)
        {
            bullCounter = 0;
            if (skipCounter == border)
            {
                skipCounter = 0;
                onWay = false;
                destroyed = false;
            }
            if (onWay)
                skipCounter++;
        }
        static void BulletOnWay(ref int bullCounter, ref int skipCounter, ref bool onWay, Coordinates coords, string bullt, Direction dir)
        {
            if (dir == Direction.Up) 
            {
                DrawBulletUp(coords.xCoords[2], coords.yCoords[2], bullCounter, bullt);
                ClearLastBulletUp(ref bullCounter, coords, ref onWay, ref skipCounter);
            }
            if (dir == Direction.Left)
            {
                DrawBulletLeft(coords.xCoords[0], coords.yCoords[0], bullCounter, bullt);
                ClearLastBulletLeft(ref bullCounter, coords, ref onWay, ref skipCounter);
            }
            if (dir == Direction.Right)
            {
                DrawBulletRight(coords.xCoords[1], coords.yCoords[1], bullCounter, bullt);
                ClearLastBulletRight(ref bullCounter, coords, ref onWay, ref skipCounter);
            }
            if (onWay)
            {
                bullCounter++;
                skipCounter++;
            }
        }

        static void DrawBulletUp(int coordX, int coordY, int bullCounter, string bullt)
        {
            CleanOrWriteBullet(coordX, coordY - bullCounter, bullt);
            CleanOrWriteBullet(coordX, (coordY + 1) - bullCounter, " ");
        }
        static void DrawBulletLeft(int coordX, int coordY, int bullCounter, string bullt)
        {
            CleanOrWriteBullet(coordX - bullCounter, coordY, bullt);
            CleanOrWriteBullet(coordX + 1 - bullCounter, coordY, " ");
        }

        static void DrawBulletRight(int coordX, int coordY, int bullCounter, string bullt)
        {
            CleanOrWriteBullet(coordX + bullCounter, coordY, bullt);
            CleanOrWriteBullet(coordX - 1 + bullCounter, coordY, " ");
        }
        static void ClearLastBulletUp(ref int bullCounter, Coordinates coords, ref bool onWay, ref int skipCounter)
        {
            if (bullCounter == coords.yCoords[2] - 1)
            {
                bullCounter = 0;
                onWay = false;
                skipCounter = 0;
                CleanOrWriteBullet(coords.xCoords[2], YTopBorder, " ");
            }
        }
        static void ClearLastBulletLeft(ref int bullCounter, Coordinates coords, ref bool onWay, ref int skipCounter)
        {
            if (bullCounter == coords.xCoords[0] - 1)
            {
                bullCounter = 0;
                onWay = false;
                skipCounter = 0;
                CleanOrWriteBullet(XLeftBorder, coords.yCoords[0], " ");
            }
        }

        static void ClearLastBulletRight(ref int bullCounter, Coordinates coords, ref bool onWay, ref int skipCounter)
        {
            if (bullCounter + coords.xCoords[1] == bulletRightRange - 1)
            {
                bullCounter = 0;
                onWay = false;
                skipCounter = 0;
                CleanOrWriteBullet(bulletRightRange - 1, coords.yCoords[1], " ");
            }
        }
        static void CleanOrWriteBullet(int coordx, int coordy, string symb)
        {
            Console.SetCursorPosition(coordx, coordy);
            Console.Write(symb);
        }  
        static void BulletHitSpider()
        {
            if(SpiderEnemies.Count > 0)
            {
                SpiderEnemy firstSpider = SpiderEnemies.Peek();
                int bulletCoord = YBoxRoof - BulletcounterUp + 1;
                int spiderCoord = mSpawn.YUpSpawn + firstSpider.wayCounter;
                if (firstSpider.CheckOnHit(bulletCoord, spiderCoord))
                {
                    firstSpider.GetDamaged();
                    if (firstSpider.Health <= 0)
                    {
                        SpiderDie(firstSpider);
                        SpiderEnemies.Dequeue();
                        killsCounter++;
                    }

                }
            }
        }
        static void BulletHitZombie()
        {
            if (Zombies.Count > 0)
            {
                Zombie firstZomb = Zombies.Peek();
                int bulletCoord = rightBorderBox + BulletcounterUp -1;
                int zombCoord = mSpawn.XRightSpawn - firstZomb.wayCounter;
                if (firstZomb.CheckOnHit(bulletCoord, zombCoord))
                {
                    firstZomb.GetDamaged();
                    if (firstZomb.Health <= 0)
                    {
                        ZombieDie(firstZomb);
                        Zombies.Dequeue();
                        killsCounter++;
                    }
                }
            }
        }
        static void BulletHitHooker()
        {
            if (Hooker.Count > 0)
            {
                Hooker firsthooker = Hooker.Peek();
                int bulletCoord = leftBorderBox - BulletcounterUp + 1;
                int zombCoord = mSpawn.XLeftSpawn - firsthooker.wayCounter;
                if (firsthooker.CheckOnHit(bulletCoord, zombCoord))
                {
                    firsthooker.GetDamaged();
                    if (firsthooker.Health <= 0)
                    {
                        HookerDie(firsthooker);
                        Hooker.Dequeue();
                        killsCounter++;
                    }
                }
            }
        }
        static void InitializeSpider(SpiderEnemy spiderEnemy)
        {
            spiderEnemy.isAttacking = false;
            bool playerUnderHit = player.CheckOnHit(mSpawn.YUpSpawn + spiderEnemy.wayCounter+zombieLngth, YBoxRoof);
            bool enemyUnderHit = spiderEnemy.CheckOnHit(YBoxRoof - BulletcounterUp, mSpawn.YUpSpawn + spiderEnemy.wayCounter);
            bool spiderTurn = spiderRate % spiderEnemy.Speed == 0;
            if (spiderEnemy.Health > 0)
            {
                if (playerUnderHit && spiderTurn)
                {
                    player.GetDamaged();
                }
                if (enemyUnderHit)
                {
                    spiderEnemy.GetDamaged();
                    bulletUpDestroyed = true;
                }
                if (spiderTurn && !YcoordEngaged[spiderEnemy.EngagedYcoord + 1])
                {
                    spiderEnemy.AnimateEnemy();
                    YcoordEngaged[spiderEnemy.EngagedYcoord] = true;
                    YcoordEngaged[spiderEnemy.EngagedYcoord - 1] = false;
                }
                else if (spiderTurn)
                    spiderEnemy.isAttacking = true;
                spiderRate++;
            }
            else
            { 
                SpiderDie(spiderEnemy);
            }
        }
        static void InitializeZombie(Zombie zombie)
        {
            zombie.isAttacking = false;
            bool playerUnderHit = player.CheckOnHit(mSpawn.XRightSpawn - zombie.wayCounter-1, rightBorderBox);
            bool enemyUnderHit = zombie.CheckOnHit(rightBorderBox + BulletcounterRight, mSpawn.XRightSpawn - zombie.wayCounter);
            bool zombieTurn = zombieRate % zombie.Speed == 0;
            if (zombie.Health > 0)
            {
                if (playerUnderHit && zombieTurn)
                {
                    player.GetDamaged();
                }
                if (enemyUnderHit)
                {
                    zombie.GetDamaged();
                    bulletRightDestroyed = true;
                }
                if (zombieTurn && !XcoordEngaged[zombie.EngagedXcoord - zombieLngth])
                {
                    zombie.AnimateEnemy();
                    for (int i = 0; i < zombieLngth; i++)
                    {
                        XcoordEngaged[zombie.EngagedXcoord - i] = true;
                    }
                    XcoordEngaged[zombie.EngagedXcoord + 1] = false;
                }
                else if (zombieTurn)
                    zombie.isAttacking = true;
                zombieRate++;
            }
            else
                ZombieDie(zombie);
        }
        static void InitializeHooker(Hooker hooker)
        {
            hooker.isAttacking = false;
            bool playerUnderHit = player.CheckOnHit(mSpawn.XLeftSpawn + hooker.wayCounter + entireHookerLngth, leftBorderBox);
            bool enemyUnderHit = hooker.CheckOnHit(leftBorderBox - BulletcounterLeft, mSpawn.XLeftSpawn + hooker.wayCounter + hookerLngth);
            bool hookerTurn = hookerRate % hooker.Speed == 0;
            if (hooker.Health > 0)
            {
                if (playerUnderHit && hookerTurn)
                {
                    player.GetDamaged();
                }
                if (enemyUnderHit)
                {
                    hooker.GetDamaged();
                    bulletLeftDestroyed = true;
                }
                if (hookerTurn && !XcoordEngaged[hooker.EngagedXcoord + entireHookerLngth])
                {
                    hooker.AnimateEnemy();
                    for (int i = 0; i < entireHookerLngth; i++)
                    {
                        XcoordEngaged[hooker.EngagedXcoord + i] = true;
                    }
                    XcoordEngaged[hooker.EngagedXcoord - 1] = false;
                }
                else if (hookerTurn)
                    hooker.isAttacking = true;
                hookerRate++;
            }
            else
                HookerDie(hooker);
        }
        static void SpiderDie(SpiderEnemy spider)
        {
            int spiderCoord = mSpawn.YUpSpawn + spider.wayCounter - 1;
            spider.CleanOrWriteSymbol(mSpawn.XUpSpawn, spiderCoord, new String(' ', clearSpiderLngth));
            spider.ClearWeb(mSpawn.XUpSpawn, spiderCoord, new String(' ', clearWebLngth), YBoxRoof);
            YcoordEngaged[spider.EngagedYcoord] = false;
            spider.isDead = true;
            if (startSpawnIntervals[0] > minSpawnIntervals[0])
                startSpawnIntervals[0] -= decSpiderSpwnInterv;
        }

        static void ZombieDie(Zombie zombie)
        {
            int zombieCoord = mSpawn.XRightSpawn - zombie.wayCounter;
            zombie.CleanOrWriteSymbol(zombieCoord, mSpawn.YRightSpawn - 1, new String(' ', clearZombieLngth));
            zombie.CleanOrWriteSymbol(zombieCoord, mSpawn.YRightSpawn, new String(' ', clearZombieLngth));
            zombie.CleanOrWriteSymbol(zombieCoord, mSpawn.YRightSpawn + 1, new String(' ', clearZombieLngth));
            for (int i =0; i < zombieLngth; i++)
            {
                XcoordEngaged[zombie.EngagedXcoord - i] = false;
            }
            zombie.isDead = true;
            if (startSpawnIntervals[1] > minSpawnIntervals[1])
                startSpawnIntervals[1] -= decZombieSpwnInterv;
        }
        static void HookerDie(Hooker hooker)
        {
            int hookerCoord = mSpawn.XLeftSpawn + hooker.wayCounter - 1;
            for(int i = 0; i < hookerHight; i++)
            {
                if (i == 4) 
                { 
                    hooker.CleanOrWriteSymbol(hookerCoord, YBottomBorder - hookerHight + i, new String(' ', 16));
                    continue;
                }
                hooker.CleanOrWriteSymbol(hookerCoord, YBottomBorder - hookerHight + i, new String(' ', 11));
            }
            for (int i = 0; i < hookerLngth; i++)
            {
                XcoordEngaged[hooker.EngagedXcoord + i] = false;
            }
            hooker.isDead = true;
            if (startSpawnIntervals[2] > minSpawnIntervals[2])
                startSpawnIntervals[2] -= decHookerSpwnInterv;
        }

        static void InitializePlayer()
        {
            if (player.Health > 0)
            {
                PlayerBlink();
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
        static void PlayerBlink()
        {
            if (WhiteDelay == blinkRate)
            {
                player.SetColor("White");
                WhiteDelay = 0;
            }
            WhiteDelay++;
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
