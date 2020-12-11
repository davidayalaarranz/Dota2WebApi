using DataAccessLibrary.Data;
using DataModel.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using BusinessLibrary.Model;
using DataModel;

namespace BusinessLibrary.Service
{
    public class BuildService : IBuildService
    {
        public async Task<BuildResponseModel> GetBuilds(ApplicationUser user, DataTableParameters param)
        {
            try
            {
                using (Dota2AppDbContext db = new Dota2AppDbContext())
                {
                    BuildResponseModel brm = new BuildResponseModel();
                    brm.nBuilds = (from a in db.Builds.Where(b => b.UserId == user.Id).AsNoTracking() select a).Count();
                    param.length = param.length < 1 ? brm.nBuilds : param.length;

                    var query = db.Builds.Where(b => b.UserId == user.Id).Where(b => b.UserId == user.Id);
                    if (!String.IsNullOrWhiteSpace(param.filter))
                        query = query.Where(b => b.Name.Contains(param.filter) || b.Hero.LocalizedName.Contains(param.filter));
                    brm.nBuilds = query.Count();

                    if (!String.IsNullOrEmpty(param.orderBy))
                    {
                        param.orderBy = string.Concat(param.orderBy.Substring(0, 1).ToUpper(), param.orderBy[1..]);
                        if (param.order.Equals("asc"))
                            query = query.OrderBy(p => EF.Property<object>(p, param.orderBy));
                        else
                            query = query.OrderByDescending(p => EF.Property<object>(p, param.orderBy));
                    }

                    brm.Builds = await query
                        .Include(b => b.Hero)
                        .Include(b => b.HeroUpgrades)
                        .ToListAsync();
                    
                    return brm;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public async Task<Build> CreateBuild(Build build, ApplicationUser user)
        {
            try 
            {
                using (Dota2AppDbContext db = new Dota2AppDbContext())
                {
                    // Check if build is well constructed
                    //

                    // Get the tracked entities associated with the build
                    build.Hero = db.Heroes.Where(h => h.HeroId == build.Hero.HeroId).Single();
                    foreach (BuildAbilityUpgrade bau in build.HeroUpgrades)
                    {
                        bau.Ability = db.Abilities.Where(a => a.AbilityId == bau.AbilityId).Single();
                    }
                    build.UserId = user.Id;

                    // Save build in db
                    db.Builds.Add(build);
                    await db.SaveChangesAsync();



                    return await GetBuild(build.BuildId);
                }
            }
            catch(Exception e)
            {
                throw e;
            }
        }

        public async Task<bool> DeleteBuild(Build build)
        {
            using (Dota2AppDbContext db = new Dota2AppDbContext())
            {
                // Delete build from db
                Build dbBuild = db.Builds.Find(build.BuildId);
                db.Builds.Remove(dbBuild);
                int nDeletedRegisters = await db.SaveChangesAsync();
                return nDeletedRegisters == 1;
            }
        }

        public async Task<Build> GetBuild(long id)
        {
            using (Dota2AppDbContext db = new Dota2AppDbContext())
            {
                // Get build from db
                var dbBuild = await db.Builds.Where(b => b.BuildId == id)
                    .Include(b => b.Hero)
                        .ThenInclude(h => h.Strength)
                    .Include(b => b.Hero)
                        .ThenInclude(h => h.Agility)
                    .Include(b => b.Hero)
                        .ThenInclude(h => h.Intelligence)
                    .Include(b => b.HeroUpgrades)
                    .SingleAsync();
                
                db.Entry(dbBuild.Hero)
                    .Collection(h => h.HeroAbilities)
                    .Query()
                    .Where(ha => !ha.Ability.IsHidden)
                    .OrderBy(h => h.Order)
                    .Load();

                foreach (HeroAbility ha in dbBuild.Hero.HeroAbilities)
                {
                    db.Entry(ha)
                        .Reference(ha => ha.Ability)
                        .Load();
                }

                return dbBuild;
            }
        }

        public async Task<Build> UpdateBuild(Build build)
        {
            try
            { 
                using (Dota2AppDbContext db = new Dota2AppDbContext())
                {
                    // Check if build is well constructed
                    //


                    //¿Tenemos que recuperar primero la build de base de datos o podemos hacer el update directamente? Parece que sí
                    Build b = await db.Builds.Where(b => b.BuildId == build.BuildId)
                        .Include(b => b.HeroUpgrades)
                        .Include(b => b.User)
                        .FirstAsync();

                    b.Name = build.Name;
                    b.HeroUpgrades = build.HeroUpgrades;

                    // Save build in db
                    //var heroUpgrades = db.BuildAbilityUpgrades.Where(bau => bau.BuildId == build.BuildId);
                    //if (heroUpgrades.Any())
                    //{
                    //    db.BuildAbilityUpgrades.RemoveRange(heroUpgrades);
                    //    await db.SaveChangesAsync();
                    //}

                    db.Builds.Update(b);
                    await db.SaveChangesAsync();
                    return await GetBuild(build.BuildId);
                }
            }
            catch(Exception e)
            {
                throw e;
            }
        }

        public async Task<BuildResponseModel> GetBuildsByUser(ApplicationUser user)
        {
            try
            {
                using (Dota2AppDbContext db = new Dota2AppDbContext())
                {
                    BuildResponseModel brm = new BuildResponseModel();
                    // Get build from db
                    brm.Builds = await db.Builds.Where(b => b.UserId == user.Id).ToListAsync();
                    brm.nBuilds = brm.Builds.Count();
                    return brm;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        
    }
}
