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
        public long HeroId { get; set; }
        public Hero Hero { get; set; }
        public long AbilityId { get; set; }
        public Ability Ability { get; set; }

        public bool IsTalent { get; set; }
        public int Order { get; set; }
    }

    public class AbilityUpgrade
    {
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
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long AbilityId { get; set; }
        //public int Order { get; set; }

        public bool IsHidden { get; set; }
        //public bool IsTalent { get; set; }

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
                return string.Concat("https://cdn.cloudflare.steamstatic.com/apps/dota2/images/abilities/", Name, "_lg.png"); 
            } 
        }

        public List<HeroAbility> Heroes { get; set; }


        /// de NPC_abilities
        /// base
        
        public int CastRangeBuffer { get; set; }

        //public int[] CastRange { get; set; }
        //public decimal[] CastPoints { get; set; }
        //public decimal[] ChannelTime { get; set; }
        //public decimal[] Cooldown { get; set; }
        //public decimal[] Duration { get; set; }
        //public int[] Damage { get; set; }
        //public int[] ManaCost { get; set; }
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
    }
}
