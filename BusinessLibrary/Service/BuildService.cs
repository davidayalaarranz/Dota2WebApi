using DataAccessLibrary.Data;
using DataModel.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace BusinessLibrary.Service
{
    public class BuildService : IBuildService
    {
        public async Task<Build> CreateBuild(Build build)
        {
            try 
            {
                using (Dota2AppDbContext db = new Dota2AppDbContext())
                {
                    // Check if build is well constructed
                    //

                    // Get the tracked entities associated with the build
                    build.Hero = db.Heroes.Where(h => h.HeroId == build.HeroId).Single();
                    foreach (BuildAbilityUpgrade bau in build.BuildAbilityUpgrades)
                    {
                        bau.Ability = db.Abilities.Where(a => a.AbilityId == bau.AbilityId).Single();
                    }

                    // Save build in db
                    db.Builds.Add(build);
                    await db.SaveChangesAsync();
                    return build;
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
                var dbBuild = await db.Builds.Where(b => b.BuildId == id).SingleAsync();
                return dbBuild;
            }
        }

        public async Task<Build> UpdateBuild(Build build)
        {
            using (Dota2AppDbContext db = new Dota2AppDbContext())
            {
                // Check if build is well constructed
                //


                //¿Tenemos que recuperar primero la build de base de datos o podemos hacer el update directamente?
                // Save build in db
                db.Builds.Update(build);
                await db.SaveChangesAsync();
                return build;
            }
        }
    }
}
