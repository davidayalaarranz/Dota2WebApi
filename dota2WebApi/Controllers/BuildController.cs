using BusinessLibrary.Model;
using BusinessLibrary.Service;
using DataModel;
using DataModel.Model;
using dota2WebApi.Common;
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
        public async Task<ActionResult<BuildResponseModel>> Get([FromQuery] DataTableParameters parameters)
        {
            Claim idUser = User.FindFirst(ClaimTypes.NameIdentifier);
            if (idUser != null)
            {
                ApplicationUser user = await _accountService.GetUser(idUser.Value);
                BuildResponseModel brm = await _buildService.GetBuilds(user, parameters);
                List<Build> lb = brm.Builds.ToList();
                Object[] mRet = new Object[lb.Count];
                for (var i = 0; i < lb.Count; i++)
                {
                    mRet[i] = Transformers.TransformBuild(lb[i]);
                }
                var ret = new
                {
                    nBuilds = brm.nBuilds,
                    Builds = mRet,
                };

                return Ok(ret);
            }
            return Unauthorized();
        }

        // GET api/<BuildController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Build>> Get(int id)
        {
            Claim idUser = User.FindFirst(ClaimTypes.NameIdentifier);
            if (idUser != null)
            {
                //ApplicationUser user = await _accountService.GetUser(idUser.Value);
                Build b = await _buildService.GetBuild(id);
                return Ok(Transformers.TransformBuild(b));
            }
            return Unauthorized();
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

                    return CreatedAtAction("Get", new { id = bNew.BuildId }, Transformers.TransformBuild(bNew));
                }
                return Unauthorized();
            }
            catch (Exception e)
            {
                return BadRequest("Error creating build");
            }
        }

        // PUT api/<BuildController>/5
        [HttpPut]
        public async Task<ActionResult<Build>> Update(Build build)
        {
            try
            {
                Claim idUser = User.FindFirst(ClaimTypes.NameIdentifier);
                if (idUser != null)
                {
                    ApplicationUser user = await _accountService.GetUser(idUser.Value);
                    Build bNew = await _buildService.UpdateBuild(build);

                    return Ok(Transformers.TransformBuild(bNew));
                }
                return Unauthorized();
            }
            catch (Exception e)
            {
                return BadRequest("Error creating build");
            }
        }

        // DELETE api/<BuildController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
