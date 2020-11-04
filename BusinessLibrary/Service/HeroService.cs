using BusinessLibrary.Model;
using DataAccessLibrary.Data;
using DataModel.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using System.Linq;

namespace BusinessLibrary.Service
{
    public class HeroService : IHeroService
    {
        public Task<Hero> GetHero(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<HeroResponseModel> GetHeroes(DataTableParameters param)
        {
            HeroResponseModel crm = new HeroResponseModel();
            using (Dota2AppDbContext db = new Dota2AppDbContext())
            {
                crm.nHeroes = (from a in db.Heroes.AsNoTracking() select a).Count();
                param.length = param.length < 1 ? crm.nHeroes : param.length;

                var query = from a in db.Heroes.AsNoTracking()
                            select a;
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
                              .Include(h => h.Inteligence)
                              .ToListAsync();
                return crm;
            }
        }
    }
}
