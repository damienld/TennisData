using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OnCourtData
{
    public class ServiceAndReturnStatsForListMatchesOfPlayer
    {
        private long IdPlayer;
        private List<Match> ListMatches { get; set; }
        private DateTime StartingDate { get; set; }
        public string SurfacesId { get; set; }
        public string Level { get; set; }
        private int ServiceGamesPlayed { get; set; }
        private int ServicePointsPlayed { get; set; }
        private int ReturnGamesPlayed { get; set; }
        private int ReturnPointsPlayed { get; set; }
        private int FirstServicePointsPlayed { get; set; }
        private int ServiceGamesWon { get; set; }
        private int ServicePointsWon { get; set; }
        private int ReturnGamesWon { get; set; }
        private int ReturnPointsWon { get; set; }
        private int FirstServicePointsWon { get; set; }
        public int NbMatchesCounted { get; set; }

        private double getPercentageWonOf(int aStatWon, int aStatOf, int aNbDecimals)
        {
            return Math.Round(aStatWon * 100.0 / aStatOf, aNbDecimals);
        }
        public double PercentServiceGamesWon
        {
            get
            {
                return getPercentageWonOf(ServiceGamesWon, ServiceGamesPlayed, 0);
            }
        }
        public double PercentReturnGamesWon
        {
            get
            {
                return getPercentageWonOf(ReturnGamesWon, ReturnGamesPlayed, 0);
            }
        }
        public double Cumul
        {
            get
            {
                return PercentServiceGamesWon + PercentReturnGamesWon;
            }
        }
        public ServiceAndReturnStatsForListMatchesOfPlayer(List<Match> aListMatches, DateTime aStartingDate, List<int> aSurfacesId
            , long aIdPlayer, List<int> aListTrnmtLevels, bool aIsIncludeQualies = false)
        {
            foreach (MatchDetailsWithOdds m in aListMatches)
                if (m.ProcessedResult == null)
                {
                    m.readResult();
                }
            ListMatches = aListMatches;
            StartingDate = aStartingDate;
            if (aSurfacesId != null)
                if (aSurfacesId.Count == 1)
                    SurfacesId = aSurfacesId[0]+"";
                else
                    SurfacesId = string.Join("-", aSurfacesId.ToArray());
            IdPlayer = aIdPlayer;
            int _idRoundMin = 0; //PreQ
            if (!aIsIncludeQualies)
                _idRoundMin = 4;
            Level = string.Join("-", aListTrnmtLevels.ToArray());
            List<Match> _list1 = ListMatches.Where(m => m.Id1 == IdPlayer && m.ListProcessedStats != null 
            && m.ListProcessedStats[0].nbServiceGamesWon != -1 && m.RoundId >= _idRoundMin).ToList();
            List<Match> _list2 = ListMatches.Where(m => m.Id2 == IdPlayer && m.ListProcessedStats != null 
            && m.ListProcessedStats[1].nbServiceGamesWon != -1 && m.RoundId >= _idRoundMin).ToList();
            if (aSurfacesId != null)
            {
                _list1 = _list1.Where(m => aSurfacesId.Contains(m.CourtId)).ToList();
                _list2 = _list2.Where(m => aSurfacesId.Contains(m.CourtId)).ToList();
            }
            if (aListTrnmtLevels.Count > 0)
            {
                _list1 = _list1.Where(m => aListTrnmtLevels.Contains(m.TournamentRank)).ToList();
                _list2 = _list2.Where(m => aListTrnmtLevels.Contains(m.TournamentRank)).ToList();
            }
            NbMatchesCounted = _list1.Count + _list2.Count;
            ServiceGamesWon = _list1.Sum(m => m.ListProcessedStats[0].nbServiceGamesWon) + _list2.Sum(m => m.ListProcessedStats[1].nbServiceGamesWon);
            ServiceGamesPlayed = _list1.Sum(m => m.ListProcessedStats[0].nbServiceGamesPlayed) + _list2.Sum(m => m.ListProcessedStats[1].nbServiceGamesPlayed);
            ReturnGamesWon = _list1.Sum(m => m.ListProcessedStats[0].BP_1) + _list2.Sum(m => m.ListProcessedStats[1].BP_1);
            ReturnGamesPlayed = _list1.Sum(m => m.ListProcessedStats[0].nbReturnGamesPlayed) + _list2.Sum(m => m.ListProcessedStats[1].nbReturnGamesPlayed);
            FirstServicePointsPlayed = _list1.Sum(m => m.ListProcessedStats[0].W1SOF_1) + _list2.Sum(m => m.ListProcessedStats[1].W1SOF_1);
            FirstServicePointsWon = _list1.Sum(m => m.ListProcessedStats[0].W1S_1) + _list2.Sum(m => m.ListProcessedStats[1].W1S_1);
            ServicePointsPlayed = _list1.Sum(m => m.ListProcessedStats[0].W1SOF_1+ m.ListProcessedStats[0].W2SOF_1) 
                + _list2.Sum(m => m.ListProcessedStats[1].W1SOF_1+ m.ListProcessedStats[1].W2SOF_1);
            ServicePointsPlayed = _list1.Sum(m => m.ListProcessedStats[0].W1S_1 + m.ListProcessedStats[0].W2S_1)
                + _list2.Sum(m => m.ListProcessedStats[1].W1S_1 + m.ListProcessedStats[1].W2S_1);
        }
    }
}
