using DataModel.Model;
using System.Collections.Generic;

namespace BusinessLibrary.Model
{
    public class HeroResponseModel
    {
        public int nHeroes { get; set; }
        public IEnumerable<Hero> Heroes { get; set; }
    }
}
