using System;
using System.Collections.Generic;
using System.Text;

namespace DataModel.Model
{
    public class Build
    {
        public Build()
        {
            HeroUpgrades = new List<BuildAbilityUpgrade>(17);
        }
        public long BuildId { get; set; }

        public string Name { get; set; }
        public string Color { get; set; }

        public long HeroId { get; set; }
        public Hero Hero { get; set; }
        public List<BuildAbilityUpgrade> HeroUpgrades { get; set; }

        public string UserId { get; set; } 
        public ApplicationUser User { get; set; }
    }
}
