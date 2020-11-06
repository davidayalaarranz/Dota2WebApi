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
    public class UrlService : IUrlService
    {
        public async Task<UrlResponseModel> GetUrls()
        {
            UrlResponseModel urm = new UrlResponseModel();
            using (Dota2AppDbContext db = new Dota2AppDbContext())
            {
                urm.heroUrls = await (from a in db.Heroes.AsNoTracking()
                            select new Url
                            {
                                Id = a.HeroId,
                                Value = a.ShortName,
                            })
                            .ToListAsync();
                return urm;
            }
        }
    }
}
