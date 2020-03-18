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
            string id = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            List<Gamers> gamerList = _context.Gamers.Where(x => x.UserId == id).ToList();

            if (ModelState.IsValid)
            {
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

            List<Gamers> sortedList = apiListSearch.OrderBy(x => x.Score).Reverse().ToList(); //sorts list of gamers by score
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
        public IActionResult CreateTeams(List<Gamers> gamers)
        {
            return View(Teams.TeamMaker(gamers)); //Sending a List<Teams> //may need to validate number of gamers here
        }

        //method to create teams Red vs Blue
        //public async Task<ActionResult> CreateTeams(/*List<Gamers> gamers*/)
        //{
        //    var client = new HttpClient();
        //    client.BaseAddress = new Uri($"https://www.haloapi.com/stats/h5/servicerecords/arena");

        //    client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", $"{APIKEYVARIABLE}");
        //    var response = await client.GetAsync("?players=Lethul");

        //    var searchedPlayer = await response.Content.ReadAsAsync<PlayerRootObject>();
        //    ViewBag.UserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
        //    Gamers searchedGamer = new Gamers(searchedPlayer);
        //    //
        //    var client1 = new HttpClient();
        //    client1.BaseAddress = new Uri($"https://www.haloapi.com/stats/h5/servicerecords/arena");

        //    client1.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", $"{APIKEYVARIABLE}");
        //    var response1 = await client1.GetAsync("?players=blody09");

        //    var searchedPlayer2 = await response1.Content.ReadAsAsync<PlayerRootObject>();
        //    ViewBag.UserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
        //    Gamers searchedGamer2 = new Gamers(searchedPlayer2);
        //    //
        //    var client3 = new HttpClient();
        //    client3.BaseAddress = new Uri($"https://www.haloapi.com/stats/h5/servicerecords/arena");

        //    client3.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", $"{APIKEYVARIABLE}");
        //    var response3 = await client3.GetAsync("?players=Sir%20Cruniac");

        //    var searchedPlayer3 = await response3.Content.ReadAsAsync<PlayerRootObject>();
        //    ViewBag.UserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
        //    Gamers searchedGamer3 = new Gamers(searchedPlayer3);
        //    //
        //    var client4 = new HttpClient();
        //    client4.BaseAddress = new Uri($"https://www.haloapi.com/stats/h5/servicerecords/arena");

        //    client4.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", $"{APIKEYVARIABLE}");
        //    var response4 = await client4.GetAsync("?players=blody09");

        //    var searchedPlayer4 = await response4.Content.ReadAsAsync<PlayerRootObject>();
        //    ViewBag.UserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
        //    Gamers searchedGamer4 = new Gamers(searchedPlayer4);
        //    //



        //    Teams red = new Teams(searchedGamer.Gamertag, searchedGamer2.Gamertag);
        //    Teams blue = new Teams(searchedGamer3.Gamertag, searchedGamer4.Gamertag);

        //   List<Teams> teams = new List<Teams> { red, blue};

        //   // return View("DisplayFavoriteTeams", teams);

        //    return View(teams); 

        //}

        public IActionResult AddFavoriteTeams(Teams favTeam)
        {

            if (ModelState.IsValid)
            {
                _context.Teams.Add(favTeam);
                _context.SaveChanges();
            }

            return View("DisplayFavoriteTeams");
        }

        public IActionResult DisplayFavoriteTeams(List<Teams> favTeams)
        {

            return View(favTeams);
        }
    }
}   