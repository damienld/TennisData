using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq.Mapping;
using System.Data.Linq;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace OnCourtData
{
    [Table(Name = "Games_atp")]
    public class Match
    {
        [System.ComponentModel.Browsable(false)]
        protected bool IsATP { get; set; }
        [System.ComponentModel.Browsable(false)]
        public ResultForMatch ProcessedResult { get; set; }
        [System.ComponentModel.Browsable(false)]
        //stats for P1 and P2
        public List<StatsForOneMatch> ListProcessedStats { get; set; }


        public override string ToString()
        {
            string s = (Player1Name + " v " + Player2Name);
            return s;
        }
        /// <summary>
        /// You must initialize IsATP when using this
        /// </summary>
        public Match()
        {
            ProcessedResult = null;
            ListProcessedStats = null;
        }
        public Match(bool aIsATP)
        {
            IsATP = aIsATP;
            ProcessedResult = null;
            ListProcessedStats = null;
        }

        [System.ComponentModel.Browsable(false)]
        [Column(Name = "ID1_G", IsPrimaryKey = true)]
        public long Id1;// { get; set; }

        [System.ComponentModel.Browsable(false)]
        [Column(Name = "ID2_G", IsPrimaryKey = true)]
        public long Id2;// { get; set; }

        [System.ComponentModel.Browsable(false)]
        [Column(Name = "ID_T_G", IsPrimaryKey = true)]
        public int TournamentId;// { get; set; }
        /// <summary>
        /// 0=ITF / ITF < 20K; 1=Challenger / ITF >=20 K;
        /// 2=ATP ; 3= M1000 4=GS 5=DC 6=Exhib/Juniors
        /// </summary>
        [System.ComponentModel.Browsable(false)]
        public int TournamentRank { get; internal set; } //

        /// <summary>
        /// //O preq, 1-2-3 Qualies, 8 RR, 4=1st, 10=1/2, 11 bronze, 12 final
        /// </summary>
        [System.ComponentModel.Browsable(false)]
        [Column(Name = "ID_R_G", IsPrimaryKey = true)]
        public int RoundId;// { get; set; } 
        [System.ComponentModel.Browsable(false)]
        public bool? IsP1ServingFirst { get; set; }

        [System.ComponentModel.Browsable(false)]
        public DateTime? DOB1 { get; set; }
        [System.ComponentModel.Browsable(false)]
        public DateTime? DOB2 { get; set; }

        [System.ComponentModel.DisplayName("Player 1")]
        public string Player1Name { get; set; }
        [System.ComponentModel.Browsable(false)]
        public string Player1Info { get; set; } //WC, LL, seed..
        public int PositionPlayer1 { get; set; }
        [System.ComponentModel.DisplayName("Player 2")]
        public string Player2Name { get; set; }
        [System.ComponentModel.Browsable(false)]
        public string Player2Info { get; set; } //WC, LL, seed..
        public int PositionPlayer2 { get; set; }
        public string TournamentName { get; set; }
        [System.ComponentModel.Browsable(false)]
        public string TournamentSite { get; set; }

        public DateTime? Date
        { get;
            set; }
        public string RoundName { get; set; }
        public string CourtName { get; set; }
        public string ResultString { get; set; }
        [System.ComponentModel.Browsable(false)]
        public List<int> ListCategoriesIdP1 { get; set; }
        [System.ComponentModel.Browsable(false)]
        public List<int> ListCategoriesIdP2 { get; set; }

        [System.ComponentModel.Browsable(false)]
        /// <summary>
        /// 1 to 6: "Hard", "Clay", "Indoor", "Carpet", "Grass", "Acrylic"
        /// </summary>
        public int CourtId { get; set; }
        public void readResult()
        {
            ProcessedResult = new ResultForMatch();
            ProcessedResult.readResult(ResultString);
        }
        /// <summary>
        /// Only called when loading up Stats from Db
        /// </summary>
        public void createStatsObjectForPlayers()
        {
            //if (ProcessedResult == null)
            //    readResult();
            ListProcessedStats = new List<StatsForOneMatch>();
            ListProcessedStats.Add(new StatsForOneMatch(1));
            ListProcessedStats.Add(new StatsForOneMatch(2));
        }
        ///// <summary>
        /// Require to have loaded Stats from Db before
        /// Only count Completed Matches with Stats available for BP's
        /// </summary>
        public void readHolds()
        {
            if (ProcessedResult == null)
                readResult();
            if (ListProcessedStats != null && ListProcessedStats[0].BP_1 != -1 && ListProcessedStats[1].BP_1 != -1 
                && ProcessedResult.EndType == ResultForMatch.TypeEnd.Completed)
            {
                int _nbTbWonByP1 = ProcessedResult.fListTbWinners.Where(s => s == 1).Count();
                int _nbTbWonByP2 = ProcessedResult.fListTbWinners.Where(s => s == 2).Count();
                int _nbGamesWonByP1ExclTb = ProcessedResult.fNbGamesWonP1 - _nbTbWonByP1;
                int _nbGamesWonByP2ExclTb = ProcessedResult.fNbGamesWonP2 - _nbTbWonByP2;
                int _nbBreaksForP1 = ListProcessedStats[0].BP_1;
                int _nbBreaksForP2 = ListProcessedStats[1].BP_1;
                int _nbServiceGamesWonP1 = _nbGamesWonByP1ExclTb - _nbBreaksForP1;
                int _nbServiceGamesWonP2 = _nbGamesWonByP2ExclTb - _nbBreaksForP2;
                ListProcessedStats[0].nbServiceGamesPlayed = _nbBreaksForP2 + _nbServiceGamesWonP1;
                ListProcessedStats[1].nbServiceGamesPlayed = _nbBreaksForP1 + _nbServiceGamesWonP2;
                ListProcessedStats[0].nbServiceGamesWon = _nbServiceGamesWonP1;
                ListProcessedStats[1].nbServiceGamesWon = _nbServiceGamesWonP2;
                ListProcessedStats[0].nbReturnGamesPlayed = ListProcessedStats[1].nbServiceGamesPlayed;
                ListProcessedStats[1].nbReturnGamesPlayed = ListProcessedStats[0].nbServiceGamesPlayed;

                int _nbGamesInLastSetP1 = this.ProcessedResult.fListSetResultsForP1[this.ProcessedResult.fNbSetsWonP1
                    + this.ProcessedResult.fNbSetsWonP2-1];
                int _nbGamesInLastSetP2 = this.ProcessedResult.fListSetResultsForP2[this.ProcessedResult.fNbSetsWonP1
                    + this.ProcessedResult.fNbSetsWonP2 - 1];
                bool _isTbPlayedLastSet = (_nbGamesInLastSetP1 + _nbGamesInLastSetP2 == 13);
                int _nbTbBeforeLastSet = _nbTbWonByP1 + _nbTbWonByP2;
                if (_isTbPlayedLastSet)
                    _nbTbBeforeLastSet--;
                //int _nbGamesBeforeLastSet = ProcessedResult.fNbGamesWonP1 + ProcessedResult.fNbGamesWonP2
                //    -_nbGamesInLastSetP1 - _nbGamesInLastSetP2;
                this.IsP1ServingFirst = null;
                if ((_nbTbBeforeLastSet % 2) == 0)
                {
                    if (ListProcessedStats[0].nbServiceGamesPlayed > ListProcessedStats[1].nbServiceGamesPlayed)
                        this.IsP1ServingFirst = true;
                    if (ListProcessedStats[0].nbServiceGamesPlayed < ListProcessedStats[1].nbServiceGamesPlayed)
                        this.IsP1ServingFirst = false;
                }
                else
                {
                    if (ListProcessedStats[0].nbServiceGamesPlayed > ListProcessedStats[1].nbServiceGamesPlayed)
                        this.IsP1ServingFirst = false;
                    if (ListProcessedStats[0].nbServiceGamesPlayed < ListProcessedStats[1].nbServiceGamesPlayed)
                        this.IsP1ServingFirst = true;
                }
            }
        }
        public bool getIsBestOf5()
        {
            if (!IsATP)
                return false;
            else
                return (((IsATP) && (TournamentRank == 4 || TournamentRank == 5) && RoundId >=4)
                    || (ProcessedResult != null && Math.Max(ProcessedResult.fNbSetsWonP1, ProcessedResult.fNbSetsWonP2) == 3)); //GS + DC
        }
        public bool isCountAsHcpWonForStats(long aIdPlayer, double aHcp)
        {
            if (ProcessedResult == null)
                this.readResult();
            switch (ProcessedResult.EndType)
            {
                case ResultForMatch.TypeEnd.Completed:
                    if (aIdPlayer == this.Id1)
                    {
                        return (ProcessedResult.fNbGamesWonP1 + aHcp > ProcessedResult.fNbGamesWonP2);
                    }
                    else
                    {
                        return (ProcessedResult.fNbGamesWonP2 + aHcp > ProcessedResult.fNbGamesWonP1);
                    };
                default:
                    return false;
            }
        }
        public bool IsCompletedMatch()
        {
            if (ProcessedResult == null)
                this.readResult();
            return this.ProcessedResult.EndType == ResultForMatch.TypeEnd.Completed;
        }
        public bool isCountAsHcpLostForStats(long aIdPlayer, double aHcp)
        {
            if (ProcessedResult == null)
                this.readResult();
            switch (ProcessedResult.EndType)
            {
                case ResultForMatch.TypeEnd.Completed:
                    if (aIdPlayer == this.Id1)
                    {
                        return (ProcessedResult.fNbGamesWonP2 - aHcp > ProcessedResult.fNbGamesWonP1);
                    }
                    else
                    {
                        return (ProcessedResult.fNbGamesWonP1 - aHcp > ProcessedResult.fNbGamesWonP2);
                    };
                default:
                    return false;
            }
        }
        public bool isCountAsTotalWonForStats(double aTotal)
        {
            if (ProcessedResult == null)
                this.readResult();
            switch (ProcessedResult.EndType)
            {
                case ResultForMatch.TypeEnd.Completed:
                    return (ProcessedResult.fNbGamesWonP1 + ProcessedResult.fNbGamesWonP2 > aTotal);
                default:
                    return false;
            }
        }
        public bool isCountAsTotalLostForStats(double aTotal)
        {
            if (ProcessedResult == null)
                this.readResult();
            switch (ProcessedResult.EndType)
            {
                case ResultForMatch.TypeEnd.Completed:
                    return (ProcessedResult.fNbGamesWonP1 + ProcessedResult.fNbGamesWonP2 < aTotal);
                default:
                    return false;
            }
        }
        public bool isCountForStatsWinOrLoss()
        {
            if (ProcessedResult == null)
                this.readResult();
            switch (ProcessedResult.EndType)
            {
                case ResultForMatch.TypeEnd.Completed:
                    return (true);
                case ResultForMatch.TypeEnd.PulledOut:
                    return false;
                case ResultForMatch.TypeEnd.Retirement:
                    return (ProcessedResult.fNbSetsWonP1 + ProcessedResult.fNbSetsWonP2 >= 1);//1 set played
                case ResultForMatch.TypeEnd.Disqualified:
                    return (ProcessedResult.fNbSetsWonP1 + ProcessedResult.fNbSetsWonP2 >= 1);//1 set played
                default:
                    return true;
            }
        }
        public bool isCountAsWinForStats(long aIdPlayer)
        {
            if (ProcessedResult == null)
                this.readResult();
            switch (ProcessedResult.EndType)
            {
                case ResultForMatch.TypeEnd.Completed:
                    return (aIdPlayer == this.Id1);
                case ResultForMatch.TypeEnd.PulledOut:
                    return false;
                case ResultForMatch.TypeEnd.Retirement:
                    return (ProcessedResult.fNbSetsWonP1 + ProcessedResult.fNbSetsWonP2 >= 1 && aIdPlayer == this.Id1);//1 set played
                case ResultForMatch.TypeEnd.Disqualified:
                    return (ProcessedResult.fNbSetsWonP1 + ProcessedResult.fNbSetsWonP2 >= 1 && aIdPlayer == this.Id1);//1 set played
                default:
                    return true;
            }
        }
        public bool isCountAsLossForStats(long aIdPlayer)
        {
            if (ProcessedResult == null)
                this.readResult();
            switch (ProcessedResult.EndType)
            {
                case ResultForMatch.TypeEnd.Completed:
                    return (aIdPlayer == this.Id2);
                case ResultForMatch.TypeEnd.PulledOut:
                    return false;
                case ResultForMatch.TypeEnd.Retirement:
                    return (ProcessedResult.fNbSetsWonP1 + ProcessedResult.fNbSetsWonP2 >= 1 && aIdPlayer == this.Id2);//1 set played
                case ResultForMatch.TypeEnd.Disqualified:
                    return (ProcessedResult.fNbSetsWonP1 + ProcessedResult.fNbSetsWonP2 >= 1 && aIdPlayer == this.Id2);//1 set played
                default:
                    return true;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="aIdset">base 0</param>
        /// <param name="aIdPlayer"></param>
        /// <returns></returns>
        public bool isCountAsSetXWonForStats(int aIdset, long aIdPlayer)
        {
            if (ProcessedResult == null)
                this.readResult();
            if (ProcessedResult.fListSetResultsForP1[aIdset] != -1 && ProcessedResult.fListSetResultsForP2[aIdset] != -1)
            {
                if (aIdPlayer == Id1)
                    return (ProcessedResult.fListSetResultsForP1[aIdset] > ProcessedResult.fListSetResultsForP2[aIdset]);
                else
                    return (ProcessedResult.fListSetResultsForP1[aIdset] < ProcessedResult.fListSetResultsForP2[aIdset]);
            }
            else
                return false;
        }
        /// 
        /// </summary>
        /// <param name="aIdset">base 0</param>
        /// <param name="aIndexPlayer">base 1</param>
        /// <returns></returns>
        public bool isCountAsSetXWonForStats(int aIdset, List<string> aPlayerInfoToSearch)
        {
            if (ProcessedResult.fListSetResultsForP1[aIdset] != -1 && ProcessedResult.fListSetResultsForP2[aIdset] != -1
                && (Player1Info != Player2Info))
            {
                if (aPlayerInfoToSearch.IndexOf(Player1Info) > -1)
                    return (ProcessedResult.fListSetResultsForP1[aIdset] > ProcessedResult.fListSetResultsForP2[aIdset]);
                else if (aPlayerInfoToSearch.IndexOf(Player2Info) > -1)
                    return (ProcessedResult.fListSetResultsForP1[aIdset] < ProcessedResult.fListSetResultsForP2[aIdset]);
                else
                    return false;
            }
            else
                return false;
        }
        
        public bool isCountAsSetXLostForStats(int aIdset, List<string> aPlayerInfoToSearch)
        {
            if (ProcessedResult.fListSetResultsForP1[aIdset] != -1 && ProcessedResult.fListSetResultsForP2[aIdset] != -1
                && (Player1Info != Player2Info))
            {
                if (aPlayerInfoToSearch.IndexOf(Player1Info) > -1)
                    return (ProcessedResult.fListSetResultsForP1[aIdset] < ProcessedResult.fListSetResultsForP2[aIdset]);
                else if (aPlayerInfoToSearch.IndexOf(Player2Info) > -1)
                    return (ProcessedResult.fListSetResultsForP1[aIdset] > ProcessedResult.fListSetResultsForP2[aIdset]);
                else
                    return false;
            }
            else
                return false;
        }
        public bool isCountAsSetXLostForStats(int aIdset, long aIdPlayer)
        {
            if (ProcessedResult.fListSetResultsForP1[aIdset] != -1 && ProcessedResult.fListSetResultsForP2[aIdset] != -1)
            {
                if (aIdPlayer == Id1)
                    return (ProcessedResult.fListSetResultsForP1[aIdset] < ProcessedResult.fListSetResultsForP2[aIdset]);
                else
                    return (ProcessedResult.fListSetResultsForP1[aIdset] > ProcessedResult.fListSetResultsForP2[aIdset]);
            }
            else
                return false;
        }
        public bool isCountAsSetBettingWonForStats(int aNbSetsToWin, int aNbSetsToLose, long aIdPlayer)
        {
            if (ProcessedResult == null)
                this.readResult();
            
                if (aIdPlayer == Id1)
                    return (ProcessedResult.fNbSetsWonP1 == aNbSetsToWin && ProcessedResult.fNbSetsWonP2 == aNbSetsToLose);
                else
                    return (ProcessedResult.fNbSetsWonP2 == aNbSetsToWin && ProcessedResult.fNbSetsWonP1 == aNbSetsToLose);

        }
        public bool isInPeriod(int aDayStart, int aMonthStart, int aDayEnd, int aMonthEnd)
        {
            int aYearStart = this.Date.Value.Year;
            if (aMonthStart == 12)
                aYearStart -= 1;
            if (this.Date >= new DateTime(aYearStart, aMonthStart, aDayStart)
            && this.Date <= new DateTime(this.Date.Value.Year, aMonthEnd, aDayEnd))
            {
                return true;
            }
            else
                return false;
        }
        public bool isInPeriodAndCourt(int aDayStart, int aMonthStart, int aDayEnd, int aMonthEnd
            , int aCourtIndex1, int aCourtIndex2, int aCourtIndex3, int aCourtIndex4, List<int> aListCourtsId)
        {
            int aYearStart = this.Date.Value.Year;
            if (aMonthStart == 12)
                aYearStart -= 1;
            //if all courts are null then use court of trnmt
            if (aCourtIndex1 <= 0 && aCourtIndex2 <= 0 && aCourtIndex3 <= 0 && aCourtIndex4 <= 0)
            {
                if (this.Date >= new DateTime(aYearStart, aMonthStart, aDayStart)
                && this.Date <= new DateTime(this.Date.Value.Year, aMonthEnd, aDayEnd)
                && (aListCourtsId == null || aListCourtsId.Contains( this.CourtId)))
                {
                    return true;
                }
                else
                    return false;
            }
            else
            {
                if (this.Date >= new DateTime(aYearStart, aMonthStart, aDayStart)
                && this.Date <= new DateTime(this.Date.Value.Year, aMonthEnd, aDayEnd)
                && (this.CourtId == aCourtIndex1 || this.CourtId == aCourtIndex2 || this.CourtId == aCourtIndex3
                 || this.CourtId == aCourtIndex4))
                {
                    return true;
                }
                else
                    return false;
            }
        }
        public bool isPlayerFirst(long aPlayerId)
        {
            return (aPlayerId == Id1);
        }
        /// <summary>
        /// return true if Match respects the following conditions:
        /// - Opponent's rank is between aLower and aUpper
        /// </summary>
        /// <param name="aLower"></param>
        /// <param name="aUpper"></param>
        /// <param name="aPlayerId"></param>
        /// <returns></returns>
        public bool isFilterRankingOpponent(decimal aLower, decimal aUpper, long aPlayerId)
        {
            double a = Convert.ToDouble(aLower);
            double b = Convert.ToDouble(aUpper);
            return (this.PositionPlayer2 >= a && this.PositionPlayer2 <= b && isPlayerFirst(aPlayerId))
                    || (this.PositionPlayer1 >= a && this.PositionPlayer1 <= b && !isPlayerFirst(aPlayerId));
        }

        public bool isFilterCategoryOpponent(int aCategoryOppId, long aPlayerId)
        {
            return ((isPlayerFirst(aPlayerId) && ListCategoriesIdP2.Contains(aCategoryOppId))
                    || (!isPlayerFirst(aPlayerId) && ListCategoriesIdP1.Contains(aCategoryOppId)));
        }
        public bool isFilterTwoCombinedCategoriesOpponent(int aCategoryOppId1, int aCategoryOppId2, long aPlayerId)
        {
            return ((isPlayerFirst(aPlayerId) && ListCategoriesIdP2.Contains(aCategoryOppId1)) && ListCategoriesIdP2.Contains(aCategoryOppId2))
                    || (!isPlayerFirst(aPlayerId) && ListCategoriesIdP1.Contains(aCategoryOppId1) && ListCategoriesIdP1.Contains(aCategoryOppId2));
        }
    }

    /*public class MatchesCollection : ObservableCollection<Match>
    {
        public override string ToString()
        {
            string s = "";
            foreach (Match m in this)
                s += (m.Player1Name + " v " + m.Player2Name);
            return s;
        }
    }*/
}
