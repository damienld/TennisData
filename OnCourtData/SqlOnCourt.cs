using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using System.Data.OleDb;
using System.Data;
using System.Windows.Forms;
using System.Diagnostics;

namespace OnCourtData
{
    public class SqlOnCourt
    {
        public static List<MatchDetailsWithOdds> getListMatchesForPlayerBySql(long aIdPlayer, string aConnectionString
            , int aMaxcount, bool aIsIncludeOdds, bool aIsIncludeStats, bool aIsAtp, DateTime? aStartingDate=null)
        {
            string _strClauseSelect_Odds = " , AVG(IIF(odds.K1 is null, Int(-100*odds_inverted.K2)/-100, Int(-100*odds.K1)/-100)) as cote1 " + " , AVG(IIF(odds.K2 is null, Int(-100*odds_inverted.K1)/-100, Int(-100*odds.K2)/-100)) as cote2 ";
            string _strClauseJoin_Odds = " left outer join Odds_atp as odds " +
     " ON odds.ID1_O = games.ID1_G AND odds.ID2_O = games.ID2_G AND odds.id_t_o = games.id_t_g AND odds.id_r_o = games.id_r_g) " +
     " left outer join Odds_atp as odds_inverted " +
     " ON odds_inverted.ID2_O = games.ID1_G AND odds_inverted.ID1_O = games.ID2_G AND odds_inverted.id_t_o = games.id_t_g AND odds_inverted.id_r_o = games.id_r_g  ) ";
            string _strClauseSelect_stats = " , stats.fs_1, stats.fsof_1, stats.aces_1, stats.df_1, stats.ue_1, stats.w1s_1, stats.W1SOF_1, stats.W2S_1, stats.W2SOF_1, stats.WIS_1, stats.BP_1, stats.BPOF_1, stats.NA_1, stats.NAOF_1, stats.TPW_1, stats.FAST_1, stats.A1S_1, stats.A2S_1, " +
                " stats.fs_2, stats.fsof_2, stats.aces_2, stats.df_2, stats.ue_2, stats.w1s_2, stats.W1SOF_2, stats.W2S_2, stats.W2SOF_2, stats.WIS_2, stats.BP_2, stats.BPOF_2, stats.NA_2, stats.NAOF_2, stats.TPW_2, stats.FAST_2, stats.A1S_2, stats.A2S_2, RPW_1, RPW_2, RPWOF_1, RPWOF_2, MT ";
            string _strClauseJoin_Stats = " left outer join stat_atp as stats " +
     " ON stats.ID1 = games.ID1_G AND stats.ID2 = games.ID2_G AND stats.id_t = games.id_t_g AND stats.id_r = games.id_r_g  ) ";

            string _strQuery = "SELECT TOP " + aMaxcount + " player1.NAME_P AS J1, Player2.NAME_P AS J2 " +
                (aIsIncludeOdds ? _strClauseSelect_Odds : "") +
                (aIsIncludeStats ? _strClauseSelect_stats : "") +
    " , (select MIN(pos_r) from ratings_atp where ratings_atp.date_r = trnmt.date_t and id_p_r = games.ID1_g) as RankJ1 " +
    " , (select MIN(pos_r) from ratings_atp where ratings_atp.date_r = trnmt.date_t and id_p_r = games.ID2_g) as RankJ2 " +
     " , trnmt.name_t as Tournoi, trnmt.ID_t as TournoiID " +
  " , games.DATE_G as DateM " +
  " , rounds.name_r as Tour, rounds.ID_r as TourID " +
    " , surfaces.name_c as Surface " +
    " , surfaces.id_c as SurfaceId " +
  " , games.RESULT_G as Resultat " +
    " , trnmt.date_t as DateT " +
    " , games.ID1_g as ID1, games.ID2_g  as ID2 " +
  " , trnmt.rank_t as TournoiRang  " +
" FROM (((((" + (aIsIncludeOdds ? "((" : "") + (aIsIncludeStats ? "(" : "") + " games_atp as games " +
    " LEFT OUTER JOIN Players_atp as player1 " +
        " ON games.ID1_g = player1.ID_p ) " +
    " LEFT OUTER JOIN Players_atp as player2 " +
        " ON games.ID2_g = Player2.ID_P ) " +
     " LEFT OUTER JOIN Rounds " +
        " ON rounds.ID_r = games.ID_r_g ) " +
    " LEFT OUTER JOIN tours_atp as trnmt " +
        " ON trnmt.ID_t = games.ID_t_g ) " +
  " LEFT OUTER JOIN courts as surfaces " +
        " ON trnmt.id_c_t = surfaces.id_c ) " +
     (aIsIncludeOdds ? _strClauseJoin_Odds : "") +
     (aIsIncludeStats ? _strClauseJoin_Stats : "") +
" WHERE ((games.ID1_g =" + aIdPlayer + ") OR (games.ID2_g =" + aIdPlayer + " )) " +
 (aStartingDate.HasValue ? " AND games.DATE_G > #"+aStartingDate.Value.ToString("MM/dd/yyyy")+"# ": "") +
" GROUP BY player1.NAME_P , Player2.NAME_P, trnmt.name_t, trnmt.ID_t, games.DATE_G , rounds.name_r, rounds.ID_r, surfaces.name_c, surfaces.id_c, games.RESULT_G, trnmt.date_t, games.ID1_g , games.ID2_g, trnmt.rank_t " +
(aIsIncludeStats ? _strClauseSelect_stats : "") +
" ORDER BY games.DATE_G DESC, trnmt.date_t DESC ; ";
            if (!aIsAtp)
                _strQuery=_strQuery.Replace("_atp", "_wta");
            DbDataReader rdr = null;
            DbConnection myConnection;
            string _stringConnection = aConnectionString;
            myConnection = new OleDbConnection(_stringConnection);

            try
            {
                OleDbCommand myCommand = new OleDbCommand(_strQuery, (OleDbConnection)myConnection);
                myCommand.CommandType = CommandType.Text;
                myConnection.Open();
                rdr = myCommand.ExecuteReader();

                List<MatchDetailsWithOdds> _collectionMatches = new List<MatchDetailsWithOdds>();
                //_tabOdds.Add(new int[4]);
                while (rdr.Read())
                {
                    MatchDetailsWithOdds _match = new MatchDetailsWithOdds(aIsAtp)
                    {
                        Date = (Convert.ToString(rdr["DateM"]) != "" ? Convert.ToDateTime(rdr["DateM"]) : Convert.ToDateTime(rdr["DateT"])),
                        Player1Name = Convert.ToString(rdr["J1"]),
                        Player2Name = Convert.ToString(rdr["J2"]),
                        Id1 = Convert.ToInt32(rdr["ID1"]),
                        Id2 = Convert.ToInt32(rdr["ID2"]),
                        PositionPlayer1 = (Convert.ToString(rdr["RankJ1"]) != "" ? Convert.ToInt32(rdr["RankJ1"]) : -1),
                        PositionPlayer2 = (Convert.ToString(rdr["RankJ2"]) != "" ? Convert.ToInt32(rdr["RankJ2"]) : -1),
                        CourtName = Convert.ToString(rdr["Surface"]),
                        CourtId = Convert.ToInt16(rdr["SurfaceId"]),
                        RoundName = Convert.ToString(rdr["Tour"]),
                        RoundId = Convert.ToInt32(rdr["TourID"]),
                        TournamentName = Convert.ToString(rdr["Tournoi"]),
                        TournamentId = Convert.ToInt32(rdr["TournoiID"]),
                        TournamentRank = Convert.ToInt32(rdr["TournoiRang"]),
                        ResultString = Convert.ToString(rdr["Resultat"])
                    };
                    if (aIsIncludeOdds)
                    {
                        if (Convert.ToString(rdr["cote1"]) != "")
                            _match.Odds1 =  Math.Round(Convert.ToDouble(rdr["cote1"]), 2);
                        if (Convert.ToString(rdr["cote2"]) != "")
                            _match.Odds2 = Math.Round(Convert.ToDouble(rdr["cote2"]), 2);
                    }
                    if (aIsIncludeStats)
                    {
                        _match.readStats();
                        foreach (StatsForOneMatch _statsForOnePlayer in _match.ListProcessedStats)
                        {
                            if (rdr["MT"].ToString() != "")
                                _statsForOnePlayer.MT = Convert.ToDateTime(rdr["MT"]).ToString("HH:mm:ss");
                            if (rdr["FS_" + _statsForOnePlayer.IndexStatForMatch].ToString() != "")
                                _statsForOnePlayer.FS_1 = Convert.ToInt16(rdr["FS_" + _statsForOnePlayer.IndexStatForMatch]);
                            if (rdr["FSOF_" + _statsForOnePlayer.IndexStatForMatch].ToString() != "")
                                _statsForOnePlayer.FSOF_1 = Convert.ToInt16(rdr["FSOF_" + _statsForOnePlayer.IndexStatForMatch]);
                            if (rdr["ACES_" + _statsForOnePlayer.IndexStatForMatch].ToString() != "")
                                _statsForOnePlayer.ACES_1 = Convert.ToInt16(rdr["ACES_" + _statsForOnePlayer.IndexStatForMatch]);
                            if (rdr["DF_" + _statsForOnePlayer.IndexStatForMatch].ToString() != "")
                                _statsForOnePlayer.DF_1 = Convert.ToInt16(rdr["DF_" + _statsForOnePlayer.IndexStatForMatch]);
                            if (rdr["UE_" + _statsForOnePlayer.IndexStatForMatch].ToString() != "")
                                _statsForOnePlayer.UE_1 = Convert.ToInt16(rdr["UE_" + _statsForOnePlayer.IndexStatForMatch]);
                            if (rdr["W1S_" + _statsForOnePlayer.IndexStatForMatch].ToString() != "")
                                _statsForOnePlayer.W1S_1 = Convert.ToInt16(rdr["W1S_" + _statsForOnePlayer.IndexStatForMatch]);
                            if (rdr["W1SOF_" + _statsForOnePlayer.IndexStatForMatch].ToString() != "")
                                _statsForOnePlayer.W1SOF_1 = Convert.ToInt16(rdr["W1SOF_" + _statsForOnePlayer.IndexStatForMatch]);
                            if (rdr["W2S_" + _statsForOnePlayer.IndexStatForMatch].ToString() != "")
                                _statsForOnePlayer.W2S_1 = Convert.ToInt16(rdr["W2S_" + _statsForOnePlayer.IndexStatForMatch]);
                            if (rdr["W2SOF_" + _statsForOnePlayer.IndexStatForMatch].ToString() != "")
                                _statsForOnePlayer.W2SOF_1 = Convert.ToInt16(rdr["W2SOF_" + _statsForOnePlayer.IndexStatForMatch]);
                            if (rdr["WIS_" + _statsForOnePlayer.IndexStatForMatch].ToString() != "")
                                _statsForOnePlayer.WIS_1 = Convert.ToInt16(rdr["WIS_" + _statsForOnePlayer.IndexStatForMatch]);
                            if (rdr["BP_" + _statsForOnePlayer.IndexStatForMatch].ToString() != "")
                                _statsForOnePlayer.BP_1 = Convert.ToInt16(rdr["BP_" + _statsForOnePlayer.IndexStatForMatch]);
                            if (rdr["BPOF_" + _statsForOnePlayer.IndexStatForMatch].ToString() != "")
                                _statsForOnePlayer.BPOF_1 = Convert.ToInt16(rdr["BPOF_" + _statsForOnePlayer.IndexStatForMatch]);
                            if (rdr["NA_" + _statsForOnePlayer.IndexStatForMatch].ToString() != "")
                                _statsForOnePlayer.NA_1 = Convert.ToInt16(rdr["NA_" + _statsForOnePlayer.IndexStatForMatch]);
                            if (rdr["NAOF_" + _statsForOnePlayer.IndexStatForMatch].ToString() != "")
                                _statsForOnePlayer.NAOF_1 = Convert.ToInt16(rdr["NAOF_" + _statsForOnePlayer.IndexStatForMatch]);
                            if (rdr["TPW_" + _statsForOnePlayer.IndexStatForMatch].ToString() != "")
                                _statsForOnePlayer.TPW_1 = Convert.ToInt16(rdr["TPW_" + _statsForOnePlayer.IndexStatForMatch]);
                            if (rdr["FAST_" + _statsForOnePlayer.IndexStatForMatch].ToString() != "")
                                _statsForOnePlayer.FAST_1 = Convert.ToInt16(rdr["FAST_" + _statsForOnePlayer.IndexStatForMatch]);
                            if (rdr["A1S_" + _statsForOnePlayer.IndexStatForMatch].ToString() != "")
                                _statsForOnePlayer.A1S_1 = Convert.ToInt16(rdr["A1S_" + _statsForOnePlayer.IndexStatForMatch]);
                            if (rdr["A2S_" + _statsForOnePlayer.IndexStatForMatch].ToString() != "")
                                _statsForOnePlayer.A2S_1 = Convert.ToInt16(rdr["A2S_" + _statsForOnePlayer.IndexStatForMatch]);
                            if (rdr["RPW_" + _statsForOnePlayer.IndexStatForMatch].ToString() != "")
                                _statsForOnePlayer.RPW_1 = Convert.ToInt16(rdr["RPW_" + _statsForOnePlayer.IndexStatForMatch]);
                            if (rdr["RPWOF_" + _statsForOnePlayer.IndexStatForMatch].ToString() != "")
                                _statsForOnePlayer.RPWOF_1 = Convert.ToInt16(rdr["RPWOF_" + _statsForOnePlayer.IndexStatForMatch]);


                        }
                    }
                    _collectionMatches.Add(_match);
                }
                return _collectionMatches;
            }
            catch (Exception e)
            {
                MessageBox.Show("Impossible to connect to the database, please contact the support (Error: " + e.Message + " )");
                return null;
            }
            finally
            {
                if (rdr != null)
                {
                    rdr.Close();
                }
                if (myConnection != null)
                {
                    myConnection.Close();
                }

            }

        }
        public static PlayersCollection getListPlayersSql(string aFilter, string aConnectionString
            , int aNbPlayersToLoadUp, bool aIsAtp)
        {
            string _strQuery = "SELECT " + (aNbPlayersToLoadUp>-1?" TOP " + aNbPlayersToLoadUp:"") +
                              " players.NAME_P AS Nom, players.ID_P as ID " +
                            "FROM Players_atp as players " +
                            "WHERE  " +
                            (aFilter!=""? "NAME_P LIKE '%"+ aFilter+"%' AND " : "") +
                            " not( NAME_P LIKE '%/%') " +
                            "ORDER BY  iif(Players.rank_p is not null,Players.rank_p,1000) ";
            if (!aIsAtp)
                _strQuery=_strQuery.Replace("_atp", "_wta");
            DbDataReader rdr = null;
            DbConnection myConnection;
            string _stringConnection = aConnectionString;
            myConnection = new OleDbConnection(_stringConnection);

            try
            {
                OleDbCommand myCommand = new OleDbCommand(_strQuery, (OleDbConnection)myConnection);
                myCommand.CommandType = CommandType.Text;
                myConnection.Open();
                rdr = myCommand.ExecuteReader();
                PlayersCollection _collectionPlayers = new PlayersCollection();
                //_tabOdds.Add(new int[4]);
                while (rdr.Read())
                {
                    Player _match = new Player
                    {
                        Name = Convert.ToString(rdr["Nom"]),
                        Id = Convert.ToInt32(rdr["ID"])
                    };
                    _collectionPlayers.Add(_match);
                }
                return _collectionPlayers;
            }
            catch (Exception e)
            {
                MessageBox.Show("Impossible to connect to the database, please contact the support (Error: " + e.Message + " )");
                return null;
            }
            finally
            {
                if (rdr != null)
                {
                    rdr.Close();
                }
                if (myConnection != null)
                {
                    myConnection.Close();
                }

            }
        }
        public static List<Match> getListTodayMatchesSql(string aConnectionString
            , bool aIsAtp)
        {
            string _strQuery = "SELECT trnmt.NAME_T as Tournoi " +
                                ", player1.NAME_P AS J1 " +
                                ", player2.NAME_P AS J2 " +
                                ", today.RESULT as Result " +
                                ", surfaces.name_c as Surface " +
                                ", surfaces.id_c as SurfaceId " +
                                ", today.ID1 as ID1 " +
                                ", today.ID2 as ID2 " +
                                " , trnmt.rank_t as TournoiRang , trnmt.ID_t as TournoiID  " +
                                " " +
                                "FROM (((( Today_atp as today " +
                                "LEFT OUTER JOIN Players_atp as player1 " +
                                    "ON today.ID1=player1.ID_p ) " +
                                "LEFT OUTER JOIN Players_atp as player2 " +
                                    "ON today.ID2=Player2.ID_P ) " +
                                "LEFT OUTER JOIN tours_atp as trnmt " +
                                    "ON trnmt.ID_T=today.TOUR ) " +
                                "LEFT OUTER JOIN courts as surfaces " +
                                    "ON trnmt.ID_C_T=surfaces.ID_C ) " +
                                " " +
                                "WHERE (not( player1.NAME_P LIKE '%/%')) AND today.RESULT ='' " +
                                " AND not(player1.NAME_P = 'Unknown Player')"+
                                " AND not(player2.NAME_P = 'Unknown Player')" +
                                //"AND (player1.NAME_P LIKE :chaine1 OR player2.NAME_P LIKE :chaine2 ) " +
                                " " +
                                "ORDER BY trnmt.rank_t DESC, TOUR, ROUND, DRAW ASC ";
            if (!aIsAtp)
                _strQuery=_strQuery.Replace("_atp", "_wta");
            DbDataReader rdr = null;
            DbConnection myConnection;

            string _stringConnection = aConnectionString;
            myConnection = new OleDbConnection(_stringConnection);
            
            try
            {
                OleDbCommand myCommand = new OleDbCommand(_strQuery, (OleDbConnection)myConnection);
                myCommand.CommandType = CommandType.Text;
                myConnection.Open();
                rdr = myCommand.ExecuteReader();
                List<Match> _collectionMatches = new List<Match>();
                //_tabOdds.Add(new int[4]);
                while (rdr.Read())
                {
                    Match _match = new Match(aIsAtp)
                    {
                        //Date = (Convert.ToString(rdr["DateM"]) != "" ? Convert.ToDateTime(rdr["DateM"]) : Convert.ToDateTime(rdr["DateM"])),
                        TournamentName = Convert.ToString(rdr["Tournoi"]),
                        TournamentRank = Convert.ToInt16(rdr["TournoiRang"]),
                        TournamentId = Convert.ToInt32(rdr["TournoiID"]),
                        Player1Name = Convert.ToString(rdr["J1"]),
                        Player2Name = Convert.ToString(rdr["J2"]),
                        CourtName = Convert.ToString(rdr["Surface"]),
                        CourtId = Convert.ToInt16(rdr["SurfaceId"]),
                        ResultString = Convert.ToString(rdr["Result"]),
                        Id1 = Convert.ToInt32(rdr["ID1"]),
                        Id2 = Convert.ToInt32(rdr["ID2"])
                    };
                    _collectionMatches.Add(_match);
                }
                return _collectionMatches;
            }
            catch (Exception e)
            {
                MessageBox.Show("Impossible to connect to the database, please contact the support (Error: " + e.Message + " )");
                return null;
            }
            finally
            {
                if (rdr != null)
                {
                    rdr.Close();
                }
                if (myConnection != null)
                {
                    myConnection.Close();
                }

            }

        }

