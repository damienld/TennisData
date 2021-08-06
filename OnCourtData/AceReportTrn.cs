using OnCourtData;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace OnCourtData
{
    [Serializable]
    public class AceReportTrn
    {
        /*public static List<int> fListIntervallsSpeedATPClay
            = new List<int>(ConfigurationManager.AppSettings["IntervallsSpeedATPClay"].Split(new char[] { ',' }));
        public static List<int> fListIntervallsSpeedATPNonClay
            = new List<int>(ConfigurationManager.AppSettings["IntervallsSpeedATPNonClay"].Split(new char[] { ',' }));
        public static List<int> fListIntervallsSpeedWTA
            = new List<int>(ConfigurationManager.AppSettings["IntervallsSpeedWTA"].Split(new char[] { ',' }));*/
        public static AceReportTrn getTrnSpeed(int idTrn, bool isATP)
        {
            List<AceReportTrn> listTrn;
            if (isATP)
                listTrn = AcesReportingTrn.fListTrnAcesATPByYear;
            else
                listTrn = AcesReportingTrn.fListTrnAcesWTAByYear;
            return listTrn.FirstOrDefault(t => t.TrnId == idTrn);
        }
        /// <summary>
        /// IF Clay, return true if the surface is clay and surface speed > X
        /// IF Non Clay, return true if the surface is not clay and surface speed > X
        /// </summary>
        /// <param name="idCourt"></param>
        /// <returns></returns>
        /*public bool isSurfaceSpeed(int idCourt, double indexSpeedMin, double indexSpeedMax)
        {
            if (idCourt == 2)
                return Speed >= indexSpeedMin && CourtId == 2 && NbMatchesForSpeed >= 10;
            else
                return Speed > 1.15 && CourtId != 2 && NbMatchesForSpeed >= 10;
        }*/
        private bool IsRightClayOrNonClayCourt(int idCourt)
        {
            if (idCourt == 2)
                return 2 == CourtId;
            else
                return CourtId != 2;
        }
        public bool isFastSurface(int idCourt, bool isATP)
        {
            double min = 1.05;
            if (!isATP)
                min = 1.05;
            return SpeedAmongCourtCateg >= min && NbMatchesForSpeed >= 10 && IsRightClayOrNonClayCourt(idCourt);
            if (idCourt == 2)
                return Speed >= 0.83 && CourtId == 2 && NbMatchesForSpeed >= 10;
            else
                return Speed >= 1.15 && CourtId != 2 && NbMatchesForSpeed >= 10;
        }
        public bool isVeryFastSurface(int idCourt, bool isATP)
        {
            double min = 1.1;
            if (!isATP)
                min = 1.1;
            return SpeedAmongCourtCateg >= min && NbMatchesForSpeed >= 10 && IsRightClayOrNonClayCourt(idCourt);
            if (idCourt == 2)
                return Speed > 0.95 && CourtId == 2 && NbMatchesForSpeed >= 10;
            else
                return Speed >= 1.23 && CourtId != 2 && NbMatchesForSpeed >= 10;
        }
        public bool isMediumSurface(int idCourt, bool isATP)
        {
            double min = 0.93;
            double max = 1.05;
            if (!isATP)
            {
                min = 0.93;
                max = 1.05;
            }
            return SpeedAmongCourtCateg < max && SpeedAmongCourtCateg > min && NbMatchesForSpeed >= 10 && IsRightClayOrNonClayCourt(idCourt);
            if (idCourt == 2)
                return (Speed > 0.72 && Speed < 0.83) && CourtId == 2 && NbMatchesForSpeed >= 10;
            else
                return (Speed > 1.02 && Speed < 1.15) && CourtId != 2 && NbMatchesForSpeed >= 10;
        }
        public bool isSlowSurface(int idCourt, bool isATP)
        {
            double max = 0.93;
            if (!isATP)
                max = 0.93;
            return SpeedAmongCourtCateg <= max && NbMatchesForSpeed >= 10 && IsRightClayOrNonClayCourt(idCourt);
            if (idCourt == 2)
                return Speed <= 0.72 && CourtId == 2 && NbMatchesForSpeed >= 10;
            else
                return Speed <= 1.02 && CourtId != 2 && NbMatchesForSpeed >= 10;
        }
        public bool isVerySlowSurface(int idCourt, bool isATP)
        {
            double max = 0.88;
            if (!isATP)
                max = 0.88;
            return SpeedAmongCourtCateg <= max && NbMatchesForSpeed >= 10 && IsRightClayOrNonClayCourt(idCourt);
            if (idCourt == 2)
                return Speed < 0.65 && CourtId == 2 && NbMatchesForSpeed >= 10;
            else
                return Speed < 0.95 && CourtId != 2 && NbMatchesForSpeed >= 10;
        }
        /// <summary>
        /// For Serializing
        /// </summary>
        public AceReportTrn()
        { }
        public DateTime Date { get; set; }
        public long TrnId { get; set; }
        public string TrnName { get; set; }
        public int TrnRank { get; set; }
        public int CourtId { get; set; }
        public string TournamentSite { get; set; }
        public int NbMatchesTrn { get; set; }
        public double Speed  { get; set; }
        public int NbMatchesForSpeed { get; set; }
        /// <summary>
        /// Speed  among other clay courts if clay (or among all non clay courts if non clay)
        /// </summary>
        public double SpeedAmongCourtCateg { get; set; }
        public int NbMatchesForSpeedAmongCourtCateg { get; set; }
        public string MyNote { get; set; }
        //public double SumActualAceRateTrn { get; set; }
        //public double SumPredictedAceRateTrn{ get; set; }
        public override string ToString()
        {
            return $"{TrnName},{Date.Year},{Speed},{SpeedAmongCourtCateg}";
        }
    }
}