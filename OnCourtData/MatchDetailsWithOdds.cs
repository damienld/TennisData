using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

    }
}
