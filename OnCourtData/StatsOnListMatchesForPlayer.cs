using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace OnCourtData
{
    public class PercentageStatForReport
    {
        public string NameStat { get; set; }
        /// <summary>
        ///
        /// </summary>
        public int IndexStat { get; set; } = -1;
       
        //public enum TypeMarket
        //{
        //    Match, Set1, Set2, Set3, Set4, Set5, SetBetting3Sets, SetBetting5sets, GameHandicap, GameTotals
        //}
        public int NbPlayed { get; set; }
        public int NbWon { get; set; }
        public double PercentageWonInDecimal
        {
            get
            {
                if (NbPlayed > 0)
                    return NbWon*1.0F/NbPlayed;
                else
                    return 0;
            }
        }
        public int PercentageWonInt
        {
            get
            {
                return (int)(PercentageWonInDecimal * 100);
            }
        }
        /// <summary>
        /// 9999 if not set
        /// </summary>
        public int ReferenceValueToCompareWith { get; set; }
        public PercentageStatForReport()
        {
            ReferenceValueToCompareWith = 9999;
            NbWon = 0;
            NbPlayed = 0;
        }
        public override string ToString()
        {
            return (this.PercentageWonInt + " %" + " (" + NbPlayed + ")");
        }
    }

    public class ROIStatForReport
    {
        public string NameStat { get; set; }
        /// <summary>
        /// 1 to 9 = 9 categ
        /// 19=match
        /// 10=set1
        /// 11=any set
        /// 12=X 0, 13, 14=0 X, 15
        ///
        /// </summary>
        public int IndexStat { get; set; } = -1;
        public bool IsRoi { get; set; }
        
        //public enum TypeMarket
        //{
        //    Match, Set1, Set2, Set3, Set4, Set5, SetBetting3Sets, SetBetting5sets, GameHandicap, GameTotals
        //}
        public int NbMarkets { get; set; }
        public double TotalStake { get; set; }
        public double TotalProfit { get; set; }
        public int getROI()
        {
            if (NbMarkets > 0 && TotalStake>0)
                return Convert.ToInt32(Math.Round((TotalProfit * 100 / TotalStake) + 100, 0));
            else
                return 0;
        }
        public ROIStatForReport()
        {
            TotalStake = 0;
            TotalProfit = 0;
            NbMarkets = 0;
        }
        public override string ToString()
        {
            return (this.getROI() + " €" + " (" + NbMarkets + ")");
        }
    }

    /// <summary>
    /// Include ROI and % stats for a player
    /// </summary>
    [Serializable]
    public class StatsOnListMatchesForPlayer
    {
        public long PlayerId { get; set; }
        public int PlayerRank { get; set; }
        public string PlayerName { get; set; }
        public string PlayerNote { get; set; }

        [NonSerialized]
        static List<double> _listMatchPricesBo3 = new List<double>() { 2.00,  1.92, 1.85, 1.77, 1.69, 1.64, 1.59, 1.55, 1.50, 1.46, 1.43, 1.39, 1.36, 1.33, 1.30, 1.28, 1.26, 1.23, 1.22, 1.20, 1.18, 1.17, 1.15, 1.14, 1.13, 1.12, 1.11, 1.10, 1.089, 1.082, 1.075, 1.067, 1.062, 1.054, 1.049, 1.045, 1.041, 1.030, 1.018, 1.010, 1.005, 1 };
        [NonSerialized]
        static List<double> _listSet1PricesBo3 = new List<double>() {  2.00,  1.93, 1.87, 1.82, 1.77, 1.72, 1.68, 1.63, 1.60, 1.56, 1.53, 1.50, 1.47, 1.44, 1.41, 1.39, 1.37, 1.35, 1.33, 1.31, 1.29, 1.27, 1.26, 1.25, 1.23, 1.22, 1.20, 1.19, 1.18, 1.17, 1.16, 1.15, 1.14, 1.12, 1.12, 1.11, 1.10, 1.09, 1.06, 1.04, 1.03, 1 };
        [NonSerialized]
        static List<double> _list2sets_0PricesBo3 = new List<double>(){ 3.65, 3.46, 3.25, 3.07, 2.90, 2.77, 2.64, 2.53, 2.42, 2.31, 2.23, 2.15, 2.06, 1.99, 1.93, 1.86, 1.81, 1.75, 1.71, 1.66, 1.62, 1.58, 1.54, 1.52, 1.47, 1.44, 1.42, 1.39, 1.36, 1.34, 1.32, 1.30, 1.28, 1.26, 1.24, 1.22, 1.21, 1.17, 1.12, 1.08, 1.05, 1 };
        [NonSerialized]
        static List<double> _list0sets_2PricesBo3 = new List<double>() { 3.65, 3.88, 4.10, 4.39, 4.69, 5.00, 5.29, 5.71, 6.10, 6.58, 6.99, 7.58, 8.13, 8.70, 9.35, 10.10, 10.87, 11.90, 12.66, 13.89, 14.93, 16.39, 17.86, 19.23, 21.28, 23.26, 25.00, 27.78, 30.30, 33.33, 37.04, 41.67, 43.48, 50.00, 55.56, 62.50, 71.43, 100.00, 166.67, 333.33, 1000.00, 1000 };
        [NonSerialized]
        static List<double> _listMatchPricesBo5 = new List<double>() { 2.00, 1.83, 1.68, 1.56, 1.47, 1.36, 1.28, 1.21, 1.16, 1.12, 1.076, 1.056, 1.035, 1.021, 1.009, 1.000 };
        [NonSerialized]
        static List<double> _listSet1PricesBo5 = new List<double>() { 2.00, 1.87, 1.77, 1.68, 1.60, 1.50, 1.41, 1.35, 1.29, 1.25, 1.19, 1.16, 1.12, 1.09, 1.06, 1.00 };
        [NonSerialized]
        static List<double> _list3sets_0PricesBo5 = new List<double>() { 5.43, 4.65, 4.07, 3.58, 3.21, 2.75, 2.39, 2.12, 1.91, 1.75, 1.55, 1.45, 1.34, 1.25, 1.16, 1.00 };
        [NonSerialized]
        static List<double> _list0sets_3PricesBo5 = new List<double>() { 5.43, 6.29, 7.41, 8.77, 10.31, 13.51, 17.24, 23.26, 32.26, 43.48, 66.67, 100.00, 166.67, 200, 200.00, 200.00 };
        
        public static double getSet1PriceFromMatchPrice(double aMatchPrice, bool aIsBestOf5, ref double aPriceStraightSetsP1
            , ref double aPriceStraightSetsP2)
        {
            List<double> _listMatchPrices = _listMatchPricesBo3;
            List<double> _listSet1Prices = _listSet1PricesBo3;
            if (aIsBestOf5)
            {
                _listMatchPrices = _listMatchPricesBo5;
                _listSet1Prices = _listSet1PricesBo5;
            }
            if (aMatchPrice <= 2)
            {
                int _index = 0;
                double _matchPriceForIndex = _listMatchPrices[0];
                bool _isFound = false;
                while (_index <= _listMatchPrices.Count-1 && ! _isFound)
                {
                    if ((_listMatchPrices[_index] >= aMatchPrice)
                    && (_listMatchPrices[_index + 1] <= aMatchPrice))
                    {
                            double _partLine2 = (_listMatchPrices[_index] - aMatchPrice)
                                / (_listMatchPrices[_index] - _listMatchPrices[_index + 1]);
                            double _partLine1 = (aMatchPrice - _listMatchPrices[_index + 1])
                                / (_listMatchPrices[_index] - _listMatchPrices[_index + 1]);
                        if (! aIsBestOf5)
                        {
                            aPriceStraightSetsP1 = _partLine2 * _list2sets_0PricesBo3[_index + 1] / 1.06
                                + _list2sets_0PricesBo3[_index] * _partLine1 / 1.06;
                            aPriceStraightSetsP2 = _partLine2 * _list0sets_2PricesBo3[_index + 1] / 1.06
                                + _list0sets_2PricesBo3[_index] * _partLine1 / 1.06;
                        }
                        else
                        {
                            aPriceStraightSetsP1 = _partLine2 * _list3sets_0PricesBo5[_index + 1] / 1.06
                                + _list3sets_0PricesBo5[_index] * _partLine1 / 1.06;
                            aPriceStraightSetsP2 = _partLine2 * _list0sets_3PricesBo5[_index + 1] / 1.06
                                + _list0sets_3PricesBo5[_index] * _partLine1 / 1.06;
                        }
                        return _partLine2 * _listSet1Prices[_index + 1] + _listSet1Prices[_index] * _partLine1;
                    }
                    _index++;
                }
                if (!_isFound)
                    MessageBox.Show("Error!");
            }
            else
            {
                double _oppositeMatchPriceToFind = 1 / (1 - 1 / aMatchPrice);
                return (1/(1-1/(getSet1PriceFromMatchPrice(_oppositeMatchPriceToFind, aIsBestOf5, ref aPriceStraightSetsP2
                    , ref aPriceStraightSetsP1))));
            }
            return 0;
        }
        //private List<MatchDetailsWithOdds> ListMatches;
        public ROIStatForReport MatchMarket { get; set; }
        public ROIStatForReport MatchMarketOnCourt { get; set; }
        public ROIStatForReport AnySetMarketOnCourt { get; set; }
        public List<ROIStatForReport> MatchMarketByCategories { get; set; }
        public List<ROIStatForReport> AnySetMarketByCategories { get; set; }
        public List<ROIStatForReport> MatchMarketByCategoriesOnCourt { get; set; }
        public ROIStatForReport AnySetMarketFromDate1 { get; set; }
        public ROIStatForReport AnySetMarketFromDate2 { get; set; }
        public ROIStatForReport AnySetMarketFromDate3 { get; set; }
        public ROIStatForReport Set1Market { get; set; }
        public ROIStatForReport AnySetMarket { get; set; }
        public ROIStatForReport AnySetMarketOnHardCourt { get; set; }
        public ROIStatForReport AnySetMarketOnClayCourt { get; set; }
        public ROIStatForReport AnySetMarketOnIndoorOrCarpetCourt { get; set; }
        public ROIStatForReport AnySetMarketOnGrassCourt { get; set; }
        public ROIStatForReport SetBettingMarketX_0 { get; set; }
        public ROIStatForReport SetBettingMarket0_X { get; set; }
        public ROIStatForReport SetBettingMarketNotX_0 { get; set; }
        public ROIStatForReport SetBettingMarketNot0_X { get; set; }

        /// <summary>
        /// index 100
        /// </summary>
        public PercentageStatForReport SetsWon { get; set; }
        /// <summary>
        /// index 101
        /// </summary>
        public PercentageStatForReport SetsWonOnHardCourt { get; set; }
        /// <summary>
        /// index 102
        /// </summary>
        public PercentageStatForReport SetsWonOnClayCourt { get; set; }
        /// <summary>
        /// index 103
        /// </summary>
        public PercentageStatForReport SetsWonOnIndoorOrCarpetCourt { get; set; }
        /// <summary>
        /// index 104
        /// </summary>
        public PercentageStatForReport SetsWonOnGrassCourt { get; set; }
        public StatsOnListMatchesForPlayer()
        { }
        /// <summary>
        /// Exclusively opeate on aIdPlayer or aPlayerInfoToSearch or aIsForFav
        /// </summary>
        /// <param name="aListMatches"></param>
        /// <param name="aIdPlayer"></param>
        /// <param name="aListCourtsId"></param>
        /// <param name="aListCategoriesIdForOpp"></param>
        /// <param name="aDateForm1"></param>
        /// <param name="aDateForm2"></param>
        /// <param name="aDateForm3"></param>
        /// <param name="aIsIncludeFirstSetOrAnySetCalc"></param>
        /// <param name="aPlayerInfoToSearch"></param>
        /// <param name="aIsForFav"></param>
        public StatsOnListMatchesForPlayer(List<MatchDetailsWithOdds> aListMatches, long aIdPlayer, List<int> aListCourtsId
            , List<int> aListCategoriesIdForOpp, DateTime? aDateForm1, DateTime? aDateForm2, DateTime? aDateForm3
            , bool aIsIncludeFirstSetOrAnySetCalc = true, List<string> aPlayerInfoToSearch=null, bool? aIsForFav = null)
        {
            PlayerId = aIdPlayer;
            MatchMarket = new ROIStatForReport() { NameStat = "Match", IsRoi = true, IndexStat = 19 };
            Set1Market = new ROIStatForReport() { NameStat = "Set1", IsRoi = true, IndexStat = 10 };
            AnySetMarket = new ROIStatForReport() { NameStat = "AnySet", IsRoi = true, IndexStat = 11 };
            AnySetMarketOnHardCourt = new ROIStatForReport() { NameStat = "AnySetHard", IsRoi = true, IndexStat = 21 };
            AnySetMarketOnClayCourt = new ROIStatForReport() { NameStat = "AnySetClay", IsRoi = true, IndexStat = 22 };
            AnySetMarketOnIndoorOrCarpetCourt = new ROIStatForReport()
                { NameStat = "AnySetIndoor", IsRoi = true, IndexStat = 23 };
            AnySetMarketOnGrassCourt = new ROIStatForReport() { NameStat = "AnySetGrass", IsRoi = true, IndexStat = 24 };
            MatchMarketOnCourt = new ROIStatForReport() { NameStat = "MatchOnCourt", IsRoi = true, IndexStat = 31 };
            AnySetMarketOnCourt = new ROIStatForReport() { NameStat = "AnySetOnCourt", IsRoi = true, IndexStat = 32 };
            AnySetMarketFromDate1 = new ROIStatForReport() { NameStat = "SinceDate1", IsRoi = true, IndexStat = 50 };
            AnySetMarketFromDate2 = new ROIStatForReport() { NameStat = "SinceDate2", IsRoi = true, IndexStat = 51 };
            AnySetMarketFromDate3 = new ROIStatForReport() { NameStat = "SinceDate3", IsRoi = true, IndexStat = 52 };
            SetBettingMarketX_0 = new ROIStatForReport() { NameStat = "SetsX_0", IsRoi = true, IndexStat = 12 };
            SetBettingMarket0_X = new ROIStatForReport() { NameStat = "Sets0_X", IsRoi = true, IndexStat = 14 };
            SetBettingMarketNotX_0 = new ROIStatForReport() { NameStat = "SetsNotX_0", IsRoi = true, IndexStat = 13 };
            SetBettingMarketNot0_X = new ROIStatForReport() { NameStat = "SetsNot0_X", IsRoi = true, IndexStat = 15 };
            if (aListCategoriesIdForOpp != null)
            {
                MatchMarketByCategories = new List<ROIStatForReport>();
                foreach (int _catId in Categorie.fListCategories.Keys)
                    MatchMarketByCategories.Add(new ROIStatForReport()
                    {
                        NameStat = Categorie.fListCategories[_catId]
                        ,
                        IsRoi = true,
                        IndexStat = _catId
                    });
                MatchMarketByCategoriesOnCourt = new List<ROIStatForReport>();
                foreach (int _catId in Categorie.fListCategories.Keys)
                    MatchMarketByCategoriesOnCourt.Add(new ROIStatForReport()
                    {
                        NameStat = Categorie.fListCategories[_catId]
                        ,
                        IsRoi = true,
                        IndexStat = _catId
                    });
                AnySetMarketByCategories = new List<ROIStatForReport>();
                foreach (int _catId in Categorie.fListCategories.Keys)
                    AnySetMarketByCategories.Add(new ROIStatForReport() { NameStat = Categorie.fListCategories[_catId]
                        , IsRoi = true, IndexStat = _catId
                    });

            }
            getRoiListMatches(aListMatches, aIdPlayer, aListCourtsId, aListCategoriesIdForOpp, aPlayerInfoToSearch
                , aIsForFav);
            if (aIsIncludeFirstSetOrAnySetCalc || aDateForm1 != null || aDateForm2 != null || aDateForm3 != null)
                getRoiForExtraStats(aListMatches, aDateForm1, aDateForm2
                , aDateForm3, aIdPlayer, aListCategoriesIdForOpp, aListCourtsId, aPlayerInfoToSearch, aIsForFav);
            CalculatePercentageStats(aListMatches, aIdPlayer);
        }
        private void CalculatePercentageStats(List<MatchDetailsWithOdds> aListMatchFull, long aPlayerId)
        {
            foreach (MatchDetailsWithOdds match in aListMatchFull)
            {
                if (match.ProcessedResult == null)
                    match.readResult();
            }
            List<MatchDetailsWithOdds> _listMatchWherePlayerFirst = 
                aListMatchFull.Where(m => m.Id1 == aPlayerId).ToList();
            List<MatchDetailsWithOdds> _listMatchWherePlayerSecond = 
                aListMatchFull.Where(m => m.Id2 == aPlayerId).ToList();
            DateTime _dateN = DateSince.ListDates[2].ActualValue;
            DateTime _dateN1 = DateSince.ListDates[3].ActualValue;
            DateTime _dateN2 = DateSince.ListDates[4].ActualValue;
            List<MatchDetailsWithOdds> _listMatchN = aListMatchFull.Where(m => m.Date > _dateN).ToList();
            List<MatchDetailsWithOdds> _listMatchNMinus1 =
                aListMatchFull.Where(m => m.Date > _dateN1 && m.Date <= _dateN).ToList();
            List<MatchDetailsWithOdds> _listMatchNMinus2 =
                aListMatchFull.Where(m => m.Date > _dateN2 && m.Date <= _dateN1).ToList();
            List<List<MatchDetailsWithOdds>> _listOfYears
                = new List<List<MatchDetailsWithOdds>> { _listMatchN, _listMatchNMinus1, _listMatchNMinus2 };
            //SETS won
            int _nbSetsWon = 0;
            if (_listMatchWherePlayerFirst != null && _listMatchWherePlayerSecond != null)
            {
                _nbSetsWon = _listMatchWherePlayerFirst.Sum(m => m.ProcessedResult.fNbSetsWonP1)
                + _listMatchWherePlayerSecond.Sum(m => m.ProcessedResult.fNbSetsWonP2);
            }
            int _nbSetsCompleted = aListMatchFull.Sum(m => m.ProcessedResult.fNbSetsWonP2
                + m.ProcessedResult.fNbSetsWonP1);
            SetsWon = new PercentageStatForReport() {IndexStat = 101, NbPlayed = _nbSetsCompleted
            , NbWon = _nbSetsWon};

            //SETS won on each court
            for (int i = 1; i < 5; i++)
            {
                List<MatchDetailsWithOdds> _listMatchesOnCourtX = null;
                List<MatchDetailsWithOdds> _listMatchesOnCourtWherePlayerFirst = null;
                List<MatchDetailsWithOdds> _listMatchesOnCourtWherePlayerSecond = null;
                PercentageStatForReport _referenceToStatObject = this.SetsWonOnHardCourt;
                List<int> listCourtsId = new List<int>() { 1 };
                int indexStat = 101;
                string nameStat = "H";
                switch (i)
                {
                    case 1:
                        _referenceToStatObject = this.SetsWonOnHardCourt;
                        listCourtsId = new List<int>() { 1 };
                        indexStat = 101;
                        nameStat = "H";
                        break;
                    case 2:
                        _referenceToStatObject = this.SetsWonOnClayCourt;
                        listCourtsId = new List<int>() { 2 };
                        indexStat = 102;
                        nameStat = "C";
                        break;
                    case 3:
                        _referenceToStatObject = this.SetsWonOnIndoorOrCarpetCourt;
                        listCourtsId = new List<int>() { 3,4,6 };
                        indexStat = 103;
                        nameStat = "I";
                        break;
                    case 4:
                        _referenceToStatObject = this.SetsWonOnGrassCourt;
                        listCourtsId = new List<int>() { 5 };
                        indexStat = 104;
                        nameStat = "G";
                        break;
                    default:
                        break;
                }

                _listMatchesOnCourtX = aListMatchFull.Where(m => listCourtsId.Contains(m.CourtId)).ToList();
                _listMatchesOnCourtWherePlayerFirst =
                    _listMatchWherePlayerFirst.Where(m => listCourtsId.Contains(m.CourtId)).ToList();
                _listMatchesOnCourtWherePlayerSecond =
                    _listMatchWherePlayerSecond.Where(m => listCourtsId.Contains(m.CourtId)).ToList();
                PercentageStatForReport statObject = new PercentageStatForReport()
                {
                    IndexStat = indexStat
                    ,
                    NbPlayed = _listMatchesOnCourtX.Sum(m => m.ProcessedResult.fNbSetsWonP2 + m.ProcessedResult.fNbSetsWonP1)
                    ,
                    NbWon = _listMatchesOnCourtWherePlayerFirst.Sum(m => m.ProcessedResult.fNbSetsWonP1)
                    + _listMatchesOnCourtWherePlayerSecond.Sum(m => m.ProcessedResult.fNbSetsWonP2)
                    ,
                    ReferenceValueToCompareWith = SetsWon.PercentageWonInt
                    ,
                    NameStat = "Sets on " + nameStat
                };
                switch(i)
                {
                    case 1:
                        this.SetsWonOnHardCourt = statObject;
                    break;
                    case 2:
                        this.SetsWonOnClayCourt = statObject;
                    break;
                    case 3:
                        this.SetsWonOnIndoorOrCarpetCourt = statObject;
                    break;
                    case 4:
                        this.SetsWonOnGrassCourt = statObject;
                    break;
                    default:
                        break;
                }
            }
        }
        public override string ToString()
        {
            return (MatchMarket.getROI() + " €" + " (" + MatchMarket.NbMarkets + ")");
        }
        private void getRoiForExtraStats(List<MatchDetailsWithOdds> aListMatches
            , DateTime? aDateForm1, DateTime? aDateForm2, DateTime? aDateForm3
            , long aIdPlayer
            , List<int> aListCategoriesId, List<int> aListCourtsId
            , List<string> aPlayerInfoToSearch = null, bool? aIsForFav = null)
        {
            //TODO get set 1 price from match price!
            foreach (MatchDetailsWithOdds m in aListMatches)
            {
                if (!m.Odds1.HasValue || !m.Odds1.HasValue || m.Odds1.Value < 1 || m.Odds2.Value < 1)
                { }
                else
                {
                    bool _isPlayerFirst = true;
                    if (aIdPlayer != -1)
                        _isPlayerFirst = m.isPlayerFirst(aIdPlayer);
                    if (aPlayerInfoToSearch != null)
                        _isPlayerFirst = (aPlayerInfoToSearch.IndexOf(m.Player1Info) > -1);
                    if (aIsForFav != null)
                        _isPlayerFirst = m.Odds1 < m.Odds2;
                    double _stakeSet1 = 0;
                    double _profitSet1 = 0;
                    double _oddsMatchP1 = m.Odds1.Value;
                    double _oddsMatchP2 = m.Odds2.Value;
                    double _priceP1WonInStraightSets = 0;
                    double _priceP2WonInStraightSets = 0;
                    if (!_isPlayerFirst)
                    {
                        _oddsMatchP1 = m.Odds2.Value;
                        _oddsMatchP2 = m.Odds1.Value;
                    }
                    double _oddsSet1 = getSet1PriceFromMatchPrice(_oddsMatchP1, m.getIsBestOf5()
                        , ref _priceP1WonInStraightSets, ref _priceP2WonInStraightSets);
                    double _priceP1WonNotInStraightSets = 1 / (1 / _oddsMatchP1 - 1 / _priceP1WonInStraightSets);
                    double _priceP2WonNotInStraightSets = 1 / (1 / _oddsMatchP2 - 1 / _priceP2WonInStraightSets);
                    //Set 1
                    bool _isSetWon = false;
                    if (aIdPlayer != -1)
                        _isSetWon = m.isCountAsSetXWonForStats(0, aIdPlayer);
                    if (aPlayerInfoToSearch != null)
                        _isSetWon = m.isCountAsSetXWonForStats(0, aPlayerInfoToSearch);
                    if (aIsForFav != null)
                    {
                        if (aIsForFav.Value)
                            _isSetWon = m.isCountAsSetXWonForStatsForFav(0);
                        else
                            _isSetWon = m.isCountAsSetXWonForStatsForDog(0);
                    }
                    bool _isSetLost = false;
                    if (aIdPlayer != -1)
                        _isSetLost = m.isCountAsSetXLostForStats(0, aIdPlayer);
                    if (aPlayerInfoToSearch != null)
                        _isSetLost = m.isCountAsSetXLostForStats(0, aPlayerInfoToSearch);
                    if (aIsForFav != null)
                    {
                        if (aIsForFav.Value)
                            _isSetLost = m.isCountAsSetXLostForStatsForFav(0);
                        else
                            _isSetLost = m.isCountAsSetXLostForStatsForDog(0);
                    }
                    if (_isSetWon)
                    {
                        _stakeSet1 = 1 / _oddsSet1;
                        _profitSet1 = _stakeSet1 * (_oddsSet1 - 1);
                        Set1Market.NbMarkets++;
                    }
                    if (_isSetLost)
                    {
                        _stakeSet1 = 1 / _oddsSet1;
                        _profitSet1 = -_stakeSet1;
                        Set1Market.NbMarkets++;
                    }
                    Set1Market.TotalStake += _stakeSet1;
                    Set1Market.TotalProfit += _profitSet1;

                    //set betting X 0
                    calcStatsWonStraigthSets(aIdPlayer, m, _priceP1WonInStraightSets);
                    //set betting not X 0
                    calcStatsWonNotStraigthSets(aIdPlayer, m, _priceP1WonNotInStraightSets);
                    //set betting 0 X
                    calcStatsLostStraigthSets(aIdPlayer, m, _priceP2WonInStraightSets);
                    //set betting not 0 X
                    calcStatsLostNotStraigthSets(aIdPlayer, m, _priceP2WonNotInStraightSets);
                    //any set
                    for (int i = 0; i <= m.ProcessedResult.fNbSetsWonP1 + m.ProcessedResult.fNbSetsWonP2 - 1; i++)
                    {
                        bool _isSetXWon = false;
                        if (aIdPlayer != -1)
                            _isSetXWon = m.isCountAsSetXWonForStats(i, aIdPlayer);
                        if (aPlayerInfoToSearch != null)
                            _isSetXWon = m.isCountAsSetXWonForStats(i, aPlayerInfoToSearch);

                        bool _isSetXLost = false;
                        if (aIdPlayer != -1)
                            _isSetXLost = m.isCountAsSetXLostForStats(i, aIdPlayer);
                        if (aPlayerInfoToSearch != null)
                            _isSetXLost = m.isCountAsSetXLostForStats(i, aPlayerInfoToSearch);

                        if (_isSetXWon)
                        {
                            _stakeSet1 = 1 / _oddsSet1;
                            _profitSet1 = _stakeSet1 * (_oddsSet1 - 1);
                            AnySetMarket.NbMarkets++;
                            AnySetMarket.TotalStake += _stakeSet1;
                            AnySetMarket.TotalProfit += _profitSet1;
                            //for each court
                            switch (m.CourtId)
                            {
                                case 1://H
                                    AnySetMarketOnHardCourt.NbMarkets++;
                                    AnySetMarketOnHardCourt.TotalStake += _stakeSet1;
                                    AnySetMarketOnHardCourt.TotalProfit += _profitSet1;
                                    break;
                                case 2://C
                                    AnySetMarketOnClayCourt.NbMarkets++;
                                    AnySetMarketOnClayCourt.TotalStake += _stakeSet1;
                                    AnySetMarketOnClayCourt.TotalProfit += _profitSet1;
                                    break;
                                case 3://I
                                    AnySetMarketOnIndoorOrCarpetCourt.NbMarkets++;
                                    AnySetMarketOnIndoorOrCarpetCourt.TotalStake += _stakeSet1;
                                    AnySetMarketOnIndoorOrCarpetCourt.TotalProfit += _profitSet1;
                                    break;
                                case 4://I
                                    AnySetMarketOnIndoorOrCarpetCourt.NbMarkets++;
                                    AnySetMarketOnIndoorOrCarpetCourt.TotalStake += _stakeSet1;
                                    AnySetMarketOnIndoorOrCarpetCourt.TotalProfit += _profitSet1;
                                    break;
                                case 5://G
                                    AnySetMarketOnGrassCourt.NbMarkets++;
                                    AnySetMarketOnGrassCourt.TotalStake += _stakeSet1;
                                    AnySetMarketOnGrassCourt.TotalProfit += _profitSet1;
                                    break;
                                case 6://I
                                    AnySetMarketOnIndoorOrCarpetCourt.NbMarkets++;
                                    AnySetMarketOnIndoorOrCarpetCourt.TotalStake += _stakeSet1;
                                    AnySetMarketOnIndoorOrCarpetCourt.TotalProfit += _profitSet1;
                                    break;
                                default:
                                    break;
                            }
                        }
                        if (_isSetXLost)
                        {
                            _stakeSet1 = 1 / _oddsSet1;
                            _profitSet1 = -_stakeSet1;
                            AnySetMarket.NbMarkets++;
                            AnySetMarket.TotalStake += _stakeSet1;
                            AnySetMarket.TotalProfit += _profitSet1;
                            //for each court
                            switch (m.CourtId)
                            {
                                case 1://H
                                    AnySetMarketOnHardCourt.NbMarkets++;
                                    AnySetMarketOnHardCourt.TotalStake += _stakeSet1;
                                    AnySetMarketOnHardCourt.TotalProfit += _profitSet1;
                                    break;
                                case 2://C
                                    AnySetMarketOnClayCourt.NbMarkets++;
                                    AnySetMarketOnClayCourt.TotalStake += _stakeSet1;
                                    AnySetMarketOnClayCourt.TotalProfit += _profitSet1;
                                    break;
                                case 3://I
                                    AnySetMarketOnIndoorOrCarpetCourt.NbMarkets++;
                                    AnySetMarketOnIndoorOrCarpetCourt.TotalStake += _stakeSet1;
                                    AnySetMarketOnIndoorOrCarpetCourt.TotalProfit += _profitSet1;
                                    break;
                                case 4://I
                                    AnySetMarketOnIndoorOrCarpetCourt.NbMarkets++;
                                    AnySetMarketOnIndoorOrCarpetCourt.TotalStake += _stakeSet1;
                                    AnySetMarketOnIndoorOrCarpetCourt.TotalProfit += _profitSet1;
                                    break;
                                case 5://G
                                    AnySetMarketOnGrassCourt.NbMarkets++;
                                    AnySetMarketOnGrassCourt.TotalStake += _stakeSet1;
                                    AnySetMarketOnGrassCourt.TotalProfit += _profitSet1;
                                    break;
                                case 6://I
                                    AnySetMarketOnIndoorOrCarpetCourt.NbMarkets++;
                                    AnySetMarketOnIndoorOrCarpetCourt.TotalStake += _stakeSet1;
                                    AnySetMarketOnIndoorOrCarpetCourt.TotalProfit += _profitSet1;
                                    break;
                                default:
                                    break;
                            }
                        }
                        //on specified court
                        if (aListCourtsId != null && aListCourtsId.Contains(m.CourtId))
                        {
                            if (_stakeSet1 != 0)
                                AnySetMarketOnCourt.NbMarkets++;
                            AnySetMarketOnCourt.TotalStake += _stakeSet1;
                            AnySetMarketOnCourt.TotalProfit += _profitSet1;
                        }
                        if (aDateForm1 != null && m.Date >= aDateForm1)
                        {
                            if (_stakeSet1 != 0)
                                AnySetMarketFromDate1.NbMarkets++;
                            AnySetMarketFromDate1.TotalStake += _stakeSet1;
                            AnySetMarketFromDate1.TotalProfit += _profitSet1;
                        }
                        if (aDateForm2 != null && m.Date >= aDateForm2)
                        {
                            if (_stakeSet1 != 0)
                                AnySetMarketFromDate2.NbMarkets++;
                            AnySetMarketFromDate2.TotalStake += _stakeSet1;
                            AnySetMarketFromDate2.TotalProfit += _profitSet1;
                        }
                        if (aDateForm3 != null && m.Date >= aDateForm3)
                        {
                            if (_stakeSet1 != 0)
                                AnySetMarketFromDate3.NbMarkets++;
                            AnySetMarketFromDate3.TotalStake += _stakeSet1;
                            AnySetMarketFromDate3.TotalProfit += _profitSet1;
                        }
                        if (aListCategoriesId != null)
                        {
                            List<int> _catIdsForMatch = m.ListCategoriesIdP1;
                            if (_isPlayerFirst)
                                _catIdsForMatch = m.ListCategoriesIdP2;

                            foreach (int _catIdForMatch in _catIdsForMatch)
                            {
                                if (_stakeSet1 != 0)
                                    AnySetMarketByCategories[_catIdForMatch - 1].NbMarkets++;
                                AnySetMarketByCategories[_catIdForMatch - 1].TotalStake += _stakeSet1;
                                AnySetMarketByCategories[_catIdForMatch - 1].TotalProfit += _profitSet1;
                            }
                        }
                    }

                }
            }

        }

        private void calcStatsLostNotStraigthSets(long aIdPlayer, MatchDetailsWithOdds m, double _priceP2WonNotInStraightSets)
        {
            double _stakeLostNotStraightSets = 0;
            double _profitLostNotStraightSets = 0;
            if (m.ProcessedResult.EndType == ResultForMatch.TypeEnd.Completed)
            {
                _stakeLostNotStraightSets = 1 / _priceP2WonNotInStraightSets;
                bool _isWonNotStraightSets = false;
                if (m.getIsBestOf5())
                    _isWonNotStraightSets = (m.isCountAsSetBettingWonForStats(1, 3, aIdPlayer)) ||
                        (m.isCountAsSetBettingWonForStats(2, 3, aIdPlayer));
                else
                    _isWonNotStraightSets = (m.isCountAsSetBettingWonForStats(1, 2, aIdPlayer));
                if (_isWonNotStraightSets)
                {
                    _profitLostNotStraightSets = _stakeLostNotStraightSets * (_priceP2WonNotInStraightSets - 1);
                }
                else
                    _profitLostNotStraightSets = -_stakeLostNotStraightSets;
                SetBettingMarketNot0_X.NbMarkets++;
            }
            SetBettingMarketNot0_X.TotalStake += _stakeLostNotStraightSets;
            SetBettingMarketNot0_X.TotalProfit += _profitLostNotStraightSets;
        }

        private void calcStatsLostStraigthSets(long aIdPlayer, MatchDetailsWithOdds m, double _priceP2WonInStraightSets)
        {
            double _stakeLostStraightSets = 0;
            double _profitLossStraightSets = 0;
            if (m.ProcessedResult.EndType == ResultForMatch.TypeEnd.Completed)
            {
                _stakeLostStraightSets = 1 / _priceP2WonInStraightSets;
                if (m.isCountAsSetBettingWonForStats(0, (m.getIsBestOf5() ? 3 : 2), aIdPlayer))
                {
                    _profitLossStraightSets = _stakeLostStraightSets * (_priceP2WonInStraightSets - 1);
                    SetBettingMarketX_0.NbMarkets++;
                }
                else
                    _profitLossStraightSets = -_stakeLostStraightSets;
                SetBettingMarket0_X.NbMarkets++;
            }
            SetBettingMarket0_X.TotalStake += _stakeLostStraightSets;
            SetBettingMarket0_X.TotalProfit += _profitLossStraightSets;
        }

        private void calcStatsWonNotStraigthSets(long aIdPlayer, MatchDetailsWithOdds m, double _priceP1WonNotInStraightSets)
        {
            double _stakeNotStraightSets = 0;
            double _profitNotStraightSets = 0;
            if (m.ProcessedResult.EndType == ResultForMatch.TypeEnd.Completed)
            {
                _stakeNotStraightSets = 1 / _priceP1WonNotInStraightSets;
                bool _isWonNotStraightSets = false;
                if (m.getIsBestOf5())
                    _isWonNotStraightSets = (m.isCountAsSetBettingWonForStats(3, 1, aIdPlayer)) ||
                        (m.isCountAsSetBettingWonForStats(3, 2, aIdPlayer));
                else
                    _isWonNotStraightSets = (m.isCountAsSetBettingWonForStats(2, 1, aIdPlayer));
                if (_isWonNotStraightSets)
                {
                    _profitNotStraightSets = _stakeNotStraightSets * (_priceP1WonNotInStraightSets - 1);
                }
                else
                    _profitNotStraightSets = -_stakeNotStraightSets;
                SetBettingMarketNotX_0.NbMarkets++;
            }
            SetBettingMarketNotX_0.TotalStake += _stakeNotStraightSets;
            SetBettingMarketNotX_0.TotalProfit += _profitNotStraightSets;
        }

        private void calcStatsWonStraigthSets(long aIdPlayer, MatchDetailsWithOdds m, double _priceP1WonInStraightSets)
        {
            double _stakeStraightSets = 0;
            double _profitStraightSets = 0;
            if (m.ProcessedResult.EndType == ResultForMatch.TypeEnd.Completed)
            {
                _stakeStraightSets = 1 / _priceP1WonInStraightSets;
                if (m.isCountAsSetBettingWonForStats((m.getIsBestOf5() ? 3 : 2), 0, aIdPlayer))
                {
                    _profitStraightSets = _stakeStraightSets * (_priceP1WonInStraightSets - 1);

                }
                else
                    _profitStraightSets = -_stakeStraightSets;
                SetBettingMarketX_0.NbMarkets++;
            }
            SetBettingMarketX_0.TotalStake += _stakeStraightSets;
            SetBettingMarketX_0.TotalProfit += _profitStraightSets;
        }

        private void getRoiListMatches(List<MatchDetailsWithOdds> aListMatches, long aIdPlayer, List<int> aListCourtsId
            , List<int> aListCategoriesId, List<string> aPlayerInfoToSearch = null, bool? aIsForFav = null)
        {
            foreach (MatchDetailsWithOdds m in aListMatches)
            {
                if (!m.Odds1.HasValue || !m.Odds1.HasValue || m.Odds1.Value < 1 || m.Odds2.Value < 1)
                { }
                else
                {
                    //MATCH
                    bool _isPlayerFirst = true;
                    if (aIdPlayer != -1)
                        _isPlayerFirst = m.isPlayerFirst(aIdPlayer);
                    if (aPlayerInfoToSearch != null)
                        _isPlayerFirst = (aPlayerInfoToSearch.IndexOf(m.Player1Info) >-1);
                    if (aIsForFav != null)
                        _isPlayerFirst = m.Odds1 < m.Odds2;
                    double _stakeMatch = 0;
                    double _profitMatch = 0;
                    double? _oddsPlayerForMatch = m.Odds1;
                    if (!_isPlayerFirst)
                        _oddsPlayerForMatch = m.Odds2;
                    bool _isWinMatch = false;
                    if (aIdPlayer != -1)
                        _isWinMatch = m.isCountAsWinForStats(aIdPlayer);
                    if (aPlayerInfoToSearch != null)
                        _isWinMatch = m.isCountForStatsWinOrLoss() && (aPlayerInfoToSearch.IndexOf(m.Player1Info) > -1);
                    if (aIsForFav != null)
                    {
                        if (aIsForFav.Value)
                            _isWinMatch = m.isCountForStatsWinOrLoss() && m.Odds1 < m.Odds2;
                        else
                            _isWinMatch = m.isCountForStatsWinOrLoss() && m.Odds1 > m.Odds2;
                    }
                    bool _isLoss = false;
                    if (aIdPlayer != -1)
                        _isLoss = m.isCountAsLossForStats(aIdPlayer);
                    if (aPlayerInfoToSearch != null)
                        _isLoss = m.isCountForStatsWinOrLoss() && (aPlayerInfoToSearch.IndexOf(m.Player2Info) > -1);
                    if (aIsForFav != null)
                    {
                        if (aIsForFav.Value)
                            _isLoss = m.isCountForStatsWinOrLoss() && m.Odds1 > m.Odds2;
                        else
                            _isLoss = m.isCountForStatsWinOrLoss() && m.Odds1 < m.Odds2;
                    }
                    if (_isWinMatch)
                    {
                        _stakeMatch = 1 / _oddsPlayerForMatch.Value;
                        _profitMatch = _stakeMatch * (_oddsPlayerForMatch.Value-1);
                        MatchMarket.NbMarkets++;
                    }
                    if (_isLoss)
                    {
                        _stakeMatch = 1 / _oddsPlayerForMatch.Value;
                        _profitMatch = - _stakeMatch;
                        MatchMarket.NbMarkets++;
                    }
                    MatchMarket.TotalStake += _stakeMatch;
                    MatchMarket.TotalProfit += _profitMatch;
                    //on COURT
                    if (aListCourtsId != null && aListCourtsId.Contains(m.CourtId))
                    {
                        if (_stakeMatch != 0)
                            MatchMarketOnCourt.NbMarkets++;
                        MatchMarketOnCourt.TotalStake += _stakeMatch;
                        MatchMarketOnCourt.TotalProfit += _profitMatch;
                    }
                    if (aListCategoriesId!= null)
                    {
                        List<int> _catIdsForMatch = m.ListCategoriesIdP1;
                        if (_isPlayerFirst)
                            _catIdsForMatch = m.ListCategoriesIdP2;
                        foreach (int _catIdForMatch in _catIdsForMatch)
                        {
                            if (_stakeMatch != 0)
                                MatchMarketByCategories[_catIdForMatch-1].NbMarkets++;
                            MatchMarketByCategories[_catIdForMatch - 1].TotalStake += _stakeMatch;
                            MatchMarketByCategories[_catIdForMatch - 1].TotalProfit += _profitMatch;
                            if (aListCourtsId != null && aListCourtsId.Contains(m.CourtId))
                            {
                                if (_stakeMatch != 0)
                                    MatchMarketByCategoriesOnCourt[_catIdForMatch - 1].NbMarkets++;
                                MatchMarketByCategoriesOnCourt[_catIdForMatch - 1].TotalStake += _stakeMatch;
                                MatchMarketByCategoriesOnCourt[_catIdForMatch - 1].TotalProfit += _profitMatch;
                            }
                        }
                    }
                    
                }
            }
           
        }

    }
}
