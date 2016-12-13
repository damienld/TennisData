using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OleDb;

namespace OnCourtData
{
    public static class MsAccessGet
    {
        public static string fConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=..\\Data\\OnCourt.mdb;User ID=;Jet OLEDB:Database Password=qKbE8lWacmYQsZ2;Persist Security Info=true;";
        
        public class TodaysMatches
	    {
		    public string trn;
            public long trn_id;
            public long p1_id;
            public long p2_id;
            public string p1;
            public string p2;
            public string resul;
            public string surf;
            public long surf_id;
        }
        /// <summary>
        /// Get List of matches to played in current tournaments
        /// </summary>
        /// <returns></returns>
        public static List<TodaysMatches> getListTodayMatches()
        {
            List<TodaysMatches> _list = new List<TodaysMatches>();
            string _query = "SELECT trnmt.NAME_T as TRN, player1.NAME_P AS J1, player2.NAME_P AS J2, today.RESULT as Result, surfaces.name_c as Surface "
            + ", today.ID1 as ID1, today.ID2 as ID2 FROM (((( Today_wta as today, LEFT OUTER JOIN Players_wta as player1 "
	        + " ON today.ID1=player1.ID_p ) LEFT OUTER JOIN Players_wta as player2	ON today.ID2=Player2.ID_P ) "
            + " LEFT OUTER JOIN tours_wta as trnmt 	ON trnmt.ID_T=today.TOUR ) LEFT OUTER JOIN courts as surfaces "
	        + " ON trnmt.ID_C_T=surfaces.ID_C ) WHERE (not( player1.NAME_P LIKE '%/%')) AND today.RESULT ='' "
            //+ " AND (player1.NAME_P LIKE :chaine1 OR player2.NAME_P LIKE :chaine2 ) "
            + " ORDER BY TOUR, ROUND, DRAW ASC";
            OleDbDataReader reader = null;
            try
            {
                OleDbConnection con = new OleDbConnection(fConnectionString);
                OleDbCommand cmd = new OleDbCommand(_query, con);
                reader = cmd.ExecuteReader();
            
                //cmd.Parameters.AddWithValue("@id", textBox1.Text);
                while (reader.Read())
                {
                    TodaysMatches _tod = new TodaysMatches();
                    _tod.trn = reader.GetString(1);
                    _tod.p1 = reader.GetString(2);
                    _tod.p2 = reader.GetString(3);
                    _tod.resul = reader.GetString(4);
                    _tod.surf = reader.GetString(5);
                    _tod.p1_id = Convert.ToInt64(reader.GetString(6));
                    _tod.p2_id = Convert.ToInt64(reader.GetString(7));
                    //_tod.trn_id = Convert.ToInt64(reader.GetString(8));
                    _list.Add(_tod);
                }
                return _list;
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }
        }
    }
}
