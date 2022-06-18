using System;
using System.Collections.Generic;
using System.Threading;
using static Constants;


class Game: BasicStats
{
    private Player player = Player.GetInstance();

    private MonstersSpawns mSpawn;

    private Queue<SpiderEnemy> SpiderEnemies = new Queue<SpiderEnemy>();
    private Queue<Zombie> Zombies = new Queue<Zombie>();
    private Queue<Hooker> Hooker = new Queue<Hooker>();

    private List<int> startSpawnIntervals = new List<int>() { 400, 400, 800 };
    private List<int> minSpawnIntervals = new List<int>() { 120, 150, 400 };

    private Random random = new Random();

    private const int FieldSizeX = 95;
    private const int FieldSizeY = 30;

    private const int XCoordPlayer = Player.XCoord;
    private const int YCoordPlayer = Player.YCoord;

    private int BulletcounterLeft = 0;
    private int BulletcounterRight = 0;
    private int BulletcounterUp = 0;
    private int BulletSkipUpCounter = 0;
    private int BulletSkipLeftCounter = 0;
    private int BulletSkipRightCounter = 0;

    private bool bulletOnLeftWay;
    private bool bulletOnUpWay;
    private bool bulletOnRightWay;

    private int WhiteDelay = 0;

    private int spiderRate = 0;
    private int zombieRate = 0;
    private int hookerRate = 0;
    private bool bulletUpDestroyed = false;
    private bool bulletLeftDestroyed = false;
    private bool bulletRightDestroyed = false;
    public static int killsCounter = 0;

   
    public delegate void InitializeEnemy<Enemy>(Enemy enemy);

    public InitializeEnemy<SpiderEnemy> spiderDelegate;
    public InitializeEnemy<Zombie> zombieDelegate;
    public InitializeEnemy<Hooker> hookerDelegate;
    
    public bool[] YcoordEngaged = new bool[SpiderEnemy.wayLength + 3];
    public bool[] XcoordEngaged = new bool[FieldSizeX];

    public void StartGame()
    {
        SetColor("White");
        InitializeField();
        while (!player.isDead)
            Update();
    }
    public void Update()
    {
        GatherMethods(); 
        Thread.Sleep(sleepTime);
    }
    
    private void GatherMethods()
    {
        spiderDelegate = InitializeSpider;
        zombieDelegate = InitializeZombie;
        hookerDelegate = InitializeHooker;
        SpawnerEnemies<SpiderEnemy>(SpiderEnemies, new SpiderEnemy(), spiderDelegate);
        SpawnerEnemies<Zombie>(Zombies, new Zombie(), zombieDelegate);
        SpawnerEnemies<Hooker>(Hooker, new Hooker(), hookerDelegate);
        InitializePlayer();
    }

    public void SpawnerEnemies<Enemy>(Queue<Enemy> enemies, Enemy enemyClass, InitializeEnemy<Enemy> initialize)
    {
        bool toSpawn = random.Next(0, startSpawnIntervals[0]) == spawnNumber;
        if (toSpawn)
            enemies.Enqueue(enemyClass);
        foreach(var foe in enemies)
        {
            initialize.Invoke(foe);
        }
    }

    
    public bool FreeSpawner(int length, int pozition)
    {
        for (int i = pozition; i < pozition + length; i++)
            if (XcoordEngaged[i])
                return false;
        return true;
    }
    

    public void AnimateBullet(Coordinates coords)
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
    public void AnimateBulletUp(Coordinates coords)
    {
        var direct = Direction.Up;
        if (bulletUpDestroyed)
            BulletDestroyed(ref BulletcounterUp, ref BulletSkipUpCounter, ref bulletOnUpWay, ref bulletUpDestroyed, coords.yCoords[2] - 1);
        else if (!bulletUpDestroyed)
            BulletOnWay(ref BulletcounterUp, ref BulletSkipUpCounter, ref bulletOnUpWay, coords, "'", direct);
    }
    public void AnimateBulletLeft(Coordinates coords)
    {
        var direct = Direction.Left;
        if (bulletLeftDestroyed)
            BulletDestroyed(ref BulletcounterLeft, ref BulletSkipLeftCounter, ref bulletOnLeftWay, ref bulletLeftDestroyed, coords.xCoords[0]);
        else if (!bulletLeftDestroyed)
            BulletOnWay(ref BulletcounterLeft, ref BulletSkipLeftCounter, ref bulletOnLeftWay, coords, "-", direct);
    }