        public static List<MatchDetailsWithOdds> getListMatchupsSql(long aIdPlayer1, long aIdPlayer2, string aConnectionString
            , bool aIsAtp)
        {
            string _strQuery = "SELECT player1.NAME_P AS J1, Player2.NAME_P AS J2 " +
    " , AVG(IIF(odds.K1 is null, Int(-100*odds_inverted.K2)/-100, Int(-100*odds.K1)/-100)) as cote1 " +
    " , AVG(IIF(odds.K2 is null, Int(-100*odds_inverted.K1)/-100, Int(-100*odds.K2)/-100)) as cote2 " +
     " , trnmt.name_t as Tournoi, trnmt.ID_t as TournoiID " +
    " , (select MIN(pos_r) from ratings_atp where ratings_atp.date_r = trnmt.date_t and id_p_r = games.ID1_g) as RankJ1 " +
    " , (select MIN(pos_r) from ratings_atp where ratings_atp.date_r = trnmt.date_t and id_p_r = games.ID2_g) as RankJ2 " +
    " , games.DATE_G as DateM " +
     " , rounds.name_r as Tour, rounds.ID_r as TourID " +
    " , surfaces.name_c as Surface " +
    " , games.RESULT_G as Resultat " +
    " , trnmt.date_t as DateT " +
    " , games.ID1_g as ID1, games.ID2_g  as ID2 " +
  " , trnmt.rank_t as TournoiRang  " +
" FROM ((((((( games_atp as games " +
    " LEFT OUTER JOIN Players_atp as player1 " +
        " ON games.ID1_g = player1.ID_p ) " +
    " LEFT OUTER JOIN Players_atp as player2 " +
        " ON games.ID2_g = Player2.ID_P ) " +
     " LEFT OUTER JOIN Rounds " +
        " ON rounds.ID_r = games.ID_r_g ) " +
    " LEFT OUTER JOIN tours_atp as trnmt " +
        " ON trnmt.ID_t = games.ID_t_g ) " +
  " LEFT OUTER JOIN courts as surfaces " +
        " ON trnmt.id_c_t = surfaces.id_c ) " +
     " left outer join Odds_atp as odds " +
     " ON odds.ID1_O = games.ID1_G AND odds.ID2_O = games.ID2_G AND odds.id_t_o = games.id_t_g AND odds.id_r_o = games.id_r_g) " +
     " left outer join Odds_atp as odds_inverted " +
     " ON odds_inverted.ID2_O = games.ID1_G AND odds_inverted.ID1_O = games.ID2_G AND odds_inverted.id_t_o = games.id_t_g AND odds_inverted.id_r_o = games.id_r_g  ) "+
" WHERE ((games.ID1_g =" + aIdPlayer1 + " and games.ID2_g =" + aIdPlayer2 + ") OR (games.ID1_g =" + aIdPlayer2 + " and games.ID2_g =" + aIdPlayer1 + ")) " +
" GROUP BY player1.NAME_P , Player2.NAME_P, trnmt.name_t, trnmt.ID_t, games.DATE_G , rounds.name_r, rounds.ID_r, surfaces.name_c, games.RESULT_G, trnmt.date_t, games.ID1_g , games.ID2_g, trnmt.rank_t " +
" ORDER BY games.DATE_G DESC, trnmt.date_t DESC ; ";
            if (!aIsAtp)
                _strQuery=_strQuery.Replace("_atp", "_wta");
            DbDataReader rdr = null;
            DbConnection myConnection;

            string _stringConnection = aConnectionString;
            myConnection = new OleDbConnection(_stringConnection);
            
            try
            {
                OleDbCommand myCommand = new OleDbCommand(_strQuery, (OleDbConnection)myConnection);
                myCommand.CommandType = CommandType.Text;
                myConnection.Open();
                rdr = myCommand.ExecuteReader();
                List<MatchDetailsWithOdds> _collectionMatches = new List<MatchDetailsWithOdds>();
                //_tabOdds.Add(new int[4]);
                while (rdr.Read())
                {
                    MatchDetailsWithOdds _match = new MatchDetailsWithOdds(aIsAtp)
                    {
                        Date = (Convert.ToString(rdr["DateM"]) != "" ? Convert.ToDateTime(rdr["DateM"]) : Convert.ToDateTime(rdr["DateT"])),
                        Player1Name = Convert.ToString(rdr["J1"]),
                        Player2Name = Convert.ToString(rdr["J2"]),
                        Id1 = Convert.ToInt32(rdr["ID1"]),
                        Id2 = Convert.ToInt32(rdr["ID2"]),
                        TournamentRank = Convert.ToInt16(rdr["TournoiRang"]),
                        PositionPlayer1 = (Convert.ToString(rdr["RankJ1"]) != "" ? Convert.ToInt32(rdr["RankJ1"]) : -1),
                        PositionPlayer2 = (Convert.ToString(rdr["RankJ2"]) != "" ? Convert.ToInt32(rdr["RankJ2"]) : -1),
                        CourtName = Convert.ToString(rdr["Surface"]),
                        RoundName = Convert.ToString(rdr["Tour"]),
                        RoundId = Convert.ToInt32(rdr["TourID"]),
                        TournamentName = Convert.ToString(rdr["Tournoi"]),
                        TournamentId = Convert.ToInt32(rdr["TournoiID"]),
                        ResultString = Convert.ToString(rdr["Resultat"])
                    };
                    if (Convert.ToString(rdr["cote1"]) != "")
                        _match.Odds1 = Convert.ToDouble(rdr["cote1"]);
                    if (Convert.ToString(rdr["cote2"]) != "")
                        _match.Odds2 = Convert.ToDouble(rdr["cote2"]);
                    _collectionMatches.Add(_match);
                }
                return _collectionMatches;
            }
            catch (Exception e)
            {
                MessageBox.Show("Impossible to connect to the database, please contact the support (Error: " + e.Message + " )");
                return null;
            }
            finally
            {
                if (rdr != null)
                {
                    rdr.Close();
                }
                if (myConnection != null)
                {
                    myConnection.Close();
                }

            }

        }

