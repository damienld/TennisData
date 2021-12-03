using System;
using System.Collections.Generic;
using System.Linq;
using static OnCourtData.StatReportPlayer;

namespace OnCourtData
{
    public class ReportPlayer
    {
        private long PlayerId;
        private string PlayerName;
        private int PositionPlayer;
        //private List<MatchDetailsWithOdds> aListMatch;
        //private List<MatchDetailsWithOdds> ListMatchFull;
        public List<StatReportPlayer> ListStats { get; set; }
        public List<StatReportPlayer> ListStatsFull { get; set; }
        private List<int> ListCategoriesOpp = null;
        private Dictionary<int, string> ListCategories = null;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="aPlayerId"></param>
        /// <param name="aPlayerName"></param>
        /// <param name="aPositionPlayer"></param>
        /// <param name="aListCategoriesOpp">dont send null, must be new List<int>() at least</param>
        public ReportPlayer(long aPlayerId, string aPlayerName, int aPositionPlayer, List<int> aListCategoriesOpp
            , Dictionary<int, string> aListCategories)
        {
            PlayerId = aPlayerId;
            PositionPlayer = aPositionPlayer;
            PlayerName = aPlayerName;
            ListStats = new List<StatReportPlayer>();
            ListStatsFull = new List<StatReportPlayer>();
            ListCategoriesOpp = aListCategoriesOpp;
            ListCategories = aListCategories;
        }
        /// <summary>
        /// Calculate Values for html row1 (stats vs ranks)
        /// </summary>
        /// <param name="aListCourtsId"></param>
        /// <param name="aMinPosOpp"></param>
        /// <param name="aMaxPosOpp"></param>
        /// <param name="aIsBestOf5"></param>
        /// <param name="aListMatch"></param>
        public void CalculateValuesForRow1OfMatchReport(bool aIsAtp, List<int> aListCourtsId, int aMinPosOpp, int aMaxPosOpp
            , bool aIsBestOf5
            , List<MatchDetailsWithOdds> aListMatch)
        {
            List<MatchDetailsWithOdds> _listMatchWherePlayerFirst = aListMatch.Where(m => m.Id1 == PlayerId).ToList();
            List<MatchDetailsWithOdds> _listMatchWherePlayerSecond = aListMatch.Where(m => m.Id2 == PlayerId).ToList();
            int _nbSetsWon = _listMatchWherePlayerFirst.Sum(m => m.ProcessedResult.fNbSetsWonP1) + _listMatchWherePlayerSecond.Sum(m => m.ProcessedResult.fNbSetsWonP2);
            int _nbSetsCompleted = aListMatch.Sum(m => m.ProcessedResult.fNbSetsWonP2 + m.ProcessedResult.fNbSetsWonP1);
            int _nbSets1Won = _listMatchWherePlayerFirst.Where(m => m.isCountAsSetXWonForStats(0, PlayerId)).ToList().Count 
                + _listMatchWherePlayerSecond.Where(m => m.isCountAsSetXWonForStats(0, PlayerId)).ToList().Count;
            int _nbSets1Completed = aListMatch.Where(m => m.ProcessedResult.fNbSetsWonP2 
            + m.ProcessedResult.fNbSetsWonP1 >=1).ToList().Count();
            DateTime _dateForm1 = DateTime.Now.AddDays(-183);
            DateTime _dateForm2 = DateTime.Now.AddDays(-91);
            DateTime _dateForm3 = DateTime.Now.AddDays(-31);

            StatsOnListMatchesForPlayer _priceStats = new StatsOnListMatchesForPlayer(aIsAtp
                , aListMatch, PlayerId, aListCourtsId
                , null, _dateForm1, _dateForm2, _dateForm3, true);
            ListStats.Add(new StatReportPlayer(false, "W/L", "", "", aListMatch.Where(m => m.isCountAsWinForStats(PlayerId)).Count()
                , aListMatch.Where(m => m.isCountAsLossForStats(PlayerId) || m.isCountAsWinForStats(PlayerId)).Count(), true, 0, 0)
            );
            ListStats.Add(new StatReportPlayer(true, "ROI", "Match", "", true, _priceStats.MatchMarket.getROI()
                , _priceStats.MatchMarket.NbMarkets, 1, 0, 7, 14));

            new StatReportPlayer(false, "Sets", "", "", _nbSetsWon, _nbSetsCompleted, true, 1, 0);//not displayed
            ListStats.Add(new StatReportPlayer(false, "Set1", "Set1 vs Sets", "", _nbSets1Won, _nbSets1Completed, true
                , (_nbSetsWon*1.0)/ _nbSetsCompleted, 2, 0)
            { FormatBgColor = FormatBgColorType.ColorGreenRed });
            ListStats.Add(new StatReportPlayer(true, "ROI Set1", "", "", true, _priceStats.Set1Market.getROI()
                , _priceStats.Set1Market.NbMarkets, _priceStats.MatchMarket.getROI() / 100.0, 3, 0, 10, 20)
            { FormatBgColor = FormatBgColorType.ColorGreenRed });
            /*ListStats.Add(new StatReportPlayer("Form", "ROI Last X weeks", true
                , _listMatchWherePlayerFirst.Where(m => m.Date >= _dateForm).Sum(m => m.ProcessedResult.fNbSetsWonP1) 
                + _listMatchWherePlayerSecond.Where(m => m.Date >= _dateForm).Sum(m => m.ProcessedResult.fNbSetsWonP2)
                , ListMatch.Where(m => m.Date >= _dateForm).Sum(m => m.ProcessedResult.fNbSetsWonP2 + m.ProcessedResult.fNbSetsWonP1)
                , 4));*/
            ListStats.Add(new StatReportPlayer(false, "Form 6m", "ROI Last 6 months - ROI all period", "", true
                , _priceStats.AnySetMarketFromDate1.getROI()
                , _priceStats.AnySetMarketFromDate1.NbMarkets, _priceStats.MatchMarket.getROI()/100.0, 4, 0, 10, 20)
            { FormatBgColor = FormatBgColorType.ColorGreenRed });
            ListStats.Add(new StatReportPlayer(true, "Form 3m", "ROI Last 3 months - ROI all period", "", true
                            , _priceStats.AnySetMarketFromDate2.getROI()
                            , _priceStats.AnySetMarketFromDate2.NbMarkets, _priceStats.MatchMarket.getROI() / 100.0, 5, 0, 10, 20)
            { FormatBgColor = FormatBgColorType.ColorGreenRed });
            int _indexPos = 6;
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
                int _posOpp = (aMaxPosOpp + aMinPosOpp) / 2;
                bool _isInThisInterval = (_posOpp >= _minPosOpponent && _posOpp <= _maxPosOpponent);
                List<MatchDetailsWithOdds> _listMatchWherePlayerFirstAndOppInInterval
                    = _listMatchWherePlayerFirst.Where(m => m.PositionPlayer2 >= _minPosOpponent && _maxPosOpponent >= m.PositionPlayer2).ToList();
                List<MatchDetailsWithOdds> _listMatchWherePlayerSecondAndOppInInterval
                    = _listMatchWherePlayerSecond.Where(m => m.PositionPlayer1 >= _minPosOpponent && _maxPosOpponent >= m.PositionPlayer1).ToList();
                int _nbSetsWonvsRk = _listMatchWherePlayerFirstAndOppInInterval.Sum(m => m.ProcessedResult.fNbSetsWonP1)
                    + _listMatchWherePlayerSecondAndOppInInterval.Sum(m => m.ProcessedResult.fNbSetsWonP2)
                    + (i != _listMinPosOpponent.Count - 1 ? 0 : _listMatchWherePlayerFirstAndOppNotRanked.Sum(m => m.ProcessedResult.fNbSetsWonP1))
                    + (i != _listMinPosOpponent.Count - 1 ? 0 : _listMatchWherePlayerSecondAndOppNotRanked.Sum(m => m.ProcessedResult.fNbSetsWonP2));
                int _nbSetsVsRk = _listMatchWherePlayerFirstAndOppInInterval.Sum(m => m.ProcessedResult.fNbSetsWonP1 + m.ProcessedResult.fNbSetsWonP2)
                    + _listMatchWherePlayerSecondAndOppInInterval.Sum(m => m.ProcessedResult.fNbSetsWonP2 + m.ProcessedResult.fNbSetsWonP1)
                    + (i != _listMinPosOpponent.Count - 1 ? 0 : _listMatchWherePlayerFirstAndOppNotRanked.Sum(m => m.ProcessedResult.fNbSetsWonP1 + m.ProcessedResult.fNbSetsWonP2))
                    + (i != _listMinPosOpponent.Count - 1 ? 0 : _listMatchWherePlayerSecondAndOppNotRanked.Sum(m => m.ProcessedResult.fNbSetsWonP2 + m.ProcessedResult.fNbSetsWonP1));
                if (i == 0)
                {
                    int _nbWin = _listMatchWherePlayerFirstAndOppInInterval.Where(m => m.isCountAsWinForStats(PlayerId)).ToList().Count;
                    int _nbLoss = _listMatchWherePlayerSecondAndOppInInterval.Where(m => m.isCountAsLossForStats(PlayerId)).ToList().Count;
                    ListStats.Add(new StatReportPlayer(false, "W/L vs Rk", "", ""
                    , _nbWin
                    , _nbWin+ _nbLoss, true, _indexPos + 1 + i, 0)
                    { FormatBgColor = FormatBgColorType.ColorPlayer });
                    int _nbSets1WonWhereFirst = _listMatchWherePlayerFirstAndOppInInterval
                        .Where(m => m.isCountAsSetXWonForStats(0, PlayerId)).ToList().Count;
                    int _nbSets1LostWhereFirst = _listMatchWherePlayerFirstAndOppInInterval
                        .Where(m => m.isCountAsSetXLostForStats(0, PlayerId)).ToList().Count;
                    int _nbSets1WonWhereSecond = _listMatchWherePlayerSecondAndOppInInterval
                        .Where(m => m.isCountAsSetXWonForStats(0, PlayerId)).ToList().Count;
                    int _nbSets1LostWhereSecond = _listMatchWherePlayerSecondAndOppInInterval
                        .Where(m => m.isCountAsSetXLostForStats(0, PlayerId)).ToList().Count;
                    ListStats.Add(new StatReportPlayer(true, "Set1 vs Rk", "", ""
                    , _nbSets1WonWhereFirst+ _nbSets1WonWhereSecond
                    , _nbSets1WonWhereFirst + _nbSets1WonWhereSecond + _nbSets1LostWhereFirst + _nbSets1LostWhereSecond
                    , true, _nbSetsWonvsRk/(_nbSetsVsRk*1.0D), _indexPos + 3 + i, 0)
                    { FormatBgColor = FormatBgColorType.ColorGreenRed });
                }
                string _nameStat = "Set " + _minPosOpponent + "-" + _maxPosOpponent;
                StatReportPlayer.FormatBgColorType _format = FormatBgColorType.None;
                bool aIsSeparatorAfterCell = false;
                if (i == 0)
                {
                    _nameStat = "Set vs Rk";
                    _format = FormatBgColorType.ColorPlayer;
                }
                else if (i == _listMinPosOpponent.Count - 1)
                {
                    _nameStat = "Set " + _minPosOpponent + " + ";
                    aIsSeparatorAfterCell = true;
                }
                else if (_isInThisInterval)
                {
                    _format = FormatBgColorType.ColorPlayer;
                }
                ListStats.Add(new StatReportPlayer(aIsSeparatorAfterCell, _nameStat, "", "", _nbSetsWonvsRk, _nbSetsVsRk, true, _indexPos + 2 + i, 0)
                { FormatBgColor = _format });
            }

