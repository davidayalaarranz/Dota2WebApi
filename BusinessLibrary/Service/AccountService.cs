using BusinessLibrary.Model;
using DataModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections;
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

        public async Task<ApplicationUser> SaveUser(ApplicationUser user)
        {
            try
            {
                IdentityResult ir = await _userManager.UpdateAsync(user);
                if (ir.Succeeded)
                    return await _userManager.FindByNameAsync(user.Email);
                else
                    throw new Exception("Error saving user");
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public async Task<ApplicationUser> GetUser(string username)
        {
            try
            {

                ApplicationUser user = await _userManager.FindByNameAsync(username);
                return user;
            }
            catch (Exception e)
            {
                throw;
            }
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
                throw;
            }
        }

        public string getSteamIdFromCode(string code)
        {
            try
            {
                long lcode = long.Parse(code);
                BitArray ba = new BitArray(BitConverter.GetBytes(lcode));

                bool[] YComponent = new bool[1];
                YComponent[0] = ba.Get(0);
                BitArray baY = new BitArray(YComponent);
                int[] YSteamId = new int[1];
                baY.CopyTo(YComponent, 0);

                bool[] XComponentUniverse = new bool[8];
                for (var i = 56; i < 63; i++)
                {
                    XComponentUniverse[i - 56] = ba.Get(i);
                }
                BitArray baXUniverse = new BitArray(XComponentUniverse);
                int[] XSteamId = new int[1];
                baXUniverse.CopyTo(XSteamId, 0);

                bool[] ZComponent = new bool[31];
                for (var i = 1; i < 32; i++)
                {
                    ZComponent[i - 1] = ba.Get(i);
                }
                BitArray baZ = new BitArray(ZComponent);
                int[] ZSteamId = new int[1];
                baZ.CopyTo(ZSteamId, 0);

                return string.Concat("STEAM_", XSteamId[0].ToString(), ":", YSteamId[0].ToString(), ":", ZSteamId[0].ToString());
            }
            catch (Exception e)
            {
                return string.Empty;
            }
        }

        public long getSteamPlayerId(long steamId64)
        {
            try
            {
                BitArray ba = new BitArray(BitConverter.GetBytes(steamId64));

                bool[] ZComponent = new bool[31];
                for (var i = 1; i < 32; i++)
                {
                    ZComponent[i - 1] = ba.Get(i);
                }
                BitArray baZ = new BitArray(ZComponent);
                int[] ZSteamId = new int[1];
                baZ.CopyTo(ZSteamId, 0);

                return (ZSteamId[0] * 2);
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public async Task<ApplicationUser> AssociateSteamIdWithUser(ApplicationUser user, string SteamId)
        {
            try
            {
                user.SteamId64 = long.Parse(SteamId);
                user.SteamIdCode = getSteamIdFromCode(SteamId);
                user.SteamPlayerId = getSteamPlayerId(user.SteamId64);
                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    return await _userManager.FindByNameAsync(user.Email);
                }
                throw new Exception(result.Errors.ToString() + ": Associate SteamId With User error");
            }
            catch(Exception e)
            {
                throw;
            }
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
                throw;
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
                throw;
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
                throw;
            }
        }


    }
}
