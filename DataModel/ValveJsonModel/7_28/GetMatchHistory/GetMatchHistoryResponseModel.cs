using DataModel.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataModel.ValveJsonModel.v7_28.GetMatchHistory
{
    public class GetMatchHistoryResponseModel
    {
        public GetMatchHistoryResponseModelResult result { get; set; }
    }

    public class GetMatchHistoryResponseModelResult
    {
        public List<Match> matches { get; set; }
        public int numResults { get; set; }
        public int totalResults { get; set; }
        public int resultsRemaining { get; set; }
    }
}
