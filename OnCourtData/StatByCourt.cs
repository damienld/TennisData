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
    public class StatByCourt<T> : List<T>
    {
        [XmlIgnore]
        public AceReportPlayer StatsParent { get; set; }
        public StatByCourt()
        {
        }
        public StatByCourt(int Length, AceReportPlayer statsParent)
        {
            this.Capacity = Length;
            StatsParent = statsParent;
        }
        public static string DisplayAllStats(StatByCourt<T> stat, AceReportPlayer statsParent)
        {
            string res = "";
            for (int i = 0; i < stat.Count; i++)
            {
                if (typeof(T) == typeof(double))
                    res += String.Format("{0:0.00}", stat[i]);
                else
                    res += stat[i];
                if (statsParent.NbMatchesByCourt[i] < 15)
                    res += $"({statsParent.NbMatchesByCourt[i]})";
                res += "-";
            }
            return res;
        }
        public string DisplayAllStats(AceReportPlayer statsParent
            , int aIndexCourt3Values)
        {
            return DisplayAllStats(statsParent, Court.getCourtindexForStatsWithNonClayAndClayOnly2(aIndexCourt3Values));
        }
            public string DisplayAllStats(AceReportPlayer statsParent
            , List<int> aListIndexOfAll6Courts)
        {
            string res = "";

            if (aListIndexOfAll6Courts == null)
            {//all courts
                int indexCourt = 0;
                if (typeof(T) == typeof(double))
                    res += String.Format("{0:0.00}", this[indexCourt]);
                else
                    res += this[indexCourt];
                if (statsParent.NbMatchesByCourt[indexCourt] < 15)
                    res += $"({statsParent.NbMatchesByCourt[indexCourt]})";
            }
            else
            {
                double countStat = 0;
                int nbMatches = 0;
                foreach (var indexCourt in aListIndexOfAll6Courts)
                {//for each listed court
                    int _indexCourt1to4 = indexCourt;
                    if (indexCourt == 5) //grass
                        _indexCourt1to4 = 4;
                    if (typeof(T) == typeof(double))
                        countStat += Convert.ToDouble(this[_indexCourt1to4]);
                    else
                        countStat += Convert.ToInt16(this[_indexCourt1to4]);
                    nbMatches += statsParent.NbMatchesByCourt[_indexCourt1to4];
                    //res += "-";
                }
                if (typeof(T) == typeof(double))
                    res += String.Format("{0:0.00}", countStat);
                else
                    res += countStat;
                if (nbMatches < 15)
                    res += $"({nbMatches})";
            }
            return res;
        }
    }
}