    public void AnimateBulletRight(Coordinates coords)
    {
        var direct = Direction.Right;
        if (bulletRightDestroyed)
            BulletDestroyed(ref BulletcounterRight, ref BulletSkipRightCounter, ref bulletOnRightWay, ref bulletRightDestroyed, coords.xCoords[0]);
        else if (!bulletRightDestroyed)
            BulletOnWay(ref BulletcounterRight, ref BulletSkipRightCounter, ref bulletOnRightWay, coords, "-", direct);
    }
    public void BulletDestroyed(ref int bullCounter, ref int skipCounter, ref bool onWay,
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
    public void BulletOnWay(ref int bullCounter, ref int skipCounter, ref bool onWay, Coordinates coords, string bullt, Direction dir)
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

    public void DrawBulletUp(int coordX, int coordY, int bullCounter, string bullt)
    {
        CleanOrWriteBullet(coordX, coordY - bullCounter, bullt);
        CleanOrWriteBullet(coordX, (coordY + 1) - bullCounter, " ");
    }
    public void DrawBulletLeft(int coordX, int coordY, int bullCounter, string bullt)
    {
        CleanOrWriteBullet(coordX - bullCounter, coordY, bullt);
        CleanOrWriteBullet(coordX + 1 - bullCounter, coordY, " ");
    }

    public void DrawBulletRight(int coordX, int coordY, int bullCounter, string bullt)
    {
        CleanOrWriteBullet(coordX + bullCounter, coordY, bullt);
        CleanOrWriteBullet(coordX - 1 + bullCounter, coordY, " ");
    }
    public void ClearLastBulletUp(ref int bullCounter, Coordinates coords, ref bool onWay, ref int skipCounter)
    {
        if (bullCounter == coords.yCoords[2] - 1)
        {
            bullCounter = 0;
            onWay = false;
            skipCounter = 0;
            CleanOrWriteBullet(coords.xCoords[2], YTopBorder, " ");
        }
    }
    public void ClearLastBulletLeft(ref int bullCounter, Coordinates coords, ref bool onWay, ref int skipCounter)
    {
        if (bullCounter == coords.xCoords[0] - 1)
        {
            bullCounter = 0;
            onWay = false;
            skipCounter = 0;
            CleanOrWriteBullet(XLeftBorder, coords.yCoords[0], " ");
        }
    }

    public void ClearLastBulletRight(ref int bullCounter, Coordinates coords, ref bool onWay, ref int skipCounter)
    {
        if (bullCounter + coords.xCoords[1] == bulletRightRange - 1)
        {
            bullCounter = 0;
            onWay = false;
            skipCounter = 0;
            CleanOrWriteBullet(bulletRightRange - 1, coords.yCoords[1], " ");
        }
    }
    public void CleanOrWriteBullet(int coordx, int coordy, string symb)
    {
        Console.SetCursorPosition(coordx, coordy);
        Console.Write(symb);
    }
    public void BulletHitSpider()
    {
        try
        {
            SpiderEnemy firstSpider = SpiderEnemies.Peek();
            int bulletCoord = YBoxRoof - BulletcounterUp + 1;
            int spiderCoord = mSpawn.YUpSpawn + firstSpider.wayCounter;
            if (CheckOnHit(bulletCoord, spiderCoord))
            {
                firstSpider.GetDamaged();
                if (firstSpider.Health <= 0)
                {
                    SpiderDie(firstSpider);
                    SpiderEnemies.Dequeue();
                }

            }
        }
        catch
        {

        }
    }
    public void BulletHitZombie()
    {
        try 
        {
            Zombie firstZomb = Zombies.Peek();
            int bulletCoord = rightBorderBox + BulletcounterUp - 1;
            int zombCoord = mSpawn.XRightSpawn - firstZomb.wayCounter;
            if (firstZomb.CheckOnHit(bulletCoord, zombCoord))
            {
                firstZomb.GetDamaged();
                if (firstZomb.Health <= 0)
                {
                    ZombieDie(firstZomb);
                    Zombies.Dequeue();
                    
                }
            }
        }
        catch
        {

        }
    }
    public void BulletHitHooker()
    {
        try 
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
                }
            }
        }
        catch{

        }
    }
    
    public void InitializeSpider(SpiderEnemy spiderEnemy)
    {
        if (!spiderEnemy.isDead) {
            bool playerUnderHit = player.CheckOnHit(mSpawn.YUpSpawn + spiderEnemy.wayCounter, YBoxRoof);
            bool enemyUnderHit = spiderEnemy.CheckOnHit(YBoxRoof - BulletcounterUp, mSpawn.YUpSpawn + spiderEnemy.wayCounter);
            bool spiderTurn = spiderRate % spiderEnemy.Speed == 0;
            if (spiderEnemy.Health > 0)
            {
                if (playerUnderHit && spiderTurn)
                    player.GetDamaged();
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
                spiderRate++;
            }
            else
            {
                SpiderDie(spiderEnemy);
                killsCounter++;
            } }
    }
    public void InitializeZombie(Zombie zombie)
    {
        if (!zombie.isDead)
        {
            int XzombieCoord = mSpawn.XRightSpawn - zombie.wayCounter - 1;
            bool playerUnderHit = player.CheckOnHit(XzombieCoord, rightBorderBox);
            bool enemyUnderHit = zombie.CheckOnHit(rightBorderBox + BulletcounterRight, XzombieCoord);
            bool enemyUnderHit1 = zombie.CheckOnHit(rightBorderBox + BulletcounterRight, XzombieCoord + 1);
            bool zombieTurn = zombieRate % zombie.Speed == 0;
            if (zombie.Health > 0)
            {
                if (playerUnderHit && zombieTurn)
                {
                    player.GetDamaged();
                }
                if (enemyUnderHit || enemyUnderHit1)
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
                zombieRate++;
            }
            else
            {
                ZombieDie(zombie);
                killsCounter++;
            }
        }
    }
    public void InitializeHooker(Hooker hooker)
    {
        if (hooker.isDead)
        {
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
                hookerRate++;
            }
            else
            {
                HookerDie(hooker);

                killsCounter++;
            }
        }
    }
    public void SpiderDie(SpiderEnemy spider)
    {
        int spiderCoord = mSpawn.YUpSpawn + spider.wayCounter - 1;
        spider.CleanOrWriteSymbol(mSpawn.XUpSpawn, spiderCoord, new String(' ', clearSpiderLngth));
        spider.ClearWeb(mSpawn.XUpSpawn, spiderCoord, new String(' ', clearWebLngth), YBoxRoof);
        YcoordEngaged[spider.EngagedYcoord] = false;
        spider.isDead = true;
        if (startSpawnIntervals[0] > minSpawnIntervals[0])
            startSpawnIntervals[0] -= decSpiderSpwnInterv;
    }

    public void ZombieDie(Zombie zombie)
    {
        int zombieCoord = mSpawn.XRightSpawn - zombie.wayCounter;
        zombie.CleanOrWriteSymbol(zombieCoord, mSpawn.YRightSpawn - 1, new String(' ', clearZombieLngth));
        zombie.CleanOrWriteSymbol(zombieCoord, mSpawn.YRightSpawn, new String(' ', clearZombieLngth));
        zombie.CleanOrWriteSymbol(zombieCoord, mSpawn.YRightSpawn + 1, new String(' ', clearZombieLngth));
        for (int i = 0; i < zombieLngth; i++)
        {
            XcoordEngaged[zombie.EngagedXcoord - i] = false;
        }
        zombie.isDead = true;
        if (startSpawnIntervals[1] > minSpawnIntervals[1])
            startSpawnIntervals[1] -= decZombieSpwnInterv;
    }
    public void HookerDie(Hooker hooker)
    {
        int xhookerCoord = mSpawn.XLeftSpawn + hooker.wayCounter-1;
        int yhookerCoord = YBottomBorder - hookerHight;
        for (int i = 0; i < hookerHight; i++)
        {
            if (i == hookHight)
            {
                hooker.CleanOrWriteSymbol(xhookerCoord, yhookerCoord + i, new String(' ', clearHookerLngth1));
                continue;
            }
            hooker.CleanOrWriteSymbol(xhookerCoord, yhookerCoord + i, new String(' ', clearHookerLngth));
        }
        for (int i = 0; i < hookerLngth; i++)
        {
            XcoordEngaged[hooker.EngagedXcoord + i] = false;
        }
        hooker.isDead = true;
        if (startSpawnIntervals[2] > minSpawnIntervals[2])
            startSpawnIntervals[2] -= decHookerSpwnInterv;
    }

    public void InitializePlayer()
    {
        if (player.Health > 0)
        {
            PlayerBlink();
            player.DrawBox(new Coordinates(
                new int[] { leftBorderBox, rightBorderBox, X1roofHole, X2roofHole },
                new int[] { bottomBorderBox, topBorderBox, Y2WallHole, Y1WallHole }));
            player.DrawCreature();
            AnimateBullet(new Coordinates(
                new int[] { XbulletLeftcoord, XbulletRightcoord, XbulletUpcoord },
                new int[] { YbulletLeftcoord, YbulletRightcoord, YbulletUpcoord }));
        }
        else
            player.isDead = true;
    }
    public void PlayerBlink()
    {
        if (WhiteDelay == blinkRate)
        {
            player.SetColor("White");
            WhiteDelay = 0;
        }
        WhiteDelay++;
    }
    public void InitializeField()
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

    public void DrawBorders()
    {
        for (int i = 0; i < FieldSizeY; i++)
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
