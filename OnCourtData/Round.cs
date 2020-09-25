using System.Data.Linq.Mapping;
using System.Collections.ObjectModel;

namespace OnCourtData
{// { get; set; } //O preq, 1-2-3 Qualies, 8 RR, 4=1st, 10=1/2, 11 bronze, 12 final
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

        public Round(int aId, string aName)
        {
            Id = aId;
            Name = aName;
        }
    }


    public class RoundsCollection : ObservableCollection<Round>
    {
    }
}
