using System;
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

        public HaloRankController(HaloRankDbContext context, IConfiguration configuration)
        {
            _context = context;
            APIKEYVARIABLE = configuration.GetSection("APIKeys")["APIKeyName"];
        }
        
        public IActionResult RegisterUser()
        {
            string id = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            List<Users> UserList = _context.Users.Where(x => x.UserId == id).ToList();
            for(int i = 0; i < UserList.Count; i++)
            {
                if (UserList[i].Gamertag != null)
                {
                    return RedirectToAction("YourProfile", UserList[i]);
                }
            }
            return View();
        }

        
        //public IActionResult YourProfile(Users newUser)
        //{

        //    if (ModelState.IsValid)
        //    {
        //        newUser.UserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
        //        _context.Users.Add(newUser);
        //        _context.SaveChanges();
        //    }
        //    else
        //    {
        //        //more validation
        //        return RedirectToAction("RegisterUser");
        //    }
        //    return View(newUser);
            
        //}

        public async Task<ActionResult> YourProfile(Users newUser)
        {
            // Calling on the API to check if Gamertag is valid
            var client = new HttpClient();
            client.BaseAddress = new Uri($"https://www.haloapi.com/stats/h5/servicerecords/arena");
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", $"{APIKEYVARIABLE}");
            var response = await client.GetAsync($"?players={newUser.Gamertag}");
            var searchedPlayer = await response.Content.ReadAsAsync<PlayerRootObject>();
            if (searchedPlayer == null)
            {
                return View("Error", "This Gamertag does not exist, please try again.");
            }
            // Taking users into a list and assigning ID in the DB
            string id = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            List<Users> userList = _context.Users.Where(x => x.UserId == id).ToList();
            //looking at searched gamers to see if the Gamertag already exists in the database
            Gamers searchedGamer = new Gamers(searchedPlayer, 0);
            for (int i =0; i<userList.Count; i++)
            {
                if(userList[i].UserId != null)
                {
                    UsersGamers MyProfile2 = new UsersGamers(userList[i], searchedGamer);
                    return View(MyProfile2);
                }
            }
           // then if it doesnt you are brough to the  add new user functions that will 
           //redirect you to your profile if you havent made one
            if (ModelState.IsValid)
            {
                if(newUser.UserId == null)
                {
                    newUser.UserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                   _context.Users.Add(newUser);
                }
                searchedGamer.UserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                _context.Gamers.Add(searchedGamer);
                _context.SaveChanges();
            }
            else 
            {
                //more validation
                return RedirectToAction("RegisterUser");
            }
            UsersGamers MyProfile = new UsersGamers(newUser, searchedGamer);
            return View(MyProfile);

        }
        public IActionResult Error(string message)
        {
            return View(message);
        }

        public async Task<ActionResult> GetPlayerBySearch(string search)
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri($"https://www.haloapi.com/stats/h5/servicerecords/arena");
            //client.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (compatible; GrandCircus/1.0)");
            //client.DefaultRequestHeaders.Add("x-rapidapi-host", "brianiswu-open-brewery-db-v1.p.rapidapi.com");
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", $"{APIKEYVARIABLE}");
            var response = await client.GetAsync($"?players={search}");
            //ADD NUGET PACKAGE - Microsoft.aspnet.webapi.client
            var searchedPlayer = await response.Content.ReadAsAsync<PlayerRootObject>();
            ViewBag.UserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            Gamers searchedGamer = new Gamers(searchedPlayer, 0);
            
            if(searchedGamer.Gamertag == null)
            {
                
                return View("Error", "This Gamertag does not exist, please try again.");
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
        //public IActionResult DisplayGamers()
        //{
        //    string id = User.FindFirst(ClaimTypes.NameIdentifier).Value;
        //    List<Gamers> gamerList = _context.Gamers.Where(x => x.UserId == id).ToList();
        //    List<Gamers> sortedList = gamerList.OrderBy(x => x.Score).Reverse().ToList();
        //    for (int i = 0; i < sortedList.Count; i++)
        //    {
        //        sortedList[i].Ranking = i + 1;
        //    }
        //    //List<Gamers> gamerList = _context.Gamers.ToList();
        //    //This line above for quickly showing all gamers in database instead of just associated with UserID
        //    return View(sortedList);
        //}
        public async Task<ActionResult> DisplayGamers()
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
            List<Gamers> apiListSearch = new List<Gamers>();
            for (int i = 0; i < searchedPlayer.Results.Length; i++) //adds each Gamers from the PlayerRootObject to a List<Gamers>
            {
                apiListSearch.Add(new Gamers(searchedPlayer, i));
            }

            List<Gamers> sortedList = apiListSearch.OrderByDescending(x => x.Score).ToList(); //sorts list of gamers by score
            foreach(Gamers gamer in gamerList)
            {
                _context.Gamers.Remove(gamer);
                _context.SaveChanges();
            }

            for (int i = 0; i < sortedList.Count; i++) //assigns a ranking based on order of list (based on score)
            {
                sortedList[i].Ranking = i + 1;
                sortedList[i].UserId = id;
                _context.Gamers.Add(sortedList[i]);
                _context.SaveChanges();
            }

            List<Gamers> newGamerList = _context.Gamers.Where(x => x.UserId == id).ToList();
            //List<Gamers> gamerList = _context.Gamers.ToList();
            //This line above for quickly showing all gamers in database instead of just associated with UserID
            return View(newGamerList); //displays sorted list of gamers
        }


        public IActionResult DeleteFromGamers(int id)
        {
            Gamers found = _context.Gamers.Find(id);
            if (found != null)
            {
                _context.Gamers.Remove(found);
                _context.SaveChanges();
            }
            return RedirectToAction("DisplayGamers");
        }
        public IActionResult CreateTeams(List<string> gamers, string submit, string teamName)
        {
            if(submit == "Generate Teams")
            {
                if (gamers.Count < 4 || gamers.Count > 16)
                {
                    return View("Error", "Must select between 4 and 16 players for your teams");
                }
                string id = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                List<Gamers> newGamerList = _context.Gamers.Where(x => x.UserId == id && gamers.Contains(x.Gamertag)).ToList();
                ViewBag.UserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                ViewBag.UserId2 = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                List<Teams> teams = Teams.TeamMaker(newGamerList);
                return View(teams); //Sending a List<Teams> //may need to validate number of gamers here
            }
            else
            {
                if (gamers.Count < 2 || gamers.Count > 8)
                {
                    return View("Error", "Must select between 2 and 8 players for your teams");
                }
                if(teamName == null)
                {
                    return View("Error", "Enter a name for your new favorite team");
                }
                string id = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                List<Gamers> newGamerList = _context.Gamers.Where(x => x.UserId == id && gamers.Contains(x.Gamertag)).ToList();
                Teams newTeam = new Teams(newGamerList);
                newTeam.TeamName = teamName;
                newTeam.UserId = id;
                _context.Teams.Add(newTeam);
                _context.SaveChanges();
                return RedirectToAction("DisplayFavoriteTeams");
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
            return View(favTeams);
        }
    }
}   