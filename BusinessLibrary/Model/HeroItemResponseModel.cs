using DataModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLibrary.Model
{
    public class HeroItemResponseModel
    {
        public int nHeroItems { get; set; }
        public IEnumerable<HeroItem> HeroItems { get; set; }
    }
}
