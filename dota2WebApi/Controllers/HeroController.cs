using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using BusinessLibrary.Model;
using BusinessLibrary.Service;
using DataModel.Model;
using DataModel.Model.JsonConverters;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace dota2WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HeroController : ControllerBase
    {
        private readonly IHeroService _heroService;
        public HeroController(IHeroService heroService)
        {
            _heroService = heroService;
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
        public async Task<IActionResult> Get([FromQuery] DataTableParameters parameters)
        {
            try
            {
                HeroResponseModel hrm = await _heroService.GetHeroes(parameters);
                List<Object> lh = new List<object>();
                foreach (Hero h in hrm.Heroes)
                {
                    lh.Add(TransformHero(h));
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

        private static Object TransformHero(Hero h)
        {
            foreach (HeroAbility ha in h.Abilities)
            {
                ha.Hero = null;
                ha.Ability.Heroes = null;
            }
            var ret = new
            {
                HeroId = h.HeroId,
                Name = h.Name,
                ShortName = h.ShortName,
                ImageUrl = h.ImageUrl,
                VerticalImageUrl = h.VerticalImageUrl,
                LocalizedName = h.LocalizedName,
                PrincipalAttribute = h.PrincipalAttribute,
                Roles = h.Roles,
                RightClickAttack = h.RightClickAttack,
                Biography = h.Biography,
                Strength = new
                {
                    Id = h.Strength.Id,
                    Initial = h.Strength.Initial,
                    Gain = h.Strength.Gain
                },
                Agility = new
                {
                    Id = h.Agility.Id,
                    Initial = h.Agility.Initial,
                    Gain = h.Agility.Gain
                },
                Inteligence = new
                {
                    Id = h.Inteligence.Id,
                    Initial = h.Inteligence.Initial,
                    Gain = h.Inteligence.Gain,
                },
                MinDamage = h.MinDamage,
                MaxDamage = h.MaxDamage,
                AttackRange = h.AttackRange,
                Armor = h.Armor,
                BaseArmor = h.BaseArmor,
                AttackRate = h.AttackRate,
                MovementSpeed = h.MovementSpeed,
                TurnRate = h.TurnRate,
                BaseHpRegen = h.BaseHpRegen,
                BaseManaRegen = h.BaseManaRegen,
                BaseHp = h.BaseHp,
                BaseMana = h.BaseMana,
                VisionDaytimeRange = h.VisionDaytimeRange,
                VisionNighttimeRange = h.VisionNighttimeRange,
                MagicalResistance = h.MagicalResistance,
                BaseAttackSpeed = h.BaseAttackSpeed,
                AttackAnimationPoint = h.AttackAnimationPoint,
                AttackAcquisitionRange = h.AttackAcquisitionRange,
                Abilities = (from a in h.Abilities
                             select a.Ability)

            };

            return ret;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            try
            {
                Hero h = await _heroService.GetHero(id);
                // Eliminamos los ciclos antes de serializar.
                var ret = TransformHero(h);
                
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
