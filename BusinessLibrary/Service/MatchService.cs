using BusinessLibrary.Model;
using DataAccessLibrary.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace BusinessLibrary.Service
{
    public class MatchService : IMatchService
    {

        public async Task<MatchResponseModel> getMatches()
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
                                .Include(m => m.Players)
                                    .ThenInclude(mp => mp.Hero)
                                .Include(m => m.Players)
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
