using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DataModel.Model.v7_28
{
    public class Player
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long PlayerId { get; set; }
        public List<MatchPlayer> Matches { get; set; }
    }
}
