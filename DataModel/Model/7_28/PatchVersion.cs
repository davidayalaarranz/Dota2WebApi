using System;
using System.Collections.Generic;
using System.Text;

namespace DataModel.Model.v7_28
{
    public class PatchVersion
    {
        public PatchVersion() : base() { }

        public long PatchVersionId { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Changes { get; set; }

        public List<HeroItem> HeroItems { get; set; }
    }
}
