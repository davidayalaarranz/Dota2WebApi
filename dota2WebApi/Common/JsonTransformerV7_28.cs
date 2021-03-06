﻿using DataModel.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dota2WebApi.Common
{
    public class JsonTransformerV7_28Creator : AbstractJsonTransformerCreator
    {
        private JsonTransformerV7_28 _instance;
        public override AbstractJsonTransformer FactoryMethod()
        {
            if (_instance == null)
                _instance = new JsonTransformerV7_28();
            return _instance;
        }
    }

    public class JsonTransformerV7_28 : AbstractJsonTransformer
    {
        public override object TransformMatch(Match m)
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

        public override object TransformMatchPlayerHeroItemUpgrade(MatchPlayerHeroItemUpgrade mphiu)
        {
            if (mphiu == null) return null;
            var ret = new
            {
                StartLevel = mphiu.StartLevel,
                EndLevel = mphiu.EndLevel,
                HeroItem = TransformHeroItem(mphiu.HeroItem),
                HeroItemId = mphiu.HeroItemId,
                HeroItemSlot = mphiu.HeroItemSlot,
                IsSold = mphiu.IsSold
            };
            return ret;
        }

        public override object TransformHeroItem(HeroItem hi)
        {
            if (hi == null) return null;
            var ret = new
            {
                HeroItemId = hi.HeroItemId,
                Name = hi.Name,
                LocalizedName = hi.LocalizedName,
                ShortName = hi.ShortName,
                ImageUrl = hi.ImagePath,
                IsRecipe = hi.IsRecipe,
                IsSecretShop = hi.IsSecretShop,
                IsSideShip = hi.IsSideShop,
                Description = hi.Description,
                Notes = hi.Notes,
                Lore = hi.Lore,
                Attrib = hi.Attrib,
                Qual = hi.Qual,
                Cost = hi.Cost,
                Cooldown = hi.Cooldown,
                ManaCost = hi.ManaCost,
                Created = hi.Created,
                //Components = hi.Components,
                Components = hi.Components.Select(hic => new
                {
                    Quantity = hic.Quantity,
                    ComponentId = hic.Component.HeroItemId,
                    Name = hic.Component.LocalizedName,
                    ImageUrl = hic.Component.ImageUrl,
                }).ToList(),

                BonusStrength = hi.BonusStrength,
                BonusAgility = hi.BonusAgility,
                BonusIntelligence = hi.BonusIntelligence,
                BonusAttackSpeed = hi.BonusAttackSpeed,
                BonusMovementSpeed = hi.BonusMovementSpeed,
                BonusDamage = hi.BonusDamage,
                BonusManaRegeneration = hi.BonusManaRegeneration,
                BonusHPRegeneration = hi.BonusHPRegeneration,
                BonusMana = hi.BonusMana,
                BonusHealth = hi.BonusHealth
            };
            return ret;
        }

        public override object TransformHeroAbilityUpgrade(AbilityUpgrade hau)
        {
            if (hau == null) return null;
            var ret = new
            {
                AbilityId = hau.AbilityId,
                Level = hau.Level,
                Time = hau.Time.TotalSeconds,
            };
            return ret;
        }

        public override object TransformBuild(Build build)
        {
            if (build == null) return null;
            Object[] buRet = new Object[build.HeroUpgrades.Count];
            for (var i = 0; i < build.HeroUpgrades.Count; i++)
            {
                buRet[i] = TransformHeroAbilityUpgrade(build.HeroUpgrades[i]);
            }
            var ret = new
            {
                BuildId = build.BuildId,
                Name = build.Name,
                Color = build.Color,
                Hero = TransformHero(build.Hero),
                HeroUpgrades = buRet
            };
            return ret;
        }

        public override object TransformMatchPlayer(MatchPlayer mp)
        {
            if (mp == null) return null;
            Object[] hauRet = new Object[mp.HeroUpgrades.Count];
            Object[] mphiuRet = new object[mp.HeroItemUpgrades.Count];
            for (var i = 0; i < mp.HeroUpgrades.Count; i++)
            {
                hauRet[i] = TransformHeroAbilityUpgrade(mp.HeroUpgrades[i]);
            }
            for (var i = 0; i < mp.HeroItemUpgrades.Count; i++)
            {
                mphiuRet[i] = TransformMatchPlayerHeroItemUpgrade(mp.HeroItemUpgrades[i]);
            }
            var ret = new
            {
                PlayerId = mp.PlayerId,
                MatchId = mp.MatchId,
                HeroId = mp.HeroId,
                PlayerSlot = mp.PlayerSlot,

                Player = TransformPlayer(mp.Player),
                Hero = TransformHero(mp.Hero),

                HeroUpgrades = hauRet,
                HeroItemUpgrades = mphiuRet,

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

        public override object TransformPlayer(Player p)
        {
            if (p == null) return null;
            var ret = new
            {
                PlayerId = p.PlayerId,
            };
            return ret;
        }

        public override object TransformHeroAbility(HeroAbility ha)
        {
            if (ha == null) return null;

            var ret = new
            {
                AbilityId = ha.AbilityId,
                IsHidden = ha.Ability.IsHidden,
                IsTalent = ha.IsTalent,
                Name = ha.Ability.Name,
                LocalizedName = ha.IsTalent ? ha.Ability.LocalizedName.Replace("{s:value}", ha.Ability.Value) : ha.Ability.LocalizedName,
                Affects = ha.Ability.Affects,
                Description = ha.Ability.Description,
                Notes = ha.Ability.Notes,
                Attrib = ha.Ability.Attrib,
                Cmb = ha.Ability.Cmb,
                Lore = ha.Ability.Lore,
                Hurl = ha.Ability.Hurl,
                ImageUrl = ha.IsTalent ? "/images/dota_2_talent_tree.png" : ha.Ability.ImageUrl,
                CastRangeBuffer = ha.Ability.CastRangeBuffer,
                CastRange = ha.Ability.CastRange,
                CastPoint = ha.Ability.CastPoint,
                ChannelTime = ha.Ability.ChannelTime,
                Cooldown = ha.Ability.Cooldown,
                Duration = ha.Ability.Duration,
                Damage = ha.Ability.Damage,
                ManaCost = ha.Ability.ManaCost,
                HasScepterUpgrade = ha.Ability.HasScepterUpgrade,
                IsGrantedByScepter = ha.Ability.IsGrantedByScepter,
                MaxLevel = ha.Ability.MaxLevel,
                BonusStrength = ha.Ability.BonusStrength,
                BonusAgility = ha.Ability.BonusAgility,
                BonusIntelligence = ha.Ability.BonusIntelligence
            };
            return ret;
        }

        public override object TransformHero(Hero h)
        {
            if (h == null) return null;
            
            Object[] abilitiesRet = new Object[h.HeroAbilities.Count];
            for (var i = 0; i < h.HeroAbilities.Count; i++)
            {
                abilitiesRet[i] = TransformHeroAbility(h.HeroAbilities[i]);
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
                Intelligence = new
                {
                    Id = h.Intelligence.Id,
                    Initial = h.Intelligence.Initial,
                    Gain = h.Intelligence.Gain,
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
