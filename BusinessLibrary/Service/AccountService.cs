using BusinessLibrary.Model;
using DataModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLibrary.Service
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountService(SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<ApplicationUser> GetUser(string username, string password)
        {
            try
            {
                ApplicationUser user = await _userManager.FindByNameAsync(username);
                if (user != null && await _userManager.CheckPasswordAsync(user, password))
                    return user;
                throw new UnauthorizedAccessException("Intento de acceso a usuario con contraseña incorrecta");
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public string getSteamIdFromCode(string code)
        {
            return string.Empty;
            //STEAM_1:0:74926258
        }

        public string getPlayerIdFromSteamId(string SteamId)
        {
            // Entra: STEAM_1:0:74926258
            return string.Empty;
            // Sale el id del jugador en Valve API GetMatchHistory

        }


        public async Task<JwtSecurityToken> GetToken(string username, string password)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(username);
                if (user != null && await _userManager.CheckPasswordAsync(user, password))
                {
                    var authClaims = new[]
                    {

                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Sub, username)
                    };

                    SymmetricSecurityKey authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("davidfernandoayalaarranz"));

                    JwtSecurityToken token = new JwtSecurityToken(
                        issuer: "https://localhost:44373",
                        audience: "https://localhost:44373",
                        expires: DateTime.Now.AddHours(3),
                        claims: authClaims,
                        signingCredentials: new Microsoft.IdentityModel.Tokens.SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                        );

                    return token;
                }
                throw new UnauthorizedAccessException("Intento de obtener token de un usuario con contraseña incorrecta");
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<bool> CheckUser(string username, string password)
        {
            try
            { 
                var user = await _userManager.FindByNameAsync(username);
                return (user != null && await _userManager.CheckPasswordAsync(user, password));
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<ApplicationUser> SignUp(SignUpModel user)
        {
            try
            {
                ApplicationUser au = new ApplicationUser()
                {
                    FirstName = user.firstName,
                    LastName = user.lastName,
                    Email = user.email,
                    UserName = user.email
                };
                var result = await _userManager.CreateAsync(au, user.password);

                if (result.Succeeded)
                {
                    return await _userManager.FindByNameAsync(user.email);
                }
                throw new Exception(result.Errors.ToString() + ": Sign Up error");
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
