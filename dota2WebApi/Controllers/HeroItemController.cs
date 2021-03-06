﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessLibrary.Model;
using BusinessLibrary.Service;
using DataModel.Model;
using dota2WebApi.Common;
using Microsoft.AspNetCore.Mvc;
using Serilog;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace dota2WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HeroItemController : ControllerBase
    {
        private readonly IHeroItemService _heroItemService;
        public HeroItemController(IHeroItemService heroItemService)
        {
            _heroItemService = heroItemService;
        }

        // GET: api/<HeroItemController>
        //[HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                HeroItemResponseModel hirm = await _heroItemService.GetHeroItems();
                AbstractJsonTransformer ajt = AbstractJsonTransformerCreator.CreateTransformer();
                var result = hirm.HeroItems.Select(hi => ajt.TransformHeroItem(hi));
                var ret = new
                {
                    HeroItems = result,
                    nHeroItems = hirm.nHeroItems,
                };
                return Ok(ret);
            }
            catch (Exception)
            {
                return BadRequest("Error getting Hero Items");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] DataTableParameters parameters)
        {
            try
            {
                //Log.Information("Get customer executing");
                HeroItemResponseModel hirm = await _heroItemService.GetHeroItems(parameters);
                AbstractJsonTransformer ajt = AbstractJsonTransformerCreator.CreateTransformer();
                var result = hirm.HeroItems.Select(hi => ajt.TransformHeroItem(hi));
                var ret = new
                {
                    HeroItems = result,
                    nHeroItems = hirm.nHeroItems,
                };
                return Ok(ret);
                //return Ok(await _heroItemService.GetHeroItems(parameters));
            }
            catch (Exception e)
            {
                return BadRequest("Error getting Hero Items");
            }
        }

        // GET api/<HeroItemController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<HeroItemController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<HeroItemController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<HeroItemController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
