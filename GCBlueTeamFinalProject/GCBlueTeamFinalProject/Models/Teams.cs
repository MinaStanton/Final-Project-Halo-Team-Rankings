using System;
using System.Collections.Generic;
using System.Linq;

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
        public Teams() { }
        public Teams(List<Gamers> gamers)
        {
            Player1 = gamers[0].Gamertag;
            Player2 = gamers[1].Gamertag;
            Player3 = gamers[2].Gamertag;
            Player4 = gamers[3].Gamertag;
            Player5 = gamers[4].Gamertag;
            Player6 = gamers[5].Gamertag;
            Player7 = gamers[6].Gamertag;
            Player8 = gamers[7].Gamertag;
            AvgScore = SetAvgScore(gamers);
            AvgKdratio = SetAvgKDRatio(gamers);
            AvgKdaratio = SetAvgKDARatio(gamers);
            AvgAccRatio = SetAvgAccRatio(gamers);
            AvgWlratio = SetAvgWLRatio(gamers);
        }
        public static List<Teams> TeamMaker(List<Gamers> gamers)
        {
            List<Gamers> SortedList = gamers.OrderBy(x => x.Score).Reverse().ToList();

            List<Gamers> team1 = new List<Gamers>();
            List<Gamers> team2 = new List<Gamers>();

            if (gamers.Count == 4)
            {
                team1.Add(gamers[0]);
                team2.Add(gamers[1]);
                team2.Add(gamers[2]);
                team1.Add(gamers[3]);
            }
            else if (gamers.Count == 6)
            {
                team1.Add(gamers[0]);
                team2.Add(gamers[1]);
                team2.Add(gamers[2]);
                team1.Add(gamers[3]);
                team1.Add(gamers[4]);
                team2.Add(gamers[5]);
            }
            else if (gamers.Count == 8)
            {
                team1.Add(gamers[0]);
                team2.Add(gamers[1]);
                team2.Add(gamers[2]);
                team1.Add(gamers[3]);
                team1.Add(gamers[4]);
                team2.Add(gamers[5]);
                team2.Add(gamers[6]);
                team1.Add(gamers[7]);
            }

            Teams teamsTeam1 = new Teams(team1);
            Teams teamsTeam2 = new Teams(team2);

            return new List<Teams>() { teamsTeam1, teamsTeam2 };
        }
        public int SetAvgScore(List<Gamers> team)
        {
            int sum = 0;
            foreach (Gamers gamer in team)
            {
                sum = sum + (int)gamer.Score;
            }
            return sum / team.Count;
        }
        public double SetAvgKDRatio(List<Gamers> team)
        {
            double sum = 0;
            foreach (Gamers gamer in team)
            {
                sum = sum + (double)gamer.Kdratio;
            }
            return sum / team.Count;
        }
        public double SetAvgKDARatio(List<Gamers> team)
        {
            double sum = 0;
            foreach (Gamers gamer in team)
            {
                sum = sum + (double)gamer.Kdaratio;
            }
            return sum / team.Count;
        }
        public double SetAvgAccRatio(List<Gamers> team)
        {
            double sum = 0;
            foreach (Gamers gamer in team)
            {
                sum = sum + (double)gamer.AccuracyRatio;
            }
            return sum / team.Count;
        }
        public double SetAvgWLRatio(List<Gamers> team)
        {
            double sum = 0;
            foreach (Gamers gamer in team)
            {
                sum = sum + (double)gamer.WinLossRatio;
            }
            return sum / team.Count;
        }
        public static string DisplayPercent(double number) //for displaying percentages as a % instead of a double (used in a view, NOT used in the contructor) using for both accuracy and winlossratio
        {
            return $"{number * 100}%";
        }
    }
}
