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
        public Tournament(bool aIsATP)
        {
            IsATP = aIsATP;
        }
        [System.ComponentModel.Browsable(false)]
        protected bool IsATP { get; set; }

        [Column(Name = "ID_T", IsPrimaryKey = true, IsDbGenerated = true)]
        /// <summary>
        /// 1 to 6: "Hard", "Clay", "Indoor", "Carpet", "Grass", "Acrylic"
        /// </summary>
        public int Id { get; set; }

        [Column(Name = "NAME_T")]
        public string Name { get; set; }

        [Column(Name = "ID_C_T")]
        public int? CourtId { get; set; }
        
        [Column(Name = "DATE_T")]
        public DateTime? Date { get; set; }

        [Column(Name = "RANK_T")]
        public byte Rank { get; set; }

        public override string ToString()
        {
            return this.Name;
        }
    }

    public class TournamentsCollection : ObservableCollection<Tournament>
    {
    }
}
