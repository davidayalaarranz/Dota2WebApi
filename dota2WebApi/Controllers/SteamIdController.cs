using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BusinessLibrary.Service;
using DataModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace dota2WebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class SteamIdController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public SteamIdController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        // GET: api/<SteamIdController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<SteamIdController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<SteamIdController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] string steamId)
        {
            Claim idUser = User.FindFirst(ClaimTypes.NameIdentifier);
            if (idUser != null)
            { 
                ApplicationUser user = await _accountService.GetUser(idUser.Value);
                user = await _accountService.AssociateSteamIdWithUser(user, steamId);
                return Ok(user);
            }
            return Unauthorized();
        }

        // PUT api/<SteamIdController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<SteamIdController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
