using System;
using System.Collections.Generic;

namespace GCBlueTeamFinalProject.Models
{
    public partial class Teams
    {
        public int Id { get; set; }
        public string TeamName { get; set; }
        public string Player1 { get; set; }
        public string Player2 { get; set; }
        public string Player3 { get; set; }
        public string Player4 { get; set; }
        public string Player5 { get; set; }
        public string Player6 { get; set; }
        public string Player7 { get; set; }
        public string Player8 { get; set; }
        public double? AvgWlratio { get; set; }
        public double? AvgKdratio { get; set; }
        public double? AvgKdaratio { get; set; }
        public double? AvgAccRatio { get; set; }
        public double? AvgScore { get; set; }
        public string Images { get; set; }
        public string Notes { get; set; }
        public string UserId { get; set; }

        public virtual AspNetUsers User { get; set; }

        public Teams()
        {

        }

        public Teams(string gamer1, string gamer2)
        {
            Player1 = gamer1;
            Player2 = gamer2;
        }
    }
}
