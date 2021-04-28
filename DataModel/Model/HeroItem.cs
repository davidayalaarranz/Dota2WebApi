using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataModel.Model
{
    public class HeroItemComponent
    {
        public HeroItemComponent() { }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long HeroItemPatchVersionId { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long HeroItemId { get; set; }
        public HeroItem HeroItem { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long ComponentPatchVersionId { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long ComponentId { get; set; }
        public HeroItem Component { get; set; }

        public int Quantity { get; set; }
    }
    public class ItemUpgrade
    {
        public ItemUpgrade() { }

        public long PatchVersionId { get; set; }
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

            // 7.29
            NotesList = new List<HeroItemNote>();
            CastRangeList = new List<HeroItemCastRange>();
            CastPointList = new List<HeroItemCastPoint>();
            ChannelTimeList = new List<HeroItemChannelTime>();
            CooldownList = new List<HeroItemCooldown>();
            DurationList = new List<HeroItemDuration>();
            DamageList = new List<HeroItemDamage>();
            ManaCostList = new List<HeroItemManaCost>();
            GoldCostList = new List<HeroItemGoldCost>();
            SpecialValues = new List<HeroItemSpecialValue>();
        }
        
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long HeroItemId { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long PatchVersionId { get; set; }
        public PatchVersion PatchVersion { get; set; }

        public string Name { get; set; }
        public string ShortName { get { if (Name != null) return Name.Remove(0, 5); else return string.Empty; } }
        public int Cost { get; set; }
        public string Qual { get; set; }
        public bool IsSecretShop { get; set; }
        public bool IsSideShop { get; set; }
        public bool IsRecipe { get; set; }
        public string LocalizedName { get; set; }
        public string ImagePath { get; set; }
        public string ImageUrl { get { 
                if (IsRecipe)
                    return string.Concat("https://cdn.cloudflare.steamstatic.com/apps/dota2/images/items/recipe_lg.png");
                else
                    return string.Concat("https://cdn.cloudflare.steamstatic.com/apps/dota2/images/items/", ImagePath); } 
        }
        public int Cooldown { get; set; }
        public int ManaCost { get; set; }
        public string Description { get; set; }
        public string Notes { get; set; }
        public string Lore { get; set; }
        public List<HeroItemComponent> Components { get; set; }
        public List<HeroItemComponent> IsComponentOf { get; set; }
        public List<MatchPlayerHeroItemUpgrade> MatchPlayerUpgrades { get; set; }
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

        // Añadido en 7.29
        public decimal NeutralItemTier { get; set; }
        public List<HeroItemNote> NotesList { get; set; }
        public List<HeroItemCastRange> CastRangeList { get; set; }
        public List<HeroItemCastPoint> CastPointList { get; set; }
        public List<HeroItemChannelTime> ChannelTimeList { get; set; }
        public List<HeroItemCooldown> CooldownList { get; set; }
        public List<HeroItemDuration> DurationList { get; set; }
        public List<HeroItemDamage> DamageList { get; set; }
        public List<HeroItemManaCost> ManaCostList { get; set; }
        public List<HeroItemGoldCost> GoldCostList { get; set; }
        public List<HeroItemSpecialValue> SpecialValues { get; set; }
    }

    public class HeroItemSpecialValue : SpecialValue { }
    public class HeroItemNote : ArrayValue<string>
    {
        public HeroItemNote(string value) : base(value) { }
    }
    public class HeroItemCastRange : ArrayValue<int>
    {
        public HeroItemCastRange(int value) : base(value) { }
    }
    public class HeroItemCastPoint : ArrayValue<decimal>
    {
        public HeroItemCastPoint(decimal value) : base(value) { }
    }
    public class HeroItemChannelTime : ArrayValue<decimal>
    {
        public HeroItemChannelTime(decimal value) : base(value) { }
    }
    public class HeroItemCooldown : ArrayValue<decimal>
    {
        public HeroItemCooldown(decimal value) : base(value) { }
    }
    public class HeroItemDuration : ArrayValue<decimal>
    {
        public HeroItemDuration(decimal value) : base(value) { }
    }
    public class HeroItemDamage : ArrayValue<int>
    {
        public HeroItemDamage(int value) : base(value) { }
    }
    public class HeroItemManaCost : ArrayValue<int>
    {
        public HeroItemManaCost(int value) : base(value) { }
    }
    public class HeroItemGoldCost : ArrayValue<int>
    {
        public HeroItemGoldCost(int value) : base(value) { }
    }
    public class HeroItemSpecialFloatValue : ArrayValue<decimal>
    {
        public HeroItemSpecialFloatValue(decimal value) : base(value) { }
    }
    public class HeroItemSpecialIntValue : ArrayValue<int>
    {
        public HeroItemSpecialIntValue(int value) : base(value) { }
    }
}
