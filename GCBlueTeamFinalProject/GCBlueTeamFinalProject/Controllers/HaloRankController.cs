﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using GCBlueTeamFinalProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace GCBlueTeamFinalProject.Controllers
{
    [Authorize]
    public class HaloRankController : Controller
    {
        private readonly string APIKEYVARIABLE;
        private readonly HaloRankDbContext _context;
        private readonly string FRIENDSLISTID1;
        private readonly string FRIENDSLISTID2;

        public HaloRankController(HaloRankDbContext context, IConfiguration configuration)
        {
            _context = context;
            APIKEYVARIABLE = configuration.GetSection("APIKeys")["APIKeyName"];
            FRIENDSLISTID1 = configuration.GetSection("FriendsListKeys")["List1"];
            FRIENDSLISTID2 = configuration.GetSection("FriendsListKeys")["List2"];
        }
        public IActionResult RegisterUser()
        {
            string id = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            try // tries to find a user for the user logging in, if not found, sends to register user, if found, redirects to profile
            {
                Users newUser = _context.Users.Where(x => x.UserId == id).First();
                return RedirectToAction("MyProfile");
            }
            catch
            {//created a new layout that doesn't have the header buttons for the register user page
                //sending the new layout to register user view 
                return View("RegisterUser", "_RegisterUserLayout");
            }
        }
        public async Task<ActionResult> CreateProfile(Users newUser)
        {
            //creates a profile from the register user page, grabs the gamertag for that profile as well, adds both to database if they're valid
            if (ModelState.IsValid)
            {
                string id = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var client = new HttpClient();
                client.BaseAddress = new Uri($"https://www.haloapi.com/stats/h5/servicerecords/arena");
                client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", $"{APIKEYVARIABLE}");
                var response = await client.GetAsync($"?players={newUser.Gamertag}");
                var searchedPlayer = await response.Content.ReadAsAsync<PlayerRootObject>();
                if (searchedPlayer == null || searchedPlayer.Results[0].Result.PlayerId.Gamertag == null)
                {
                    ViewBag.Message = "This Gamertag does not exist, please try again!";
                    return View("RegisterUser", "_RegisterUserLayout");
                }
                if (searchedPlayer.Results[0].Result.ArenaStats.TotalTimePlayed == "")
                {
                    ViewBag.Message = "This Gamertag does not have any Halo 5 play time!";
                    return View("RegisterUser", "_RegisterUserLayout");
                }
                Gamers searchedGamer = new Gamers(searchedPlayer, 0);
                searchedGamer.UserId = id;
                newUser.UserId = id;
                newUser.Gamertag = searchedGamer.Gamertag;
                _context.Gamers.Add(searchedGamer);
                _context.Users.Add(newUser);
                _context.SaveChanges();
                return RedirectToAction("MyProfile");
            }
            return RedirectToAction("RegisterUser");
        }
        public IActionResult MyProfile() //displays your profile. finds both your user and gamer and combines them to send to the view
        {
            string id = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            Users foundUser = _context.Users.Where(x => x.UserId == id).First();
            Gamers foundGamer = _context.Gamers.Where(x => x.UserId == id && x.Gamertag == foundUser.Gamertag).First();
            UsersGamers UserProfile = new UsersGamers(foundUser, foundGamer);
            return View(UserProfile);

        }
        public async Task<ActionResult> GetPlayerBySearch(string search) //searches for player, checks if it's valid and returns view with gamer
        {
            if (search == null)
            {
                string id = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                ViewBag.Gamertag = _context.Users.Where(x => x.UserId == id).First().Gamertag;
                List<Gamers> userGamers = GetGamerList();
                ViewBag.Message = "This gamertag does not exist";
                return View("DisplayGamers", userGamers);
            }
            var client = new HttpClient();
            client.BaseAddress = new Uri($"https://www.haloapi.com/stats/h5/servicerecords/arena");
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", $"{APIKEYVARIABLE}");
            var response = await client.GetAsync($"?players={search}");
            var searchedPlayer = await response.Content.ReadAsAsync<PlayerRootObject>();
            ViewBag.UserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            Gamers searchedGamer = new Gamers(searchedPlayer, 0);
            if (searchedGamer.Gamertag == null)
            {
                string id = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                Users foundUser = _context.Users.Where(x => x.UserId == id).First();
                Gamers foundGamer = _context.Gamers.Where(x => x.UserId == id && x.Gamertag == foundUser.Gamertag).First();
                UsersGamers UserProfile = new UsersGamers(foundUser, foundGamer);
                ViewBag.Message = "This Gamertag does not exist, please try again!";
                return View("MyProfile", UserProfile);
            }
            else if(searchedGamer.TotalTimePlayed == "")
            {
                string id = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                Users foundUser = _context.Users.Where(x => x.UserId == id).First();
                Gamers foundGamer = _context.Gamers.Where(x => x.UserId == id && x.Gamertag == foundUser.Gamertag).First();
                UsersGamers UserProfile = new UsersGamers(foundUser, foundGamer);
                ViewBag.Message = "This Gamertag does not have any Halo 5 play time!";
                return View("MyProfile", UserProfile);
            }  
            return View(searchedGamer);
        }
        public IActionResult AddToGamers(Gamers newPlayer)
        {
            if (ModelState.IsValid)
            {
                string id = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                List<Gamers> gamerList = _context.Gamers.Where(x => x.UserId == id).ToList();
                for (int i = 0; i < gamerList.Count; i++)
                {
                    if (gamerList[i].Gamertag == newPlayer.Gamertag)
                    {
                        return RedirectToAction("DisplayGamers");
                    }
                }
                _context.Gamers.Add(newPlayer);
                _context.SaveChanges();
            }
            return RedirectToAction("DisplayGamers");
        }
        public IActionResult DeleteFromGamers(int id)
        {
            Gamers found = _context.Gamers.Find(id);
            if (found != null)
            {
                string id2 = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                Users foundUser = _context.Users.Where(x => x.UserId == id2).First();
                if (found.Gamertag.ToLower() == foundUser.Gamertag.ToLower())
                {
                    List<Gamers> userGamers = _context.Gamers.Where(x => x.UserId == id2).ToList();
                    ViewBag.Gamertag = foundUser.Gamertag;
                    return View("DisplayGamers", userGamers);
                }
                else
                {
                    _context.Gamers.Remove(found);
                    _context.SaveChanges();
                }   
            }
            return RedirectToAction("DisplayGamers");
        }
        public IActionResult CreateTeams(List<string> gamers, string submit, string teamName)
        {
            string id = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            ViewBag.Gamertag = _context.Users.Where(x => x.UserId == id).First().Gamertag;
            List<Gamers> userGamers = GetGamerList();
            if (submit == "Team Matchup")
            {
                if (gamers.Count < 4 || gamers.Count > 16)
                {
                    ViewBag.Message = "Must select between 4 and 16 players for your teams";
                    return View("DisplayGamers", userGamers);
                }
                List<Gamers> newGamerList = _context.Gamers.Where(x => x.UserId == id && gamers.Contains(x.Gamertag)).ToList();
                List<Teams> teams = Teams.TeamMaker(newGamerList);
                List<int> teamProbabilities = Teams.CalculateProbabilty((double)teams[0].AvgScore, (double)teams[1].AvgScore);
                ViewBag.Team1Probability = teamProbabilities[0];
                ViewBag.Team2Probability = teamProbabilities[1];
                return View(teams); //Sending a List<Teams> 
            }
            else if (submit == "Add As Favorite Team")
            {
                if (gamers.Count < 2 || gamers.Count > 8 || gamers.Count % 2 != 0)
                {
                    ViewBag.Message = "Teams must be even and between 2 to 8 players.";
                    return View("DisplayGamers", userGamers);
                }
                if (teamName == null)
                {
                    ViewBag.Message = "Enter a name for your new favorite team";
                    return View("DisplayGamers", userGamers);
                }
                
                List<Gamers> newGamerList = _context.Gamers.Where(x => x.UserId == id && gamers.Contains(x.Gamertag)).ToList();
                Teams newTeam = new Teams(newGamerList);
                newTeam.TeamName = teamName;
                newTeam.UserId = id;
                _context.Teams.Add(newTeam);
                _context.SaveChanges();
                return RedirectToAction("DisplayFavoriteTeams");
            }
            else
            {
                if (gamers.Count < 2)
                {
                    ViewBag.Message = "Select at least 2 people to compare";
                    return View("DisplayGamers", userGamers);
                }
                List<Gamers> newGamerList = _context.Gamers.Where(x => x.UserId == id && gamers.Contains(x.Gamertag)).ToList();
                return View("CompareGamers", newGamerList);
            }
        }
        public IActionResult AddFavoriteTeams(List<string> favTeam, string teamName)
        {
            string id = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            List<Gamers> newGamerList = _context.Gamers.Where(x => x.UserId == id && favTeam.Contains(x.Gamertag)).ToList();
            Teams newFavTeam = new Teams(newGamerList);
            newFavTeam.UserId = id;
            newFavTeam.TeamName = teamName;
            //pulling a list of teams from the DB then checking if the team name already exists, if so then return to
            //previous view which was create teams
            List<Teams> already = _context.Teams.Where(x => x.UserId == id).ToList();
            for (int i = 0; i < already.Count; i++)
            {
                if (already[i].TeamName == newFavTeam.TeamName)
                {
                    return RedirectToAction("DisplayFavoriteTeams");
                }
            }
            if (ModelState.IsValid)
            {
                _context.Teams.Add(newFavTeam);
                _context.SaveChanges();
            }
            return RedirectToAction("DisplayFavoriteTeams");
        }
        //this method removes a team from the favorite teams list
        public IActionResult DeleteFavoriteTeams(int id)
        {
            Teams found = _context.Teams.Find(id);
            if (found != null)
            {
                _context.Teams.Remove(found);
                _context.SaveChanges();
            }
            return RedirectToAction("DisplayFavoriteTeams");
        }
        public IActionResult DisplayFavoriteTeams()
        {
            string id = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            List<Teams> favTeams = _context.Teams.Where(x => x.UserId == id).ToList();
            int teamMembers = 0;
            int totalMembers = 0;
            foreach (Teams team in favTeams)
            {
                if (team.Player2 != null && team.Player3 == null)
                {
                    teamMembers = 2;
                }
                if (team.Player4 != null && team.Player5 == null)
                {
                    teamMembers = 4;
                }
                if(team.Player6 != null && team.Player7 == null)
                {
                    teamMembers = 6;
                }
                if(team.Player8 != null)
                {
                    teamMembers = 8;
                }
                if (teamMembers > totalMembers)
                {
                    totalMembers = teamMembers;
                }              
            }

            ViewBag.teamMembers = totalMembers;
            if(favTeams.Count == 0)
            {
                List<Gamers> GamerList = GetGamerList();
                ViewBag.Gamertag = _context.Users.Where(x => x.UserId == id).First().Gamertag;
                ViewBag.Message = "You do not have any favorite teams saved";
                return View("DisplayGamers", GamerList);
            }
            return View(favTeams);
        }
        public IActionResult DisplayGamers()
        {
            string id = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            List<Gamers> GamerList = GetGamerList();
            ViewBag.Gamertag = _context.Users.Where(x => x.UserId == id).First().Gamertag;
            return View(GamerList);
        }

        public async Task<ActionResult> UpdateGamers()
        {
            string id = User.FindFirst(ClaimTypes.NameIdentifier).Value; //gets UserID from ASP login
            List<Gamers> gamerList = _context.Gamers.Where(x => x.UserId == id).ToList(); //finds list of gamers associated with that UserID
            string search = "";
            for (int i = 0; i < gamerList.Count; i++) //builds a gamertag string based on friends list gamertags for API search
            {
                if (i > 0)
                {
                    search = search + ",";
                }
                search = search + gamerList[i].Gamertag;
            }
            var client = new HttpClient();
            client.BaseAddress = new Uri($"https://www.haloapi.com/stats/h5/servicerecords/arena");
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", $"{APIKEYVARIABLE}");
            var response = await client.GetAsync($"?players={search}"); //searches for players in the API based on the search
            var searchedPlayer = await response.Content.ReadAsAsync<PlayerRootObject>(); //comes in as a PlayerRootObject
            for (int i = 0; i < searchedPlayer.Results.Length; i++) //adds each Gamers from the PlayerRootObject to a List<Gamers>
            {
                var newData = searchedPlayer.Results[i];
                //search gamerList for the appropriate matching Gamers class for this result (e.g. pull old information class) and set to oldData
                var oldData = gamerList.Where(oldDbGamer => oldDbGamer.Gamertag == newData.Result.PlayerId.Gamertag).Single();

                oldData.UpdateWith(searchedPlayer, i);

                //1. pull object from the database
                //2. update information
                //3. savechanges
            }
            _context.SaveChanges();
            return RedirectToAction("DisplayGamers"); //displays sorted list of gamers
        }
        public IActionResult GenerateFriends() 
        {
            string id = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            string id2 = $"{FRIENDSLISTID1}";
            string id3 = $"{FRIENDSLISTID2}";
            List<Gamers> gamerList = _context.Gamers.Where(x => x.UserId == id2 || x.UserId == id3).ToList();
            List<Gamers> userGamerList = _context.Gamers.Where(x => x.UserId == id).ToList();
            List<string> gamertags = new List<string>();
            foreach (Gamers gamers in userGamerList)
            {
                gamertags.Add(gamers.Gamertag);
            }
            for (int i = 0; i < gamerList.Count; i++)
            {
                Gamers newgamer = new Gamers();
                newgamer.CopyGamer(gamerList[i]);
                newgamer.UserId = id;

                if (gamertags.Contains(gamerList[i].Gamertag) == false) // if our old friendslist doesn't contain this new gametag 
                {
                    _context.Gamers.Add(newgamer);
                    _context.SaveChanges();
                }
            }
            return RedirectToAction("DisplayGamers");
        }
        public List<Gamers> GetGamerList()
        {
            string id = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            List<Gamers> GamerList = _context.Gamers.Where(x => x.UserId == id).OrderByDescending(x => x.Score).ToList();
            for (int i = 0; i < GamerList.Count; i++)
            {
                GamerList[i].Ranking = i + 1;
            }
            return GamerList;
        }
    }
}