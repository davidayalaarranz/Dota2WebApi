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
    public class PatchVersionService : IPatchVersionService
    {
        public async Task<PatchVersion> GetPatchVersion(string name)
        {
            using (Dota2AppDbContext db = new Dota2AppDbContext())
            {
                PatchVersion pv = await (from a in db.PatchVersions
                            select a)
                      .Where(pv => pv.Name.Equals(name))
                      .FirstOrDefaultAsync();

                return pv;
            }
        }

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
