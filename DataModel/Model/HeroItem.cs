using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataModel.Model
{
    public class HeroItemComponent
    {
        public HeroItemComponent() { }

        public long HeroItemId { get; set; }
        public HeroItem HeroItem { get; set; }
        public long ComponentId { get; set; }
        public HeroItem Component { get; set; }
        public int Quantity
        {
            get;
            set;
        }
    }
    public class ItemUpgrade
    {
        public ItemUpgrade() { }

        public long HeroItemId { get; set; }
        public HeroItem HeroItem { get; set; }
        public int StartLevel { get; set; }
        public int EndLevel { get; set; }
        public bool IsSold { get; set; }
        public int HeroItemSlot { get; set; }
    }
    public class MatchPlayerHeroItemUpgrade : ItemUpgrade
    {
        public MatchPlayerHeroItemUpgrade() { }

        [Key]
        public long MatchPlayerHeroItemUpgradeId { get; set; }
        public long MatchPlayerMatchId { get; set; }
        public long MatchPlayerPlayerId { get; set; }
        public int MatchPlayerPlayerSlot { get; set; }
    }
    public class HeroItem
    {
        public HeroItem()
        {
            Components = new List<HeroItemComponent>();
            IsComponentOf = new List<HeroItemComponent>();
            ComponentsName = new List<string>();
            Description = string.Empty;
            Notes = string.Empty;
            Lore = string.Empty;
            Attrib = string.Empty;
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long HeroItemId { get; set; }

        public string Name { get; set; }
        public string ShortName { get { if (Name != null) return Name.Remove(0, 5); else return string.Empty; } }
        public int Cost { get; set; }
        public bool IsSecretShop { get; set; }
        public bool IsSideShop { get; set; }
        public bool IsRecipe { get; set; }
        public string LocalizedName { get; set; }
        public string ImageUrl { get { 
                if (IsRecipe)
                    return string.Concat("https://cdn.cloudflare.steamstatic.com/apps/dota2/images/items/recipe_lg.png");
                else
                    return string.Concat("https://cdn.cloudflare.steamstatic.com/apps/dota2/images/items/", ShortName, "_lg.png"); } 
        }
        public int Cooldown { get; set; }
        public int ManaCost { get; set; }
        public string Description { get; set; }
        public string Notes { get; set; }
        public string Lore { get; set; }
        public List<HeroItemComponent> Components { get; set; }
        public List<HeroItemComponent> IsComponentOf { get; set; }
        public bool Created { get; set; }
        public string Attrib { get; set; }


        public bool IsStrengthBonus { get; set; }
        public bool IsAgilityBonus { get; set; }
        public bool IsIntelligenceBonus { get; set; }

        public int BonusStrength { get; set; }
        public int BonusAgility { get; set; }
        public int BonusIntelligence { get; set; }

        public int BonusAttackSpeed { get; set; }
        public int BonusMovementSpeed { get; set; }
        public int BonusDamage { get; set; }
        public int BonusManaRegeneration { get; set; }
        public int BonusHPRegeneration { get; set; }
        public int BonusMana { get; set; }
        public int BonusHealth { get; set; }


        [NotMapped]
        public List<string> ComponentsName { get; set; }
    }
}