        /// <summary>
        /// TODO read Stats
        /// </summary>
        /// <param name="aIdPlayer1"></param>
        /// <param name="aIdPlayer2"></param>
        /// <param name="aIdTrn"></param>
        /// <param name="aIdRound"></param>
        /// <param name="aConnectionString"></param>
        /// <param name="aIsAtp"></param>
        /// <returns></returns>
        /*public static List<Stats> getStatsForMatchSql(long aIdPlayer1, long aIdPlayer2, long aIdTrn
            , long aIdRound, string aConnectionString, bool aIsAtp)
        {
            string _strQuery = "SELECT * FROM Stat_atp " +
                " WHERE((ID1 = "+aIdPlayer1+ ") and(ID2 =" + aIdPlayer2 + ") and(ID_T =" + aIdTrn + ") and(ID_R =" + aIdRound + "))";
            if (!aIsAtp)
                _strQuery=_strQuery.Replace("_atp", "_wta");
            DbDataReader rdr = null;
            DbConnection myConnection;

            string _stringConnection = aConnectionString;
            myConnection = new OleDbConnection(_stringConnection);
            
            try
            {
                OleDbCommand myCommand = new OleDbCommand(_strQuery, (OleDbConnection)myConnection);
                myCommand.CommandType = CommandType.Text;
                myConnection.Open();
                rdr = myCommand.ExecuteReader();
                List<Stats> _collectionStats = new List<Stats>();
                //_tabOdds.Add(new int[4]);
                while (rdr.Read())
                {
                    Stats _match = new Stats
                    {

                    };
                    _collectionStats.Add(_match);
                }
                return _collectionStats;
            }
            catch (Exception e)
            {
                MessageBox.Show("Impossible to connect to the database, please contact the support (Error: " + e.Message + " )");
                return null;
            }
            finally
            {
                if (rdr != null)
                {
                    rdr.Close();
                }
                if (myConnection != null)
                {
                    myConnection.Close();
                }

            }

        }
        */
        public static PlayersCollection getListPlayersByCategorySql(string aConnectionString, int aCategoryId, bool aIsAtp)
        {
            string _strQuery = "select name_p from categories_atp as categories " +
                " left join players_atp as players ON players.id_p=categories.id_p "+
                " where categories.cat"+ aCategoryId + " = true ORDER BY Rank_p ASC";
            if (!aIsAtp)
                _strQuery=_strQuery.Replace("_atp", "_wta");
            DbDataReader rdr = null;
            DbConnection myConnection;

            string _stringConnection = aConnectionString;
            myConnection = new OleDbConnection(_stringConnection);
            
            try
            {
                OleDbCommand myCommand = new OleDbCommand(_strQuery, (OleDbConnection)myConnection);
                myCommand.CommandType = CommandType.Text;
                myConnection.Open();
                rdr = myCommand.ExecuteReader();
                PlayersCollection _collectionPlayers = new PlayersCollection();
                //_tabOdds.Add(new int[4]);
                while (rdr.Read())
                {
                    Player _match = new Player
                    {
                        Name = Convert.ToString(rdr["Nom"]),
                        Id = Convert.ToInt32(rdr["ID"])
                    };
                    _collectionPlayers.Add(_match);
                }
                return _collectionPlayers;
            }
            catch (Exception e)
            {
                MessageBox.Show("Impossible to connect to the database, please contact the support (Error: " + e.Message + " )");
                return null;
            }
            finally
            {
                if (rdr != null)
                {
                    rdr.Close();
                }
                if (myConnection != null)
                {
                    myConnection.Close();
                }

            }
        }

