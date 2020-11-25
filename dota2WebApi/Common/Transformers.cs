using DataModel.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dota2WebApi.Common
{
    public class Transformers
    {
        public static Object TransformMatch(Match m)
        {
            if (m == null) return null;
            Object[] mpRet = new Object[m.MatchPlayers.Count];
            for (var i = 0; i < m.MatchPlayers.Count; i++)
            {
                mpRet[i] = TransformMatchPlayer(m.MatchPlayers[i]);
            }

            var ret = new
            {
                MatchId = m.MatchId,
                MatchSeqNum = m.MatchSeqNum,
                StartTime = m.StartTime,
                RadiantWin = m.RadiantWin,
                Duration = m.Duration,
                PreGameDuration = m.PreGameDuration,
                TowerStatusRadiant = m.TowerStatusRadiant,
                TowerStatusDire = m.TowerStatusDire,
                BarracksStatusRadiant = m.BarracksStatusRadiant,
                BarracksStatusDire = m.BarracksStatusDire,
                FirstBloodTime = m.FirstBloodTime,
                GameMode = m.GameMode,
                RadiantScore = m.RadiantScore,
                DireScore = m.DireScore,
                Picks = m.Picks,
                Bans = m.Bans,

                MatchPlayers = mpRet,
            };
            return ret;
        }

        public static Object TransformMatchPlayer(MatchPlayer mp)
        {
            if (mp == null) return null;
            var ret = new
            {
                PlayerId = mp.PlayerId,
                MatchId = mp.MatchId,
                HeroId = mp.HeroId,
                PlayerSlot = mp.PlayerSlot,

                Player = TransformPlayer(mp.Player),
                Hero = TransformHero(mp.Hero),

                Level = mp.Level,
                Kills = mp.Kills,
                Deaths = mp.Deaths,
                Assists = mp.Assists,
                LastHits = mp.LastHits,
                Denies = mp.Denies,
                GPM = mp.GPM,
                XPM = mp.XPM,
                HeroDamage = mp.HeroDamage,
                TowerDamage = mp.TowerDamage,
                HeroHealing = mp.HeroHealing,
                Gold = mp.Gold,
            };
            return ret;
        }

        public static Object TransformPlayer(Player p)
        {
            if (p == null) return null;
            var ret = new
            {
                PlayerId = p.PlayerId,
            };
            return ret;
        }

        public static Object TransformAbility(Ability a)
        {
            if (a == null) return null;

            var ret = new
            {
                AbilityId = a.AbilityId,
                IsHidden = a.IsHidden,
                Name = a.Name,
                LocalizedName = a.LocalizedName,
                Affects = a.Affects,
                Description = a.Description,
                Notes = a.Notes,
                Attrib = a.Attrib,
                Cmb = a.Cmb,
                Lore = a.Lore,
                Hurl = a.Hurl,
                ImageUrl = a.ImageUrl,
                CastRangeBuffer = a.CastRangeBuffer,
                CastRange = a.CastRange,
                CastPoint = a.CastPoint,
                ChannelTime = a.ChannelTime,
                Cooldown = a.Cooldown,
                Duration = a.Duration,
                Damage = a.Damage,
                ManaCost = a.ManaCost,
                HasScepterUpgrade = a.HasScepterUpgrade,
                IsGrantedByScepter = a.IsGrantedByScepter,
            };
            return ret;
        }

        public static Object TransformHero(Hero h)
        {
            if (h == null) return null;
            
            Object[] abilitiesRet = new Object[h.Abilities.Count];
            for (var i = 0; i < h.Abilities.Count; i++)
            {
                abilitiesRet[i] = TransformAbility(h.Abilities[i].Ability);
            }
            var ret = new
            {
                HeroId = h.HeroId,
                Name = h.Name,
                ShortName = h.ShortName,
                ImageUrl = h.ImageUrl,
                VerticalImageUrl = h.VerticalImageUrl,
                LocalizedName = h.LocalizedName,
                PrincipalAttribute = h.PrincipalAttribute,
                Roles = h.Roles,
                RightClickAttack = h.RightClickAttack,
                Biography = h.Biography,
                Strength = new
                {
                    Id = h.Strength.Id,
                    Initial = h.Strength.Initial,
                    Gain = h.Strength.Gain
                },
                Agility = new
                {
                    Id = h.Agility.Id,
                    Initial = h.Agility.Initial,
                    Gain = h.Agility.Gain
                },
                Inteligence = new
                {
                    Id = h.Inteligence.Id,
                    Initial = h.Inteligence.Initial,
                    Gain = h.Inteligence.Gain,
                },
                MinDamage = h.MinDamage,
                MaxDamage = h.MaxDamage,
                AttackRange = h.AttackRange,
                Armor = h.Armor,
                BaseArmor = h.BaseArmor,
                AttackRate = h.AttackRate,
                MovementSpeed = h.MovementSpeed,
                TurnRate = h.TurnRate,
                BaseHpRegen = h.BaseHpRegen,
                BaseManaRegen = h.BaseManaRegen,
                BaseHp = h.BaseHp,
                BaseMana = h.BaseMana,
                VisionDaytimeRange = h.VisionDaytimeRange,
                VisionNighttimeRange = h.VisionNighttimeRange,
                MagicalResistance = h.MagicalResistance,
                BaseAttackSpeed = h.BaseAttackSpeed,
                AttackAnimationPoint = h.AttackAnimationPoint,
                AttackAcquisitionRange = h.AttackAcquisitionRange,
                Abilities = abilitiesRet

            };

            return ret;
        }
    }
}
