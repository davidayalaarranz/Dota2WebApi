using System;
using System.Collections.Generic;
using System.Text;

namespace DataModel.Model
{
    public class MatchPlayer
    {
        public long PlayerId { get; set; }
        public Player Player { get; set; }
        public long MatchId { get; set; }
        public Match Match { get; set; }
        public int PlayerSlot { get; set; }
        public long HeroId { get; set; }
        public Hero Hero { get; set; }
    }
}
