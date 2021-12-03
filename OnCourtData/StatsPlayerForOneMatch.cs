using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq.Mapping;
using System.Collections.ObjectModel;

namespace OnCourtData
{
    [Serializable]
    [Table(Name = "Stat_atp")]
    public class StatsPlayerForOneMatch
    {
        public int courtIndex { get; set; }
        public int nbServiceGamesPlayed { get; set; }
        public int nbServiceGamesWon { get; set; }
        public int nbReturnGamesPlayed { get; set; }
        public int nbTBPlayed { get; set; }

        //return -1 sometimes
        public int getFirstServePct()
        {
            if (FSOF_1 <= 0)
                return -1;
            else
                return Convert.ToInt16(Math.Round(100.0 * FS_1 / FSOF_1, 0));
        }
        //return -1 sometimes
        public int getFirstServePctWon()
        {
            if (W1SOF_1 <= 0)
                return -1;
            else
                return Convert.ToInt16(Math.Round(100.0 * W1S_1 / W1SOF_1, 0));
        }
        public double getPctAceByPointsOnServe()
        {
            try
            {
                if (W1SOF_1 <= 0 || ACES_1 < 0)
                    return -1;
                else
                    return Math.Round(100.0 * ACES_1 / FSOF_1, 1);
            }
            catch (Exception)
            {
                return -1;
            }
        }
        public double getPctDfByPointsOnServe()
        {
            try
            {
                if (W1SOF_1 <= 0)
                    return -1;
                else
                    return Math.Round(100.0 * DF_1 / FSOF_1, 1);
            }
            catch (Exception)
            {
                return -1;
            }
        }
        public double getAceByGamesOnServe()
        {
            try
            {
                if (W1SOF_1 <= 0)
                    return -1;
                else
                    return Math.Round(1.0 * ACES_1 / (nbServiceGamesPlayed+nbTBPlayed), 2);
            }
            catch (Exception)
            {
                return -1;
            }
        }
        public double getDfByGamesOnServe()
        {
            try
            {
                if (W1SOF_1 <= 0)
                    return -1;
                else
                    return Math.Round(1.0 * DF_1 / (nbServiceGamesPlayed + nbTBPlayed), 2);
            }
            catch (Exception)
            {
                return -1;
            }
        }
        /// <summary>
        /// return -1 sometimes
        /// </summary>
        /// <returns></returns>
        public int getServePctWon()
        {
            if (W1SOF_1 + W2SOF_1 <= 0)
                return -1;
            else         
                return Convert.ToInt16(Math.Round(100.0 * (W1S_1 + W2S_1) / (W1SOF_1 + W2SOF_1), 0));
        }
        /// <summary>
        /// return -1 sometimes
        /// </summary>
        /// <returns></returns>
        public int getReturnPctWon()
        {
            if (RPWOF_1 <= 0)
                return -1;
            else
                return Convert.ToInt16(Math.Round(100.0 * (RPW_1) / (RPWOF_1), 0));
        }
        public int getSecondServePctWon()
        {
            if (W2SOF_1 <= 0)
                return -1;
            else
                return Convert.ToInt16(Math.Round(100.0 * W2S_1 / W2SOF_1, 0));
        }
        /// <summary>
        /// Index Base 1
        /// </summary>
        public int IndexStatForMatch { get; set; }

        [Column(Name = "ID1", IsPrimaryKey = true)]
        public int Id1 { get; set; }

        [Column(Name = "ID2", IsPrimaryKey = true)]
        public int Id2 { get; set; }

        [Column(Name = "ID_T", IsPrimaryKey = true)]
        public int IdTournament { get; set; }

        [Column(Name = "ID_R", IsPrimaryKey = true)]
        public int IdRound { get; set; }

        /// <summary>
        /// 1st serves in
        /// </summary>
        [Column(Name = "FS_1")]
        public int FS_1 { get; set; }

        /// <summary>
        /// 1st serves tried
        /// </summary>
        [Column(Name = "FSOF_1")]
        public int FSOF_1 { get; set; }

        [Column(Name = "ACES_1")]
        public int ACES_1 { get; set; }

        [Column(Name = "DF_1")]
        public int DF_1 { get; set; }

        [Column(Name = "UE_1")]
        public int UE_1 { get; set; }

        /// <summary>
        /// 1st serv pts won
        /// </summary>
        [Column(Name = "W1S_1")]
        public int W1S_1 { get; set; }

        /// <summary>
        /// 1st serv pts played
        /// </summary>
        [Column(Name = "W1SOF_1")]
        public int W1SOF_1 { get; set; }

        /// <summary>
        /// 2nd serv pts won
        /// </summary>
        [Column(Name = "W2S_1")]
        public int W2S_1 { get; set; }

        /// <summary>
        /// 2nd serv pts played
        /// </summary>
        [Column(Name = "W2SOF_1")]
        public int W2SOF_1 { get; set; }

        /// <summary>
        /// Winners
        /// </summary>
        [Column(Name = "WIS_1")]
        public int WIS_1 { get; set; }

        /// <summary>
        /// BP won
        /// </summary>
        [Column(Name = "BP_1")]
        public int BP_1 { get; set; }

