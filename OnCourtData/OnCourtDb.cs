using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq;
using System.Data;

namespace OnCourtData
{
    public class OnCourtDb : DataContext
    {
        //public Table<Player> TablePlayers;// = dc.GetTable<Player>();
        public Table<Today> TableToday;// = dc.GetTable<Today>();
        public Table<Match> TableMatches;// = dc.GetTable<Match>();
        public Table<StatsPlayerForOneMatch> TableStats;// = dc.GetTable<Match>();
        //public Table<Round> TableRounds;// = dc.GetTable<Round>();
        //public Table<Tournament> TableTournaments;// = dc.GetTable<Tournament>();
        //public Table<Ratings> TableRatings;// = dc.GetTable<Tournament>();

        public OnCourtDb(IDbConnection connection)
            : base(connection)
        {
        }

    }
}
