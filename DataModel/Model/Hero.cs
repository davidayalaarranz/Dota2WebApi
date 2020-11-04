using DataModel.Model.JsonConverters;
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
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long HeroId { get; set; }

        public string Name { get; set; }
        public string ShortName { get { return Name.Remove(0,14); } }
        public string ImageUrl { get { return string.Concat("https://cdn.cloudflare.steamstatic.com/apps/dota2/images/heroes/", Name.Remove(0, 14), "_lg.png"); } }
        public string LocalizedName { get; set; }
        [JsonConverter(typeof(HeroPrincipalAttributeJsonConverter))]
        public HeroPrincipalAttribute PrincipalAttribute { get; set; }
        public int MinDamage { get; set; }
        public int MaxDamage { get; set; }
        public int MovementSpeed { get; set; }
        public decimal Armor { get; set; }
        public string Roles { get; set; }
        
        public HeroAttribute Strength { get; set; }
        public HeroAttribute Agility { get; set; }
        public HeroAttribute Inteligence { get; set; }

        public string RightClickAttack { get; set; }
    }
}
