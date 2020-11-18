using BusinessLibrary.Model;
using DataModel;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLibrary.Service
{
    public interface IAccountService
    {
        Task<bool> CheckUser(string username, string password);
        Task<ApplicationUser> SignUp(SignUpModel user);
        Task<ApplicationUser> GetUser(string username, string password);
        Task<JwtSecurityToken> GetToken(string username, string password);
    }
}
