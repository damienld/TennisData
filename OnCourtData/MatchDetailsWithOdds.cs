using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyTools;

namespace OnCourtData
{
    [Serializable]
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
        public static string csvHeader = "Date,TrnId,Trn,TrnRk,TrnSite,TrnSpeed,TrnSpeedNb,"
            + "CourtId,RoundId,Round,P1Id,P1,P2Id,P2,"
            + "Result,IndexP,Player,AceRatePlayer,Srv1PtsP1,Srv1PtsWonP1,Srv2PtsP1,Srv2PtsWonP1,"
             + "Rk1,Rk2,Odds1,Odds2,IsCompleted,P1ServingFirst,"
                + "StatusP1,StatusP2,SetsP1,SetsP2,GamesP1,GamesP2,S1,S2,S3,S4,S5,S1_2,S2_2,S3_2,S4_2,S5_2"
            ;
        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="aListMatches"></param>
        /// <param name="aPlayerId"></param>
        public string ToCsvLine(bool isPlayer1, bool isATP)
        {
            int indexPlayer = 0;
            string nameP = this.Player1Name.Replace(",", ";");
            int nbsets1 = ProcessedResult.fNbSetsWonP1;
            int nbsets2 = ProcessedResult.fNbSetsWonP2;
            int gamesP1 = ProcessedResult.fNbGamesWonP1;
            int gamesP2 = ProcessedResult.fNbGamesWonP2;
            List<int> listSetResultsForP1 = ProcessedResult.fListSetResultsForP1;
            List<int> listSetResultsForP2 = ProcessedResult.fListSetResultsForP2;
            if (!isPlayer1)
            {
                indexPlayer = 1;
                nameP = this.Player2Name.Replace(",", ";");
                nbsets1 = ProcessedResult.fNbSetsWonP2;
                nbsets2 = ProcessedResult.fNbSetsWonP1;
            }
            return $"{Date},{TournamentId},{TournamentName.Replace(",", ";").Replace("Challenger", "CH")}"
                + $",{TournamentRank},{TournamentSite},{getTrnSpeed(isATP).Item1},{getTrnSpeed(isATP).Item2}"
                + $",{this.CourtId},{this.RoundId},{this.RoundName},{this.Id1}"
                + $",{this.Player1Name.Replace(",", ";")},{this.Id2},{this.Player2Name.Replace(",", ";")}"
                + $",{this.ResultString},{indexPlayer},{nameP}"
                + $",{this.StatsByPlayers[indexPlayer].getPctAceByPointsOnServe()}"
                + $",{this.StatsByPlayers[indexPlayer].W1SOF_1},{this.StatsByPlayers[indexPlayer].W1S_1}"
                + $",{this.StatsByPlayers[indexPlayer].W2SOF_1},{this.StatsByPlayers[indexPlayer].W2S_1}"
                + $",{PositionPlayer1},{PositionPlayer2}"
                + $",{Odds1},{Odds2},{IsCompletedMatch()}"
                + $",{IsP1ServingFirst},{Player1Info},{Player2Info}"
                + $",{nbsets1},{nbsets2},{gamesP1},{gamesP2}"
                + $",{listSetResultsForP1[0]},{listSetResultsForP1[1]}"
                + $",{listSetResultsForP1[2]},{listSetResultsForP1[3]}"
                + $",{listSetResultsForP1[4]},{listSetResultsForP2[0]}"
                + $",{listSetResultsForP2[1]},{listSetResultsForP2[2]}"
                + $",{listSetResultsForP2[3]},{listSetResultsForP2[4]}"
                ;
        }

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
