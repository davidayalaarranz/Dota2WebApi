using DataModel.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLibrary.Model
{
    public class BuildResponseModel
    {
        public int nBuilds { get; set; }
        public IEnumerable<Build> Builds { get; set; }
    }
}