        public static List<Tournament> getListTodayTrnSql(string aConnectionString
            , bool aIsAtp, bool aIsIncludeFinishedTrn, int aNbTrnForFinished)
        {
            string _strQuery = "select DISTINCT tours.id_t, tours.name_t, tours.site_t, tours.date_t as dateA, tours.ID_C_T "
            + " from today_atp as today left join tours_atp as tours on today.tour = tours.id_t "
            + " where (tours.rank_t > -1)";
            if (aIsIncludeFinishedTrn)
                _strQuery = "select TOP "+ aNbTrnForFinished + " id_t, name_t, tours.date_t as dateA, tours.ID_C_T "
                + " from tours_atp as tours where (tours.rank_t > 0) AND (tours.date_t < now)"
                + " order by date_t'";
            if (!aIsAtp)
                _strQuery=_strQuery.Replace("_atp", "_wta");
            DbDataReader rdr = null;
            DbConnection myConnection;
            string _stringConnection = aConnectionString;
            myConnection = new OleDbConnection(_stringConnection);

            try
            {
                OleDbCommand myCommand = new OleDbCommand(_strQuery, (OleDbConnection)myConnection);
                myCommand.CommandType = CommandType.Text;
                myConnection.Open();
                rdr = myCommand.ExecuteReader();

                List<Tournament> _collectionTournaments = new List<Tournament>();
                while (rdr.Read())
                {
                    Tournament _trn = new Tournament(aIsAtp)
                    {
                        //Date = (Convert.ToString(rdr["DateM"]) != "" ? Convert.ToDateTime(rdr["DateM"]) : Convert.ToDateTime(rdr["DateM"])),
                        Name = Convert.ToString(rdr["name_t"]),
                        Id = Convert.ToInt32(rdr["id_t"]),
                        Date = Convert.ToDateTime(rdr["DateA"]),
                        CourtId = Convert.ToInt32(rdr["ID_C_T"])
                    };
                    _collectionTournaments.Add(_trn);
                }
                return _collectionTournaments;
            }
            catch (Exception e)
            {
                MessageBox.Show("Impossible to connect to the database, please contact the support (Error: " + e.Message + " )");
                return null;
            }
            finally
            {
                if (rdr != null)
                {
                    rdr.Close();
                }
                if (myConnection != null)
                {
                    myConnection.Close();
                }

            }

        }

