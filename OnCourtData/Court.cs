using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq.Mapping;
using System.Collections.ObjectModel;

namespace OnCourtData
{
    [Table(Name = "Courts")]
    public class Court
    {
        [Column(Name = "ID_C", IsPrimaryKey=true)]
        public int Id { get; set; }

        [Column(Name = "NAME_C")]
        public string Name { get; set; }

        public Court(int aId, string aName)
        {
            Id = aId;
            Name = aName;
        }
        public override string ToString()
        {
            return this.Name;
        }
        public static List<Court> fListSurfaces = new List<Court>()
        { new Court(1, "Hard"), new Court(2, "Clay"), new Court(3, "Indoor"), new Court(4, "Carpet"), new Court(5, "Grass")
        , new Court(6, "Acrylic")};

        public static List<Court> getListCourtsToSelect(int? aCurrentTournamentCourt)
        {
            switch (aCurrentTournamentCourt)
            {
                case null:
                    return null;
                case 3:
                    return fListSurfaces.Where(c => new List<int> { 3, 4, 6 }.Contains(c.Id)).ToList();
                case 4:
                    return fListSurfaces.Where(c => new List<int> { 3, 4, 6 }.Contains(c.Id)).ToList();
                case 6:
                    return fListSurfaces.Where(c => new List<int> { 3, 4, 6 }.Contains(c.Id)).ToList();
                default:
                    return fListSurfaces.Where(c => c.Id == aCurrentTournamentCourt).ToList();
            }
        }
    }

}
