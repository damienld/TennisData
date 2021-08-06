using OnCourtData;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace OnCourtData
{
    [Serializable]
    public class AceReportPlayer
    {
        //Juste set when getting a playereport from the list of all reports by year
        [XmlIgnore]
        public int Year { get; set; }
        /// <summary>
        /// For Serializing
        /// </summary>
        public AceReportPlayer()
        { }
        public AceReportPlayer(long playerId, int playerRank
            , string playerName, string playerNote, List<MatchDetailsWithOdds> listMatches)
        {
            PlayerId = playerId;
            PlayerRank = playerRank;
            PlayerName = playerName;
            PlayerNote = playerNote;
            ListMatches = listMatches;
            initStats(this);
            readMatches(listMatches, this, playerId);
        }
        public static void initStats(AceReportPlayer playerStats)
        {
            playerStats.AcesByCourt = new StatByCourt<int>(5, playerStats) { 0, 0, 0, 0, 0 };
            playerStats.DfByCourt = new StatByCourt<int>(5, playerStats) { 0, 0, 0, 0, 0 };
            playerStats.NbServiceGamesByCourt = new StatByCourt<int>(5, playerStats) { 0, 0, 0, 0, 0 };
            playerStats.NbReturnGamesByCourt = new StatByCourt<int>(5, playerStats) { 0, 0, 0, 0, 0 };
            playerStats.NbServicePointsByCourt = new StatByCourt<int>(5, playerStats) { 0, 0, 0, 0, 0 };
            playerStats.MatchAvgAceRateByCourtPct = new StatByCourt<double>(5, playerStats) { 0, 0, 0, 0, 0 };
            playerStats.MatchAvgDfRateByCourtPct = new StatByCourt<double>(5, playerStats) { 0, 0, 0, 0, 0 };
            playerStats.MatchAvgPtsWonOnServeCourt = new StatByCourt<double>(5, playerStats) { 0, 0, 0, 0, 0 };
            playerStats.MatchAvgPtsWonOnReturnCourt = new StatByCourt<double>(5, playerStats) { 0, 0, 0, 0, 0 };
            playerStats.MatchAvgPctPtsWonFirstServeCourt = new StatByCourt<double>(5, playerStats) { 0, 0, 0, 0, 0 };
            playerStats.NbMatchesByCourt = new StatByCourt<int>(5, playerStats) { 0, 0, 0, 0, 0 };

            playerStats.AcesByCourtOpp = new StatByCourt<int>(5, playerStats) { 0, 0, 0, 0, 0 };
            playerStats.DfByCourtOpp = new StatByCourt<int>(5, playerStats) { 0, 0, 0, 0, 0 };
            playerStats.NbReturnPointsByCourt = new StatByCourt<int>(5, playerStats) { 0, 0, 0, 0, 0 };
            playerStats.MatchAvgAceRateByCourtPctOpp = new StatByCourt<double>(5, playerStats) { 0, 0, 0, 0, 0 };
            playerStats.MatchAvgDfRateByCourtPctOpp = new StatByCourt<double>(5, playerStats) { 0, 0, 0, 0, 0 };
            playerStats.MatchAvgPctPtsWonFirstReturnCourt = new StatByCourt<double>(5, playerStats) { 0, 0, 0, 0, 0 };
            playerStats.GameAvgAceRateByCourt = new StatByCourt<double>(5, playerStats) { 0, 0, 0, 0, 0 };
            playerStats.GameAvgDfRateByCourt = new StatByCourt<double>(5, playerStats) { 0, 0, 0, 0, 0 };
            playerStats.GameAvgAceRateByCourtOpp = new StatByCourt<double>(5, playerStats) { 0, 0, 0, 0, 0 };
            playerStats.GameAvgDfRateByCourtOpp = new StatByCourt<double>(5, playerStats) { 0, 0, 0, 0, 0 };
        }
        public static void readMatches(List<MatchDetailsWithOdds> aListMatchFull
            , AceReportPlayer playerStats, long aPlayerId)
        {
            foreach (var match in aListMatchFull)
            {
                if (match.ProcessedResult == null)
                    match.readResult();
                if (match.IsCompletedMatch())//ONLY completed matches
                    match.readServeAndReturnStats();
            }
            List<MatchDetailsWithOdds> _listMatchWherePlayerFirst =
                aListMatchFull.Where(m => m.Id1 == aPlayerId).ToList();
            List<MatchDetailsWithOdds> _listMatchWherePlayerSecond =
                aListMatchFull.Where(m => m.Id2 == aPlayerId).ToList();
            playerStats.ListStatsMatchForPlayer
                = _listMatchWherePlayerFirst.Select(m=> m.StatsByPlayers[0]).ToList();
            playerStats.ListStatsMatchForOpp
                = _listMatchWherePlayerFirst.Select(m => m.StatsByPlayers[1]).ToList();

            if (playerStats.ListStatsMatchForPlayer == null)
                playerStats.ListStatsMatchForPlayer = new List<StatsPlayerForOneMatch>();
            if (playerStats.ListStatsMatchForOpp == null)
                playerStats.ListStatsMatchForOpp = new List<StatsPlayerForOneMatch>();
            playerStats.ListStatsMatchForPlayer.AddRange(_listMatchWherePlayerSecond.Select(m => m.StatsByPlayers[1]).ToList());
            playerStats.ListStatsMatchForOpp.AddRange(_listMatchWherePlayerSecond.Select(m => m.StatsByPlayers[0]).ToList());
            if (playerStats.ListStatsMatchForPlayer == null || playerStats.ListStatsMatchForOpp == null)
                return;
            //playerStats.AcesByCourt['0'] = playerStats.ListStatsMatchForPlayer.Sum(a => a.ACES_1);
            //playerStats.AcesByCourt['0'] = playerStats.ListStatsMatchForPlayer.Count();
            //All,H,C,I,G => 1-2-3-4-5-6,1, 2, 3-4-6, 5
            Console.WriteLine(playerStats.PlayerName);
            for (int i = 0; i <= Court.ListCourtIndex.Count-1; i++)
            {
                int[] listCourtsIndexForThisSurface = Court.ListCourtIndex[i];
                //read all stats where 
                List<StatsPlayerForOneMatch> statsOnCourt =
                playerStats.ListStatsMatchForPlayer
                    .Where(m=> listCourtsIndexForThisSurface.Contains(m.courtIndex)).ToList();
                Console.WriteLine($"Courts:{String.Join("-",listCourtsIndexForThisSurface)},{statsOnCourt.Count()} matches");
                if (statsOnCourt != null && statsOnCourt.Count() > 0)
                {
                    playerStats.AcesByCourt[i] = statsOnCourt.Sum(p => p.ACES_1);
                    playerStats.DfByCourt[i] = statsOnCourt.Sum(p => p.DF_1);
                    playerStats.NbServiceGamesByCourt[i] = statsOnCourt.Sum(p => p.nbServiceGamesPlayed);
                    playerStats.NbReturnGamesByCourt[i] = statsOnCourt.Sum(p => p.nbReturnGamesPlayed);
                    playerStats.NbServicePointsByCourt[i] = statsOnCourt.Sum(p => p.FSOF_1);
                    playerStats.MatchAvgAceRateByCourtPct[i] = Math.Round(statsOnCourt.Average(p => p.getPctAceByPointsOnServe()),1);
                    playerStats.MatchAvgDfRateByCourtPct[i] = Math.Round(statsOnCourt.Average(p => p.getPctDfByPointsOnServe()), 1);
                    playerStats.GameAvgAceRateByCourt[i] = Math.Round(statsOnCourt.Average(p => p.getAceByGamesOnServe()), 2);
                    playerStats.GameAvgDfRateByCourt[i] = Math.Round(statsOnCourt.Average(p => p.getDfByGamesOnServe()), 2);
                    playerStats.MatchAvgPtsWonOnServeCourt[i] = Math.Round(statsOnCourt.Average(p => p.getServePctWon()), 1);
                    playerStats.MatchAvgPtsWonOnReturnCourt[i] = Math.Round(statsOnCourt.Average(p => p.getReturnPctWon()), 1);
                    playerStats.MatchAvgPctPtsWonFirstServeCourt[i] = Math.Round(statsOnCourt.Average(p => p.getFirstServePctWon()), 1);
                    playerStats.NbMatchesByCourt[i] = statsOnCourt.Count();

                    IEnumerable<StatsPlayerForOneMatch> statsOppOnCourt =
                    playerStats.ListStatsMatchForOpp
                        .Where(m => listCourtsIndexForThisSurface.Contains(m.courtIndex));
                    playerStats.AcesByCourtOpp[i] = statsOppOnCourt.Sum(p => p.ACES_1);
                    playerStats.DfByCourtOpp[i] = statsOppOnCourt.Sum(p => p.DF_1);
                    playerStats.NbReturnPointsByCourt[i] = statsOppOnCourt.Sum(p => p.FSOF_1);
                    playerStats.MatchAvgAceRateByCourtPctOpp[i] = Math.Round(statsOppOnCourt.Average(p => p.getPctAceByPointsOnServe()), 1);
                    playerStats.MatchAvgDfRateByCourtPctOpp[i] = Math.Round(statsOppOnCourt.Average(p => p.getPctDfByPointsOnServe()), 1);
                    playerStats.MatchAvgPctPtsWonFirstReturnCourt[i] = Math.Round(100 -statsOnCourt.Average(p => p.getFirstServePctWon()), 1);
                    playerStats.GameAvgAceRateByCourtOpp[i] = Math.Round(statsOppOnCourt.Average(p => p.getAceByGamesOnServe()), 2);
                    playerStats.GameAvgDfRateByCourtOpp[i] = Math.Round(statsOppOnCourt.Average(p => p.getDfByGamesOnServe()), 2);

                }
            }

        }
        public long PlayerId { get; set; }
        public int PlayerRank { get; set; }
        public string PlayerName { get; set; }
        public string PlayerNote { get; set; }
        /// <summary>
        /// Get the average of all ace rate per match
        /// NOTcalculated by total/nb match
        /// </summary>
        public StatByCourt<double> MatchAvgAceRateByCourtPct { get; set; }
        public StatByCourt<double> MatchAvgDfRateByCourtPct { get; set; }
        public StatByCourt<double> MatchAvgAceRateByCourtPctOpp { get; set; }
        public StatByCourt<double> MatchAvgDfRateByCourtPctOpp { get; set; }
        /// <summary>
        /// Average of all average matches ace by game
        /// </summary>
        public StatByCourt<double> GameAvgAceRateByCourt { get; set; }
        /// <summary>
        /// Average of all average matches DF by game
        /// </summary>
        public StatByCourt<double> GameAvgDfRateByCourt { get; set; }
        public StatByCourt<double> GameAvgAceRateByCourtOpp { get; set; }
        public StatByCourt<double> GameAvgDfRateByCourtOpp { get; set; }
        public StatByCourt<double> MatchAvgPtsWonOnServeCourt { get; set; }
        public StatByCourt<double> MatchAvgPtsWonOnReturnCourt { get; set; }

        public StatByCourt<double> MatchAvgPctPtsWonFirstServeCourt { get; set; }
        public StatByCourt<double> MatchAvgPctPtsWonFirstReturnCourt { get; set; }
        public StatByCourt<int> AcesByCourt { get; set; }
        public StatByCourt<int> DfByCourt { get; set; }
        public StatByCourt<int> AcesByCourtOpp { get; set; }
        public StatByCourt<int> DfByCourtOpp { get; set; }
        //// <summary>
        /// Key will 0,H, C, I, G (0=All, in on court = 1, 2, 3-4-6, 5) 
        /// </summary>
        public StatByCourt<int> NbMatchesByCourt { get; set; }
        public StatByCourt<int> NbServiceGamesByCourt { get; set; }
        public StatByCourt<int> NbReturnGamesByCourt { get; set; }
        public StatByCourt<int> NbServicePointsByCourt { get; set; }
        public StatByCourt<int> NbReturnPointsByCourt { get; set; }

        [XmlIgnore]
        public List<MatchDetailsWithOdds> ListMatches { get; set; }
        [XmlIgnore]
        public List<StatsPlayerForOneMatch> ListStatsMatchForPlayer { get; set; } = new List<StatsPlayerForOneMatch>();
        [XmlIgnore]
        public List<StatsPlayerForOneMatch> ListStatsMatchForOpp { get; set; } = new List<StatsPlayerForOneMatch>();
    }
}