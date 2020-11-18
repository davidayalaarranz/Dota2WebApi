using DataModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLibrary.Model
{
    public  class LoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
    public class LoginResponse
    {
        public Object user { get; set; }
        public string token { get; set; }
        public DateTime tokenExpiration { get; set; }
    }
}
