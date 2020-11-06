using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessLibrary.Service;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace dota2WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UrlController : ControllerBase
    {

        private readonly IUrlService _urlService;
        public UrlController(IUrlService urlService)
        {
            _urlService = urlService;
        }

        // GET: api/<UrlController>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _urlService.GetUrls());
        }

        // GET api/<UrlController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<UrlController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<UrlController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<UrlController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
