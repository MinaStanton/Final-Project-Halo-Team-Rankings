using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GCBlueTeamFinalProject.Models
{
    public class UsersGamers
    {
        public Users MyUser { get; set; }
        public Gamers MyGamer { get; set; }

        public UsersGamers(Users myUser, Gamers myGamer)
        {
            MyUser = myUser;
            MyGamer = myGamer;

        }
       
    }



}
