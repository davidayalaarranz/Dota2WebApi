using DataModel.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLibrary.Model
{
    public class MatchResponseModel
    {
        public int nMatches { get; set; }
        public IEnumerable<Match> Matches { get; set; }
    }
}
