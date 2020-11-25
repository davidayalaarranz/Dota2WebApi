using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BusinessLibrary.Model;
using BusinessLibrary.Service;
using DataModel.Model;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using dota2WebApi.Common;

namespace dota2WebApi.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class MatchController : ControllerBase
    {
        private readonly IMatchService _matchService;
        public MatchController(IMatchService matchService)
        {
            _matchService = matchService;
        }

        // GET: api/Match
        [HttpGet]
        public async Task<ActionResult<MatchResponseModel>> GetMatches()
        {
            try
            {
                MatchResponseModel mr = await _matchService.GetMatches();
                List<Match> lm = mr.Matches.ToList();
                Object[] mRet = new Object[lm.Count];
                for (var i = 0; i < lm.Count; i++)
                {
                    mRet[i] = Transformers.TransformMatch(lm[i]);
                }
                var ret = new
                {
                    nMatches = mr.nMatches,
                    Matches = mRet,
                };

                return Ok(ret);
            }
            catch (Exception e)
            {
                return BadRequest("Error getting Heroes");
            }
        }

        // GET: api/Match/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] long id)
        {
            try
            {
                Match m = await _matchService.GetMatch(id);
                return Ok(Transformers.TransformMatch(m));
            }
            catch (Exception e)
            {
                return BadRequest("Error getting Heroes");
            }
        }

        // GET: api/Match/5
        //[HttpGet("{id}")]
        //public async Task<ActionResult<Match>> GetMatch(long id)
        //{
        //    try
        //    {
        //        //var serializeOptions = new JsonSerializerOptions
        //        //{
        //        //    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        //        //};
        //        //serializeOptions.Converters.Add(new HeroPrincipalAttributeJsonConverter());
        //        //serializeOptions.WriteIndented = true;
        //        return Ok(await _matchService.getMatches()[0]);
        //    }
        //    catch (Exception e)
        //    {
        //        return BadRequest("Error getting Heroes");
        //    }
        //}

        //// PUT: api/Match/5
        //// To protect from overposting attacks, enable the specific properties you want to bind to, for
        //// more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutMatch(long id, Match match)
        //{
        //    if (id != match.MatchId)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(match).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!MatchExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        //// POST: api/Match
        //// To protect from overposting attacks, enable the specific properties you want to bind to, for
        //// more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        //[HttpPost]
        //public async Task<ActionResult<Match>> PostMatch(Match match)
        //{
        //    _context.Matches.Add(match);
        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateException)
        //    {
        //        if (MatchExists(match.MatchId))
        //        {
        //            return Conflict();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return CreatedAtAction("GetMatch", new { id = match.MatchId }, match);
        //}

        //// DELETE: api/Match/5
        //[HttpDelete("{id}")]
        //public async Task<ActionResult<Match>> DeleteMatch(long id)
        //{
        //    var match = await _context.Matches.FindAsync(id);
        //    if (match == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.Matches.Remove(match);
        //    await _context.SaveChangesAsync();

        //    return match;
        //}

        //private bool MatchExists(long id)
        //{
        //    return _context.Matches.Any(e => e.MatchId == id);
        //}
    }
}
