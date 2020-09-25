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
        public static int getRankPlayer_Sql(long aIdPlayer, string aConnectionString, bool aIsAtp, DateTime aMaxDate)
        {
            DateTime _dateStartSearch = aMaxDate.AddDays(-30);
            string _strQuery = "select TOP 1 pos_r from ratings_atp where ratings_atp.date_r <= #" 
                + aMaxDate.Month +"/"+ aMaxDate.Day+"/" +aMaxDate.Year + "# and ratings_atp.date_r >= #" 
                + _dateStartSearch.Month + "/" + _dateStartSearch.Day + "/" + _dateStartSearch.Year + "# and id_p_r = "+ aIdPlayer+" order by date_r desc";
            if (!aIsAtp)
                _strQuery = _strQuery.Replace("_atp", "_wta");
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
                    return Convert.ToInt32(rdr["pos_r"]);
                }
                return -1;
            }
            catch (Exception e)
            {
                MessageBox.Show("Impossible to connect to the database, please contact the support (Error: " + e.Message + " )");
                return -1;
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
        public static List<StatsForOneMatch> getStatsForMatchSql(long aIdPlayer1, long aIdPlayer2, long aIdTrn
            , long aIdRound, string aConnectionString, bool aIsAtp)
        {
            string _strQuery = "SELECT * FROM Stat_atp " +
                " WHERE((ID1 = " + aIdPlayer1 + ") and(ID2 =" + aIdPlayer2 + ") and(ID_T =" + aIdTrn + ") and(ID_R =" + aIdRound + "))";
            if (!aIsAtp)
                _strQuery = _strQuery.Replace("_atp", "_wta");
            DbDataReader rdr = null;
            DbConnection myConnection;

            string _stringConnection = aConnectionString;
            myConnection = new OleDbConnection(_stringConnection);

            try
            {
                List<StatsForOneMatch> _ListProcessedStats = new List<StatsForOneMatch>();
                _ListProcessedStats.Add(new StatsForOneMatch(1));
                _ListProcessedStats.Add(new StatsForOneMatch(2));

                OleDbCommand myCommand = new OleDbCommand(_strQuery, (OleDbConnection)myConnection);
                myCommand.CommandType = CommandType.Text;
                myConnection.Open();
                rdr = myCommand.ExecuteReader();

                while (rdr.Read())
                {
                    foreach (StatsForOneMatch _statsForOnePlayer in _ListProcessedStats)
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
                return _ListProcessedStats;
            }
            catch (Exception e)
            {
                MessageBox.Show("Impossible to connect to the database, please contact the support (Error: " + e.Message + " )");
                List<StatsForOneMatch> _ListProcessedStats = new List<StatsForOneMatch>();
                _ListProcessedStats.Add(new StatsForOneMatch(1));
                _ListProcessedStats.Add(new StatsForOneMatch(2));
                return _ListProcessedStats;
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

        public static List<MatchDetailsWithOdds> getListMatchesForPlayerBySql(long aIdPlayer, List<long> aIdTrn, string aConnectionString
            , int aMaxcount, bool aIsIncludeOdds, bool aIsIncludeStats, bool aIsIncludeCategories
            , bool aIsAtp, DateTime? aStartingDate=null, DateTime? aEndingDate = null, bool aIsExtractRankPlayer=true
            , bool aIsIncludeSeed = false)
        {
            string _listTrn = "";
            if (aIdTrn != null)
                _listTrn = string.Join(",", aIdTrn.ToArray());
            bool _isPinnyOnly = false;
            string _sqlPinnyOnly = (_isPinnyOnly ? " and odds.id_b_o=2 " : "");
            string _strClauseSelect_Odds = " , AVG(IIF(odds.K1 is null, Int(-100*odds_inverted.K2)/-100, Int(-100*odds.K1)/-100)) as cote1 " + " , AVG(IIF(odds.K2 is null, Int(-100*odds_inverted.K1)/-100, Int(-100*odds.K2)/-100)) as cote2 ";
            string _strClauseJoin_Odds = " left outer join Odds_atp as odds " +
     " ON odds.ID1_O = games.ID1_G AND odds.ID2_O = games.ID2_G AND odds.id_t_o = games.id_t_g AND odds.id_r_o = games.id_r_g ) " +
     " left outer join Odds_atp as odds_inverted " +
     " ON odds_inverted.ID2_O = games.ID1_G AND odds_inverted.ID1_O = games.ID2_G AND odds_inverted.id_t_o = games.id_t_g AND odds_inverted.id_r_o = games.id_r_g  ) ";
            string _strClauseSelect_stats = " , stats.fs_1, stats.fsof_1, stats.aces_1, stats.df_1, stats.ue_1, stats.w1s_1, stats.W1SOF_1, stats.W2S_1, stats.W2SOF_1, stats.WIS_1, stats.BP_1, stats.BPOF_1, stats.NA_1, stats.NAOF_1, stats.TPW_1, stats.FAST_1, stats.A1S_1, stats.A2S_1, " +
                " stats.fs_2, stats.fsof_2, stats.aces_2, stats.df_2, stats.ue_2, stats.w1s_2, stats.W1SOF_2, stats.W2S_2, stats.W2SOF_2, stats.WIS_2, stats.BP_2, stats.BPOF_2, stats.NA_2, stats.NAOF_2, stats.TPW_2, stats.FAST_2, stats.A1S_2, stats.A2S_2, RPW_1, RPW_2, RPWOF_1, RPWOF_2, MT ";
            string _strClauseJoin_Stats = " left outer join stat_atp as stats " +
     " ON stats.ID1 = games.ID1_G AND stats.ID2 = games.ID2_G AND stats.id_t = games.id_t_g AND stats.id_r = games.id_r_g  ) ";
            string _strClauseSelect_Seed = " , seedP1.seeding as seedP1 , seedP2.seeding as seedP2 ";
            string _strClauseGroupby_Seed = " , seedP1.seeding, seedP2.seeding ";
            string _strClauseJoin_Seed = " left outer join seed_atp as seedP1 " +
     " ON seedP1.ID_P_S = games.ID1_G AND seedP1.ID_T_S = games.id_t_g ) " +
     " left outer join seed_atp as seedP2 " +
     " ON seedP2.ID_P_S = games.ID2_G AND seedP2.ID_T_S = games.id_t_g ) ";
            string _strClauseSelect_Categories = " , categoriesP1.CAT1 as P1C1, categoriesP1.CAT2 as P1C2, categoriesP1.CAT3 as P1C3, categoriesP1.CAT4 as P1C4, categoriesP1.CAT5 as P1C5, " +
                "  categoriesP1.CAT6 as P1C6, categoriesP1.CAT7 as P1C7, categoriesP1.CAT8 as P1C8, categoriesP1.CAT9 as P1C9 " 
                + " , categoriesP2.CAT1 as P2C1, categoriesP2.CAT2 as P2C2, categoriesP2.CAT3 as P2C3, categoriesP2.CAT4 as P2C4, categoriesP2.CAT5 as P2C5, " +
                "  categoriesP2.CAT6 as P2C6, categoriesP2.CAT7 as P2C7, categoriesP2.CAT8 as P2C8, categoriesP2.CAT9 as P2C9 ";
            string _strClauseGroupby_Categories = " , categoriesP1.CAT1, categoriesP1.CAT2, categoriesP1.CAT3, categoriesP1.CAT4, categoriesP1.CAT5, " +
                "  categoriesP1.CAT6, categoriesP1.CAT7 , categoriesP1.CAT8 , categoriesP1.CAT9  "
                + " , categoriesP2.CAT1, categoriesP2.CAT2, categoriesP2.CAT3, categoriesP2.CAT4, categoriesP2.CAT5, " +
                "  categoriesP2.CAT6, categoriesP2.CAT7 , categoriesP2.CAT8 , categoriesP2.CAT9  ";
            string _strClauseJoin_Categories = " LEFT OUTER JOIN categories_atp as categoriesP1 " +
        " ON categoriesP1.id_p = games.ID1_g ) " +
        " LEFT OUTER JOIN categories_atp as categoriesP2 " +
        " ON categoriesP2.id_p = games.ID2_g ) ";

            string _strQuery = "SELECT TOP " + aMaxcount + " player1.NAME_P AS J1, Player2.NAME_P AS J2 " +
                (aIsIncludeOdds ? _strClauseSelect_Odds : "") +
                (aIsIncludeStats ? _strClauseSelect_stats : "") +
                (aIsIncludeCategories ? _strClauseSelect_Categories : "") +
                (aIsIncludeSeed ? _strClauseSelect_Seed : "") +
    " , (select MIN(pos_r) from ratings_atp where ratings_atp.date_r = trnmt.date_t and id_p_r = games.ID1_g) as RankJ1 " +
    " , (select MIN(pos_r) from ratings_atp where ratings_atp.date_r = trnmt.date_t and id_p_r = games.ID2_g) as RankJ2 " +
     " , trnmt.name_t as Tournoi, trnmt.ID_t as TournoiID " +
  " , games.DATE_G as DateM " +
  " , rounds.name_r as Tour, rounds.ID_r as TourID " +
    " , surfaces.name_c as Surface " +
    " , surfaces.id_c as SurfaceId " +
  " , games.RESULT_G as Resultat " +
    " , trnmt.date_t as DateT " +
    " , trnmt.site_t as TournoiSite " +
    " , games.ID1_g as ID1, games.ID2_g  as ID2 " +
  " , trnmt.rank_t as TournoiRang  " +
" FROM (((((" + (aIsIncludeOdds ? "((" : "") + (aIsIncludeStats ? "(" : "") + (aIsIncludeSeed ? "((" : "")
+ (aIsIncludeCategories ? "((" : "") + " games_atp as games " +
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
        (aIsIncludeCategories ? _strClauseJoin_Categories : "") +
     (aIsIncludeOdds ? _strClauseJoin_Odds : "") +
     (aIsIncludeStats ? _strClauseJoin_Stats : "") +
     (aIsIncludeSeed ? _strClauseJoin_Seed : "") +
" WHERE " + (aIdPlayer==-1?"true ":"((games.ID1_g =" + aIdPlayer + ") OR (games.ID2_g =" + aIdPlayer + " )) ") +
 (aStartingDate.HasValue ? " AND games.DATE_G > #"+aStartingDate.Value.ToString("MM/dd/yyyy").Replace('.','/')+"# ": "") 
 + (aEndingDate.HasValue ? " AND games.DATE_G < #" + aEndingDate.Value.ToString("MM/dd/yyyy").Replace('.', '/') + "# " : "")
 + (aIdTrn != null ? " AND trnmt.ID_t IN (" + _listTrn +") " :"")
 + _sqlPinnyOnly +
" GROUP BY player1.NAME_P , Player2.NAME_P, trnmt.name_t, trnmt.ID_t, games.DATE_G , rounds.name_r, rounds.ID_r, surfaces.name_c, surfaces.id_c, games.RESULT_G, trnmt.date_t, trnmt.site_t, games.ID1_g , games.ID2_g, trnmt.rank_t " +
(aIsIncludeStats ? _strClauseSelect_stats : "") + (aIsIncludeSeed ? _strClauseGroupby_Seed : "") + (aIsIncludeCategories ? _strClauseGroupby_Categories : "") +
" ORDER BY games.DATE_G DESC, trnmt.date_t DESC, rounds.ID_r DESC ; ";
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
                        CourtId = Convert.ToInt16(rdr["SurfaceId"]), //TODO courtId might be missing sometimes
                        RoundName = Convert.ToString(rdr["Tour"]),
                        RoundId = Convert.ToInt32(rdr["TourID"]),
                        TournamentName = Convert.ToString(rdr["Tournoi"]),
                        TournamentId = Convert.ToInt32(rdr["TournoiID"]),
                        TournamentRank = Convert.ToInt32(rdr["TournoiRang"]),
                        TournamentSite = Convert.ToString(rdr["TournoiSite"]).Trim(),
                        ResultString = Convert.ToString(rdr["Resultat"])
                    };
                    try
                    {
                        if (_match.PositionPlayer1 == -1 && aIsExtractRankPlayer)
                        {
                            _match.PositionPlayer1 = getRankPlayer_Sql(_match.Id1, aConnectionString, aIsAtp, _match.Date.Value);
                        }
                        if (_match.PositionPlayer2 == -1 && aIsExtractRankPlayer)
                        {
                            _match.PositionPlayer2 = getRankPlayer_Sql(_match.Id2, aConnectionString, aIsAtp, _match.Date.Value);
                        }
                    }
                    catch { }

                    if (aIsIncludeOdds)
                    {
                        if (Convert.ToString(rdr["cote1"]) != "" && Convert.ToString(rdr["cote2"]) != "")
                        {
                            double o1 = Math.Round(Convert.ToDouble(rdr["cote1"]), 2);
                            double o2 = Math.Round(Convert.ToDouble(rdr["cote2"]), 2);
                            double margin = 1 / o1 + 1 / o2;
                            if (margin > 1.04)
                            {
                                //Console.WriteLine(o1 + " ; " + o2);
                                if (o1 > 1.2)
                                    o1 = o1 * margin/1.04;
                                if (o2 > 1.2)
                                    o2 = o2 * margin/1.04;
                                //Console.WriteLine(o1 + " ; " + o2);

                            }
                            if (Convert.ToString(rdr["cote1"]) != "")
                                _match.Odds1 = Math.Round(o1, 2);
                            if (Convert.ToString(rdr["cote2"]) != "")
                                _match.Odds2 = Math.Round(o2, 2);
                        }
                        
                    }
                    if (aIsIncludeSeed)
                    {
                        _match.Player1Info = Convert.ToString(rdr["seedP1"]).Trim();
                        _match.Player2Info = Convert.ToString(rdr["seedP2"]).Trim();
                    }
                    if (aIsIncludeCategories)
                    {
                        _match.ListCategoriesIdP1 = new List<int>();
                        _match.ListCategoriesIdP2 = new List<int>();
                        for (int j = 1; j <= 2; j++)
                        {
                            for (int i = 1; i <= 9; i++)
                            {
                                string _nameField = "P" + j + "C" + i;
                                if (Convert.ToBoolean(rdr[_nameField]))
                                    if (j == 1)
                                        _match.ListCategoriesIdP1.Add(i);
                                    else
                                        _match.ListCategoriesIdP2.Add(i);
                            } 
                        }
                    }
                    if (aIsIncludeStats)
                    {
                        _match.createStatsObjectForPlayers();
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
            , int aNbTopPlayersToLoadUp, bool aIsAtp)
        {
            string _strClauseSelect_Categories = " , categoriesP1.CAT1 as P1C1, categoriesP1.CAT2 as P1C2, categoriesP1.CAT3 as P1C3, categoriesP1.CAT4 as P1C4, categoriesP1.CAT5 as P1C5, " +
                "  categoriesP1.CAT6 as P1C6, categoriesP1.CAT7 as P1C7, categoriesP1.CAT8 as P1C8, categoriesP1.CAT9 as P1C9 ";
            string _strClauseJoin_Categories = " LEFT OUTER JOIN categories_atp as categoriesP1 " +
        " ON categoriesP1.id_p = players.ID_P  ";

            string _strQuery = "SELECT " + (aNbTopPlayersToLoadUp>-1?" TOP " + aNbTopPlayersToLoadUp:"") +
                              " players.NAME_P AS Nom, players.ID_P as ID, Players.rank_p as Rank" +
                              _strClauseSelect_Categories +
                            "FROM Players_atp as players " +
                            _strClauseJoin_Categories +
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
                    Player _player = new Player
                    {
                        Name = Convert.ToString(rdr["Nom"]),
                        Id = Convert.ToInt32(rdr["ID"])
                    };
                    int colIndex = rdr.GetOrdinal("Rank");
                    _player.Rank = -1;
                    if (!rdr.IsDBNull(colIndex))
                        _player.Rank = Convert.ToInt32(rdr["Rank"]);
                    _player.ListCategoriesId = new List<int>();
                        for (int i = 1; i <= 9; i++)
                        {
                            string _nameField = "P1C" + i;
                            if (Convert.ToBoolean(rdr[_nameField]))
                                _player.ListCategoriesId.Add(i);
                        }
                    _collectionPlayers.Add(_player);
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

        public static Player getPlayersDetailFromIdSql(long aIdPlayer, string aConnectionString, bool aIsAtp)
        {
            string _strClauseSelect_Categories = " , categoriesP1.CAT1 as P1C1, categoriesP1.CAT2 as P1C2, categoriesP1.CAT3 as P1C3, categoriesP1.CAT4 as P1C4, categoriesP1.CAT5 as P1C5, " +
                "  categoriesP1.CAT6 as P1C6, categoriesP1.CAT7 as P1C7, categoriesP1.CAT8 as P1C8, categoriesP1.CAT9 as P1C9 ";
            string _strClauseJoin_Categories = " LEFT OUTER JOIN categories_atp as categoriesP1 ON categoriesP1.id_p = players.ID_P  ";

            string _strQuery = "SELECT  players.NAME_P AS Nom, players.ID_P as ID, Players.rank_p as Rank " +
                              _strClauseSelect_Categories +
                            "FROM Players_atp as players " +
                            _strClauseJoin_Categories +
                            "WHERE players.ID_P = " + aIdPlayer + "  ";
            if (!aIsAtp)
                _strQuery = _strQuery.Replace("_atp", "_wta");
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
                Player _player = null;
                while (rdr.Read())
                {
                    _player = new Player
                    {
                        Name = Convert.ToString(rdr["Nom"]),
                        Id = Convert.ToInt32(rdr["ID"])
                    };
                    int colIndex = rdr.GetOrdinal("Rank");
                    _player.Rank = -1;
                    if (!rdr.IsDBNull(colIndex))
                        _player.Rank = Convert.ToInt32(rdr["Rank"]);

                    _player.ListCategoriesId = new List<int>();
                    for (int i = 1; i <= 9; i++)
                    {
                        string _nameField = "P1C" + i;
                        if (Convert.ToBoolean(rdr[_nameField]))
                            _player.ListCategoriesId.Add(i);
                    }
                }
                return _player;
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

        public static List<MatchDetailsWithOdds> getListH2hSql(long aIdPlayer1, long aIdPlayer2, string aConnectionString
            , bool aIsAtp, bool aIsIncludeStats)
        {
            string _strClauseSelect_stats = " , stats.fs_1, stats.fsof_1, stats.aces_1, stats.df_1, stats.ue_1, stats.w1s_1, stats.W1SOF_1, stats.W2S_1, stats.W2SOF_1, stats.WIS_1, stats.BP_1, stats.BPOF_1, stats.NA_1, stats.NAOF_1, stats.TPW_1, stats.FAST_1, stats.A1S_1, stats.A2S_1, " +
                " stats.fs_2, stats.fsof_2, stats.aces_2, stats.df_2, stats.ue_2, stats.w1s_2, stats.W1SOF_2, stats.W2S_2, stats.W2SOF_2, stats.WIS_2, stats.BP_2, stats.BPOF_2, stats.NA_2, stats.NAOF_2, stats.TPW_2, stats.FAST_2, stats.A1S_2, stats.A2S_2, RPW_1, RPW_2, RPWOF_1, RPWOF_2, MT ";
            string _strClauseJoin_Stats = " left outer join stat_atp as stats " +
     " ON stats.ID1 = games.ID1_G AND stats.ID2 = games.ID2_G AND stats.id_t = games.id_t_g AND stats.id_r = games.id_r_g  ) ";

            string _strQuery = "SELECT player1.NAME_P AS J1, Player2.NAME_P AS J2 " +
                (aIsIncludeStats ? _strClauseSelect_stats : "") +
    " , AVG(IIF(odds.K1 is null, Int(-100*odds_inverted.K2)/-100, Int(-100*odds.K1)/-100)) as cote1 " +
    " , AVG(IIF(odds.K2 is null, Int(-100*odds_inverted.K1)/-100, Int(-100*odds.K2)/-100)) as cote2 " +
     " , trnmt.name_t as Tournoi, trnmt.ID_t as TournoiID " +
    " , (select MIN(pos_r) from ratings_atp where ratings_atp.date_r = trnmt.date_t and id_p_r = games.ID1_g) as RankJ1 " +
    " , (select MIN(pos_r) from ratings_atp where ratings_atp.date_r = trnmt.date_t and id_p_r = games.ID2_g) as RankJ2 " +
    " , games.DATE_G as DateM " +
     " , rounds.name_r as Tour, rounds.ID_r as TourID " +
    " , surfaces.name_c as Surface " +
    " , surfaces.id_c as SurfaceId " +
    " , games.RESULT_G as Resultat " +
    " , trnmt.date_t as DateT " +
    " , games.ID1_g as ID1, games.ID2_g  as ID2 " +
  " , trnmt.rank_t as TournoiRang  " +
" FROM ((((((( " + (aIsIncludeStats ? "(" : "") + " games_atp as games " +
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
     " ON odds_inverted.ID2_O = games.ID1_G AND odds_inverted.ID1_O = games.ID2_G AND odds_inverted.id_t_o = games.id_t_g AND odds_inverted.id_r_o = games.id_r_g  ) " +
     (aIsIncludeStats ? _strClauseJoin_Stats : "") +
" WHERE ((games.ID1_g =" + aIdPlayer1 + " and games.ID2_g =" + aIdPlayer2 + ") OR (games.ID1_g =" + aIdPlayer2 + " and games.ID2_g =" + aIdPlayer1 + ")) " +
" GROUP BY player1.NAME_P , Player2.NAME_P, trnmt.name_t, trnmt.ID_t, games.DATE_G , rounds.name_r, rounds.ID_r, surfaces.name_c, surfaces.id_c, games.RESULT_G, trnmt.date_t, games.ID1_g , games.ID2_g, trnmt.rank_t " +
(aIsIncludeStats ? _strClauseSelect_stats : "") +
" ORDER BY games.DATE_G DESC, trnmt.date_t DESC, rounds.ID_r DESC ; ";
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
                        CourtId = Convert.ToInt16(rdr["SurfaceId"]),
                        RoundName = Convert.ToString(rdr["Tour"]),
                        RoundId = Convert.ToInt32(rdr["TourID"]),
                        TournamentName = Convert.ToString(rdr["Tournoi"]),
                        TournamentId = Convert.ToInt32(rdr["TournoiID"]),
                        ResultString = Convert.ToString(rdr["Resultat"])
                    };
                    try
                    {
                        if (_match.PositionPlayer1 == -1)
                        {
                            _match.PositionPlayer1 = getRankPlayer_Sql(_match.Id1, aConnectionString, aIsAtp, _match.Date.Value);
                        }
                        if (_match.PositionPlayer2 == -1)
                        {
                            _match.PositionPlayer2 = getRankPlayer_Sql(_match.Id2, aConnectionString, aIsAtp, _match.Date.Value);
                        }
                    }
                    catch { }

                    if (aIsIncludeStats)
                    {
                        _match.createStatsObjectForPlayers();
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
                    if (Convert.ToString(rdr["cote1"]) != "")
                        _match.Odds1 = Math.Round(Convert.ToDouble(rdr["cote1"]), 2);
                    if (Convert.ToString(rdr["cote2"]) != "")
                        _match.Odds2 = Math.Round(Convert.ToDouble(rdr["cote2"]), 2);
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
            string _strQuery = "select DISTINCT tours.id_t, tours.name_t, tours.site_t, tours.date_t as dateA, tours.ID_C_T, tours.link_t, tours.rank_t, tours.site_t "
            + " from today_atp as today left join tours_atp as tours on today.tour = tours.id_t "
            + " where (tours.rank_t > -1)";
            if (aIsIncludeFinishedTrn)
                _strQuery = "select TOP "+ aNbTrnForFinished + " id_t, name_t, tours.date_t as dateA, tours.ID_C_T, tours.link_t, tours.rank_t, tours.site_t "
                + " from tours_atp as tours where (tours.rank_t > 0) AND (tours.date_t < now)"
                + " order by date_t DESC";
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
                        Id = Convert.ToInt64(rdr["id_t"]),
                        Date = Convert.ToDateTime(rdr["DateA"]),
                        CourtId = Convert.ToInt32(rdr["ID_C_T"]),
                        Rank = Convert.ToInt16(rdr["rank_t"]),
                        TournamentSite = Convert.ToString(rdr["site_t"]).Trim()
                    };
                    _trn.IdPreviousEdition = (Convert.ToString(rdr["link_t"]) != "" ? Convert.ToInt64(rdr["link_t"]) : -1);
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

        public static List<Tournament> getTrnPastEditions(string aConnectionString, bool aIsAtp, long aTrnId
            , int aNbEditions)
        {
            List<Tournament> _list = new List<Tournament>();
            long _idTrn = aTrnId;
            for (int i = 0; i <= aNbEditions-1; i++)
            {
                Tournament _t = getTrnById(aConnectionString, aIsAtp, _idTrn);
                if (_t != null)
                {
                    _list.Add(_t);
                    if (_t.IdPreviousEdition != -1)
                        _idTrn = _t.IdPreviousEdition;
                    else break;
                }
                else break;
            }
            return _list;
        }

        public static Tournament getTrnById(string aConnectionString
            , bool aIsAtp, long aTrnId)
        {
            string _strQuery = "select id_t, name_t, date_t as dateA, ID_C_T, link_t, rank_t, site_t from tours_atp "
                + " where id_t =" + aTrnId;
            if (!aIsAtp)
                _strQuery = _strQuery.Replace("_atp", "_wta");
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

                Tournament _trn = null;
                while (rdr.Read())
                {
                    _trn = new Tournament(aIsAtp)
                    {
                        //Date = (Convert.ToString(rdr["DateM"]) != "" ? Convert.ToDateTime(rdr["DateM"]) : Convert.ToDateTime(rdr["DateM"])),
                        Name = Convert.ToString(rdr["name_t"]),
                        Id = Convert.ToInt32(rdr["id_t"]),
                        Date = Convert.ToDateTime(rdr["DateA"]),
                        CourtId = Convert.ToInt32(rdr["ID_C_T"]),
                        Rank = Convert.ToInt16(rdr["rank_t"]),
                        TournamentSite = Convert.ToString(rdr["site_t"]).Trim()
                    };
                    _trn.IdPreviousEdition = (Convert.ToString(rdr["link_t"]) != "" ? Convert.ToInt64(rdr["link_t"]) : -1);
                }
                return _trn;
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

        public static List<Tournament> getMyTrn(string aConnectionString
            , bool aIsAtp)
        {
            string _strQuery = "select * from trn_atp ";
            if (!aIsAtp)
                _strQuery = _strQuery.Replace("_atp", "_wta");
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

                List<Tournament> _list = new List<Tournament>();
                while (rdr.Read())
                {
                    Tournament _trn = new Tournament(aIsAtp)
                    {
                        //Date = (Convert.ToString(rdr["DateM"]) != "" ? Convert.ToDateTime(rdr["DateM"]) : Convert.ToDateTime(rdr["DateM"])),
                        Name = Convert.ToString(rdr["NameTrn"]),
                        Id = Convert.ToInt32(rdr["ID"]),
                        SpeedAdvanced = Convert.ToDouble(rdr["SpeedAdvanced"])
                    };
                    _list.Add(_trn);
                }
                return _list;
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

        //not completed
        public static List<Match> getListMatchesByLevelSql(string aConnectionString
            , bool aIsAtp, bool aIsOnlyFinishedTournaments, DateTime? aStartingDate = null, DateTime? aEndingDate = null
            , bool aIsIncludeUnknownValues = false)
        {
            string _sqlClauseOrderBy = " ORDER BY today.round DESC, today.DRAW ASC ";
            if (aIsOnlyFinishedTournaments)
                _sqlClauseOrderBy = " ORDER BY rounds.id_r DESC, games.ID_R_G ASC";
            string _strQuery = "SELECT trnmt.NAME_T as TRN, trnmt.rank_t as TournoiRang, Player1.NAME_P AS J1, Player2.NAME_P AS J2 "
            + ", today.RESULT as Result, surfaces.name_c as Surface, surfaces.id_c as IDSurface "
            + ", today.ID1 as ID1, today.ID2 as ID2, rounds.name_r as Tour, rounds.id_r as IDTour "
            + ", player1.rank_p  as RANK1, player2.rank_p  as RANK2, today.draw as DRAW "
            + "FROM ((((( today_atp as today "
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
            //+ " AND today.TOUR= " + aTrnId
            + (!aIsIncludeUnknownValues ? " AND not( player1.NAME_P = 'Unknown Player') AND not( player2.NAME_P = 'Unknown Player') "
            : " AND (not( player1.NAME_P = 'Unknown Player') or not( player2.NAME_P = 'Unknown Player')) ")
            + (aStartingDate.HasValue ? " AND games.DATE_G > #" + aStartingDate.Value.ToString("MM/dd/yyyy").Replace('.', '/') + "# " : "")
            + (aEndingDate.HasValue ? " AND games.DATE_G < #" + aEndingDate.Value.ToString("MM/dd/yyyy").Replace('.', '/') + "# " : "")
            + _sqlClauseOrderBy;
            if (!aIsAtp)
                _strQuery = _strQuery.Replace("_atp", "_wta");
            if (aIsOnlyFinishedTournaments)
            {
                _strQuery = _strQuery.Replace("today.RESULT", "games.RESULT_G");
                _strQuery = _strQuery.Replace("today.ID1", "games.ID1_G");
                _strQuery = _strQuery.Replace("today.ID2", "games.ID2_G");
                _strQuery = _strQuery.Replace("today.draw", "games.RESULT_G");
                _strQuery = _strQuery.Replace("today_atp", "Games_atp");
                _strQuery = _strQuery.Replace("today.round", "games.ID_R_G");
                _strQuery = _strQuery.Replace("today.TOUR", "games.ID_T_G");
                _strQuery = _strQuery.Replace("today.RESULT", "games.RESULT_G");
                _strQuery = _strQuery.Replace("today", "games");
            }
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
                        TournamentRank = Convert.ToInt16(rdr["TournoiRang"]),
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
                    try
                    {
                        if (_match.PositionPlayer1 == -1)
                        {
                            _match.PositionPlayer1 = getRankPlayer_Sql(_match.Id1, aConnectionString, aIsAtp, DateTime.Now);
                        }
                        if (_match.PositionPlayer2 == -1)
                        {
                            _match.PositionPlayer2 = getRankPlayer_Sql(_match.Id2, aConnectionString, aIsAtp, DateTime.Now);
                        }
                    }
                    catch { }

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

        public static List<Match> getListMatchesForTrnNotFinishedSql(string aConnectionString, long aTrnId
            , bool aIsAtp, bool aIsOnlyFinishedTournaments
            , bool aIsIncludeCategories
            , bool aIsIncludeUnknownValues)
        {
            string _strClauseSelect_Categories = " , categoriesP1.CAT1 as P1C1, categoriesP1.CAT2 as P1C2, categoriesP1.CAT3 as P1C3, categoriesP1.CAT4 as P1C4, categoriesP1.CAT5 as P1C5, " +
                "  categoriesP1.CAT6 as P1C6, categoriesP1.CAT7 as P1C7, categoriesP1.CAT8 as P1C8, categoriesP1.CAT9 as P1C9 "
                + " , categoriesP2.CAT1 as P2C1, categoriesP2.CAT2 as P2C2, categoriesP2.CAT3 as P2C3, categoriesP2.CAT4 as P2C4, categoriesP2.CAT5 as P2C5, " +
                "  categoriesP2.CAT6 as P2C6, categoriesP2.CAT7 as P2C7, categoriesP2.CAT8 as P2C8, categoriesP2.CAT9 as P2C9 ";
            string _strClauseGroupby_Categories = " , categoriesP1.CAT1, categoriesP1.CAT2, categoriesP1.CAT3, categoriesP1.CAT4, categoriesP1.CAT5, " +
                "  categoriesP1.CAT6, categoriesP1.CAT7 , categoriesP1.CAT8 , categoriesP1.CAT9  "
                + " , categoriesP2.CAT1, categoriesP2.CAT2, categoriesP2.CAT3, categoriesP2.CAT4, categoriesP2.CAT5, " +
                "  categoriesP2.CAT6, categoriesP2.CAT7 , categoriesP2.CAT8 , categoriesP2.CAT9  ";
            string _strClauseJoin_Categories = " LEFT OUTER JOIN categories_atp as categoriesP1 " +
        " ON categoriesP1.id_p = today.ID1 ) " +
        " LEFT OUTER JOIN categories_atp as categoriesP2 " +
        " ON categoriesP2.id_p = today.ID2 ) ";
            string _sqlClauseOrderBy = " ORDER BY today.round DESC, today.DRAW ASC ";
            if (aIsOnlyFinishedTournaments)
                _sqlClauseOrderBy = " ORDER BY rounds.id_r DESC, games.ID_R_G ASC";
            string _strQuery = "SELECT trnmt.NAME_T as TRN, trnmt.rank_t as TournoiRang, Player1.NAME_P AS J1, Player2.NAME_P AS J2 "
            + ", today.RESULT as Result, surfaces.name_c as Surface, surfaces.id_c as IDSurface "
            + ", today.ID1 as ID1, today.ID2 as ID2, rounds.name_r as Tour, rounds.id_r as IDTour "
            + ", player1.rank_p  as RANK1, player2.rank_p  as RANK2, today.draw as DRAW ,player1.date_p as date1, player2.date_p as date2 "
            + (aIsIncludeCategories ? _strClauseSelect_Categories : "") 
            + " FROM (((((" + (aIsIncludeCategories ? "((" : "") +  " today_atp as today "
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
            + (aIsIncludeCategories ? _strClauseJoin_Categories : "")
            + "WHERE not( player1.NAME_P LIKE '%/%') "
            + " AND today.TOUR= " + aTrnId
            + (!aIsIncludeUnknownValues ? " AND not( player1.NAME_P = 'Unknown Player') AND not( player2.NAME_P = 'Unknown Player') "
            : " AND (not( player1.NAME_P = 'Unknown Player') or not( player2.NAME_P = 'Unknown Player')) ")
            + _sqlClauseOrderBy;
            if (!aIsAtp)
                _strQuery=_strQuery.Replace("_atp", "_wta");
            if (aIsOnlyFinishedTournaments)
            {
                _strQuery = _strQuery.Replace("today.RESULT", "games.RESULT_G");
                _strQuery = _strQuery.Replace("today.ID1", "games.ID1_G");
                _strQuery = _strQuery.Replace("today.ID2", "games.ID2_G");
                _strQuery = _strQuery.Replace("today.draw", "games.RESULT_G");
                _strQuery = _strQuery.Replace("today_atp", "Games_atp");
                _strQuery = _strQuery.Replace("today.round", "games.ID_R_G");
                _strQuery = _strQuery.Replace("today.TOUR", "games.ID_T_G");
                _strQuery = _strQuery.Replace("today.RESULT", "games.RESULT_G");
                _strQuery = _strQuery.Replace("today", "games");
            }
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
                        TournamentRank = Convert.ToInt16(rdr["TournoiRang"]),
                        Player1Name = Convert.ToString(rdr["J1"]),
                        Player2Name = Convert.ToString(rdr["J2"]),
                        CourtId = Convert.ToInt16(rdr["IDSurface"]),
                        CourtName = Convert.ToString(rdr["Surface"]),
                        ResultString = Convert.ToString(rdr["Result"]),
                        PositionPlayer1 = (Convert.ToString(rdr["RANK1"]) != "" ? Convert.ToInt32(rdr["RANK1"]) : -1),
                        PositionPlayer2 = (Convert.ToString(rdr["RANK2"]) != "" ? Convert.ToInt32(rdr["RANK2"]) : -1),
                        Id1 = Convert.ToInt32(rdr["ID1"]),
                        Id2 = Convert.ToInt32(rdr["ID2"]),
                        DOB1 = (Convert.ToString(rdr["date1"]) != "" ? Convert.ToDateTime(rdr["date1"]) : (DateTime?)null),
                        DOB2 = (Convert.ToString(rdr["date2"]) != "" ? Convert.ToDateTime(rdr["date2"]) : (DateTime?)null),
                        RoundId = Convert.ToInt16(rdr["IDTour"]),
                        RoundName = Convert.ToString(rdr["Tour"])
                    };
                    if (aIsIncludeCategories)
                    {
                        _match.ListCategoriesIdP1 = new List<int>();
                        _match.ListCategoriesIdP2 = new List<int>();
                        for (int j = 1; j <= 2; j++)
                        {
                            for (int i = 1; i <= 9; i++)
                            {
                                string _nameField = "P" + j + "C" + i;
                                if (Convert.ToBoolean(rdr[_nameField]))
                                    if (j == 1)
                                        _match.ListCategoriesIdP1.Add(i);
                                    else
                                        _match.ListCategoriesIdP2.Add(i);
                            }
                        }
                    }
                    try
                    {
                        if (_match.PositionPlayer1 == -1)
                        {
                            _match.PositionPlayer1 = getRankPlayer_Sql(_match.Id1, aConnectionString, aIsAtp, DateTime.Now);
                        }
                        if (_match.PositionPlayer2 == -1)
                        {
                            _match.PositionPlayer2 = getRankPlayer_Sql(_match.Id2, aConnectionString, aIsAtp, DateTime.Now);
                        }
                    }
                    catch { }

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
                _strQuery = "insert into MyRankings_atp "
                        + " (rankestim1_m, rankestim2_m, note1, note2, id1_m, id2_m, id_t_m, id_r_m) "
                        + " VALUES (@Note1P1, @Note1P2, @Note2P1, @Note2P2, @IdP1, @IdP2, @IdTrn, @IdRound)";
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

        public static string getNoteFromIdSql(long aIdPlayer, string aConnectionString)
        {
            string aNote = "";
            string _strQuery = "SELECT * FROM Notes where Id="  + aIdPlayer;
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
                    //aNote = Convert.ToString(rdr["Id"]);
                    aNote = Convert.ToString(rdr["Note"]);
                    return aNote;
                }
                return "";
            }
            catch (Exception e)
            {
                MessageBox.Show("Impossible to connect to the database, please contact the support (Error: " + e.Message + " )");
                return "";
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

        public static void updatePlayerNoteSql(long aIdPlayer, string aNote, string aConnectionString)
        {
            if (aNote == "")
                return;
            string _strQuery = "DELETE FROM Notes "
                + " WHERE id=" + aIdPlayer + " ";
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
                _strQuery = "insert into Notes (id, [note]) "
                        + " VALUES (@id, @note)";
                myCommand = new OleDbCommand(_strQuery, (OleDbConnection)myConnection);
                myCommand.Parameters.AddWithValue("@id", aIdPlayer);
                myCommand.Parameters.AddWithValue("@note", aNote);
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
    

    }
}
