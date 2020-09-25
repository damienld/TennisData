using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq.Mapping;
using System.Data.Linq;
using System.Collections.ObjectModel;

namespace OnCourtData
{
    [Table(Name = "Players_atp")]
    public class Player
    {
        //private EntitySet<Match> link_Match = new EntitySet<Match>();
        //public List<Match> ListMatches {get; set;}
        [System.ComponentModel.Browsable(false)]
        public string Note { get; set; }

        [System.ComponentModel.Browsable(false)]
        [Column(Name = "ID_P", IsPrimaryKey = true, IsDbGenerated = true)]
        public long Id { get; set; }

        [Column(Name = "NAME_P")]
        public string Name { get; set; }

        [Column(Name = "RANK_P", DbType = "smallint")]
        public int? Rank { 
            get; 
            set; }

        [Column(Name = "DATE_P")]
        public DateTime? DateBorn { get; set; }
        [System.ComponentModel.Browsable(false)]
        public List<int> ListCategoriesId { get; set; }
        //public List<CategoriesPlayer> ListCategories { get; set; }

        /*[Association(Storage = "link_Match", OtherKey = "Id1, Id2, TournamentId, RoundId"
            , ThisKey = "Id")]
        public ICollection<Match> MatchesForPlayer 
        {
            get { return this.link_Match; }
            set { this.link_Match.Assign(value); } 
        }*/

        /*private EntitySet<Ratings> link_Ratings = new EntitySet<Ratings>();
        [Association(Storage = "link_Ratings", OtherKey = "PlayerId"
            , ThisKey = "Id")]
        public ICollection<Ratings> RatingsForPlayer 
        {
            get { return this.link_Ratings; }
            set { this.link_Ratings.Assign(value); } 
        }*/

        public override string ToString()
        {
            return this.Name;
        }
    }

    public class PlayersCollection : ObservableCollection<Player>
    {
    }
}
