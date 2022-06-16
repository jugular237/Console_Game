using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

class ResultsMenu : BasicStats
{
   

    public string Path { get; } = @"Records.txt";

    public bool CheckOnKillRecord(int killsResult)
    {
        return killsResult > ReadRecords().Item1; 
    }

    public bool CheckOnTimeRecord(long timeResult)
    {
        return timeResult > ReadRecords().Item2;
    }

    public (int, long) ReadRecords()
    {
        string[] twoRecords = File.ReadAllText(Path).Split();
        int KillsRecord = int.Parse(twoRecords[0]);
        int SecondsRecord = int.Parse(twoRecords[1]);
        return (KillsRecord, SecondsRecord);
    }

    public void WriteNewRecords(int killsRes, long timeRes)
    {
        int killsToWrite = 0;
        long timeToWrite = 0;
        killsToWrite = CheckOnKillRecord(killsRes) ?  killsRes : ReadRecords().Item1;
        timeToWrite = CheckOnTimeRecord(timeRes) ? timeRes : ReadRecords().Item2;
        SetColor("Yellow");
        Console.WriteLine($"Your result :\n kills : {killsRes}\n time : {timeRes} ");
        if(killsRes > ReadRecords().Item1)
            Console.WriteLine("NEW Kills RECORD!");
        if(timeToWrite > ReadRecords().Item2)
            Console.WriteLine("NEW Time RECORD!");
        File.WriteAllText(Path, $"{killsToWrite} {timeToWrite}");
        Console.WriteLine($"\n kills record : {killsToWrite}\n time record : {timeToWrite}" +
            $"\nPress space to restart...\nPress any other key to exit...");
    }
}

