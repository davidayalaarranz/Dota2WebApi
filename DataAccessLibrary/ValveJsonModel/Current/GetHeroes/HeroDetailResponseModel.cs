using DataModel.Model;
using System.Collections.Generic;

namespace DataAccessLibrary.ValveJsonModel.Current.GetHeroes
{
    public class HeroDetailResponseModel
    {
        public HeroDetailResponseModelResult result { get; set; }
    }

    public class HeroDetailResponseModelResult
    {
        public HeroDetailResponseModelData data { get; set; }
    }

    public class HeroDetailResponseModelData
    {
        public List<Hero> heroes { get; set; }
    }
}
