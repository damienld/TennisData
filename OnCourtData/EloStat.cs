using Ratings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OnCourtData
{
    public class EloStat
    {
        [XmlIgnore]
        private EloStat baseReference = null;
        public void SetBaseReference(EloStat value)
        {
            baseReference = value;
        }
        public override string ToString()
        {
            if (Rating <= 0)
                return "  ";
            else if (baseReference != null && baseReference.Rating > 0)
            {
                int surperf = Rating - baseReference.Rating;
                return (surperf > 0? "+": "") + (Rating - baseReference.Rating).ToString()
                    + (nbCounts < 20 ? "(" + nbCounts + ")" : "");
            }
            else
                return Rating.ToString() + (nbCounts < 20 ? "(" + nbCounts + ")" : "");
        }
        public int Rating { get; set; } = -1;
        public int nbCounts { get; set; } = 0;

        public static (double ratingPlayer, int aNbPlayedForRatingPlayer, double ratingPlayerOnCourt
            , int aNbForRatingPlayerOnCourt
            , double ratingPlayerLast6M, int aNbForRatingPlayerLast6M
            , double ratingPlayerLast9MNonClay, int aNbForRatingPlayerLast9MNonClay
            , double ratingPlayerLast9MClay, int aNbForRatingPlayerLast9MClay
            )
            CalculateEloFromAllMatches(List<MatchDetailsWithOdds> listMatchesToEvaluate
            , long idPlayer, bool aIsATP, List<int> aListIdSurfaceOnCourt, bool isAnySetCalc=false
            )
        {
            double ratingPlayer = -1;
            double ratingPlayerOnCourt = -1;
            double ratingPlayerLast6M = -1;
            double ratingPlayerLast9MNonClay = -1;
            double ratingPlayerLast9MClay = -1;
            int aNbPlayedForRatingPlayer = 0;
            int aNbForRatingPlayerLast6M = 0;
            int aNbForRatingPlayerLast9MNonClay = 0;
            int aNbForRatingPlayerLast9MClay = 0;
            int aNbForRatingPlayerOnCourt = 0;
            Console.WriteLine("");
            Console.WriteLine("*********************STARTED RATING:" + ratingPlayer);
            foreach (MatchDetailsWithOdds match in listMatchesToEvaluate.OrderBy(m => m.Date))
            {
                if (match.IsCompletedMatch())//&& match.TournamentRank >= 1)
                {
                    try
                    {
                        long oppId = match.Id1;
                        int playerRanking = match.PositionPlayer2;
                        int oppRanking = match.PositionPlayer1;
                        string oppName = match.Player1Name;
                        bool isWon = false;
                        if (match.isPlayerFirst(idPlayer))
                        {
                            isWon = true;
                            oppId = match.Id2;
                            oppRanking = match.PositionPlayer2;
                            playerRanking = match.PositionPlayer1;
                            oppName = match.Player2Name;
                        }
                        //at the start if started with -1, use ranking of first match, if still -1 then 1200
                        if (ratingPlayer < 0)
                        {
                            ratingPlayer = Math.Max(1200, getEloValueFromAtpRank(playerRanking, aIsATP));
                            Console.WriteLine("RATING INITIALIZED at:" + ratingPlayer);
                        }
                        if (ratingPlayerOnCourt < 0)
                        {
                            ratingPlayerOnCourt = Math.Max(1200, getEloValueFromAtpRank(playerRanking, aIsATP));
                        }
                        double ratingOpp = getEloValueFromAtpRank(oppRanking, aIsATP);
                        int nbMatchesOpp = 300; //doesnt matter as it only has an impact on updated rating P2
                        if (ratingOpp > -1)
                        {
                            if (match.ProcessedResult == null)
                                match.readResult();
                            int nbSetsWonP1 = match.isPlayerFirst(idPlayer) ? match.ProcessedResult.fNbSetsWonP1
                                    : match.ProcessedResult.fNbSetsWonP2;
                            int nbSetsWonP2 = match.isPlayerFirst(idPlayer) ? match.ProcessedResult.fNbSetsWonP2
                                : match.ProcessedResult.fNbSetsWonP1;
                            if (isAnySetCalc)
                            {
                                ratingPlayer = EloRating.UpdateEloRatingBySet
                                (ratingPlayer
                                , ratingOpp, aNbPlayedForRatingPlayer, nbMatchesOpp
                                , match.TournamentRank == 4, match.RoundId, nbSetsWonP1
                                    , nbSetsWonP2, nbSetsWonP1 + nbSetsWonP2);
                                aNbPlayedForRatingPlayer += match.ProcessedResult.fNbSetsWonP1
                                    + match.ProcessedResult.fNbSetsWonP2;
                            }
                            else
                            {
                                ratingPlayer = EloRating.UpdateEloRatingMatch
                               (ratingPlayer
                               , ratingOpp, aNbPlayedForRatingPlayer, nbMatchesOpp, isWon
                               , match.TournamentRank == 4, match.RoundId, nbSetsWonP1 + nbSetsWonP2);
                                aNbPlayedForRatingPlayer += 1;
                            }
                            if (aListIdSurfaceOnCourt!=null && aListIdSurfaceOnCourt.Contains(match.CourtId))
                            {
                                ratingPlayerOnCourt = EloRating.UpdateEloRatingMatch
                                    (ratingPlayerOnCourt, ratingOpp, aNbForRatingPlayerOnCourt
                                    , nbMatchesOpp, isWon, match.TournamentRank == 4, match.RoundId, nbSetsWonP1 + nbSetsWonP2);
                                aNbForRatingPlayerOnCourt += 1;
                            }
                            if (match.Date >= DateTime.Now.AddMonths(-6))
                            {
                                /*

    • cc = 261.8 , effect of clay court match on clay court ratings
    • hc = 144.5 , effect of hard court match on clay court ratings
    • gc = 138.2 , effect of grass court match on clay court ratings

    • hh = 198.2 , parameter scaling the effect of a hard court match on the hard court ratings
    • ch = 180.2 , effect of clay court match on hard court ratings
    • gh = 188.2 , effect of grass court match on hard court ratings
    
    • gg = 261.8 , effect of grass court match on grass court ratings
    • hg = 164.4 , effect of hard court match on grass court ratings
    • cg = 138.2 , effect of clay court match on grass court ratings
                                     * */
                                //at the start if started with -1, use ranking of first match, if still -1 then 1200
                                if (ratingPlayerLast6M < 0)
                                {
                                    ratingPlayerLast6M = Math.Max(1200, getEloValueFromAtpRank(playerRanking, aIsATP));
                                }
                                ratingPlayerLast6M = EloRating.UpdateEloRatingBySet
                                    (ratingPlayerLast6M, ratingOpp, aNbForRatingPlayerLast6M
                                    , nbMatchesOpp, match.TournamentRank == 4, match.RoundId, nbSetsWonP1
                                    , nbSetsWonP2, nbSetsWonP1 + nbSetsWonP2);
                                aNbForRatingPlayerLast6M += match.ProcessedResult.fNbSetsWonP1
                                    + match.ProcessedResult.fNbSetsWonP2;
                            }
                            if (match.Date >= DateTime.Now.AddMonths(-9))
                            {
                                if (match.CourtId == 2)
                                {//clay
                                    //at the start if started with -1, use ranking of first match, if still -1 then 1200
                                    if (ratingPlayerLast9MClay < 0)
                                    {
                                        ratingPlayerLast9MClay = Math.Max(1200, getEloValueFromAtpRank(playerRanking, aIsATP));
                                    }
                                    ratingPlayerLast9MClay = EloRating.UpdateEloRatingBySet
                                        (ratingPlayerLast9MClay, ratingOpp, aNbForRatingPlayerLast9MClay
                                        , nbMatchesOpp, match.TournamentRank == 4, match.RoundId, nbSetsWonP1
                                        , nbSetsWonP2, nbSetsWonP1 + nbSetsWonP2);
                                    aNbForRatingPlayerLast9MClay += match.ProcessedResult.fNbSetsWonP1
                                        + match.ProcessedResult.fNbSetsWonP2;
                                }
                                else 
                                {//non clay
                                    if (ratingPlayerLast9MNonClay < 0)
                                    {
                                        ratingPlayerLast9MNonClay = Math.Max(1200, getEloValueFromAtpRank(playerRanking, aIsATP));
                                    }
                                    ratingPlayerLast9MNonClay = EloRating.UpdateEloRatingBySet
                                        (ratingPlayerLast9MNonClay, ratingOpp, aNbForRatingPlayerLast9MNonClay
                                        , nbMatchesOpp, match.TournamentRank == 4, match.RoundId, nbSetsWonP1
                                        , nbSetsWonP2, nbSetsWonP1 + nbSetsWonP2);
                                    aNbForRatingPlayerLast9MNonClay += match.ProcessedResult.fNbSetsWonP1
                                        + match.ProcessedResult.fNbSetsWonP2;
                                }
                            }
                            Console.WriteLine("Updated Ratings after " + (isWon ? "W" : "L")
                                + " <" + oppName + "(" + oppRanking + "-"
                                + ratingOpp + ")> :" + ratingPlayer + " - " + ratingPlayerOnCourt
                                + " - " + ratingPlayerLast6M);
                        }
                    }
                    catch (Exception exc)
                    {
                    }
                }
            }
            return (ratingPlayer, aNbPlayedForRatingPlayer, ratingPlayerOnCourt
            , aNbForRatingPlayerOnCourt, ratingPlayerLast6M, aNbForRatingPlayerLast6M
            , ratingPlayerLast9MNonClay, aNbForRatingPlayerLast9MNonClay
            , ratingPlayerLast9MClay, aNbForRatingPlayerLast9MClay
            );
        }
        public static int getEloValueFromAtpRank(int aEloRank, bool aIsATP)
        {
            List<int> ListRankEloMin = ListRankEloATPMin;
            List<int> ListValueElo = ListValueEloATP;
            if (!aIsATP)
            {
                ListRankEloMin = ListRankEloWTAMin;
                ListValueElo = ListValueEloWTA;
            }
            if (aEloRank < 1 || aEloRank > ListRankEloMin[0])
                return -1;
            int index = 0;
            while (aEloRank < ListRankEloMin[index])
            {
                index += 1;
            }
            //valueELo will be a weighted average of the value 
            int valueEloIntervallMin = 0;
            int intervalMin = ListRankEloMin[0];
            int intervalMax = ListRankEloMin[index];
            if (index > 0)
            {
                valueEloIntervallMin = ListValueElo[index - 1];
                intervalMin = ListRankEloMin[index - 1];
            }
            int valueEloIntervallMax = ListValueElo[index];
            int valueElo = (valueEloIntervallMax * (intervalMin - aEloRank)
                + valueEloIntervallMin * (aEloRank - intervalMax)) / (intervalMin - intervalMax);
            return valueElo;
        }

        public static readonly List<int> ListRankEloATPMin
            = new List<int> { 450, 380, 300, 250, 200, 150, 100, 75, 50, 25, 15, 10, 1 };
        public static readonly List<int> ListValueEloATP
            = new List<int> { 1200, 1300, 1420, 1473, 1532, 1602, 1685, 1730, 1797, 1860, 1920, 1960, 2200 };
        public static readonly List<int> ListRankEloWTAMin
            = new List<int> { 700, 500, 450, 400, 350, 300, 250, 200, 150, 100, 75, 50, 25, 15, 10, 1 };
        public static readonly List<int> ListValueEloWTA
            = new List<int> { 970, 1040, 1070, 1100, 1195, 1297, 1397, 1492, 1592, 1674, 1742, 1806, 1866, 1931, 1973, 2100 };


    }
}
