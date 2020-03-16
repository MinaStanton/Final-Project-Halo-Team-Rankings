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

        public virtual AspNetUsers User { get; set; }
    }
}
