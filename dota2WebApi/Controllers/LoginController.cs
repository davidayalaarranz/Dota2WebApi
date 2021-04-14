using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using BusinessLibrary.Model;
using BusinessLibrary.Service;
using DataModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Dota2WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public LoginController(IAccountService accountService, SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager)
        {
            _accountService = accountService;
            _userManager = userManager;
            _signInManager = signInManager;
        }


        // POST: api/Login
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] LoginRequest value)
        {
            if (await _accountService.CheckUser(value.Username, value.Password))
            {
                ApplicationUser au = await _accountService.GetUser(value.Username, value.Password);
                JwtSecurityToken token = await _accountService.GetToken(value.Username, value.Password);
                
                LoginResponse response = new LoginResponse()
                {
                    user = new ApplicationUser{
                        FirstName = au.FirstName,
                        LastName = au.LastName,
                        SteamId64 = au.SteamId64,
                        SteamIdCode = au.SteamIdCode,
                        SteamPlayerId = au.SteamPlayerId,
                        Email = au.Email
                    },
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    tokenExpiration = token.ValidTo
                };

                return Ok(response);
            }
            return Unauthorized();
        }
    }
}
