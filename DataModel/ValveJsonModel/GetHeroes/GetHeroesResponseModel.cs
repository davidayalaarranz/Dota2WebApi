using DataModel.Model;
using System.Collections.Generic;

namespace DataModel.ValveJsonModel.GetHeroes
{
    public class GetHeroesResponseModel
    {
        public GetHeroesResponseModelResult result { get; set; }
    }

    public class GetHeroesResponseModelResult
    {
        public List<Hero> heroes { get; set; }
        public int status { get; set; }
        public int count { get; set; }
    }
}
