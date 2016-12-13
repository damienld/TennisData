using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;

namespace OnCourtData.UnitTesting
{
    [TestClass]
    public class PriceStatsListMatchesForPlayer_test
    {
        [TestMethod]
        public void getSet1PriceFromMatchPrice()
        {
            double _straight = 0;
            double _straightP2 = 0;
            Trace.WriteLine(PriceStatsListMatchesForPlayer.getSet1PriceFromMatchPrice(1.9, false, ref _straight, ref _straightP2));
            Trace.WriteLine(_straight + ";" + _straightP2);
            Trace.WriteLine(PriceStatsListMatchesForPlayer.getSet1PriceFromMatchPrice(1.001, false, ref _straight, ref _straightP2));
            Trace.WriteLine(_straight + ";" + _straightP2);
            Trace.WriteLine(PriceStatsListMatchesForPlayer.getSet1PriceFromMatchPrice(1.63, false, ref _straight, ref _straightP2));
            Trace.WriteLine(_straight + ";" + _straightP2);
            Trace.WriteLine(PriceStatsListMatchesForPlayer.getSet1PriceFromMatchPrice(2.1, false, ref _straight, ref _straightP2));
            Trace.WriteLine(_straight + ";" + _straightP2);
            Trace.WriteLine(PriceStatsListMatchesForPlayer.getSet1PriceFromMatchPrice(4, false, ref _straight, ref _straightP2));
            Trace.WriteLine(_straight + ";" + _straightP2);
            Trace.WriteLine(PriceStatsListMatchesForPlayer.getSet1PriceFromMatchPrice(9, false, ref _straight, ref _straightP2));
            Trace.WriteLine(_straight + ";" + _straightP2);

        }
    }
}
