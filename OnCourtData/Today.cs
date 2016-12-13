using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq.Mapping;
using System.Data.Linq;
using System.Collections.ObjectModel;

namespace OnCourtData
{
    [Table(Name = "Today_atp")]
    public class Today
    {
        
        [Column(Name = "TOUR", IsPrimaryKey = true)]
        public int TournamentId { get; set; }

        [Column(Name = "DATE_GAME")]
        public DateTime? DateGame { get; set; }

        [Column(Name = "ID1", IsPrimaryKey = true)]
        private int player1Id;// { get; set; }     categoryId

        [Column(Name = "ID2", IsPrimaryKey = true)]
        private int player2Id; //public int Id2 { get; set; }

        [Column(Name = "ROUND", IsPrimaryKey = true)]
        public int RoundId { get; set; }

        [Column(Name = "RESULT")]
        public string ResultString { get; set; }
        
        [Column(Name = "DRAW")]
        public int Draw { get; set; }

        public bool getIsBestOf5()
        {
            return true;
        }

        public bool getIsUncompleted()
        {
            return true;
        }
    }

    public class TodaysCollection : ObservableCollection<Today>
    {
    }
}
