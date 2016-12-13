using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace OnCourtData
{
    public class StatReportPlayer
    {
        /// <summary>
        /// Create for Win %
        /// </summary>
        /// <param name="aNameStat"></param>
        /// <param name="aNbWin"></param>
        /// <param name="aNbOf"></param>
        /// <param name="aIsDisplayNbOf"></param>
        /// <param name="aNbDecimals"></param>
        public StatReportPlayer(string aNameStat, string aToolTip, int aNbWin, int aNbOf, bool aIsDisplayNbOf
            , int aIndexDisplay, int aNbDecimals)
        {
            NameStat = aNameStat;
            ToolTip = aToolTip;
            NbWin = aNbWin;
            NbOf = aNbOf;
            NbDecimals = aNbDecimals;
            IsDisplayNbOf = aIsDisplayNbOf;
            ReferenceToCompare = -1;
            IndexDisplay = aIndexDisplay;
            Percentage = stat(aNbWin, aNbOf, aNbDecimals);
        }
        public StatReportPlayer(string aNameStat, string aToolTip, int aNbWin, int aNbOf, bool aIsDisplayNbOf
            , double aReferenceToCompare, int aIndexDisplay, int aNbDecimals)
        {
            NameStat = aNameStat;
            ToolTip = aToolTip;
            NbWin = aNbWin;
            NbOf = aNbOf;
            NbDecimals = aNbDecimals;
            IsDisplayNbOf = aIsDisplayNbOf;
            ReferenceToCompare = aReferenceToCompare;
            IndexDisplay = aIndexDisplay;
            Percentage = stat(aNbWin, aNbOf, aNbDecimals);
        }
        /// <summary>
        /// Create for ROI
        /// </summary>
        /// <param name="aNameStat"></param>
        /// <param name="aPercentage"></param>
        /// <param name="aNbOf"></param>
        /// <param name="aIsDisplayNbOf"></param>
        /// <param name="aNbDecimals"></param>
        public StatReportPlayer(string aNameStat, string aToolTip, bool aIsDisplayNbOf
           , double aPercentage, int aNbOf, int aIndexDisplay, int aNbDecimals)
        {
            NameStat = aNameStat;
            ToolTip = aToolTip;
            NbOf = aNbOf;
            NbDecimals = aNbDecimals;
            Percentage = aPercentage;
            IndexDisplay = aIndexDisplay;
        }
        /// <summary>
        /// Create for ROI
        /// </summary>
        /// <param name="aNameStat"></param>
        /// <param name="aPercentage"></param>
        /// <param name="aNbOf"></param>
        /// <param name="aIsDisplayNbOf"></param>
        /// <param name="aNbDecimals"></param>
        public StatReportPlayer(string aNameStat, string aToolTip, bool aIsDisplayNbOf
           , double aPercentage, int aNbOf, double aReferenceToCompare, int aIndexDisplay, int aNbDecimals)
        {
            NameStat = aNameStat;
            ToolTip = aToolTip;
            NbOf = aNbOf;
            NbDecimals = aNbDecimals;
            Percentage = aPercentage;
            IndexDisplay = aIndexDisplay;
        }
        /// <summary>
        /// Base 0
        /// </summary>
        public int IndexDisplay { get; set; }
        public string NameStat { get; set; }
        public string ToolTip { get; set; }
        public double Percentage { get; set; }
        public int NbWin { get; set; }
        public int NbOf { get; set; }
        public int NbDecimals { get; set; }
        /*public int aROI { get; set; }
        public int aNbMatchesForROI { get; set; }*/
        public bool IsDisplayNbOf { get; set; }
        public double ReferenceToCompare { get; set; }

        private double stat(int aNbWin, int aNbOf, int aNbDecimals)
        {
            return Math.Round(aNbWin * 100.0 / aNbOf, 0);
        }
        public string getDisplayString()
        {
            string _res = Percentage.ToString()+"%";
            if (ReferenceToCompare != -1)
                _res = Convert.ToInt16((Percentage-Math.Round(100*ReferenceToCompare))).ToString() + "%";
            if (IsDisplayNbOf)
                _res += "(" + NbOf + ")";
            Trace.WriteLine(NameStat + ":" + _res);
            return _res;
        }
        public string getHtmlForCell()
        {
            string _res = getDisplayString();
            
            return "<td>"+_res+ "</td>";
        }
        public string getHtmlForHeaderCell()
        {
            string _res = NameStat;
            return "<td>" + _res + "</td>";
        }
    }
    public class ReportPlayer
    {
        private long PlayerId;
        private string PlayerName;
        private int PositionPlayer;
        private List<MatchDetailsWithOdds> ListMatch;
        private List<StatReportPlayer> ListStats;
        public ReportPlayer(long aPlayerId, string aPlayerName, int aPositionPlayer, List<MatchDetailsWithOdds> aListMatch)
        {
            PlayerId = aPlayerId;
            ListMatch = aListMatch;
            PositionPlayer = aPositionPlayer;
            PlayerName = aPlayerName;
            ListStats = new List<StatReportPlayer>();
        }
        public void calculateValues(List<int> aListCourtsId, int aMinPosOpp, int aMaxPosOpp, bool aIsBestOf5)
        {
            DateTime _dateForm = DateTime.Now.AddDays(-183);
            PriceStatsListMatchesForPlayer _priceStats = new PriceStatsListMatchesForPlayer(ListMatch, PlayerId, aListCourtsId
                , _dateForm);
            ListStats.Add(new StatReportPlayer("W/L", "", ListMatch.Where(m => m.isCountAsWinForStats(PlayerId)).Count()
                , ListMatch.Where(m => m.isCountAsLossForStats(PlayerId) || m.isCountAsWinForStats(PlayerId)).Count(), true, 0, 0));
            ListStats.Add(new StatReportPlayer("ROI", "Match", true, _priceStats.MatchMarket.getROI()
                , _priceStats.MatchMarket.NbMarkets, 1, 0));
            List<MatchDetailsWithOdds> _listMatchWherePlayerFirst = ListMatch.Where(m => m.Id1 == PlayerId).ToList();
            List<MatchDetailsWithOdds> _listMatchWherePlayerSecond = ListMatch.Where(m => m.Id2 == PlayerId).ToList();
            int _nbSetsWon = _listMatchWherePlayerFirst.Sum(m => m.ProcessedResult.fNbSetsWonP1) + _listMatchWherePlayerSecond.Sum(m => m.ProcessedResult.fNbSetsWonP2);
            int _nbSetsCompleted = ListMatch.Sum(m => m.ProcessedResult.fNbSetsWonP2 + m.ProcessedResult.fNbSetsWonP1);
            ListStats.Add(new StatReportPlayer("Sets", ""
                , _nbSetsWon, _nbSetsCompleted, true, 2, 0));
            ListStats.Add(new StatReportPlayer("Set1", "ROI", true, _priceStats.Set1Market.getROI()
                , _priceStats.Set1Market.NbMarkets, 3, 0));
            /*ListStats.Add(new StatReportPlayer("Form", "ROI Last X weeks", true
                , _listMatchWherePlayerFirst.Where(m => m.Date >= _dateForm).Sum(m => m.ProcessedResult.fNbSetsWonP1) 
                + _listMatchWherePlayerSecond.Where(m => m.Date >= _dateForm).Sum(m => m.ProcessedResult.fNbSetsWonP2)
                , ListMatch.Where(m => m.Date >= _dateForm).Sum(m => m.ProcessedResult.fNbSetsWonP2 + m.ProcessedResult.fNbSetsWonP1)
                , 4));*/
            ListStats.Add(new StatReportPlayer("Form", "ROI Last X weeks", true
                , _priceStats.MatchMarketFromDate.getROI()
                , _priceStats.MatchMarketFromDate.NbMarkets
                , 4, 0));

            List<int> _listMinPosOpponent = new List<int>() {aMinPosOpp, 1, 6, 16, 31, 61, 101, 201};
            List<int> _listMaxPosOpponent = new List<int>() { aMaxPosOpp, 5, 15, 30, 60, 100, 200, 9999 };
            List<MatchDetailsWithOdds> _listMatchWherePlayerSecondAndOppNotRanked
                    = _listMatchWherePlayerSecond.Where(m => m.PositionPlayer1 == -1).ToList();
            List<MatchDetailsWithOdds> _listMatchWherePlayerFirstAndOppNotRanked
                    = _listMatchWherePlayerSecond.Where(m => m.PositionPlayer2 == -1).ToList();
            for (int i = 0; i <= _listMinPosOpponent.Count - 1; i++)
            {
                int _minPosOpponent = _listMinPosOpponent[i];
                int _maxPosOpponent = _listMaxPosOpponent[i];
                List<MatchDetailsWithOdds> _listMatchWherePlayerFirstAndOppInInterval
                    = _listMatchWherePlayerFirst.Where(m => m.PositionPlayer2 >= _minPosOpponent && _maxPosOpponent >= m.PositionPlayer2).ToList();
                List<MatchDetailsWithOdds> _listMatchWherePlayerSecondAndOppInInterval
                    = _listMatchWherePlayerSecond.Where(m => m.PositionPlayer1 >= _minPosOpponent && _maxPosOpponent >= m.PositionPlayer1).ToList();
                if (i == 0)
                    ListStats.Add(new StatReportPlayer("W/L " + _minPosOpponent + "-" + _maxPosOpponent, ""
                    , _listMatchWherePlayerFirstAndOppInInterval.Where(m => m.isCountAsWinForStats(PlayerId)).ToList().Count
                    + _listMatchWherePlayerSecondAndOppInInterval.Where(m => m.isCountAsLossForStats(PlayerId)).ToList().Count
                    , _listMatchWherePlayerFirstAndOppInInterval.Where(m => m.isCountAsLossForStats(PlayerId) || m.isCountAsWinForStats(PlayerId)).ToList().Count
                    + _listMatchWherePlayerSecondAndOppInInterval.Where(m => m.isCountAsLossForStats(PlayerId) || m.isCountAsWinForStats(PlayerId)).ToList().Count
                    , true, 5+i, 0));
                ListStats.Add(new StatReportPlayer("Set " + _minPosOpponent + "-" + _maxPosOpponent, ""
                    , _listMatchWherePlayerFirstAndOppInInterval.Sum(m => m.ProcessedResult.fNbSetsWonP1)
                    + _listMatchWherePlayerSecondAndOppInInterval.Sum(m => m.ProcessedResult.fNbSetsWonP2)
                    + (i != _listMinPosOpponent.Count - 1 ? 0 : _listMatchWherePlayerFirstAndOppNotRanked.Sum(m => m.ProcessedResult.fNbSetsWonP1))
                    + (i != _listMinPosOpponent.Count - 1 ? 0 : _listMatchWherePlayerSecondAndOppNotRanked.Sum(m => m.ProcessedResult.fNbSetsWonP2))
                    , _listMatchWherePlayerFirstAndOppInInterval.Sum(m => m.ProcessedResult.fNbSetsWonP1 + m.ProcessedResult.fNbSetsWonP2)
                    + _listMatchWherePlayerSecondAndOppInInterval.Sum(m => m.ProcessedResult.fNbSetsWonP2 + m.ProcessedResult.fNbSetsWonP1)
                    + (i != _listMinPosOpponent.Count - 1 ? 0: _listMatchWherePlayerFirstAndOppNotRanked.Sum(m => m.ProcessedResult.fNbSetsWonP1 + m.ProcessedResult.fNbSetsWonP2))
                    + (i != _listMinPosOpponent.Count - 1 ? 0 : _listMatchWherePlayerSecondAndOppNotRanked.Sum(m => m.ProcessedResult.fNbSetsWonP2 + m.ProcessedResult.fNbSetsWonP1))
                    , true, 6+i, 0));
            }
            ListStats.Add(new StatReportPlayer("Sets on Court", ""
                , _listMatchWherePlayerFirst.Where(m=>aListCourtsId.Contains(m.CourtId)).Sum(m => m.ProcessedResult.fNbSetsWonP1) 
                + _listMatchWherePlayerSecond.Where(m => aListCourtsId.Contains(m.CourtId)).Sum(m => m.ProcessedResult.fNbSetsWonP2)
                , ListMatch.Where(m => aListCourtsId.Contains(m.CourtId)).Sum(m => m.ProcessedResult.fNbSetsWonP2 + m.ProcessedResult.fNbSetsWonP1)
                , true, 6 + _listMinPosOpponent.Count - 1 + 1, 0));
            ListStats.Add(new StatReportPlayer("Court", "ROI on Court", true, _priceStats.MatchMarket.getROI()
                , _priceStats.MatchMarket.NbMarkets, (_nbSetsWon/_nbSetsCompleted), 6 + _listMinPosOpponent.Count - 1 + 2, 0));

        }
        public string getHtml(bool aisHeader)
        {
            string _html = "<tr><td>" + PositionPlayer + " - " + PlayerName + "</td>";
            if (aisHeader)
                _html = "<tr style='font-weight:900;' bgcolor='lightyellow'><td></td>";
            foreach (StatReportPlayer _s in ListStats.OrderBy(m=>m.IndexDisplay))
            {
                if (aisHeader)
                    _html += _s.getHtmlForHeaderCell();
                else
                    _html += _s.getHtmlForCell();
            }
            return _html + "<tr>";
        }
    }
}
