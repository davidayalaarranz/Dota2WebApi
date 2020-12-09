//using DataModel.Model.JsonConverters;
using DataModel.Model.JsonConverters;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace DataModel.Model
{
    public enum HeroPrincipalAttribute
    {
        Strength = 1,
        Agility = 2,
        Inteligence = 3
    }

    public class HeroAttribute
    {
        public long Id { get; set; }
        public decimal Initial { get; set; }
        public decimal Gain { get; set; }
    }
    

    public class Hero
    {
        public Hero()
        {
            Strength = new HeroAttribute();
            Agility = new HeroAttribute();
            Inteligence = new HeroAttribute();
            HeroAbilities = new List<HeroAbility>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long HeroId { get; set; }

        public string Name { get; set; }
        public string ShortName { get { return Name.Remove(0,14); } }
        public string ImageUrl { get { return string.Concat("https://cdn.cloudflare.steamstatic.com/apps/dota2/images/heroes/", Name.Remove(0, 14), "_lg.png"); } }
        public string VerticalImageUrl { get { return string.Concat("https://cdn.cloudflare.steamstatic.com/apps/dota2/images/heroes/", Name.Remove(0, 14), "_vert.jpg"); } }
        public string LocalizedName { get; set; }
        //[JsonConverter(typeof(HeroPrincipalAttributeJsonConverter))]
        public HeroPrincipalAttribute PrincipalAttribute { get; set; }
        public string Roles { get; set; }
        public string RightClickAttack { get; set; }
        public string Biography { get; set; }

        public HeroAttribute Strength { get; set; }
        public HeroAttribute Agility { get; set; }
        public HeroAttribute Inteligence { get; set; }

        public List<HeroAbility> HeroAbilities { get; set; }

        public List<MatchPlayer> Matches { get; set; }

        
        public int MinDamage { get; set; }
        public int MaxDamage { get; set; }
        public int AttackRange { get; set; }        
        public decimal Armor { get; set; }
        public decimal BaseArmor { get; set; }

        public decimal AttackRate { get; set; }
        public int MovementSpeed { get; set; }
        public decimal TurnRate { get; set; }

        public decimal BaseHpRegen { get; set; }
        public decimal BaseManaRegen { get; set; }
        public decimal BaseHp { get; set; }
        public decimal BaseMana { get; set; }

        public int VisionDaytimeRange { get; set; }
        public int VisionNighttimeRange { get; set; }
        
        public decimal MagicalResistance { get; set; }
        public int BaseAttackSpeed { get; set; }

        public decimal AttackAnimationPoint { get; set; }
        public int AttackAcquisitionRange { get; set; }

        public int AbilityTalentStart { get; set; }
    }
}
