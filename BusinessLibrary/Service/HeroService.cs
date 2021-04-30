using BusinessLibrary.Model;
using DataAccessLibrary.Data;
using DataModel.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;

namespace BusinessLibrary.Service
{
    public class HeroService : IHeroService
    {
        public async Task<Hero> GetHero(int id)
        {
            using (Dota2AppDbContext db = new Dota2AppDbContext())
            {
                try { 
                    var lh1 = await (from a in db.Heroes
                              select a)
                        .Where(x => x.HeroId == id)
                        .Include(h => h.Strength)
                        .Include(h => h.Agility)
                        .Include(h => h.Intelligence)
                        //.Include(h => h.Abilities)    
                        .ToListAsync()
                        ;

                    foreach (Hero h in lh1)
                    {
                        db.Entry(h)
                                .Collection(h => h.HeroAbilities)
                                .Query()
                                .Where(ha => !ha.Ability.IsHidden)
                                .Load();
                        foreach (HeroAbility ha in h.HeroAbilities)
                        {
                            db.Entry(ha)
                                .Reference(ha => ha.Ability)
                                .Load();
                        }

                    }
                    List<Hero> lh = lh1;
                    if (lh.Count > 0)
                    {
                        lh[0].HeroAbilities = lh[0].HeroAbilities.OrderBy(ha => ha.Order).ToList();
                        return lh[0];
                    }
                    else
                    {
                        return null;
                    }
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        public async Task<HeroResponseModel> GetHeroes(DataTableParameters param, PatchVersion pv = null)
        {
            if (pv == null) pv = AppConfiguration.CurrentDotaPatchVersion;
            HeroResponseModel crm = new HeroResponseModel();
            using (Dota2AppDbContext db = new Dota2AppDbContext())
            {
                crm.nHeroes = (from a in db.Heroes.AsNoTracking() select a)
                    .Where(h => h.PatchVersionId == pv.PatchVersionId)
                    .Where(a => a.RightClickAttack != null)
                    .Count();
                param.length = param.length < 1 ? crm.nHeroes : param.length;

                var query = (from a in db.Heroes.AsNoTracking()
                            select a)
                            .Where(h => h.PatchVersionId == pv.PatchVersionId)
                            .Where(a => a.RightClickAttack != null);
                if (!String.IsNullOrWhiteSpace(param.filter))
                    query = query.Where(x => x.Name.Contains(param.filter) || x.LocalizedName.Contains(param.filter));
                crm.nHeroes = query.Count();
                if (!String.IsNullOrEmpty(param.orderBy))
                {
                    param.orderBy = string.Concat(param.orderBy.Substring(0, 1).ToUpper(), param.orderBy[1..]);
                    if (param.order.Equals("asc"))
                        query = query.OrderBy(p => EF.Property<object>(p, param.orderBy));
                    else
                        query = query.OrderByDescending(p => EF.Property<object>(p, param.orderBy));
                }

                crm.Heroes = await query.Skip(param.start)
                              .Take(param.length)
                              .Include(h => h.Strength)
                              .Include(h => h.Agility)
                              .Include(h => h.Intelligence)
                              //.Include(h => h.Abilities)
                              .ToListAsync();
                foreach (Hero h in crm.Heroes)
                {
                    h.HeroAbilities = (from a in db.HeroAbilities
                                   select a)
                                   .Where(ha => ha.HeroId == h.HeroId && !ha.IsTalent & !ha.Ability.IsHidden)
                                   .Include(ha => ha.Ability)
                                   .OrderBy(ha => ha.Order)
                                   .ToList();

                }
                crm.Heroes.ToList().ForEach(h => h.HeroAbilities = h.HeroAbilities.OrderBy(h => h.Order).ToList());

                return crm;
            }
        }
    }
}
