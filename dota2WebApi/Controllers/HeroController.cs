using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using BusinessLibrary.Model;
using BusinessLibrary.Service;
using DataAccessLibrary.Data;
using DataModel.Model;
using DataModel.Model.JsonConverters;
using dota2WebApi.Common;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace dota2WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HeroController : ControllerBase
    {
        private readonly IHeroService _heroService;
        private readonly IPatchVersionService _patchVersionService;
        public HeroController(IHeroService heroService, IPatchVersionService patchVersionService)
        {
            _heroService = heroService;
            _patchVersionService = patchVersionService;
        }

        // GET: api/<HeroItemController>
        //[HttpGet]
        //public async Task<IActionResult> Get()
        //{
        //    try
        //    {
        //        return Ok(await _heroService.GetHeroes());
        //    }
        //    catch (Exception)
        //    {
        //        return BadRequest("Error getting Heroes");
        //    }
        //}

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] DataTableParameters parameters, string patchVersion)
        {
            try
            {
                PatchVersion pv;
                if (string.IsNullOrEmpty(patchVersion))
                    pv = AppConfiguration.CurrentDotaPatchVersion;
                else
                    pv = await _patchVersionService.GetPatchVersion(patchVersion);
                HeroResponseModel hrm = await _heroService.GetHeroes(parameters, pv);
                List<Object> lh = new List<object>();
                AbstractJsonTransformer ajt = AbstractJsonTransformerCreator.CreateTransformer();
                foreach (Hero h in hrm.Heroes)
                {
                    lh.Add(ajt.TransformHero(h));
                }

                var ret = new
                {
                    nHeroes = hrm.nHeroes,
                    Heroes = lh
                };
                return Ok(ret);
            }
            catch (Exception e)
            {
                return BadRequest("Error getting Heroes");
            }
        }

        

        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            try
            {
                Hero h = await _heroService.GetHero(id);
                // Eliminamos los ciclos antes de serializar.
                AbstractJsonTransformer ajt = AbstractJsonTransformerCreator.CreateTransformer();
                var ret = ajt.TransformHero(h);
                
                return Ok(ret);
                //return Ok(await _heroService.GetHero(id));
            }
            catch (Exception e)
            {
                return BadRequest("Error getting Heroes");
            }
        }

        // POST api/<HeroController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<HeroController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<HeroController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
