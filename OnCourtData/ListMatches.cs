using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnCourtData
{
    public static class ListMatches
    {
        public static List<Match> getMatchesFilterByTrnIds(List<Match> list
            , List<long> _listTrnIds)
        {

            return list.Where
                            (m => _listTrnIds.Contains(m.TournamentId)
                            ).ToList();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="list"></param>
        /// <param name="idCourt">if 2 it will use clay speed, else non clay speeds</param>
        /// <param name="indexSpeed">0=V Fast...5=V slow</param>
        /// <returns></returns>
        public static List<MatchDetailsWithOdds> 
            getMatchesFilterBySpeed(List<MatchDetailsWithOdds> list
            , int idCourt, int indexSpeed, List<AceReportTrn> trnReport, bool isATP)
        {
            if (trnReport != null)
            {
                List<long> _listTrnIds = new List<long>();

                switch (indexSpeed)
                {
                    case 0:
                        _listTrnIds
                        = trnReport.Where(t => t.isVeryFastSurface(idCourt, isATP))
                        .Select(m => m.TrnId).ToList();
                        break;
                    case 1:
                        _listTrnIds
                        = trnReport.Where(t => t.isFastSurface(idCourt, isATP))
                        .Select(m => m.TrnId).ToList();
                        break;
                    case 2:
                        _listTrnIds
                        = trnReport.Where(t => t.isMediumSurface(idCourt, isATP))
                        .Select(m => m.TrnId).ToList();
                        break;
                    case 3:
                        _listTrnIds
                        = trnReport.Where(t => t.isSlowSurface(idCourt, isATP))
                        .Select(m => m.TrnId).ToList();
                        break;
                    case 4:
                        _listTrnIds
                        = trnReport.Where(t => t.isVerySlowSurface(idCourt, isATP))
                        .Select(m => m.TrnId).ToList();
                        break;
                    default:
                        break;
                }

                if (list == null)
                    return null;
                else
                    return list.Where
                            (m => _listTrnIds.Contains(m.TournamentId)
                            ).ToList();
            }
            else return new List<MatchDetailsWithOdds>();
        }
        
        public static List<MatchDetailsWithOdds>
            getMatchesFilterBySlam(List<MatchDetailsWithOdds> list)
        {
            if (list == null)
                return null;
            else
                return list.Where(m => m.TournamentRank == 4).ToList();
        }

        public static List<MatchDetailsWithOdds>
            getMatchesFilterByWind(List<MatchDetailsWithOdds> list
            , List<string> listWindSites)
        {
            if (list == null)
                return null;
            else
                return list.Where
                        (m => listWindSites.Contains(m.TournamentSite)
                        ).ToList();
        }
        public static List<MatchDetailsWithOdds>
            getMatchesFilterBySites(List<MatchDetailsWithOdds> list
            , List<string> listSites)
        {
            if (list == null)
                return null;
            else
                return list.Where
                        (m => listSites.Contains(m.TournamentSite)
                        ).ToList();
        }
        
        public static List<MatchDetailsWithOdds>
            getMatchesFilterByWind2(bool aIsAtp, List<MatchDetailsWithOdds> list
            , long aIdP, string cnnString)
        {
            if (list == null)
                return null;
            else
            {
                List<MatchDetailsWithOdds> listFiltered = new List<MatchDetailsWithOdds>();
                List<(long idT, int idR, long idP1, long idP2)> listMatchesWind2 = SqlOnCourt.loadMyRankingsWithWind2ForPlayerSql(cnnString, aIsAtp, aIdP);
                foreach (var item in listMatchesWind2)
                {
                    MatchDetailsWithOdds m = list.FirstOrDefault
                        (p => p.Id1 == item.idP1 && p.Id2 == item.idP2 && p.RoundId == item.idR && p.TournamentId == item.idT);
                    if (m != null)
                        listFiltered.Add(m);
                }
                if (listFiltered != null)
                    listFiltered=listFiltered.OrderByDescending(p => p.Date).ToList();
                return listFiltered;
            }
        }
    }
}
