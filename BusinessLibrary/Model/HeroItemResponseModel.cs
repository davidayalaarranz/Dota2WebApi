using DataModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLibrary.Model
{
    public class HeroItemResponseModel
    {
        public int nHeroItems { get { return this.HeroItems.Count; } }
        public List<HeroItem> HeroItems { get; set; }
    }
}
