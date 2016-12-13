using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq.Mapping;
using System.Collections.ObjectModel;

namespace OnCourtData
{
    [Table(Name = "Rounds")]
    public class Round
    {
        [Column(Name = "ID_R", IsPrimaryKey = true, IsDbGenerated = true)]
        public int Id { get; set; }

        [Column(Name = "NAME_R")]
        public string Name { get; set; }

        public override string ToString()
        {
            return this.Name;
        }
    }


    public class RoundsCollection : ObservableCollection<Round>
    {
    }
}
