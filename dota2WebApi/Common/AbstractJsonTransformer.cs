using DataAccessLibrary.Data;
using DataModel.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dota2WebApi.Common
{
    public abstract class AbstractJsonTransformerCreator
    {
        public abstract AbstractJsonTransformer FactoryMethod();
        public static AbstractJsonTransformer CreateTransformer()
        {
            return CreateTransformer(AppConfiguration.CurrentDotaPatchVersion);
        }
        public static AbstractJsonTransformer CreateTransformer(PatchVersion pv)
        {
            AbstractJsonTransformer ret;
            switch (pv.Name)
            {
                case "7.28a":
                case "7.28b":
                    ret = new JsonTransformerV7_28Creator().FactoryMethod();
                    break;
                case "7.29a":
                case "7.29b":
                    ret = new JsonTransformerV7_29Creator().FactoryMethod();
                    break;
                default:
                    ret = new JsonTransformerV7_29Creator().FactoryMethod();
                    break;
            }
            return ret;
        }
    }

    public abstract class AbstractJsonTransformer
    {
        public abstract object TransformMatch(Match m);
        public abstract object TransformMatchPlayerHeroItemUpgrade(MatchPlayerHeroItemUpgrade mphiu);
        public abstract object TransformHeroItem(HeroItem hi);
        public abstract object TransformHeroAbilityUpgrade(AbilityUpgrade hau);
        public abstract object TransformBuild(Build build);
        public abstract object TransformMatchPlayer(MatchPlayer mp);
        public abstract object TransformPlayer(Player p);
        public abstract object TransformHeroAbility(HeroAbility ha);
        public abstract object TransformHero(Hero h);
    }
}