        public static List<Match> getListMatchesForTrnNotFinishedSql(string aConnectionString, long aTrnId
            , bool aIsAtp)
        {
            string _strQuery = "SELECT trnmt.NAME_T as TRN, Player1.NAME_P AS J1, Player2.NAME_P AS J2 "
            + ", today.RESULT as Result, surfaces.name_c as Surface, surfaces.id_c as IDSurface "
            + ", today.ID1 as ID1, today.ID2 as ID2, rounds.name_r as Tour, rounds.id_r as IDTour "
            + ", player1.rank_p  as RANK1, player2.rank_p  as RANK2, today.draw as DRAW "
            + "FROM ((((( Today_atp as today "
            + "LEFT OUTER JOIN Players_atp as player1 "
            + "ON today.ID1=player1.ID_p ) "
            + "LEFT OUTER JOIN Players_atp as player2 "
            + "ON today.ID2=Player2.ID_P ) "
            + "LEFT OUTER JOIN tours_atp as trnmt "
            + "ON trnmt.ID_T=today.TOUR ) "
            + "LEFT OUTER JOIN courts as surfaces "
            + "ON trnmt.ID_C_T=surfaces.ID_C ) "
            + "LEFT OUTER JOIN rounds as rounds "
            + "ON today.round=rounds.ID_R ) "
            + "WHERE not( player1.NAME_P LIKE '%/%') "
            + " AND today.TOUR= " + aTrnId
            + " AND not( player1.NAME_P = 'Unknown Player') "
            + " AND not( player2.NAME_P = 'Unknown Player') "
            + " ORDER BY today.round DESC, today.DRAW ASC ";
            if (!aIsAtp)
                _strQuery=_strQuery.Replace("_atp", "_wta");
            DbDataReader rdr = null;
            DbConnection myConnection;

            string _stringConnection = aConnectionString;
            myConnection = new OleDbConnection(_stringConnection);

            try
            {
                OleDbCommand myCommand = new OleDbCommand(_strQuery, (OleDbConnection)myConnection);
                myCommand.CommandType = CommandType.Text;
                myConnection.Open();
                rdr = myCommand.ExecuteReader();
                List<Match> _collectionMatches = new List<Match>();
                //_tabOdds.Add(new int[4]);
                while (rdr.Read())
                {
                    Match _match = new Match(aIsAtp)
                    {
                        //Date = (Convert.ToString(rdr["DateM"]) != "" ? Convert.ToDateTime(rdr["DateM"]) : Convert.ToDateTime(rdr["DateM"])),
                        TournamentName = Convert.ToString(rdr["TRN"]),
                        Player1Name = Convert.ToString(rdr["J1"]),
                        Player2Name = Convert.ToString(rdr["J2"]),
                        CourtId = Convert.ToInt16(rdr["IDSurface"]),
                        CourtName = Convert.ToString(rdr["Surface"]),
                        ResultString = Convert.ToString(rdr["Result"]),
                        PositionPlayer1 = (Convert.ToString(rdr["RANK1"]) != "" ? Convert.ToInt32(rdr["RANK1"]) : -1),
                        PositionPlayer2 = (Convert.ToString(rdr["RANK2"]) != "" ? Convert.ToInt32(rdr["RANK2"]) : -1),
                        Id1 = Convert.ToInt32(rdr["ID1"]),
                        Id2 = Convert.ToInt32(rdr["ID2"]),
                        RoundId = Convert.ToInt16(rdr["IDTour"]),
                        RoundName = Convert.ToString(rdr["Tour"])
                    };
                    _collectionMatches.Add(_match);
                }
                return _collectionMatches;
            }
            catch (Exception e)
            {
                MessageBox.Show("Impossible to connect to the database, please contact the support (Error: " + e.Message + " )");
                return null;
            }
            finally
            {
                if (rdr != null)
                {
                    rdr.Close();
                }
                if (myConnection != null)
                {
                    myConnection.Close();
                }

            }

        }

