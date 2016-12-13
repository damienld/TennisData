using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq.Mapping;
using System.Collections.ObjectModel;

namespace OnCourtData
{
    [Table(Name = "Ratings_atp")]
    public class Ratings
    {
        [Column(Name = "DATE_R", IsPrimaryKey = true)]
        public DateTime? DateRanking { get; set; }

        [Column(Name = "ID_P_R", IsPrimaryKey = true)]
        public int PlayerId { get; set; }
        
        [Column(Name = "POS_R", DbType = "smallint")]
        public int Position { get; set; }

        public override string ToString()
        {
            return Position.ToString();
        }
    }

    public class RatingsCollection : ObservableCollection<Ratings>
    {
    }
}
