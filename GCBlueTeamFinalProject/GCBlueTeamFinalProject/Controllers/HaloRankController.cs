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
                    return View("YourProfile", UserList[i]);
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

        public async  Task<ActionResult> YourProfile(Users newUser)
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri($"https://www.haloapi.com/stats/h5/servicerecords/arena");
          
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", $"{APIKEYVARIABLE}");
            var response = await client.GetAsync($"?players={newUser.Gamertag}");
       
            var searchedPlayer = await response.Content.ReadAsAsync<PlayerRootObject>();
            ViewBag.UserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            

            if (searchedPlayer == null)
            {

                return View("Error", "This Gamertag does not exist, please try again.");
            }

            Gamers searchedGamer = new Gamers(searchedPlayer);
            if (ModelState.IsValid)
            {
                newUser.UserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                _context.Users.Add(newUser);
                _context.Gamers.Add(searchedGamer);
                _context.SaveChanges();
            }
            else
            {
                //more validation
                return RedirectToAction("RegisterUser");
            }
            return View(newUser);

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
            Gamers searchedGamer = new Gamers(searchedPlayer);
            
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
                _context.Gamers.Add(newPlayer);
                _context.SaveChanges();
            }
            return RedirectToAction("DisplayGamers");
        }
        public IActionResult DisplayGamers()
        {
            string id = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            List<Gamers> gamerList = _context.Gamers.Where(x => x.UserId == id).ToList();
            //List<Gamers> gamerList = _context.Gamers.ToList();
            //This line above for quickly showing all gamers in database instead of just associated with UserID
            return View(gamerList);
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