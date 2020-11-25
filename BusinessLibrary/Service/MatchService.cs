using BusinessLibrary.Model;
using DataAccessLibrary.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using DataModel.Model;

namespace BusinessLibrary.Service
{
    public class MatchService : IMatchService
    {
        public async Task<Match> GetMatch(long id)
        {
            try
            { 
                using (Dota2AppDbContext db = new Dota2AppDbContext())
                {
                    List<Match> lm = await (from a in db.Matches.AsNoTracking()
                                           select a)
                        .Where(x => x.MatchId == id)
                        .Include(m => m.MatchPlayers)
                            .ThenInclude(mp => mp.Hero)
                        .Include(m => m.MatchPlayers)
                            .ThenInclude(mp => mp.Hero)
                            .ThenInclude(h => h.Strength)
                        .Include(m => m.MatchPlayers)
                            .ThenInclude(mp => mp.Hero)
                            .ThenInclude(h => h.Agility)
                        .Include(m => m.MatchPlayers)
                            .ThenInclude(mp => mp.Hero)
                            .ThenInclude(h => h.Inteligence)
                        .Include(m => m.MatchPlayers)
                            .ThenInclude(mp => mp.Player)
                        .ToListAsync();

                    foreach (Match m in lm)
                    {
                        foreach (MatchPlayer mp in m.MatchPlayers)
                        {
                            mp.Hero.Abilities = (from a in db.HeroAbilities
                                                 select a)
                                       .Where(ha => ha.HeroId == mp.Hero.HeroId && !ha.IsTalent & !ha.Ability.IsHidden)
                                       .Include(ha => ha.Ability)
                                       .ToList();
                        }
                    }

                    if (lm.Count > 0)
                    {
                        return lm[0];
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch(Exception e)
            {
                throw e;
            }
        }

        public async Task<MatchResponseModel> GetMatches()
        //public MatchResponseModel getMatches()
        {
            MatchResponseModel mrm = new MatchResponseModel();
            using (Dota2AppDbContext db = new Dota2AppDbContext())
            {
                mrm.nMatches = (from a in db.Matches.AsNoTracking() select a).Count();

                var query = from a in db.Matches.AsNoTracking()
                            select a;

                mrm.nMatches = query.Count();

                mrm.Matches = await query
                                .Include(m => m.MatchPlayers)
                                    .ThenInclude(mp => mp.Hero)
                                .Include(m => m.MatchPlayers)
                                    .ThenInclude(mp => mp.Player)
                                .ToListAsync();
            }
            return mrm;
        }

        //public async Task<MatchResponseModel GetHeroes(DataTableParameters param)
        //{
        //    HeroResponseModel crm = new HeroResponseModel();
        //    using (Dota2AppDbContext db = new Dota2AppDbContext())
        //    {
        //        crm.nHeroes = (from a in db.Heroes.AsNoTracking() select a).Count();
        //        param.length = param.length < 1 ? crm.nHeroes : param.length;

        //        var query = from a in db.Heroes.AsNoTracking()
        //                    select a;
        //        if (!String.IsNullOrWhiteSpace(param.filter))
        //            query = query.Where(x => x.Name.Contains(param.filter) || x.LocalizedName.Contains(param.filter));
        //        crm.nHeroes = query.Count();
        //        if (!String.IsNullOrEmpty(param.orderBy))
        //        {
        //            param.orderBy = string.Concat(param.orderBy.Substring(0, 1).ToUpper(), param.orderBy[1..]);
        //            if (param.order.Equals("asc"))
        //                query = query.OrderBy(p => EF.Property<object>(p, param.orderBy));
        //            else
        //                query = query.OrderByDescending(p => EF.Property<object>(p, param.orderBy));
        //        }

        //        crm.Heroes = await query.Skip(param.start)
        //                      .Take(param.length)
        //                      .Include(h => h.Strength)
        //                      .Include(h => h.Agility)
        //                      .Include(h => h.Inteligence)
        //                      .Include(h => h.Abilities)
        //                      .ToListAsync();
        //        crm.Heroes.ToList().ForEach(h => h.Abilities = h.Abilities.OrderBy(h => h.Order).ToList());
        //        return crm;
        //    }
        //}

        
    }
}
