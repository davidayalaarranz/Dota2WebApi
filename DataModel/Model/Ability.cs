using DataModel.Model.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DataModel.Model
{
    public class HeroAbility
    {
        [Key]
        public long HeroAbilityId { get; set; }
        
        public long HeroPatchVersionId { get; set; }
        public long HeroId { get; set; }
        public Hero Hero { get; set; }

        public long AbilityPatchVersionId { get; set; }
        public long AbilityId { get; set; }
        public Ability Ability { get; set; }

        public bool IsTalent { get; set; }
        public int Order { get; set; }
    }

    public class AbilityUpgrade
    {
        public long PatchVersionId { get; set; }
        public long AbilityId { get; set; }
        public Ability Ability { get; set; }

        public int Level { get; set; }
        public TimeSpan Time { get; set; }
    }
    public class MatchPlayerAbilityUpgrade : AbilityUpgrade
    {
        public long MatchPlayerMatchId { get; set; }
        public long MatchPlayerPlayerId { get; set; }
        public int MatchPlayerPlayerSlot { get; set; }
    }
    public class BuildAbilityUpgrade : AbilityUpgrade
    {
        public long BuildId { get; set; }
    }

    public class Ability
    {
        public Ability()
        {
            Heroes = new List<HeroAbility>();
            BuildUpgrades = new List<BuildAbilityUpgrade>();
            MatchPlayerUpgrades = new List<MatchPlayerAbilityUpgrade>();

            NotesList = new List<AbilityNote>();
            CastRangeList = new List<AbilityCastRange>();
            CastPointList = new List<AbilityCastPoint>();
            ChannelTimeList = new List<AbilityChannelTime>();
            CooldownList = new List<AbilityCooldown>();
            DurationList = new List<AbilityDuration>();
            DamageList = new List<AbilityDamage>();
            ManaCostList = new List<AbilityManaCost>();
            GoldCostList = new List<AbilityGoldCost>();
            SpecialValues = new List<AbilitySpecialValue>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long AbilityId { get; set; }
        public long PatchVersionId { get; set; }
        public PatchVersion PatchVersion { get; set; }

        public bool IsHidden { get; set; }

        public string Name { get; set; }
        public string LocalizedName { get; set; }
        public string Affects { get; set; }
        public string Description { get; set; }
        public string Notes { get; set; }
        public string Attrib { get; set; }
        public string Cmb { get; set; }
        public string Lore { get; set; }
        public string Hurl { get; set; }
        public string ImageUrl { get
            {
                return string.Concat(Name, "_lg.png");
            }
        }
        
        public List<HeroAbility> Heroes { get; set; }
        public List<BuildAbilityUpgrade> BuildUpgrades { get; set; }
        public List<MatchPlayerAbilityUpgrade> MatchPlayerUpgrades { get; set; }


        /// de NPC_abilities
        /// base
        
        public int CastRangeBuffer { get; set; }

        public string CastRange { get; set; }
        public string CastPoint { get; set; }
        public string ChannelTime { get; set; }
        public string Cooldown { get; set; }
        public string Duration { get; set; }
        public string Damage { get; set; }
        public string ManaCost { get; set; }

        /// otros
        /// 
        public bool HasScepterUpgrade { get; set; }
        public bool IsGrantedByScepter { get; set; }
        // Esta propiedad Value almacena el {s:value} de los LocalizedName de los talentos
        public string Value { get; set; }

        public int MaxLevel { get; set; }
        public bool IsPassive { get; set; }
        public bool IsAttributte { get; set; }
        public bool IsStrengthBonus { get; set; }
        public bool IsAgilityBonus { get; set; }
        public bool IsIntelligenceBonus { get; set; }

        public int BonusStrength
        {
            get
            {
                if (IsStrengthBonus) return int.Parse(Value);
                return 0;
            }
        }
        public int BonusAgility { get
            {
                if (IsAgilityBonus) return int.Parse(Value);
                return 0;
            } 
        }
        public int BonusIntelligence
        {
            get
            {
                if (IsIntelligenceBonus) return int.Parse(Value);
                return 0;
            }
        }

        // Añadido en 7.29
        public decimal NeutralItemTier { get; set; }
        public string ShardDescription { get; set; }
        public string ScepterDescription { get; set; }
        public int Dispellable { get; set; }
        public bool IsItem { get; set; }
        public bool HasShardUpgrade { get; set; }
        public bool IsGrantedByShard { get; set; }
        public int ItemInitialCharges { get; set; }

        //[NotMapped]
        public List<AbilityNote> NotesList { get; set; }
        public List<AbilityCastRange> CastRangeList { get; set; }
        public List<AbilityCastPoint> CastPointList { get; set; }
        public List<AbilityChannelTime> ChannelTimeList { get; set; }
        public List<AbilityCooldown> CooldownList { get; set; }
        public List<AbilityDuration> DurationList { get; set; }
        public List<AbilityDamage> DamageList { get; set; }
        public List<AbilityManaCost> ManaCostList { get; set; }
        public List<AbilityGoldCost> GoldCostList { get; set; }
        public List<AbilitySpecialValue> SpecialValues { get; set; }
        //special_values
    }

    #region Clases para almacenar los special_values
    public class AbilitySpecialFloatValue : SpecialFloatValue 
    {
        public AbilitySpecialFloatValue(decimal value) : base(value) { }
    }
    public class AbilitySpecialIntValue : SpecialIntValue
    {
        public AbilitySpecialIntValue(int value) : base(value) { }
    }
    public class AbilitySpecialValue : SpecialValue {
        public AbilitySpecialValue()
        {
            ValuesFloat = new List<AbilitySpecialFloatValue>();
            ValuesInt = new List<AbilitySpecialIntValue>();
        }
        public new List<AbilitySpecialFloatValue> ValuesFloat { get; set; }
        public new List<AbilitySpecialIntValue> ValuesInt { get; set; }
    }
    #endregion

    #region Clases para almacenar arrays de datos
    public class AbilityNote : ArrayValue<string> {
        public AbilityNote(string value) : base(value) { }
    }
    public class AbilityCastRange : ArrayValue<int>
    {
        public AbilityCastRange(int value) : base(value) { }
    }
    public class AbilityCastPoint : ArrayValue<decimal>
    {
        public AbilityCastPoint(decimal value) : base(value) { }
    }
    public class AbilityChannelTime : ArrayValue<decimal>
    {
        public AbilityChannelTime(decimal value) : base(value) { }
    }
    public class AbilityCooldown : ArrayValue<decimal>
    {
        public AbilityCooldown(decimal value) : base(value) { }
    }
    public class AbilityDuration : ArrayValue<decimal>
    {
        public AbilityDuration(decimal value) : base(value) { }
    }
    public class AbilityDamage : ArrayValue<int>
    {
        public AbilityDamage(int value) : base(value) { }
    }
    public class AbilityManaCost : ArrayValue<int>
    {
        public AbilityManaCost(int value) : base(value) { }
    }
    public class AbilityGoldCost : ArrayValue<int>
    {
        public AbilityGoldCost(int value) : base(value) { }
    }
    #endregion
}
