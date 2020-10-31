using BusinessLibrary.Model;
using DataAccessLibrary.Data;
using DataModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace BusinessLibrary.Service
{
    public class HeroItemService : IHeroItemService
    {
        public async Task<HeroItem> GetHeroItem(int id)
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
                return crm;
            }

            throw new NotImplementedException();
        }

        public async Task<HeroItemResponseModel> GetHeroItems(DataTableParameters p)
        {
            throw new NotImplementedException();
        }
    }
}
