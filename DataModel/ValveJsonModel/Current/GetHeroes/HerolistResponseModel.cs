using DataModel.Model;
using System.Collections.Generic;

namespace DataModel.ValveJsonModel.Current.GetHeroes
{
    public class HerolistResponseModel
    {
        public HerolistResponseModelResult result { get; set; }
    }

    public class HerolistResponseModelResult
    {
        public HerolistResponseModelData data { get; set; }
    }

    public class HerolistResponseModelData
    {
        public List<Hero> heroes { get; set; }
    }
}