        public static void postNotesForMatchSql(string aConnectionString, string aNote1P1, string aNote2P1
            , string aNote1P2, string aNote2P2, bool aIsAtp, long aIdTrn, int aIdRound, long aIdP1, long aIdP2)
        {
            //TODO
            if (aNote1P1 == "" && aNote1P2 == "" && aNote2P1 == "" && aNote2P2 == "")
                return;
            string _strQuery = "DELETE FROM MyRankings_atp "
                + " WHERE id_t_m ='" + aIdTrn + "' AND id_r_m = '" + aIdRound
                + "' AND ((id1_m = '"+ aIdP1 + "' AND id2_m = '"+ aIdP2+"' ) OR (id1_m = '" + aIdP2 + "' AND id2_m = '" + aIdP1 + "' ))";
            if (!aIsAtp)
                _strQuery=_strQuery.Replace("_atp", "_wta");
            DbConnection myConnection;
            string _stringConnection = aConnectionString;
            myConnection = new OleDbConnection(_stringConnection);
            try
            {
                OleDbCommand myCommand = new OleDbCommand(_strQuery, (OleDbConnection)myConnection);
                myCommand.CommandType = CommandType.Text;
                myConnection.Open();
                int _nbDel = myCommand.ExecuteNonQuery();
                Trace.WriteLine(_nbDel + " records deleted");
                myConnection.Close();
                /*if (_isFound)
                {//update
                    strQuery = "update MyRankings_atp "
                        + " SET rankestim1_m = @Note1P1, rankestim2_m =@Note1P2, note1 = @Note2P1, note2 = @Note2P2 "
                        + " WHERE id_t_m = @IdTrn AND id_r_m = @IdRound " 
                        + " AND ((id1_m = @IdP1 AND id2_m = @IdP2 ) OR (id1_m = @IdP2 AND id2_m = @IdP1 ))";
                    
                }
                else
                {//insert into*/
                    _strQuery = "insert into MyRankings_atp "
                        + " (rankestim1_m, rankestim2_m, note1, note2, id1_m, id2_m, id_t_m, id_r_m) "
                        + " VALUES (@Note1P1, @Note1P2, @Note2P1, @Note2P2, @IdP1, @IdP2, @IdTrn, @IdRound)";
                //}
                if (!aIsAtp)
                    _strQuery=_strQuery.Replace("_atp", "_wta");
                myCommand = new OleDbCommand(_strQuery, (OleDbConnection)myConnection);
                myCommand.Parameters.AddWithValue("@Note1P1", aNote1P1);
                myCommand.Parameters.AddWithValue("@Note1P2", aNote1P2);
                myCommand.Parameters.AddWithValue("@Note2P1", aNote2P1);
                myCommand.Parameters.AddWithValue("@Note2P2", aNote2P2);
                myCommand.Parameters.AddWithValue("@IdP1", aIdP1);
                myCommand.Parameters.AddWithValue("@IdP2", aIdP2);
                myCommand.Parameters.AddWithValue("@IdTrn", aIdTrn);
                myCommand.Parameters.AddWithValue("@IdRound", aIdRound);
                myCommand.CommandType = CommandType.Text;
                myConnection.Open();
                int _nbRows = myCommand.ExecuteNonQuery();
                Trace.WriteLine(_nbRows + " records added");

            }
            catch (Exception e)
            {
                MessageBox.Show("Impossible to connect to the database, please contact the support (Error: " + e.Message + " )");
            }
        }

