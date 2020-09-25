using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyTools;

namespace OnCourtData
{
    public class MatchDetailsWithOdds: Match
    {
        public double? Odds1 { get; set; }
        public double? Odds2 { get; set; }
        
        public bool isFilterOdds(decimal aLowerOdds, decimal aUpperOdds, long aPlayerId)
        {
            double a = Convert.ToDouble(aLowerOdds);
            double b = Convert.ToDouble(aUpperOdds);
            return (Odds1.HasValue && Odds1.Value >= a && Odds1.Value <= b && isPlayerFirst(aPlayerId))
                    || (Odds2.HasValue && Odds2.Value >= a && Odds2.Value <= b && !isPlayerFirst(aPlayerId));
        }

        public MatchDetailsWithOdds(bool aIsATP)
        {
            IsATP = aIsATP;
        }
        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="aListMatches"></param>
        /// <param name="aPlayerId"></param>
        public static void getMedianRankSetsWon(List<MatchDetailsWithOdds> aListMatches, long aPlayerId)
        {
            /*List<MatchDetailsWithOdds> _listWherePlayerIsFirst = aListMatches.Where(
                m => m.Id1 == aPlayerId).Select( m => m.PositionPlayer2);
            List<MatchDetailsWithOdds> _listWherePlayerIsNotFirst = aListMatches.Where(
                m => m.Id2 == aPlayerId).ToList();
            aListMatches.Select( m => m.)
            MyTools.MyMath.GetMedian<int>(aListMatches)*/
        }
        public bool isCountAsSetXWonForStatsForFav(int aIdset)
        {
            if (ProcessedResult == null)
                this.readResult();
            if (ProcessedResult.fListSetResultsForP1[aIdset] != -1 && ProcessedResult.fListSetResultsForP2[aIdset] != -1
                && (Odds1 != Odds2) && Odds1 != null && Odds2 != null)
            {
                if (Odds1 < Odds2)
                    return (ProcessedResult.fListSetResultsForP1[aIdset] > ProcessedResult.fListSetResultsForP2[aIdset]);
                else if (Odds2 < Odds1)
                    return (ProcessedResult.fListSetResultsForP2[aIdset] > ProcessedResult.fListSetResultsForP1[aIdset]);
                return false;
            }
            else
                return false;
        }
        public bool isCountAsSetXLostForStatsForFav(int aIdset)
        {
            if (ProcessedResult == null)
                this.readResult();
            if (ProcessedResult.fListSetResultsForP1[aIdset] != -1 && ProcessedResult.fListSetResultsForP2[aIdset] != -1
                && (Odds1 != Odds2) && Odds1 != null && Odds2 != null)
            {
                if (Odds1 < Odds2)
                    return (ProcessedResult.fListSetResultsForP1[aIdset] < ProcessedResult.fListSetResultsForP2[aIdset]);
                else if (Odds2 < Odds1)
                    return (ProcessedResult.fListSetResultsForP2[aIdset] < ProcessedResult.fListSetResultsForP1[aIdset]);
                return false;
            }
            else
                return false;
        }
        public bool isCountAsSetXWonForStatsForDog(int aIdset)
        {
            if (ProcessedResult == null)
                this.readResult();
            if (ProcessedResult.fListSetResultsForP1[aIdset] != -1 && ProcessedResult.fListSetResultsForP2[aIdset] != -1
                && (Odds1 != Odds2) && Odds1 != null && Odds2 != null)
            {
                if (Odds2 < Odds1)
                    return (ProcessedResult.fListSetResultsForP1[aIdset] > ProcessedResult.fListSetResultsForP2[aIdset]);
                else if (Odds1 < Odds2)
                    return (ProcessedResult.fListSetResultsForP2[aIdset] > ProcessedResult.fListSetResultsForP1[aIdset]);
                return false;
            }
            else
                return false;
        }
        public bool isCountAsSetXLostForStatsForDog(int aIdset)
        {
            if (ProcessedResult == null)
                this.readResult();
            if (ProcessedResult.fListSetResultsForP1[aIdset] != -1 && ProcessedResult.fListSetResultsForP2[aIdset] != -1
                && (Odds1 != Odds2) && Odds1 != null && Odds2 != null)
            {
                if (Odds2 < Odds1)
                    return (ProcessedResult.fListSetResultsForP1[aIdset] < ProcessedResult.fListSetResultsForP2[aIdset]);
                else if (Odds1 < Odds2)
                    return (ProcessedResult.fListSetResultsForP2[aIdset] < ProcessedResult.fListSetResultsForP1[aIdset]);
                return false;
            }
            else
                return false;
        }
    }
}
