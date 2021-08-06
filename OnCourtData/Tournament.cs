using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq.Mapping;
using System.Data.Linq;
using System.Collections.ObjectModel;

namespace OnCourtData
{
    [Table(Name = "Tours_atp")]
    public class Tournament
    {
        public static string csvHeader = "Date,TrnId,Trn,TrnRk,TrnSite,"
            + "CourtId";
        public string ToCsvLine()
        {
            return $"{this.Date},{this.Id},{this.Name.Replace(",", ";")},{this.Rank},{this.TournamentSite}"
                + $",{this.CourtId}"
                ;
        }
        public void getAcesStats(List<MatchDetailsWithOdds> listMatches)
        {

        }
        public Tournament(bool aIsATP)
        {
            IsATP = aIsATP;
        }
        [System.ComponentModel.Browsable(false)]
        protected bool IsATP { get; set; }

        [Column(Name = "ID_T", IsPrimaryKey = true, IsDbGenerated = true)]

        public long Id { get; set; }

        [Column(Name = "NAME_T")]
        public string Name { get; set; }
        /// <summary>
        /// 1 to 6: "Hard", "Clay", "Indoor", "Carpet", "Grass", "Acrylic"
        /// </summary>
        [Column(Name = "ID_C_T")]
        public int? CourtId { get; set; }
        
        [Column(Name = "DATE_T")]
        public DateTime? Date { get; set; }

        [Column(Name = "RANK_T")]
        /// <summary>
        /// 0=ITF / ITF < 20K; 1=Challenger / ITF >=20 K;2=/ATP ; 3= M1000 4=GS 5=DC 6=Exhib/Juniors
        /// </summary>
        public int Rank { get; set; }

        [System.ComponentModel.Browsable(false)]
        public string TournamentSite { get; set; }

        public long IdPreviousEdition { get; set; }

        public string NameAndYear
        {
            get
            {
                return (Date!=null?Date.Value.Year.ToString():"") + " - " + Name + " (" +this.Id + "; Prev:" + IdPreviousEdition 
                    + "; Site:"+ TournamentSite +")";
            }
        }
        public override string ToString()
        {
            return this.Name;
        }

        public double? SpeedAdvanced { get; set; }
        public double? SpeedBasic { get; set; }
        public int? Humidity { get; set; }
        public int? Wind { get; set; }
        public int? GeoZone { get; set; }
        public string Note1 { get; set; }
        public string Note2 { get; set; }
        public string WeatherLink { get; set; }
    }

    public class TournamentsCollection : ObservableCollection<Tournament>
    {
    }
}
