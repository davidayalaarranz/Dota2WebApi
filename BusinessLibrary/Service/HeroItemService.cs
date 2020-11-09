using BusinessLibrary.Model;
using DataAccessLibrary.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using System.Linq;
using DataModel.Model;

namespace BusinessLibrary.Service
{
    public class HeroItemService : IHeroItemService
    {
        public Task<HeroItem> GetHeroItem(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<HeroItemResponseModel> GetHeroItems()
        {
            HeroItemResponseModel crm = new HeroItemResponseModel();
            using (Dota2AppDbContext db = new Dota2AppDbContext())
            {
                crm.HeroItems = await (from a in db.HeroItems.AsNoTracking()
                                       select a).ToListAsync();
                crm.nHeroItems = crm.HeroItems.Count();
                return crm;
            }

            throw new NotImplementedException();
        }

        public async Task<HeroItemResponseModel> GetHeroItems(DataTableParameters param)
        {
            HeroItemResponseModel crm = new HeroItemResponseModel();
            using (Dota2AppDbContext db = new Dota2AppDbContext())
            {
                crm.nHeroItems = (from a in db.HeroItems
                                  where !a.IsRecipe
                                  select 
                                  new HeroItem { HeroItemId = a.HeroItemId })
                                  .Count();
                param.length = param.length < 1 ? crm.nHeroItems : param.length;

                var query = (from a in db.HeroItems.AsNoTracking()
                            select a)
                            .Where(hi => !hi.IsRecipe);
                if (!String.IsNullOrWhiteSpace(param.filter))
                    query = query.Where(x => x.Name.Contains(param.filter) || x.LocalizedName.Contains(param.filter));
                crm.nHeroItems = query.Count();
                if (!String.IsNullOrEmpty(param.orderBy))
                {
                    param.orderBy = string.Concat(param.orderBy.Substring(0, 1).ToUpper(), param.orderBy[1..]);
                    if (param.order.Equals("asc"))
                        query = query.OrderBy(p => EF.Property<object>(p, param.orderBy));
                    else
                        query = query.OrderByDescending(p => EF.Property<object>(p, param.orderBy));
                }

                crm.HeroItems = await query.Skip(param.start)
                              .Take(param.length)
                              .Include(hi => hi.Components)
                              .ThenInclude(hi1 => hi1.Component)
                              .ToListAsync();
                return crm;
            }
        }
    }
}
