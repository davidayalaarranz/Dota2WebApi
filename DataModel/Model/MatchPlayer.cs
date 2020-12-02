using System;
using System.Collections.Generic;
using System.Text;

namespace DataModel.Model
{
    public class MatchPlayer
    {
        public MatchPlayer()
        {
            this.HeroUpgrades = new List<MatchPlayerAbilityUpgrade>();
        }
        public long PlayerId { get; set; }
        public Player Player { get; set; }
        public long MatchId { get; set; }
        public Match Match { get; set; }
        public int PlayerSlot { get; set; }
        public long HeroId { get; set; }
        public Hero Hero { get; set; }

        public List<MatchPlayerAbilityUpgrade> HeroUpgrades { get; set; }
        
        public int Level { get; set; }
        public int Kills { get; set; }
        public int Deaths { get; set; }
        public int Assists { get; set; }
        public int LastHits { get; set; }
        public int Denies { get; set; }
        public int GPM { get; set; }
        public int XPM { get; set; }
        public int HeroDamage { get; set; }
        public int TowerDamage { get; set; }
        public int HeroHealing { get; set; }
        public int Gold { get; set; }
    }
}