            List<MatchDetailsWithOdds> _listMatchWherePlayerFirstAndOppInIntervalOnCourt
                    = _listMatchWherePlayerFirst.Where(m => aListCourtsId.Contains(m.CourtId) && m.PositionPlayer2 >= aMinPosOpp 
                    && aMaxPosOpp >= m.PositionPlayer2).ToList();
            List<MatchDetailsWithOdds> _listMatchWherePlayerSecondAndOppInIntervalOnCourt
                = _listMatchWherePlayerSecond.Where(m => aListCourtsId.Contains(m.CourtId) && m.PositionPlayer1 >= aMinPosOpp 
                && aMaxPosOpp >= m.PositionPlayer1).ToList();
            int _nbSetsWonVsRkCourt = _listMatchWherePlayerFirstAndOppInIntervalOnCourt
                .Sum(m => m.ProcessedResult.fNbSetsWonP1) + _listMatchWherePlayerSecondAndOppInIntervalOnCourt
                .Sum(m => m.ProcessedResult.fNbSetsWonP2);
            int _nbSetsLostVsRkCourt = _listMatchWherePlayerSecondAndOppInIntervalOnCourt
                .Sum(m => m.ProcessedResult.fNbSetsWonP2) + _listMatchWherePlayerSecondAndOppInIntervalOnCourt
                .Sum(m => m.ProcessedResult.fNbSetsWonP1);
            string _StrStatSetsWonVsRkOnCourt = "Sets won vs Rk on court: "  
                + Math.Round(_nbSetsWonVsRkCourt*100.0 / (_nbSetsWonVsRkCourt+_nbSetsLostVsRkCourt), 0) + "%(" 
                + (_nbSetsWonVsRkCourt+_nbSetsLostVsRkCourt) + ")";
            ListStats.Add(new StatReportPlayer(false, "Sets on Court", "Sets on Court vs Sets All Courts", _StrStatSetsWonVsRkOnCourt
                , _listMatchWherePlayerFirst.Where(m=>aListCourtsId.Contains(m.CourtId)).Sum(m => m.ProcessedResult.fNbSetsWonP1) 
                + _listMatchWherePlayerSecond.Where(m => aListCourtsId.Contains(m.CourtId)).Sum(m => m.ProcessedResult.fNbSetsWonP2)
                , aListMatch.Where(m => aListCourtsId.Contains(m.CourtId)).Sum(m => m.ProcessedResult.fNbSetsWonP2 + m.ProcessedResult.fNbSetsWonP1)
                , true, _nbSetsWon/(_nbSetsCompleted*1.0), _indexPos + 3 + _listMinPosOpponent.Count - 1 + 1, 0)
            { FormatBgColor = FormatBgColorType.ColorGreenRed });
            ListStats.Add(new StatReportPlayer(true, "ROI Court", "ROI any Sets on Court vs ROI Any sets All", "", true, _priceStats.AnySetMarketOnCourt.getROI()
                , _priceStats.AnySetMarketOnCourt.NbMarkets, _priceStats.MatchMarket.getROI()/100.0, _indexPos + 3 + _listMinPosOpponent.Count - 1 + 2, 0, 6, 12)
            { FormatBgColor = FormatBgColorType.ColorGreenRed });
        }
        /// <summary>
        /// Calculate Values for html row2 (stats vs categ)
        /// </summary>
        /// <param name="aListCourtsId"></param>
        /// <param name="aMinPosOpp"></param>
        /// <param name="aMaxPosOpp"></param>
        /// <param name="aMinOdds"></param>
        /// <param name="aMaxOdds"></param>
        /// <param name="aIsBestOf5"></param>
        /// <param name="aListMatchFull"></param>
        /// <param name="PeriodValues"></param>
        public void CalculateValuesForRow2OfMatchReport(bool aIsAtp, List<int> aListCourtsId, int aMinPosOpp, int aMaxPosOpp
            , double aMinOdds, double aMaxOdds, bool aIsBestOf5
            , List<MatchDetailsWithOdds> aListMatchFull, int[] PeriodValues)
        {

            //period sets all yrs
            //ROI at trn

            //set 1 actual round 1
            List<MatchDetailsWithOdds> _listMatchWherePlayerFirst = aListMatchFull.Where(m => m.Id1 == PlayerId).ToList();
            List<MatchDetailsWithOdds> _listMatchWherePlayerSecond = aListMatchFull.Where(m => m.Id2 == PlayerId).ToList();
            DateTime _dateN = DateSince.ListDates[2].ActualValue;
            DateTime _dateN1 = DateSince.ListDates[3].ActualValue;
            DateTime _dateN2 = DateSince.ListDates[4].ActualValue;
            List<MatchDetailsWithOdds> _listMatchN = aListMatchFull.Where(m => m.Date > _dateN).ToList();
            List<MatchDetailsWithOdds> _listMatchNMinus1 = 
                aListMatchFull.Where(m => m.Date > _dateN1 && m.Date <= _dateN).ToList();
            List<MatchDetailsWithOdds> _listMatchNMinus2 = 
                aListMatchFull.Where(m => m.Date > _dateN2 && m.Date <= _dateN1).ToList();
            List<List < MatchDetailsWithOdds >> _listOfYears 
                = new List<List<MatchDetailsWithOdds>> { _listMatchN, _listMatchNMinus1, _listMatchNMinus2 };
            List<MatchDetailsWithOdds> _listMatchesInPeriod = 
                aListMatchFull.Where(m => m.isInPeriodAndCourt(PeriodValues[0], PeriodValues[1], PeriodValues[2]
                , PeriodValues[3], PeriodValues[4], PeriodValues[5], PeriodValues[6], PeriodValues[7], aListCourtsId)).ToList();
            List<MatchDetailsWithOdds> _listMatchesOnCourt = null;
            List<MatchDetailsWithOdds> _listMatchesOnCourtWherePlayerFirst = null;
            List<MatchDetailsWithOdds> _listMatchesOnCourtWherePlayerSecond = null;
            if (aListCourtsId != null)
            {
                _listMatchesOnCourt = aListMatchFull.Where(m => aListCourtsId.Contains(m.CourtId)).ToList();
                _listMatchesOnCourtWherePlayerFirst = 
                    _listMatchWherePlayerFirst.Where(m => aListCourtsId.Contains(m.CourtId)).ToList();
                _listMatchesOnCourtWherePlayerSecond = 
                    _listMatchWherePlayerSecond.Where(m => aListCourtsId.Contains(m.CourtId)).ToList();
            }
            //Sets
            int _nbSetsWon = _listMatchWherePlayerFirst.Sum(m => m.ProcessedResult.fNbSetsWonP1) 
                + _listMatchWherePlayerSecond.Sum(m => m.ProcessedResult.fNbSetsWonP2);
            int _nbSetsCompleted = aListMatchFull.Sum(m => m.ProcessedResult.fNbSetsWonP2 
                + m.ProcessedResult.fNbSetsWonP1);
            ListStatsFull.Add(new StatReportPlayer(true, "Sets", "", "", _nbSetsWon, _nbSetsCompleted, true, 0, 0));

            //Sets on court
            ListStatsFull.Add(new StatReportPlayer(false, "Sets on Court", "", ""
                , _listMatchesOnCourtWherePlayerFirst.Sum(m => m.ProcessedResult.fNbSetsWonP1)
                + _listMatchesOnCourtWherePlayerSecond.Sum(m => m.ProcessedResult.fNbSetsWonP2)
                , _listMatchesOnCourt.Sum(m => m.ProcessedResult.fNbSetsWonP2 + m.ProcessedResult.fNbSetsWonP1)
                , true, _nbSetsWon / (_nbSetsCompleted * 1.0), 1, 0)
            { FormatBgColor = FormatBgColorType.ColorGreenRed });
            
            //ROI on court
            StatsOnListMatchesForPlayer _priceStatsOnCourt = new StatsOnListMatchesForPlayer(aIsAtp, _listMatchesOnCourt
                , PlayerId, aListCourtsId
                , null, null, null, null);
            ListStatsFull.Add(new StatReportPlayer(true, "ROI Court", "ROI any sets on Court vs ROI any sets All", ""
                , true, _priceStatsOnCourt.AnySetMarket.getROI()
                , _priceStatsOnCourt.AnySetMarket.NbMarkets, 1, 2, 0, 8, 16)
            { FormatBgColor = FormatBgColorType.ColorGreenRed });

            //ROI in period
            StatsOnListMatchesForPlayer _priceStatsInPeriod = new StatsOnListMatchesForPlayer
                (aIsAtp, _listMatchesInPeriod
                , PlayerId, aListCourtsId
                , null, null, null, null);
            ListStatsFull.Add(new StatReportPlayer(true, "ROI in Period", "ROI in Period", "", true
                , _priceStatsInPeriod.AnySetMarket.getROI()
                , _priceStatsInPeriod.AnySetMarket.NbMarkets, 1,  4, 0, 9, 18)
            { FormatBgColor = FormatBgColorType.ColorGreenRed });

            //Sets vs rk
            List<MatchDetailsWithOdds> _listMatchesRk_PlayerFirst
                    = _listMatchWherePlayerFirst.Where( m => 
                     m.PositionPlayer2 >= aMinPosOpp && aMaxPosOpp >= m.PositionPlayer2).ToList();
            List<MatchDetailsWithOdds> _listMatchesRk_PlayerSecond
                    = _listMatchWherePlayerSecond.Where(m =>
                    m.PositionPlayer1 >= aMinPosOpp && aMaxPosOpp >= m.PositionPlayer1).ToList();
            int _nbSetsWonVsRk = _listMatchesRk_PlayerFirst.Sum(m => m.ProcessedResult.fNbSetsWonP1)
                    + _listMatchesRk_PlayerSecond.Sum(m => m.ProcessedResult.fNbSetsWonP2);
            int _nbSetsLostVsRk = _listMatchesRk_PlayerFirst.Sum(m => m.ProcessedResult.fNbSetsWonP2)
                                + _listMatchesRk_PlayerSecond.Sum(m => m.ProcessedResult.fNbSetsWonP1);
            int _nbSets1WonVsRk = _listMatchesRk_PlayerFirst.Where(m => m.isCountAsSetXWonForStats(0, PlayerId)).ToList().Count
                + _listMatchesRk_PlayerSecond.Where(m => m.isCountAsSetXWonForStats(0, PlayerId)).ToList().Count;
            int _nbSets1CompletedVsRk = _listMatchesRk_PlayerFirst.Where(m => m.ProcessedResult.fNbSetsWonP2
                + m.ProcessedResult.fNbSetsWonP1 >= 1).ToList().Count() 
                + _listMatchesRk_PlayerSecond.Where(m => m.ProcessedResult.fNbSetsWonP2
                + m.ProcessedResult.fNbSetsWonP1 >= 1).ToList().Count();
            double _aRefSetsVsRk = ListStats.FirstOrDefault(s => s.NameStat == "Set vs Rk").Percentage;
            ListStatsFull.Add(new StatReportPlayer(false, "Sets vs Rk", "% sets won vs Rk", "Set 1 vs Rk :"
                + Math.Round(100.0 * _nbSets1WonVsRk / _nbSets1CompletedVsRk) + "%"
                    , _nbSetsWonVsRk, _nbSetsWonVsRk + _nbSetsLostVsRk, true, 5, 0));

            List<MatchDetailsWithOdds> _listMatchesRkOncourt_PlayerFirst = 
                _listMatchesRk_PlayerFirst.Where(m => aListCourtsId.Contains(m.CourtId)).ToList();
            List<MatchDetailsWithOdds> _listMatchesRkOncourt_PlayerSecond = 
                _listMatchesRk_PlayerSecond.Where(m => aListCourtsId.Contains(m.CourtId)).ToList();
            int _nbSetsWonVsRkOnCourt = _listMatchesRkOncourt_PlayerFirst.Sum(m => m.ProcessedResult.fNbSetsWonP1)
                    + _listMatchesRkOncourt_PlayerSecond.Sum(m => m.ProcessedResult.fNbSetsWonP2);
            int _nbSetsLostVsRkOnCourt = _listMatchesRkOncourt_PlayerFirst.Sum(m => m.ProcessedResult.fNbSetsWonP2)
                                + _listMatchesRkOncourt_PlayerSecond.Sum(m => m.ProcessedResult.fNbSetsWonP1);
            ListStatsFull.Add(new StatReportPlayer(true, "Sets vs Rk on court", "% sets won vs Rk on court", ""
                    , _nbSetsWonVsRkOnCourt, _nbSetsWonVsRkOnCourt + _nbSetsLostVsRkOnCourt
                    , true, _nbSetsWonVsRk / ((_nbSetsWonVsRk + _nbSetsLostVsRk) * 1.0), 5, 0)
                    {FormatBgColor = FormatBgColorType.ColorGreenRed });
            
            for (int i = 0; i <= 0; i++)
            {
                int _nbWonForSet1 = aListMatchFull.Where(m => m.isCountAsSetXWonForStats(i, PlayerId)
                && m.ProcessedResult.fNbSetsWonP1 + m.ProcessedResult.fNbSetsWonP2 == 3 
                && !m.getIsBestOf5()).Count();
                int _nbAll = aListMatchFull.Where(m =>
                   m.ProcessedResult.fNbSetsWonP1 + m.ProcessedResult.fNbSetsWonP2 == 3
                && !m.getIsBestOf5()).Count();
                int _nbWonForSet3 = aListMatchFull.Where(m => m.isCountAsSetXWonForStats(2, PlayerId)
                && m.ProcessedResult.fNbSetsWonP1 + m.ProcessedResult.fNbSetsWonP2 == 3
                && !m.getIsBestOf5()).Count();
                double _set1Pct = Math.Round((_nbWonForSet1) / (_nbAll * 1.0) * 100.0, 0);
                double _set2Pct = Math.Round((_nbAll - _nbWonForSet1) / (_nbAll * 1.0)*100.0, 0);
                double _set3Pct = Math.Round(_nbWonForSet3 / (_nbAll * 1.0) * 100.0, 0);
                StatReportPlayer _s = new StatReportPlayer(true, "Set" + (i + 1) + " all"
                    , "Sets Won % (for matches ended 2-1 or 1-2)"
                    , "Set 1: " + (_set1Pct) + "% ;Set 2: " + (_set2Pct) + "% ;Set3: " + (_set3Pct) + "%"
                    , _nbWonForSet1, _nbAll, true, 0.50
                    , 6 + i, 0);
                ListStatsFull.Add(_s);
                if (i!=2)
                    _s.FormatBgColor = FormatBgColorType.ColorGreenRed;
            }
            int _lastIndexDisplay = 7;
            //ROI at odds
            List<MatchDetailsWithOdds> _listMatchesAtOdds =
                aListMatchFull.Where(m => m.isFilterOdds(Convert.ToDecimal(aMinOdds), Convert.ToDecimal(aMaxOdds), PlayerId)).ToList();
            StatsOnListMatchesForPlayer _priceStatsAtOdds 
                = new StatsOnListMatchesForPlayer(aIsAtp, _listMatchesAtOdds, PlayerId, null
                , null, null, null, null, true);
            int _nbWinAtOdds = _listMatchesAtOdds.Where(m => m.isCountAsWinForStats(PlayerId)).Count();
            int _pctWin = 0;
            if (_priceStatsAtOdds.MatchMarket.NbMarkets > 0)
                _pctWin = _nbWinAtOdds *100 / _priceStatsAtOdds.MatchMarket.NbMarkets;
            ListStatsFull.Add(new StatReportPlayer(false, "ROI Match (odds)", "ROI for match at this odds intervall [" 
                + aMinOdds + "-" + aMaxOdds + "]"
                , "W/L:" + _pctWin + "% ;All sets ROI:" + _priceStatsAtOdds.AnySetMarket.getROI(), true
                , _priceStatsAtOdds.MatchMarket.getROI()
                , _priceStatsAtOdds.MatchMarket.NbMarkets, 1, _lastIndexDisplay+1, 0, 10, 20)
            { FormatBgColor = FormatBgColorType.ColorGreenRed });
            ListStatsFull.Add(new StatReportPlayer(true, "ROI Set1 (odds)", "ROI for match at this odds intervall [" 
                        + aMinOdds + "-" + aMaxOdds + "]"
                            , "", true, _priceStatsAtOdds.Set1Market.getROI()
                            , _priceStatsAtOdds.Set1Market.NbMarkets, 1, _lastIndexDisplay + 3, 0, 6, 12)
            { FormatBgColor = FormatBgColorType.ColorGreenRed });

            //List<MatchDetailsWithOdds> _listMatchesWonAtOdds =
            //    _listMatchesAtOdds.Where(m => m.isCountAsWinForStats(PlayerId)).ToList();
            //PriceStatsListMatchesForPlayer _priceStatsWhenWonAtOdds = new PriceStatsListMatchesForPlayer(_listMatchesWonAtOdds, PlayerId, null
            //    , null, null, null, null, true);
            int _roiX_0 = _priceStatsAtOdds.SetBettingMarketX_0.getROI();
            int _roiNotX_0 = _priceStatsAtOdds.SetBettingMarketNotX_0.getROI();
            //int _nbX_0AtOdds = _listMatchesWonAtOdds.Where(m => m.isCountAsSetBettingWonForStats(aIsBestOf5?3:2,0, PlayerId)).Count();
            //int _pctX_0 = 0;
            //if (_priceStatsAtOdds.SetBettingMarketX_0.NbMarkets > 0)
            //    _pctX_0 = _nbX_0AtOdds * 100 / _priceStatsAtOdds.SetBettingMarketX_0.NbMarkets;
            ListStatsFull.Add(new StatReportPlayer(false, "ROI " +(aIsBestOf5?"3":"2")+"-0 (odds)", "ROI for set betting at this odds intervall [" 
                + aMinOdds + "-" + aMaxOdds + "]"
                , /*"W/L:" + _pctX_0 + "% ;"*/"ROI won straight sets = " + _roiX_0 + "; ROI not straight = " + _roiNotX_0 + "; at match odds[" + aMinOdds + " - " + aMaxOdds + "]", true
                , _roiX_0 - _roiNotX_0
                , _priceStatsAtOdds.SetBettingMarketX_0.NbMarkets
                , 0, _lastIndexDisplay + 4, 0, 10, 20)
            { FormatBgColor = FormatBgColorType.ColorGreenRed });
            //int _nb0_XAtOdds = _listMatchesAtOdds.Where(m => m.isCountAsSetBettingWonForStats(0, aIsBestOf5 ? 3 : 2, PlayerId)).Count();
            //int _pct0_X = 0;
            //if (_priceStatsAtOdds.SetBettingMarket0_X.NbMarkets > 0)
            //    _pct0_X = _nb0_XAtOdds * 100 / _priceStatsAtOdds.SetBettingMarket0_X.NbMarkets;
            //List<MatchDetailsWithOdds> _listMatchesLostAtOdds =
            //    _listMatchesAtOdds.Where(m => m.isCountAsLossForStats(PlayerId)).ToList();
            int _roi0_X = _priceStatsAtOdds.SetBettingMarket0_X.getROI();
            int _roiNot0_X = _priceStatsAtOdds.SetBettingMarketNot0_X.getROI();
            //PriceStatsListMatchesForPlayer _priceStatsWhenLostAtOdds = new PriceStatsListMatchesForPlayer(_listMatchesLostAtOdds, PlayerId, null
            //    , null, null, null, null, true);
            ListStatsFull.Add(new StatReportPlayer(true, "ROI 0-" + (aIsBestOf5 ? "3" : "2") 
                + " (odds)", "ROI for set betting at this odds intervall [" + aMinOdds + "-" + aMaxOdds + "]"
                , /*"W/L:" + _pctX_0 + "% ;"*/"ROI lost straight sets = " + _roi0_X + "; ROI not straight = " 
                + _roiNot0_X + "; at match odds[" + aMinOdds + " - " + aMaxOdds + "]", true
                , _roi0_X - _roiNot0_X
                , _priceStatsAtOdds.SetBettingMarket0_X.NbMarkets
                 , 0, _lastIndexDisplay + 5, 0, 10, 20)
            { FormatBgColor = FormatBgColorType.ColorGreenRed });
            
            _lastIndexDisplay = _lastIndexDisplay + 7;
            StatsOnListMatchesForPlayer _priceStats = new StatsOnListMatchesForPlayer(aIsAtp, aListMatchFull, PlayerId
                , aListCourtsId, ListCategoriesOpp, null, null, null, true);
            //CATEG
            for (int i = 1; i <= ListCategories.Count; i++)
            {
                double diffForAnySetROI = _priceStats.AnySetMarketByCategories[i - 1].getROI() 
                    - _priceStats.AnySetMarket.getROI();
                double diffForMatchROI = _priceStats.MatchMarketByCategories[i - 1].getROI() 
                    - _priceStats.MatchMarket.getROI();
                bool _isBiggestDiffForMatchThanForAnySet = Math.Abs(diffForAnySetROI) < Math.Abs(diffForMatchROI);

                string _toopTip = "Match: " + diffForMatchROI + " % (" 
                    + _priceStats.MatchMarketByCategories[i - 1].getROI() + "v" + _priceStats.MatchMarket.getROI() + ")"
                    +"\nAny set: " + diffForAnySetROI + " % (" + _priceStats.AnySetMarketByCategories[i - 1].getROI() 
                    + "v" + _priceStats.AnySetMarket.getROI() + ")";

                StatReportPlayer _s = (new StatReportPlayer(false, ListCategories[i]
                    , "", _toopTip, true, _priceStats.AnySetMarketByCategories[i - 1].getROI()
                    , _priceStats.AnySetMarketByCategories[i - 1].NbMarkets//, _priceStats.AnySetMarket.getROI() / 100.0
                    , _lastIndexDisplay + i - 1, 0, 8, 16));
                _s.DifferenceWithReference = diffForMatchROI;
                if (! _isBiggestDiffForMatchThanForAnySet)
                    _s.DifferenceWithReference = diffForAnySetROI;

                if (ListCategoriesOpp!=null && ListCategoriesOpp.Contains(i))
                    _s.FormatBgColor = FormatBgColorType.ColorPlayerAndGreenRed;
                else
                    _s.FormatBgColor = FormatBgColorType.ColorGreenRed;
                ListStatsFull.Add(_s);
            }
        }

        public string getHtmlForRowsOfMatchReport(bool aisHeader, int aIndexPlayerBase1, List<StatReportPlayer> aLisStats
            , string aTitle, bool aIsSmallHeader=false)
        {
            string _html = "<tr style='font-size:12px' height='20px'><td width='65px' class='p" + aIndexPlayerBase1 
                + "' title='" + aTitle + "'>" + PositionPlayer + " - " + PlayerName + "</td>";
            if (aisHeader)
                _html = "<tr style='font-weight:600;font-size:11px' height='20px' bgcolor='lightyellow'><td>"+aTitle+"</td>";
            if (aIsSmallHeader)
                _html = "<tr style='font-weight:600;font-size:11px' height='20px' bgcolor='lightyellow'><td style='font-weight:300;font-size:9px'>" + aTitle + "</td>";
            try
            {
                foreach (StatReportPlayer _s in aLisStats.OrderBy(m => m.IndexDisplay))
                {
                    if (aisHeader)
                        _html += _s.getHtmlForHeaderCell();
                    else
                        _html += _s.getHtmlForCell(aIndexPlayerBase1);
                }
            }
            catch (Exception e)
            {
            }
            return _html + "</tr>";
        }
    }
}
