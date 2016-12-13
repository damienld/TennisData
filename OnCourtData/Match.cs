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
        public int Id1;// { get; set; }

        [System.ComponentModel.Browsable(false)]
        [Column(Name = "ID2_G", IsPrimaryKey = true)]
        public int Id2;// { get; set; }

        [System.ComponentModel.Browsable(false)]
        [Column(Name = "ID_T_G", IsPrimaryKey = true)]
        public int TournamentId;// { get; set; }
        [System.ComponentModel.Browsable(false)]
        public int TournamentRank { get; internal set; }

        [System.ComponentModel.Browsable(false)]
        [Column(Name = "ID_R_G", IsPrimaryKey = true)]
        public int RoundId;// { get; set; }

        [System.ComponentModel.DisplayName("Player 1")]
        public string Player1Name { get; set; }
        public int PositionPlayer1 { get; set; }
        [System.ComponentModel.DisplayName("Player 2")]
        public string Player2Name { get; set; }
        public int PositionPlayer2 { get; set; }
        public string TournamentName { get; set; }
        public DateTime? Date { get; set; }
        public string RoundName { get; set; }
        public string CourtName { get; set; }
        public string ResultString { get; set; }

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
        public void readStats()
        {
            ListProcessedStats = new List<StatsForOneMatch>();
            ListProcessedStats.Add(new StatsForOneMatch(1));
            ListProcessedStats.Add(new StatsForOneMatch(2));
        }
        /// <summary>
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
            }
        }
        public bool getIsBestOf5()
        {
            if (!IsATP)
                return false;
            else
                return (((IsATP) && (TournamentRank == 4 || TournamentRank == 5))
                    || (ProcessedResult != null && Math.Max(ProcessedResult.fNbSetsWonP1, ProcessedResult.fNbSetsWonP2) == 3)); //GS + DC
        }

        public bool isCountAsWinForStats(long aIdPlayer)
        {
            switch (ProcessedResult.EndType)
            {
                case ResultForMatch.TypeEnd.Completed:
                    return (aIdPlayer == this.Id1);
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
        public bool isCountAsLossForStats(long aIdPlayer)
        {
            switch (ProcessedResult.EndType)
            {
                case ResultForMatch.TypeEnd.Completed:
                    return (aIdPlayer == this.Id2);
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="aIdset">base 0</param>
        /// <param name="aIdPlayer"></param>
        /// <returns></returns>
        public bool isCountAsSetXWonForStats(int aIdset, long aIdPlayer)
        {
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
        public bool isPlayerFirst(long aPlayerId)
        {
            return (aPlayerId == Id1);
        }
        /// <summary>
        /// return true if Match respects the following conditions:
        /// - Opponent's Odds are between aLower and aUpper
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
