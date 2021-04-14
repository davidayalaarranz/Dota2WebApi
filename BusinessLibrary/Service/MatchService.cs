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
                    List<Match> lm = await (from a in db.Matches
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
                            .ThenInclude(h => h.Intelligence)
                        .Include(m => m.MatchPlayers)
                            .ThenInclude(mp => mp.Player)
                        .ToListAsync();

                    foreach (Match m in lm)
                    {
                        foreach (MatchPlayer mp in m.MatchPlayers)
                        {
                            db.Entry(mp.Hero)
                                .Collection(h => h.HeroAbilities)
                                .Query()
                                .Where(ha => !ha.Ability.IsHidden)
                                .OrderBy(ha => ha.Order)
                                .Load();
                            foreach (HeroAbility ha in mp.Hero.HeroAbilities)
                            {
                                db.Entry(ha)
                                    .Reference(ha => ha.Ability)
                                    .Load();
                            }
                            db.Entry(mp)
                                .Collection(mp => mp.HeroUpgrades)
                                .Query()
                                .OrderBy(hu => hu.Level)
                                .Load();
                            db.Entry(mp)
                                .Collection(mp => mp.HeroItemUpgrades)
                                .Query()
                                .OrderBy(hiu => hiu.HeroItemSlot)
                                .Load();
                            foreach (MatchPlayerHeroItemUpgrade mphiu in mp.HeroItemUpgrades)
                            {
                                db.Entry(mphiu)
                                    .Reference(mphiu => mphiu.HeroItem)
                                    .Load();
                                //db.Entry(mphiu.HeroItem)
                                //    .Collection(hi => hi.Components)
                                //    .Query()
                                //    .Load();
                            }
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
    }
}
