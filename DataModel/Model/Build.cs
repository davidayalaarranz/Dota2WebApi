using System;
using System.Collections.Generic;
using System.Text;

namespace DataModel.Model
{
    public class Build
    {
        public Build()
        {
            BuildAbilityUpgrades = new List<BuildAbilityUpgrade>(17);
        }
        public long BuildId { get; set; }
        public long HeroId { get; set; }
        public Hero Hero { get; set; }
        public List<BuildAbilityUpgrade> BuildAbilityUpgrades { get; set; }
    }
}