        public static void loadNotesForMatchSql(string aConnectionString, out string aNote1P1, out string aNote2P1
            , out string aNote1P2, out string aNote2P2, bool aIsAtp, long aIdTrn, int aIdRound, long aIdP1, long aIdP2)
        {
            aNote1P1 = "";
            aNote2P1 = "";
            aNote1P2 = "";
            aNote2P2 = "";
            string _strQuery = "SELECT * FROM MyRankings_atp "
                + " WHERE id_t_m ='" + aIdTrn + "' AND id_r_m = '" + aIdRound
                + "' AND ((id1_m = '" + aIdP1 + "' AND id2_m = '" + aIdP2 + "' ) OR (id1_m = '" + aIdP2 + "' AND id2_m = '" + aIdP1 + "' ))";
            if (!aIsAtp)
                _strQuery=_strQuery.Replace("_atp", "_wta");
            DbDataReader rdr = null;
            DbConnection myConnection;
            string _stringConnection = aConnectionString;
            myConnection = new OleDbConnection(_stringConnection);
            try
            {
                OleDbCommand myCommand = new OleDbCommand(_strQuery, (OleDbConnection)myConnection);
                myCommand.CommandType = CommandType.Text;
                myConnection.Open();
                rdr = myCommand.ExecuteReader();
                while (rdr.Read())
                {
                    aNote1P1 = Convert.ToString(rdr["rankestim1_m"]);
                    aNote2P1 = Convert.ToString(rdr["note1"]);
                    aNote1P2 = Convert.ToString(rdr["rankestim2_m"]);
                    aNote2P2 = Convert.ToString(rdr["note2"]);

                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Impossible to connect to the database, please contact the support (Error: " + e.Message + " )");
            }
            finally
            {
                if (rdr != null)
                {
                    rdr.Close();
                }
                if (myConnection != null)
                {
                    myConnection.Close();
                }

            }

        }
    }
}
