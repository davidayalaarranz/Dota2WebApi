using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataModel
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public long SteamId64 { get; set; }
        public string SteamIdCode { get; set; }
        public long SteamPlayerId { get; set; }
    }
}
