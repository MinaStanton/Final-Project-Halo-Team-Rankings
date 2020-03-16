using System;
using System.Collections.Generic;

namespace GCBlueTeamFinalProject.Models
{
    public partial class Users
    {
        public int Id { get; set; }
        public string Gamertag { get; set; }
        public string UserName { get; set; }
        public DateTime? Dob { get; set; }
        public string Gender { get; set; }
        public string Images { get; set; }
        public string UserId { get; set; }

        public virtual AspNetUsers User { get; set; }
    }
}
