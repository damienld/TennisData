using OnCourtData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace OnCourtData
{
    public static class AcesReportingPlayer
    {
        /*
        /// <summary>
        /// Return 2 lists:
        /// 1) List of diff between Ace Rate and ace race exptected(0.8 * ace rate avg P + 0.2 * ace rate vs avg Opp)  
        /// 2) List of diff between Ace Rate Opp and ace race opp exptected(0.8 * ace rate avg Opp + 0.2 * ace rate Opp vs avg)  
        /// </summary>
        /// <param name="aIdPlayer"></param>
        /// <param name="matches"></param>
        /// <param name="aIsATP"></param>
        /// <returns></returns>
        public static (List<(double, Match)>, List<(double, Match)>) 
            compileStatsAces(long aIdPlayer, List<Match> matches, bool aIsATP
            , double coeffServerInAces, double coeffServerInAces_2)
        {
            List<(double, Match)> listDiffActualAceRateVsExpected = new List<(double, Match)>();
            List<(double, Match)> listDiffActualAceRateConcededVsExpected = new List<(double, Match)>();
            foreach (var aMatch in matches)
            {
                long idP= aMatch.Id1;
                long IdOpp = aMatch.Id2;
                StatsPlayerForOneMatch statsP = aMatch.StatsByPlayers[0];
                StatsPlayerForOneMatch statsOpp = aMatch.StatsByPlayers[1];
                int yearMatch = aMatch.Date.Value.Year;
                List<int> listYearsToLookInReports = new List<int> { aMatch.Date.Value.Year, aMatch.Date.Value.Year - 1 };
                List<int> listCoeffsYears = new List<int> { 1, 1 }; 
                         //, aMatch.Date.Value.Year - 2};
                if (aMatch.Id2 == aIdPlayer)
                {//player is 2nd
                    idP = aMatch.Id2;
                    IdOpp = aMatch.Id1; 
                    statsP = aMatch.StatsByPlayers[1];
                    statsOpp = aMatch.StatsByPlayers[0];
                }
                if (aMatch.StatsByPlayers[0].nbServiceGamesPlayed < 0)
                    aMatch.readServeAndReturnStats();
                int idCourtFor6Courts = aMatch.CourtId;
                List<int> listCourts = Court.getCourtindexForStatsWithNonClayAndClayOnly(idCourtFor6Courts);
                (double, double, double) regressionValues = Match.regressionValuesByCourtATP[aMatch.CourtId==2?2:1];
                if (! aIsATP)
                    regressionValues = Match.regressionValuesByCourtWTA[aMatch.CourtId == 2 ? 2 : 1];
                double actualAceRateP1 = statsP.getPctAceByPointsOnServe();
                double actualAceRateP2 = statsOpp.getPctAceByPointsOnServe();
                try
                {
                    double expectedAceRateP1 = 0; double expectedAceRateOppP1 = 0; int nbMatchesP1 = 0;
                    double expectedAceRateP2 = 0; double expectedAceRateOppP2 = 0; int nbMatchesP2 = 0;
                    List<AceReportPlayer> reportsPlayer =
                                AcesReportingPlayer.getAllYearlyAceReports(idP
                                , listYearsToLookInReports, aIsATP);
                    (expectedAceRateP1, expectedAceRateOppP1, nbMatchesP1)
                        = AcesReportingPlayer.getAvgAceRateForPlayer(reportsPlayer, listCourts, listCoeffsYears);
                    reportsPlayer =
                         AcesReportingPlayer.getAllYearlyAceReports(IdOpp
                         , listYearsToLookInReports, aIsATP);
                    (expectedAceRateP2, expectedAceRateOppP2, nbMatchesP2)
                        = AcesReportingPlayer.getAvgAceRateForPlayer(reportsPlayer, listCourts, listCoeffsYears);
                    double expRatewithCoeffP1 = (expectedAceRateP1 * regressionValues.Item1)
                        + (expectedAceRateOppP2) * regressionValues.Item2 + regressionValues.Item3;
                    double diffP1 = Math.Round(actualAceRateP1 * 1.0F / expRatewithCoeffP1, 2);

                    double expRatewithCoeffP2 = (expectedAceRateP2 * regressionValues.Item1)
                        + (expectedAceRateOppP1) * regressionValues.Item2 + regressionValues.Item3;
                    double diffP2 = Math.Round(actualAceRateP2 * 1.0F / expRatewithCoeffP2, 2);

                    listDiffActualAceRateVsExpected.Add((diffP1, aMatch));
                    listDiffActualAceRateConcededVsExpected.Add((diffP2, aMatch));

                }
                catch
                { }
            }
            return (listDiffActualAceRateVsExpected, listDiffActualAceRateConcededVsExpected);
        }
        */
        /// <summary>
        /// Aggregates the average stats of all selected courts (Hard,Clay,Indoor/Carpet,Grass)
        /// Weigthed x 2 for all matches >= year of date -1
        /// </summary>
        /// <param name="listReportPlayer"></param>
        /// <returns></returns>
        public static (double AvgPtsWonSrv, double AvgPtsWonRtrn, int NbMatches)
            getAvgServPtsForPlayer(List<AceReportPlayer> listReportPlayer, List<int> aListIndexOfAll4Courts
            , int yearEnd)
        {

            double AvgPtsWonSrv = 0;
            double AvgPtsWonRtrn = 0;
            double nbMatchesIncludingWeight = 0;
            int nbMatches = 0;
            for (int i = 0; i < listReportPlayer.Count; i++)
            {
                AceReportPlayer item = listReportPlayer[i];
                double coeffThisYear = 1;
                if (item.Year >= yearEnd - 1)
                    coeffThisYear = 1.2;
                else if (item.Year >= yearEnd - 2)
                    coeffThisYear = 0.9;
                else coeffThisYear = 0.7;
                if (aListIndexOfAll4Courts == null)
                {//all courts
                    int idCourt = 0;
                    nbMatches += item.NbMatchesByCourt[idCourt];
                    nbMatchesIncludingWeight += item.NbMatchesByCourt[idCourt] * coeffThisYear;
                    AvgPtsWonSrv += item.MatchAvgPtsWonOnServeCourt[idCourt] * item.NbMatchesByCourt[idCourt] * coeffThisYear;
                    AvgPtsWonRtrn += item.MatchAvgPtsWonOnReturnCourt[idCourt] * item.NbMatchesByCourt[idCourt] * coeffThisYear;
                }
                else
                    foreach (var indexCourt in aListIndexOfAll4Courts)
                    {//for each listed court
                        int idCourt = Court.getCourtindexBetween4Base1(indexCourt);
                        nbMatches += item.NbMatchesByCourt[idCourt];
                        nbMatchesIncludingWeight += item.NbMatchesByCourt[idCourt] * coeffThisYear;
                        AvgPtsWonSrv += item.MatchAvgPtsWonOnServeCourt[idCourt] * item.NbMatchesByCourt[idCourt] * coeffThisYear;
                        AvgPtsWonRtrn += item.MatchAvgPtsWonOnReturnCourt[idCourt] * item.NbMatchesByCourt[idCourt] * coeffThisYear;
                    }
            }
            if (nbMatchesIncludingWeight > 0)
                return (Math.Round(AvgPtsWonSrv / nbMatchesIncludingWeight, 1), Math.Round(AvgPtsWonRtrn / nbMatchesIncludingWeight, 1), nbMatches);
            else
                return (-1, -1, 0);
        }
        /// <summary>
        /// Aggregates the average stats of all selected courts (Hard,Clay,Indoor/Carpet,Grass)
        /// Weigthed x 2 for all matches >= year of date -1
        /// </summary>
        /// <param name="listReportPlayer"></param>
        /// <returns></returns>
        public static (double AceRate, double AceRateOpp, int NbMatches) 
            getAvgAceRateForPlayer(List<AceReportPlayer> listReportPlayer, List<int> aListIndexOfAll4Courts
            , int yearEnd)
        {
            
            double sumAces = 0;
            double sumAcesOpp = 0;
            double nbMatchesIncludingWeight = 0;
            double sumAvgAceRateOpp = 0;
            double sumAvgTrnSpeed = 0;
            int nbMatches = 0;
            for (int i = 0; i < listReportPlayer.Count; i++)
            {
                AceReportPlayer item = listReportPlayer[i];
                double coeffThisYear = 1;
                if (item.Year >= yearEnd - 1)
                    coeffThisYear = 1.2;
                else if(item.Year >= yearEnd - 2)
                    coeffThisYear = 0.9;
                else coeffThisYear = 0.7;
                if (aListIndexOfAll4Courts == null)
                {//all courts
                    int idCourt = 0;
                    nbMatches += item.NbMatchesByCourt[idCourt];
                    nbMatchesIncludingWeight += item.NbMatchesByCourt[idCourt] * coeffThisYear;
                    sumAces += item.MatchAvgAceRateByCourtPct[idCourt] * item.NbMatchesByCourt[idCourt] * coeffThisYear;
                    //sumAcesOpp += item.MatchAvgAceRateByCourtPctOpp[idCourt] * item.NbMatchesByCourt[idCourt] * coeffThisYear;
                    //sumAvgAceRateOpp += item. * item.NbMatchesByCourt[idCourt] * coeffThisYear;
                }
                else
                    foreach (var indexCourt in aListIndexOfAll4Courts)
                    {//for reach listed court
                        int idCourt = Court.getCourtindexBetween4Base1(indexCourt);
                        nbMatches += item.NbMatchesByCourt[idCourt];
                        nbMatchesIncludingWeight += item.NbMatchesByCourt[idCourt] * coeffThisYear;
                        sumAces += item.MatchAvgAceRateByCourtPct[idCourt] * item.NbMatchesByCourt[idCourt] * coeffThisYear;
                        sumAcesOpp += item.MatchAvgAceRateByCourtPctOpp[idCourt] * item.NbMatchesByCourt[idCourt] * coeffThisYear;
                    }
            }
            if (nbMatchesIncludingWeight > 0)
                return (Math.Round(sumAces/nbMatchesIncludingWeight,1), Math.Round(sumAcesOpp / nbMatchesIncludingWeight,1), nbMatches);
            else
                return (-1,-1,0);
        }
        public static (double AceRate, double AceRateOpp, int NbMatches)
            getAvgAcePerGameForPlayer(List<AceReportPlayer> listReportPlayer, List<int> aListIndexOfAll6Courts)
        {
            double sum = 0;
            double sumOpp = 0;
            int nbMatches = 0;
            foreach (var item in listReportPlayer)
            {
                if (aListIndexOfAll6Courts == null)
                {//all courts
                    int idCourt = 0;
                    nbMatches += item.NbMatchesByCourt[idCourt];
                    sum += item.GameAvgAceRateByCourt[idCourt] * item.NbMatchesByCourt[idCourt];
                    sumOpp += item.GameAvgAceRateByCourtOpp[idCourt] * item.NbMatchesByCourt[idCourt];
                }
                else
                    foreach (var indexCourt in aListIndexOfAll6Courts)
                    {//for reach listed court
                        int idCourt = Court.getCourtindexBetween4Base1(indexCourt);
                        nbMatches += item.NbMatchesByCourt[idCourt];
                        sum += item.GameAvgAceRateByCourt[idCourt] * item.NbMatchesByCourt[idCourt];
                        sumOpp += item.GameAvgAceRateByCourtOpp[idCourt] * item.NbMatchesByCourt[idCourt];
                    }
            }
            return (Math.Round(sum / nbMatches, 2), Math.Round(sumOpp / nbMatches, 2), nbMatches);
        }
        public static List<AceReportPlayer> getAllYearlyAceReports(long idPlayer, List<int> listYears, bool isATP)
        {
            List<AceReportPlayer> result = new List<AceReportPlayer>();
            Dictionary<int, List<AceReportPlayer>> reports =
                AcesReportingPlayer.fListPlayerServeReturnATPByYear;
            if (!isATP)
                reports = AcesReportingPlayer.fListPlayerServeReturnWTAByYear;
            foreach (var item in reports)//for each year
            {
                if (listYears == null || listYears.Contains(item.Key) )
                {//if year is from list
                    List<AceReportPlayer> listAllP = item.Value;
                    if (listAllP != null)
                    {
                        AceReportPlayer playerReport = listAllP.FirstOrDefault(p => p.PlayerId == idPlayer);
                        if (playerReport != null)
                        {
                            playerReport.Year = item.Key;
                            result.Add(playerReport);
                        }
                    }
                }
            }
            return result;
        }

        public static string FileNamePlayerServeReturnATP = @"./Files/" + "PlayerServeReturn" + "ATP_Yr" + ".xml";
        public static string FileNamePlayerServeReturnWTA = @"./Files/" + "PlayerServeReturn" + "WTA_Yr" + ".xml";
        public static Dictionary<int, List<AceReportPlayer>> fListPlayerServeReturnATPByYear = null;
        public static Dictionary<int, List<AceReportPlayer>> fListPlayerServeReturnWTAByYear = null;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="aNbPlayers"></param>
        /// <param name="aNbYearsSince">since N-1 year</param>
        public static async Task<List<AceReportPlayer>> BuildReportsPlayersAsync(
            string connectionString, int aNbPlayers
            , bool aIsIncludeStats, DateTime _date)
        {
            List<AceReportPlayer> listATP= 
                await AcesReportingPlayer.GenerateReportForTopXAsync(connectionString, _date, _date.AddYears(1)
                , true, aNbPlayers, aIsIncludeStats);
            SaveReportToXml(FileNamePlayerServeReturnATP.Replace("_Yr", (_date.Year+1)+""), listATP);
            List<AceReportPlayer> listWTA = await AcesReportingPlayer.GenerateReportForTopXAsync(
                connectionString, _date, _date.AddYears(1)
                , false, aNbPlayers, aIsIncludeStats);
            SaveReportToXml(FileNamePlayerServeReturnWTA.Replace("_Yr", (_date.Year + 1) + ""), listWTA);
            initLoadReports();
            return listATP;
        }
        public static void initLoadReports()
        {
            fListPlayerServeReturnATPByYear =
                new Dictionary<int, List<AceReportPlayer>>();
            for (int i = 2015; i <= DateTime.Now.Year; i++)
            {
                try
                {
                    fListPlayerServeReturnATPByYear.Add(i
                        , FPlayerStats_loadReportFromXml(FileNamePlayerServeReturnATP, i));
                }
                catch { }
            }
            fListPlayerServeReturnWTAByYear =
                new Dictionary<int, List<AceReportPlayer>>();
            for (int i = 2015; i <= DateTime.Now.Year; i++)
            {
                try
                {
                    fListPlayerServeReturnWTAByYear.Add(i
                    , FPlayerStats_loadReportFromXml(FileNamePlayerServeReturnWTA, i));
                }
                catch { }
            }

        }
        public static List<AceReportPlayer> FPlayerStats_loadReportFromXml
            (string aPath, int aYear)
        {
            try
            {
                aPath = aPath.Replace("_Yr", aYear+"");
                XmlSerializer xs = new XmlSerializer(typeof(List<AceReportPlayer>));
                System.IO.StreamReader file = new System.IO.StreamReader(aPath);

                object b = xs.Deserialize(file);
                List<AceReportPlayer> a = b
                    as List<AceReportPlayer>;
                file.Close();
                return a;
            }
            catch (Exception e)
            {
                return null;
            }

        }
        public static void SaveReportToXml(string aPath, List<AceReportPlayer> aList)
        {
            try
            {
                System.Xml.Serialization.XmlSerializer x =
                        new System.Xml.Serialization.XmlSerializer(typeof(List<AceReportPlayer>));
                System.IO.FileStream file = System.IO.File.Create(aPath);
                x.Serialize(file, aList);
                file.Close();
            }
            catch (Exception exc)
            {

            }

        }
        private static async Task<List<AceReportPlayer>> GenerateReportForTopXAsync
            (string connectionString, DateTime aDateSince, DateTime aDateEnd, bool aIsAtp
            , int aRankMaxForTopX, bool aIsIncludeStats)
        {
            PlayersCollection _listPlayers;
            if (aDateEnd.Year == DateTime.Now.Year)
                //current year, use current ranking
                _listPlayers = SqlOnCourt.getListPlayersSql("", connectionString
                , aRankMaxForTopX, aIsAtp);
            else
                _listPlayers = SqlOnCourt.getListPlayersSqlEndOfYear(connectionString
                , aIsAtp, aDateEnd.Year, aRankMaxForTopX);
            List<AceReportPlayer> list = await GenerateReportAsync(connectionString, aDateSince, aDateEnd, aIsAtp
                , _listPlayers, aIsIncludeStats);
            return list;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="aDateSince"></param>
        /// <param name="aIsAtp"></param>
        /// <param name="_listPlayers"></param>
        /// <param name="aFilename">leave "" for not recording</param>
        public static Task<List<AceReportPlayer>> GenerateReportAsync(string connectionString, DateTime aDateSince
            , DateTime aDateEnd, bool aIsAtp, PlayersCollection _listPlayers, bool aIsIncludeStats)
        //, string aFilename)
        {
            return Task.Run(
                () =>
                {
                    List<AceReportPlayer> _listReports = new List<AceReportPlayer>();
                    foreach (Player _player in _listPlayers)
                    {
                        List<MatchDetailsWithOdds> _matchesForPlayer =
                           SqlOnCourt.getListMatchesForPlayerBySql(_player.Id, null, connectionString, 1000, false
                            , aIsIncludeStats, false, aIsAtp, aDateSince, aDateEnd);
                        //no small itf, no DC or exhib
                        _matchesForPlayer = _matchesForPlayer.Where(m => m.TournamentRank >= 1 && m.TournamentRank <= 4)
                        .ToList();
                        AceReportPlayer s 
                        = new AceReportPlayer(_player.Id,_player.Rank.GetValueOrDefault(-1), _player.Name
                        , _player.Note, _matchesForPlayer);
                        if (s != null)
                            _listReports.Add(s);
                    };
                    return _listReports;
                }
            );
            

        }

    }
}