        /// <summary>
        /// BP played
        /// </summary>
        [Column(Name = "BPOF_1")]
        public int BPOF_1 { get; set; }

        /// <summary>
        /// Net Approach won
        /// </summary>
        [Column(Name = "NA_1")]
        public int NA_1 { get; set; }

        /// <summary>
        /// Net Approach played
        /// </summary>
        [Column(Name = "NAOF_1")]
        public int NAOF_1 { get; set; }

        /// <summary>
        /// Total Pts Won
        /// </summary>
        [Column(Name = "TPW_1")]
        public int TPW_1 { get; set; }

        /// <summary>
        /// Fastest serve
        /// </summary>
        [Column(Name = "FAST_1")]
        public int FAST_1 { get; set; }

        /// <summary>
        /// Avg 1st serve speed
        /// </summary>
        [Column(Name = "A1S_1")]
        public int A1S_1 { get; set; }

        /// <summary>
        /// Avg 2nd serve speed
        /// </summary>
        [Column(Name = "A2S_1")]
        public int A2S_1 { get; set; }
        
        /*//player2
        [Column(Name = "FS_2")]
        public int FS_2 { get; set; }

        [Column(Name = "FSOF_2")]
        public int FSOF_2 { get; set; }

        [Column(Name = "ACES_2")]
        public int ACES_2 { get; set; }

        [Column(Name = "DF_2")]
        public int DF_2 { get; set; }

        [Column(Name = "UE_2")]
        public int UE_2 { get; set; }

        [Column(Name = "W1S_2")]
        public int W1S_2 { get; set; }

        [Column(Name = "W1SOF_2")]
        public int W1SOF_2 { get; set; }

        [Column(Name = "W2S_2")]
        public int W2S_2 { get; set; }

        [Column(Name = "W2SOF_2")]
        public int W2SOF_2 { get; set; }

        [Column(Name = "WIS_2")]
        public int WIS_2 { get; set; }

        [Column(Name = "BP_2")]
        public int BP_2 { get; set; }

        [Column(Name = "BPOF_2")]
        public int BPOF_2 { get; set; }

        [Column(Name = "NA_2")]
        public int NA_2 { get; set; }

        [Column(Name = "NAOF_2")]
        public int NAOF_2 { get; set; }

        [Column(Name = "TPW_2")]
        public int TPW_2 { get; set; }

        [Column(Name = "FAST_2")]
        public int FAST_2 { get; set; }

        [Column(Name = "A1S_2")]
        public int A1S_2 { get; set; }

        [Column(Name = "A2S_2")]
        public int A2S_2 { get; set; }
        */
        /// <summary>
        /// Return Pts won
        /// </summary>
        [Column(Name = "RPW_1")]
        public int RPW_1 { get; set; }

        /// <summary>
        /// Return Pts played
        /// </summary>
        [Column(Name = "RPWOF_1")]
        public int RPWOF_1 { get; set; }
        /*
        [Column(Name = "RPW_2")]
        public int RPW_2 { get; set; }

        [Column(Name = "RPWOF_2")]
        public int RPWOF_2 { get; set; }
        */
        /// <summary>
        /// Match Time
        /// </summary>
        [Column(Name = "MT")]
        public string MT { get; set; }

        public double? getMatchTimeinMn()
        {
            if (MT.Trim() == "")
                return null;
            else
                try {
                    return TimeSpan.ParseExact(MT, "hh\\:mm\\:ss", System.Globalization.CultureInfo.InvariantCulture).TotalMinutes;
                }
                catch { return null; }
        }
        

        public StatsPlayerForOneMatch(int aIndexStatForMatch)
        {
            IndexStatForMatch = aIndexStatForMatch;
            MT = "";
            nbServiceGamesPlayed = -1;
            nbServiceGamesWon = -1;
            nbReturnGamesPlayed = -1;
            nbTBPlayed = -1;
            FS_1 = -1;
            FSOF_1 = -1;
            ACES_1 = -1;
            DF_1 = -1;
            UE_1 = -1;
            W1S_1 = -1;
            W1SOF_1 = -1;
            W2S_1 = -1;
            W2SOF_1 = -1;
            WIS_1 = -1;
            BP_1 = -1;
            BPOF_1 = -1;
            NA_1 = -1;
            NAOF_1 = -1;
            TPW_1 = -1;
            FAST_1 = -1;
            A1S_1 = -1;
            A2S_1 = -1;
            RPW_1 = -1;
            RPWOF_1 = -1;
            /*
            FS_2 = -1;
            FSOF_2 = -1;
            ACES_2 = -1;
            DF_2 = -1;
            UE_2 = -1;
            W1S_2 = -1;
            W1SOF_2 = -1;
            W2S_2 = -1;
            W2SOF_2 = -1;
            WIS_2 = -1;
            BP_2 = -1;
            BPOF_2 = -1;
            NA_2 = -1;
            NAOF_2 = -1;
            TPW_2 = -1;
            FAST_2 = -1;
            A1S_2 = -1;
            A2S_2 = -1;
            RPW_2 = -1;
            RPWOF_2 = -1;*/
        }
    }
}
