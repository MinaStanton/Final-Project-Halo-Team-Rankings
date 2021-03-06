﻿using System;
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
        public Teams()
        {

        }
        //creating a constructor for testing
        //public Teams(string gtag1, string gtag2, double wRatio, double kdRatio)
        //{
        //    Player1 = gtag1;
        //    Player2 = gtag2;
        //    AvgWlratio = wRatio;
        //    AvgKdratio = kdRatio;
        //}

        public Teams(List<Gamers> gamers)
        {
            //List<string> players = new List<string>()
            //{ 
            //    Player1,
            //    Player2,
            //    Player3,
            //    Player4,
            //    Player5,
            //    Player6,
            //    Player7,
            //    Player8
            //};
            //for (int i = 0; i < gamers.Count; i++)
            //{
            //    players[i] = gamers[i].Gamertag;
            //}

            if (gamers.Count == 2)
            {
                Player1 = gamers[0].Gamertag;
                Player2 = gamers[1].Gamertag;
            }
            else if (gamers.Count == 3)
            {
                Player1 = gamers[0].Gamertag;
                Player2 = gamers[1].Gamertag;
                Player3 = gamers[2].Gamertag;
            }
            else if (gamers.Count == 4)
            {
                Player1 = gamers[0].Gamertag;
                Player2 = gamers[1].Gamertag;
                Player3 = gamers[2].Gamertag;
                Player4 = gamers[3].Gamertag;
            }
            else if (gamers.Count == 5)
            {
                Player1 = gamers[0].Gamertag;
                Player2 = gamers[1].Gamertag;
                Player3 = gamers[2].Gamertag;
                Player4 = gamers[3].Gamertag;
                Player5 = gamers[4].Gamertag;
            }
            else if (gamers.Count == 6)
            {
                Player1 = gamers[0].Gamertag;
                Player2 = gamers[1].Gamertag;
                Player3 = gamers[2].Gamertag;
                Player4 = gamers[3].Gamertag;
                Player5 = gamers[4].Gamertag;
                Player6 = gamers[5].Gamertag;
            }
            else if (gamers.Count == 7)
            {
                Player1 = gamers[0].Gamertag;
                Player2 = gamers[1].Gamertag;
                Player3 = gamers[2].Gamertag;
                Player4 = gamers[3].Gamertag;
                Player5 = gamers[4].Gamertag;
                Player6 = gamers[5].Gamertag;
                Player7 = gamers[6].Gamertag;
            }
            else if (gamers.Count == 8)
            {
                Player1 = gamers[0].Gamertag;
                Player2 = gamers[1].Gamertag;
                Player3 = gamers[2].Gamertag;
                Player4 = gamers[3].Gamertag;
                Player5 = gamers[4].Gamertag;
                Player6 = gamers[5].Gamertag;
                Player7 = gamers[6].Gamertag;
                Player8 = gamers[7].Gamertag;
            }
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

            team1.Add(gamers[0]);
            var currentTeam = team2;
            var i = 1;
            while(i < gamers.Count)
            {
                currentTeam.Add(gamers[i]);
                i++;
                if(i < gamers.Count)
                {
                    currentTeam.Add(gamers[i]);
                    i++;
                }
                //currentTeam = (currentTeam == team1 ? team2 : team1); //same as below
                if(currentTeam == team1)
                {
                    currentTeam = team2;
                }
                else
                {
                    currentTeam = team1;
                }
            }

            Teams teamsTeam1 = new Teams(team1);
            Teams teamsTeam2 = new Teams(team2);

            return new List<Teams>() { teamsTeam1, teamsTeam2 };
        }
        public double SetAvgScore(List<Gamers> team)
        {
            double sum = 0;
            foreach (Gamers gamer in team)
            {
                sum = sum + (double)gamer.Score;
            }
            double avgSum = sum / team.Count;
            avgSum = Math.Round(avgSum);
            return avgSum;
        }
        public double SetAvgKDRatio(List<Gamers> team)
        {
            double sum = 0;
            foreach (Gamers gamer in team)
            {
                sum = sum + (double)gamer.Kdratio;
            }
            double avgSum = sum / (double)team.Count;
            avgSum = Math.Round(avgSum, 2);
            return avgSum;
        }
        public double SetAvgKDARatio(List<Gamers> team)
        {
            double sum = 0;
            foreach (Gamers gamer in team)
            {
                sum = sum + (double)gamer.Kdaratio;
            }
            double avgSum = sum / (double)team.Count;
            avgSum = Math.Round(avgSum, 2);
            return avgSum;
        }
        public double SetAvgAccRatio(List<Gamers> team)
        {
            double sum = 0;
            foreach (Gamers gamer in team)
            {
                sum = sum + (double)gamer.AccuracyRatio;
            }
            double avgSum = sum / (double)team.Count;
            int avgSumInt = (int)Math.Round(avgSum);
            return avgSumInt;
        }
        public double SetAvgWLRatio(List<Gamers> team)
        {
            double sum = 0;
            foreach (Gamers gamer in team)
            {
                sum = sum + (double)gamer.WinLossRatio;
            }
            double avgSum = sum / (double)team.Count;
            int avgSumInt = (int)Math.Round(avgSum);
            return avgSumInt;
        }

        public static List<int> CalculateProbabilty(double scoreofTeam1, double scoreOfTeam2)
        {

            double team1Probability = scoreofTeam1 / (scoreofTeam1 + scoreOfTeam2) * 100;

            double team2Probability = scoreOfTeam2 / (scoreofTeam1 + scoreOfTeam2) * 100;

            int team1ResultRounded = (int)Math.Round(team1Probability);
            int team2ResultRounded = (int)Math.Round(team2Probability);

            return new List<int>() { team1ResultRounded, team2ResultRounded };
        }
    }
}
