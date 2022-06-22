using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using static Constants;


public class Hooker : BasicStats, IHitable
{
    public int wayCounter = 0;
    public int Speed { get; set; } = 40;
    public int Health { get; set; } = 10;

    public MonstersSpawns monstrSp;

    ConfigClass conf = JsonConvert.DeserializeObject<ConfigClass>(File.ReadAllText(@"jsconfig.json"));

    public string[] hookerLines = new string[]
    {
      @"   _    _",
      @"  /╰-  -╯\",
      @"   \ ++ /",
      @" //|.  .|\\",
      @"//|      |\____)",
      @"* |      | ",
      @"  --------",
      @"  |  /\  |",
      @"   ||  ||",
      @"   |_   |__"
    };

    public int EngagedXcoord = 0;
    private int wayLength = 21;

    public void AnimateEnemy()
    {
        if (wayCounter < wayLength - 1)
        {
            int xCoordHooker = monstrSp.XLeftSpawn + wayCounter - 1;
            DrawEnemy(xCoordHooker + 1, conf.YBottomBorder, hookerLines);
            ClearSpace(xCoordHooker, conf.YBottomBorder - hookerHight);
            EngagedXcoord = monstrSp.XLeftSpawn + wayCounter;
            if (wayCounter != wayLength - 1)
                wayCounter++;
        }
    }
    
    private void ClearSpace(int xCoord, int yCoord)
    {
        string space = " ";
        for(int i = 0; i < hookerHight; i++)
        {
            CleanOrWriteSymbol(xCoord, yCoord+i, space);
        }
    }
    
    public async void GetDamaged()
    {
        int xCoordZombie = monstrSp.XLeftSpawn + wayCounter - 1;
        SetColor("Red");
        DrawEnemy(xCoordZombie, conf.YBottomBorder, hookerLines);
        await Task.Run(() => Health--);
        SetColor("White");
    }
    
    public void DrawEnemy(int coordX, int coordY, string[] hookerLines)
    {
        for(int i = 0; i < hookerHight; i++)
            CleanOrWriteSymbol(coordX, coordY - hookerHight + i, hookerLines[i]);
    }

}
