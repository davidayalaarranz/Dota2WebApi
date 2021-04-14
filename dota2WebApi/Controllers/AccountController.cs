using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using BusinessLibrary.Model;
using BusinessLibrary.Service;
using DataModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Dota2WebApi.Controllers
{
    [Route("api/[controller]")]
    [AllowAnonymous]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(IAccountService accountService, SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager)
        {
            _accountService = accountService;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        // GET: api/Account
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Account/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        //[HttpPost]
        //public IEnumerable<string> Post()
        //{
        //    return new string[] { "value1", "value2" };
        //}
        // POST: api/Account/username
        [HttpPost]
        public async Task<IActionResult> SignUp([FromBody]SignUpModel user)
        {
            //System.Threading.Thread.Sleep(3000);
            ApplicationUser au = await _accountService.SignUp(user);
            JwtSecurityToken token = await _accountService.GetToken(user.email, user.password);
            LoginResponse response = new LoginResponse()
            {
                user = au,
                token = new JwtSecurityTokenHandler().WriteToken(token),
                tokenExpiration = token.ValidTo
            };

            return Ok(response);
        }

        // PUT: api/Account/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
