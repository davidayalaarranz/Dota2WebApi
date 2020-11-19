using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DataModel.Model
{
    public class Match
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long MatchId { get; set; }
        public long MatchSeqNum { get; set; }
        public DateTime StartTime { get; set; }
        public List<MatchPlayer> Players { get; set; }
    }
}
