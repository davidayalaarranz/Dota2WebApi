using BusinessLibrary.Service;
using DataModel;
using DataModel.Model;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace dota2WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BuildController : ControllerBase
    {
        private readonly IBuildService _buildService;
        private readonly IAccountService _accountService;

        public BuildController(IBuildService buildService, IAccountService accountService)
        {
            _buildService = buildService;
            _accountService = accountService;
        }

        // GET: api/<BuildController>
        [HttpGet]
        public async Task<ActionResult<Build>> Get()
        {
            Claim idUser = User.FindFirst(ClaimTypes.NameIdentifier);
            if (idUser != null)
            {
                ApplicationUser user = await _accountService.GetUser(idUser.Value);
                
            }
            return Unauthorized();
        }

        // GET api/<BuildController>/5
        [HttpGet("{id}")]
        public string Get(int applicationUserId)
        {
            return "value";
        }

        // POST api/<BuildController>
        [HttpPost]
        public async Task<ActionResult<Build>> Create(Build build)
        {
            try
            {
                Claim idUser = User.FindFirst(ClaimTypes.NameIdentifier);
                if (idUser != null)
                {
                    ApplicationUser user = await _accountService.GetUser(idUser.Value);
                    Build bNew = await _buildService.CreateBuild(build, user);

                    return CreatedAtAction("Create Build", new { id = bNew.BuildId }, bNew);
                }
                return Unauthorized();
            }
            catch (Exception e)
            {
                return BadRequest("Error creating build");
            }
        }

        // PUT api/<BuildController>/5
        [HttpPut("{id}")]
        public void Update(int id, [FromBody] string value)
        {
        }

        // DELETE api/<BuildController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
