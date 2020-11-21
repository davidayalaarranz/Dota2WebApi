using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DataModel.Model
{
    public class PickBan
    {
        public int MatchId { get; set; }
        public int Team { get; set; } // 0 = radiant, 1 = dire
        public int Order { get; set; }
        public Hero Hero { get; set; }
    }
    public class Pick : PickBan { }
    public class Ban : PickBan { }

    public class Match
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long MatchId { get; set; }
        public long MatchSeqNum { get; set; }
        public DateTime StartTime { get; set; }
        public List<MatchPlayer> Players { get; set; }

        public bool RadiantWin { get; set; }
        public int Duration { get; set; } // duración de la partida medida en segundos
        public int PreGameDuration { get; set; } // duración del previo de la partida medida en segundos
        public int TowerStatusRadiant { get; set; }
        public int TowerStatusDire { get; set;}
        public int BarracksStatusRadiant { get; set; }
        public int BarracksStatusDire { get; set; }
        public int FirstBloodTime { get; set; }
        public int GameMode { get; set; }
        public int RadiantScore { get; set; }
        public int DireScore { get; set; }
        public List<Pick> Picks { get; set; }
        public List<Ban> Bans { get; set; }

    }
}
