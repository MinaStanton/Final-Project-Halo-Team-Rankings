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

        
        public IActionResult YourProfile(Users newUser)
        {

            if (ModelState.IsValid)
            {
                newUser.UserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                _context.Users.Add(newUser);
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
    }
}   