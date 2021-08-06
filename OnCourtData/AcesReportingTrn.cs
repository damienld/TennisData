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
    public static class AcesReportingTrn
    {
        //public static double coeffServerInAcesExp = 0.8;
        public static string FileNameAcesReportingTrnATP = @"./Files/" + "TrnAces" + "ATP" + ".xml";
        public static string FileNameAcesReportingTrnWTA = @"./Files/" + "TrnAces" + "WTA" + ".xml";
        public static List<AceReportTrn> fListTrnAcesATPByYear = null;
        public static List<AceReportTrn> fListTrnAcesWTAByYear = null;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="aNbPlayers"></param>
        /// <param name="aNbYearsSince">since N-1 year</param>
        public static async Task<List<AceReportTrn>> BuildReportsTrnAsync(
            string connectionString, int aLevelTrnMin
            , bool aIsIncludeStats, DateTime _date, bool isATP)
        {
            fListTrnAcesATPByYear = 
                await AcesReportingTrn.GetAllMatchesForTrnsReportAndGenerateReporAsync(connectionString, _date
                , true, aIsIncludeStats, aLevelTrnMin);
            SaveReportToXml(FileNameAcesReportingTrnATP, fListTrnAcesATPByYear);
            fListTrnAcesWTAByYear = await AcesReportingTrn.GetAllMatchesForTrnsReportAndGenerateReporAsync(
                connectionString, _date
                , false, aIsIncludeStats, aLevelTrnMin);
            SaveReportToXml(FileNameAcesReportingTrnWTA, fListTrnAcesWTAByYear);
            //initLoadReportsTrns();
            if (isATP)
                return fListTrnAcesATPByYear;
            else
                return fListTrnAcesWTAByYear;
        }
        public static void initLoadReports()
        {
            fListTrnAcesATPByYear =
            LoadReportFromXml(FileNameAcesReportingTrnATP);
            fListTrnAcesWTAByYear =
            LoadReportFromXml(FileNameAcesReportingTrnWTA);

        }
        public static List<AceReportTrn> LoadReportFromXml
            (string aPath)
        {
            try
            {
                XmlSerializer xs = new XmlSerializer(typeof(List<AceReportTrn>));
                System.IO.StreamReader file = new System.IO.StreamReader(aPath);

                object b = xs.Deserialize(file);
                List<AceReportTrn> a = b
                    as List<AceReportTrn>;
                file.Close();
                return a;
            }
            catch (Exception e)
            {
                return null;
            }

        }
        public static void SaveReportToXml(string aPath, List<AceReportTrn> aList)
        {
            try
            {
                System.Xml.Serialization.XmlSerializer x =
                        new System.Xml.Serialization.XmlSerializer(typeof(List<AceReportTrn>));
                System.IO.FileStream file = System.IO.File.Create(aPath);
                x.Serialize(file, aList);
                file.Close();
            }
            catch (Exception exc)
            {

            }

        }
        private static async Task<List<AceReportTrn>> GetAllMatchesForTrnsReportAndGenerateReporAsync
            (string connectionString, DateTime aDateSince, bool aIsAtp, bool aIsIncludeStats, int aLevelTrnMin)
        {
            List<Tournament> _listTrns;
                _listTrns = SqlOnCourt.getListTrnSql(connectionString, aIsAtp,1000000, aDateSince.Year
                    , aLevelTrnMin);

            List<AceReportTrn> list = await GenerateTrnsReportAsync(connectionString, aDateSince, DateTime.Now, aIsAtp
                , _listTrns, aIsIncludeStats);
            return list;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="aDateSince"></param>
        /// <param name="aIsAtp"></param>
        /// <param name="_listTrns"></param>
        /// <param name="aFilename">leave "" for not recording</param>
        public static Task<List<AceReportTrn>> GenerateTrnsReportAsync(string connectionString, DateTime aDateSince
            , DateTime aDateEnd, bool aIsAtp, List<Tournament> _listTrns, bool aIsIncludeStats)
        //, string aFilename)
        {
            return Task.Run(
                () =>
                {
                    //foreach (Tournament _trn in _listTrns)
                    //{ 
                    _listTrns = _listTrns.Where(t => t.Rank <= 4 && t.Rank > 0).ToList();
                    List<MatchDetailsWithOdds> _allMatchesForListTrns =
                        SqlOnCourt.getListMatchesForPlayerBySql(-1, _listTrns.Select(t=>t.Id).ToList()
                        , connectionString, 10000000, false
                        , aIsIncludeStats, false, aIsAtp, aDateSince, aDateEnd
                        , false);
                    //no small itf, no DC or exhib
                    //only R1+
                    //TEMP !!!! hard only && RAnk should be <= 4!
                    _allMatchesForListTrns = _allMatchesForListTrns
                    .Where(m=>m.TournamentRank <= 4 &&m.RoundId >= 1).ToList();// && m.CourtId == selCourt)

                    //StringBuilder allLines = new StringBuilder();
                    //allLines.AppendLine(Tournament.csvHeader + ",nbMatchesTrn,SpeedIndex,nbMatchesAcesStats,"
                    //    + "actualAceRateTrn,predictedAceRateTrn"); //CourtId
                    List<AceReportTrn> listForReport= new List<AceReportTrn>();
                    //all tournaments
                    foreach (var matchesGroupedByTrnId in _allMatchesForListTrns.GroupBy(m=>m.TournamentId))
                    {
                        int TrnId = matchesGroupedByTrnId.Key;
                        Tournament tournament = _listTrns.FirstOrDefault(t => t.Id == TrnId);
                        double sumAceRateDiff_Actual_vs_Expected = 0;
                        double sumAceRateDiff2_Actual_vs_Expected = 0;
                        int nbMatchesTrn = 0;
                        int nbMatchesForSpeedIndex = 0;
                        int nbMatchesForSpeedIndexForCourt = 0;
                        //all matches of selected Trn
                        foreach (var match in matchesGroupedByTrnId)
                        {
                            //Calc OVERALL SPEED
                            (double yearlyExpectedAceRateP1, double yearlyExpectedAceRateVsP1, int nbMatchesP1) =
                                Match.getYearlyStatsAcesAverageForPlayerHybrid(aIsAtp, match.Id1
                                , match.Date.Value.Year, 4);
                            (double yearlyExpectedAceRateP2, double yearlyExpectedAceRateVsP2, int nbMatchesP2) =
                                Match.getYearlyStatsAcesAverageForPlayerHybrid(aIsAtp, match.Id2
                                , match.Date.Value.Year, 4);
                            double predictedAceRateP1 = Math.Max(0, Match.getPredictedAcesRatePlayer
                            (yearlyExpectedAceRateP1, yearlyExpectedAceRateVsP2, 0, aIsAtp));// idCourtForRegressionParams));
                            double predictedAceRateP2 = Math.Max(0, Match.getPredictedAcesRatePlayer
                             (yearlyExpectedAceRateP2, yearlyExpectedAceRateVsP1, 0, aIsAtp));// idCourtForRegressionParams)) ;
                            
                            bool IsAceStatsValid = match.StatsByPlayers[0] != null
                                && match.StatsByPlayers[0].getPctAceByPointsOnServe() >= 0
                                && match.StatsByPlayers[1] != null
                                && match.StatsByPlayers[1].getPctAceByPointsOnServe() >= 0
                                && yearlyExpectedAceRateVsP1 > 0
                                && yearlyExpectedAceRateVsP2 > 0
                                && nbMatchesP1 >= 7 && nbMatchesP2 >= 7;
                            nbMatchesTrn += 1;
                            string namePlayer1 = match.Player1Name;
                            string namePlayer2 = match.Player2Name;
                            if (match.IsCompletedMatch() && IsAceStatsValid)
                            {
                                nbMatchesForSpeedIndex += 1;
                                double AceRateDiff_Actual_vs_Expected = Math.Max(0.5,Math.Min(1.5, (match.StatsByPlayers[0].getPctAceByPointsOnServe()
                                    + match.StatsByPlayers[1].getPctAceByPointsOnServe()) / Math.Max(0.1, predictedAceRateP1 + predictedAceRateP2)));
                                sumAceRateDiff_Actual_vs_Expected += AceRateDiff_Actual_vs_Expected;
                               Console.WriteLine($"{match.StatsByPlayers[0].getPctAceByPointsOnServe()} " +
                                   $"- ({predictedAceRateP1})  -{namePlayer1} vs {namePlayer2}");
                               Console.WriteLine($"{match.StatsByPlayers[1].getPctAceByPointsOnServe()} " +
                                   $"- ({predictedAceRateP2})  -{namePlayer2} vs {namePlayer1}");
                               Console.WriteLine($"{Math.Round(AceRateDiff_Actual_vs_Expected,2)}");
                            }
                            else { }
                            //Calc For Clay and NonClay CATEG
                            int idCourtForRegressionParams = 1; //Hard, Indoor, Grass, Acrylic
                            if (match.CourtId == 2)//clay
                                idCourtForRegressionParams = 2;
                            //Calc COURT CATEG SPEED
                            (yearlyExpectedAceRateP1, yearlyExpectedAceRateVsP1, nbMatchesP1) =
                                Match.getYearlyStatsAcesAverageForPlayer(aIsAtp, match.Id1
                                , match.Date.Value.Year, 4, match.CourtId);
                            (yearlyExpectedAceRateP2, yearlyExpectedAceRateVsP2, nbMatchesP2) =
                                Match.getYearlyStatsAcesAverageForPlayer(aIsAtp, match.Id2
                                , match.Date.Value.Year, 4, match.CourtId);
                            predictedAceRateP1 = Math.Max(0, Match.getPredictedAcesRatePlayer
                            (yearlyExpectedAceRateP1, yearlyExpectedAceRateVsP2, idCourtForRegressionParams, aIsAtp));// idCourtForRegressionParams));
                            predictedAceRateP2 = Math.Max(0, Match.getPredictedAcesRatePlayer
                             (yearlyExpectedAceRateP2, yearlyExpectedAceRateVsP1, idCourtForRegressionParams, aIsAtp));// idCourtForRegressionParams)) ;
                            IsAceStatsValid = match.StatsByPlayers != null && match.StatsByPlayers[0] != null
                            && match.StatsByPlayers[0].getPctAceByPointsOnServe() >= 0
                            && match.StatsByPlayers[1] != null
                            && match.StatsByPlayers[1].getPctAceByPointsOnServe() >= 0
                            && yearlyExpectedAceRateVsP1 > 0
                            && yearlyExpectedAceRateVsP2 > 0
                            && predictedAceRateP1 + predictedAceRateP2 >= 1
                            && nbMatchesP1 >= 6 && nbMatchesP2 >= 6;
                            if (match.IsCompletedMatch() && IsAceStatsValid) if (match.IsCompletedMatch() && IsAceStatsValid)
                            {
                                nbMatchesForSpeedIndexForCourt += 1;
                                double aceRateDiff2_Actual_vs_Expected = Math.Max(0.5, Math.Min(1.5, (match.StatsByPlayers[0].getPctAceByPointsOnServe()
                            + match.StatsByPlayers[1].getPctAceByPointsOnServe()) / Math.Max(0.1,predictedAceRateP1 + predictedAceRateP2)));
                                sumAceRateDiff2_Actual_vs_Expected += aceRateDiff2_Actual_vs_Expected;
                                Console.WriteLine("--INDEX 2---" + Environment.NewLine);
                                Console.WriteLine($"{match.StatsByPlayers[0].getPctAceByPointsOnServe()} - ({predictedAceRateP1})  -{namePlayer1} vs {namePlayer2}");
                                Console.WriteLine($"{match.StatsByPlayers[1].getPctAceByPointsOnServe()} - ({predictedAceRateP2})  -{namePlayer2} vs {namePlayer1}");
                                Console.WriteLine($"{Math.Round(aceRateDiff2_Actual_vs_Expected,2)}");
                                Console.WriteLine(Environment.NewLine);
                                }
                            else { }
                            
                        }
                        AceReportTrn trnReport = new AceReportTrn() { CourtId = tournament.CourtId.Value
                        , Date = tournament.Date.Value, TrnId = tournament.Id, TrnRank = tournament.Rank
                        , TrnName = tournament.Name, TournamentSite = tournament.TournamentSite
                        , NbMatchesTrn = nbMatchesTrn, NbMatchesForSpeed = nbMatchesForSpeedIndex
                        , Speed = Math.Round(sumAceRateDiff_Actual_vs_Expected / nbMatchesForSpeedIndex, 2)
                        , NbMatchesForSpeedAmongCourtCateg = nbMatchesForSpeedIndexForCourt
                        ,
                            SpeedAmongCourtCateg = Math.Round(sumAceRateDiff2_Actual_vs_Expected / nbMatchesForSpeedIndexForCourt, 2)
                        };
                        listForReport.Add(trnReport);
                    }
                    //};
                    //Console.WriteLine(allLines);
                    return listForReport;
                }
            );
            

        }
        
    }
}
