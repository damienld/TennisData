using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace OnCourtData
{
    public class MarketStats
    {
        public enum TypeMarket
        {
            Match, Set1, Set2, Set3, Set4, Set5, SetBetting3Sets, SetBetting5sets, GameHandicap, GameTotals
        }
        public int NbMarkets { get; set; }
        public double TotalStake { get; set; }
        public double TotalProfit { get; set; }
        public int getROI()
        {
            if (NbMarkets > 0)
                return Convert.ToInt16(Math.Round((TotalProfit * 100 / TotalStake) + 100, 0));
            else
                return 0;
        }
        public MarketStats()
        {
            TotalStake = 0;
            TotalProfit = 0;
            NbMarkets = 0;
        }
        
    }
    public class PriceStatsListMatchesForPlayer
    {
        static List<double> _listMatchPricesBo3 = new List<double>() { 2.00, 1.92, 1.85, 1.77, 1.69, 1.64, 1.59, 1.55, 1.50, 1.46, 1.43, 1.39, 1.36, 1.33, 1.30, 1.28, 1.26, 1.23, 1.22, 1.20, 1.18, 1.17, 1.15, 1.14, 1.13, 1.12, 1.11, 1.10, 1.089, 1.082, 1.075, 1.067, 1.062, 1.054, 1.049, 1.045, 1.041, 1.030, 1.018, 1.010, 1.005, 1 };
        static List<double> _listSet1PricesBo3 = new List<double>() { 2.00, 1.93, 1.87, 1.82, 1.77, 1.72, 1.68, 1.63, 1.60, 1.56, 1.53, 1.50, 1.47, 1.44, 1.41, 1.39, 1.37, 1.35, 1.33, 1.31, 1.29, 1.27, 1.26, 1.25, 1.23, 1.22, 1.20, 1.19, 1.18, 1.17, 1.16, 1.15, 1.14, 1.12, 1.12, 1.11, 1.10, 1.09, 1.06, 1.04, 1.03, 1 };
        static List<double> _list2sets_0PricesBo3 = new List<double>() { 3.65, 3.46, 3.25, 3.07, 2.90, 2.77, 2.64, 2.53, 2.42, 2.31, 2.23, 2.15, 2.06, 1.99, 1.93, 1.86, 1.81, 1.75, 1.71, 1.66, 1.62, 1.58, 1.54, 1.52, 1.47, 1.44, 1.42, 1.39, 1.36, 1.34, 1.32, 1.30, 1.28, 1.26, 1.24, 1.22, 1.21, 1.17, 1.12, 1.08, 1.05, 1 };
        static List<double> _list0sets_2PricesBo3 = new List<double>() { 3.65, 3.88, 4.10, 4.39, 4.69, 5.00, 5.29, 5.71, 6.10, 6.58, 6.99, 7.58, 8.13, 8.70, 9.35, 10.10, 10.87, 11.90, 12.66, 13.89, 14.93, 16.39, 17.86, 19.23, 21.28, 23.26, 25.00, 27.78, 30.30, 33.33, 37.04, 41.67, 43.48, 50.00, 55.56, 62.50, 71.43, 100.00, 166.67, 333.33, 1000.00, 1000 };
        static List<double> _listMatchPricesBo5 = new List<double>() { 2.00, 1.83, 1.68, 1.56, 1.47, 1.36, 1.28, 1.21, 1.16, 1.12, 1.076, 1.056, 1.035, 1.021, 1.009, 1.000 };
        static List<double> _listSet1PricesBo5 = new List<double>() { 2.00, 1.87, 1.77, 1.68, 1.60, 1.50, 1.41, 1.35, 1.29, 1.25, 1.19, 1.16, 1.12, 1.09, 1.06, 1.00 };
        static List<double> _list3sets_0PricesBo5 = new List<double>() { 5.46, 4.65, 4.07, 3.58, 3.21, 2.75, 2.39, 2.12, 1.91, 1.75, 1.55, 1.45, 1.34, 1.25, 1.16, 1.00 };
        static List<double> _list0sets_3PricesBo5 = new List<double>() { 5.41, 6.29, 7.41, 8.77, 10.31, 13.51, 17.24, 23.26, 32.26, 43.48, 66.67, 100.00, 166.67, 200, 200.00, 200.00 };

        public static double getSet1PriceFromMatchPrice(double aMatchPrice, bool aIsBestOf5, ref double aPriceStraightSetsP1
            , ref double aPriceStraightSetsP2)
        {
            List<double> _listMatchPrices = _listMatchPricesBo3;
            List<double> _listSet1Prices = _listSet1PricesBo3;
            if (aIsBestOf5)
            {
                _listMatchPrices = _listMatchPricesBo5;
                _listSet1Prices = _listSet1PricesBo3;
            }
            if (aMatchPrice == 2)
            {
                return 2;
            }
            else if (aMatchPrice <= 2)
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
                            aPriceStraightSetsP1 = _partLine2 * _list2sets_0PricesBo3[_index + 1] 
                                + _list2sets_0PricesBo3[_index] * _partLine1;
                            aPriceStraightSetsP2 = _partLine2 * _list0sets_2PricesBo3[_index + 1]
                                + _list0sets_2PricesBo3[_index] * _partLine1;
                        }
                        else
                        {
                            aPriceStraightSetsP1 = _partLine2 * _list3sets_0PricesBo5[_index + 1]
                                + _list3sets_0PricesBo5[_index] * _partLine1;
                            aPriceStraightSetsP2 = _partLine2 * _list0sets_3PricesBo5[_index + 1]
                                + _list0sets_3PricesBo5[_index] * _partLine1;
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
                /*int _index = 0;
                double _matchPriceForIndex = _listMatchPrices[0];
                while (_index <= _listMatchPrices.Count - 1 && _matchPriceForIndex > _oppositeMatchPriceToFind)
                {
                    _matchPriceForIndex = _listMatchPrices[_index];
                    _index++;
                }*/
            }
            return 0;
        }
        public List<MatchDetailsWithOdds> fListMatches { get; set; }
        public MarketStats MatchMarket;
        public MarketStats MatchMarketOnCourt;
        public MarketStats MatchMarketFromDate;
        public MarketStats Set1Market;

        public PriceStatsListMatchesForPlayer(List<MatchDetailsWithOdds> aListMatches, long aIdPlayer, List<int> aListCourtsId
            ,DateTime? aDateForm)
        {
            MatchMarket = new MarketStats();
            Set1Market = new MarketStats();
            MatchMarketOnCourt = new MarketStats();
            MatchMarketFromDate = new MarketStats();
            getRoiListMatches(aListMatches, aIdPlayer, aListCourtsId, aDateForm);
            getRoiListFirstSet(aListMatches, aIdPlayer);
            //Trace.WriteLine(MatchMarket.getROI() + " %" + "(" + MatchMarket.NbMarkets + ")");
            //Trace.WriteLine(Set1Market.getROI() + " %" + "(" + Set1Market.NbMarkets + ")");
        }
        public override string ToString()
        {
            return (MatchMarket.getROI() + " %" + "(" + MatchMarket.NbMarkets + ")");
        }
        private void getRoiListFirstSet(List<MatchDetailsWithOdds> aListMatches, long aIdPlayer)
        {
            foreach (MatchDetailsWithOdds m in aListMatches)
            {
                if (!m.Odds1.HasValue || !m.Odds1.HasValue || m.Odds1.Value < 1 || m.Odds2.Value < 1)
                { }
                else
                {
                    //MATCH
                    double _stake = 0;
                    double _profit = 0;
                    if (m.isCountAsSetXWonForStats(0, aIdPlayer))
                    {
                        _stake = 1 / m.Odds1.Value;
                        _profit = _stake * (m.Odds1.Value - 1);
                        Set1Market.NbMarkets++;
                    }
                    if (m.isCountAsSetXLostForStats(0, aIdPlayer))
                    {
                        _stake = 1 / m.Odds1.Value;
                        _profit = -_stake;
                        Set1Market.NbMarkets++;
                    }
                    Set1Market.TotalStake += _stake;
                    Set1Market.TotalProfit += _profit;

                    //SET1
                }
            }

        }
        private void getRoiListMatches(List<MatchDetailsWithOdds> aListMatches, long aIdPlayer, List<int> aListCourtsId
            , DateTime? aDateForm)
        {
            foreach (MatchDetailsWithOdds m in aListMatches)
            {
                if (!m.Odds1.HasValue || !m.Odds1.HasValue || m.Odds1.Value < 1 || m.Odds2.Value < 1)
                { }
                else
                {
                    //MATCH
                    double _stake = 0;
                    double _profit = 0;
                    if (m.isCountAsWinForStats(aIdPlayer))
                    {
                        _stake = 1 / m.Odds1.Value;
                        _profit = _stake * (m.Odds1.Value-1);
                        MatchMarket.NbMarkets++;
                    }
                    if (m.isCountAsLossForStats(aIdPlayer))
                    {
                        _stake = 1 / m.Odds1.Value;
                        _profit = - _stake;
                        MatchMarket.NbMarkets++;
                    }
                    MatchMarket.TotalStake += _stake;
                    MatchMarket.TotalProfit += _profit;
                    if (aListCourtsId != null && aListCourtsId.Contains(m.CourtId))
                    {
                        if (_stake != 0)
                            MatchMarketOnCourt.NbMarkets++;
                        MatchMarketOnCourt.TotalStake += _stake;
                        MatchMarketOnCourt.TotalProfit += _profit;
                    }
                    if (aDateForm != null && m.Date >= aDateForm)
                    {
                        if (_stake != 0)
                            MatchMarketFromDate.NbMarkets++;
                        MatchMarketFromDate.TotalStake += _stake;
                        MatchMarketFromDate.TotalProfit += _profit;
                    }
                }
            }
           
        }

    }
}
