using System;
using System.Collections.Generic;

namespace GCBlueTeamFinalProject.Models
{
    public partial class Gamers
    {
        public int Id { get; set; }
        public string Gamertag { get; set; }
        public string UserId { get; set; }
        public int? TotalKills { get; set; }
        public int? TotalDeaths { get; set; }
        public int? TimeSpentRespawning { get; set; }
        public double? Kdratio { get; set; }
        public int? TotalAssists { get; set; }
        public double? Kdaratio { get; set; }
        public int? TotalAssassinations { get; set; }
        public int? TotalHeadshots { get; set; }
        public int? TotalShotsFired { get; set; }
        public int? TotalShotsLanded { get; set; }
        public double? AccuracyRatio { get; set; }
        public int? TotalGamesWon { get; set; }
        public int? TotalGamesLost { get; set; }
        public double? WinLossRatio { get; set; }
        public int? TotalGamesTied { get; set; }
        public int? TotalGamesCompleted { get; set; }
        public string TotalTimePlayed { get; set; }
        public int? Score { get; set; }
        public int? Ranking { get; set; }
        public string Images { get; set; }
        public string Notes { get; set; }
        public int? GameTypeIntId { get; set; }
        public string GameTypeNvarCharId { get; set; }
        //public string DisplayAcc { get; set; }
        public virtual AspNetUsers User { get; set; }
        public Gamers() //added default construct
        {

        }
        public Gamers(PlayerRootObject player) //overloaded constructor for converting PlayerRootObject to Gamers object
        {
            Gamertag = player.Results[0].Result.PlayerId.Gamertag;
            TotalKills = player.Results[0].Result.ArenaStats.TotalKills;
            TotalDeaths = player.Results[0].Result.ArenaStats.TotalDeaths;
            TimeSpentRespawning = player.Results[0].Result.ArenaStats.TotalDeaths * 5; //Seconds dead = deaths * 5
            Kdratio = CalculateKillDeathRatio(player.Results[0].Result.ArenaStats.TotalKills, //KD = Kills / Deaths
                player.Results[0].Result.ArenaStats.TotalDeaths);
            TotalAssists = player.Results[0].Result.ArenaStats.TotalAssists;
            Kdaratio = CalculateKillDeathAssistRatio(player.Results[0].Result.ArenaStats.TotalKills, //KDA = (Kills + (Assists / 2)) / Deaths
                player.Results[0].Result.ArenaStats.TotalDeaths, 
                player.Results[0].Result.ArenaStats.TotalAssists);
            TotalAssassinations = player.Results[0].Result.ArenaStats.TotalAssassinations;
            TotalHeadshots = player.Results[0].Result.ArenaStats.TotalHeadshots;
            TotalShotsFired = player.Results[0].Result.ArenaStats.TotalShotsFired;
            TotalShotsLanded = player.Results[0].Result.ArenaStats.TotalShotsLanded;
            AccuracyRatio = CalculateAccuracy(player.Results[0].Result.ArenaStats.TotalShotsFired, //Acc = Landed / Fired
                player.Results[0].Result.ArenaStats.TotalShotsLanded);
            //DisplayAcc = DisplayPercent(CalculateAccuracy(player.Results[0].Result.ArenaStats.TotalShotsFired, //Acc = Landed / Fired
                //player.Results[0].Result.ArenaStats.TotalShotsLanded));
            TotalGamesWon = player.Results[0].Result.ArenaStats.TotalGamesWon;
            TotalGamesLost = player.Results[0].Result.ArenaStats.TotalGamesLost;
            TotalGamesTied = player.Results[0].Result.ArenaStats.TotalGamesTied;
            WinLossRatio = CalculateWinLossRatio(player.Results[0].Result.ArenaStats.TotalGamesWon, //WL = Wins / (Losses + Tied)
                player.Results[0].Result.ArenaStats.TotalGamesLost,
                player.Results[0].Result.ArenaStats.TotalGamesTied);
            TotalGamesCompleted = player.Results[0].Result.ArenaStats.TotalGamesCompleted;
            TotalTimePlayed = player.Results[0].Result.ArenaStats.TotalTimePlayed; //add parsing method, regex? (STRETCH GOAL)
            GameTypeNvarCharId = player.Results[0].Result.ArenaStats.ArenaGameBaseVariantStats[2].GameBaseVariantId; //add parsing method (STRETCH GOAL)
        }
        public static string DisplayRespawnTime(int seconds) //for displaying time spent respawning in view (NOT used in the contructor)
        {
            int minutes = seconds / 60;
            int remainderSeconds = seconds % 60;
            int hours = minutes / 60;
            int remainderMinutes = minutes % 60;
            return $"{hours}:{remainderMinutes}:{remainderSeconds}";
        }
        public double CalculateKillDeathRatio(int kills, int deaths) //for calculating K/D Ratio (used in the constructor when initializing a new Gamer object)
        {
            double KD = (double)kills / (double)deaths;
            KD = Math.Round(KD, 2);
            return KD;
        }
        public double CalculateKillDeathAssistRatio(int kills, int deaths, int assists) //for calculating KDA Ratio (used in the constructor when initializing a new Gamer object)
        {
            int calculatedAssists = assists / 2;
            int KA = kills + calculatedAssists;
            double KDA = (double)KA / (double)deaths;
            KDA = Math.Round(KDA, 2);
            return KDA;
        }
        public double CalculateAccuracy(int fired, int landed) //for calculating Accuracy as a double (used in the constructor when initializing a new Gamer object)
        {
            double accuracy = (double)landed / (double)fired;
            accuracy = Math.Round(accuracy, 2);
            return accuracy;
        }
        public static string DisplayPercent(double number) //for displaying percentages as a % instead of a double (used in a view, NOT used in the contructor) using for both accuracy and winlossratio
        {
            return $"{number * 100}%";
        }
        public double CalculateWinLossRatio(int wins, int losses, int ties) //for calculating W/L Ratio (used in the constructor when initializing a new Gamer object)
        {
            double WLRatio = (double)wins / (double)(losses + ties);
            WLRatio = Math.Round(WLRatio, 2);
            return WLRatio;
        }
    }
}
