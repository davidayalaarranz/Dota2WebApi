using BusinessLibrary.Service;
using DataModel.Model;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace dota2WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BuildController : ControllerBase
    {
        private readonly IBuildService _buildService;
        public BuildController(IBuildService buildService)
        {
            _buildService = buildService;
        }

        // GET: api/<BuildController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<BuildController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<BuildController>
        [HttpPost]
        public async Task<ActionResult<Build>> Create(Build build)
        {
            try
            {
                Build bNew = await _buildService.CreateBuild(build);
                return CreatedAtAction("Create Build", new { id = bNew.BuildId }, bNew);
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
