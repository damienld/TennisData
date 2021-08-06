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
        /// <summary>
        /// Returns a cout index base 4: 1=Hard, 2=Clay, 3=Indoors, 4=Grass
        /// </summary>
        /// <param name="aIndexCourtFromOnCourt"></param>
        /// <returns></returns>
        public static int getCourtindexBetween4Base1(int? aIndexCourtFromOnCourt)
        {
            switch (aIndexCourtFromOnCourt)
            {
                case null:
                    return 0;
                case 3:
                    return 3;
                case 4:
                    return 3;
                case 6:
                    return 3;
                case 5:
                    return 4;
                default:
                    return aIndexCourtFromOnCourt.Value;
            }
        }
        /// <summary>
        /// If Clay returns Clay else Returns all others
        /// </summary>
        /// <param name="aIndexCourt6Values"></param>
        /// <returns></returns>
        public static List<int> getCourtindexForStatsWithNonClayAndClayOnly(int? aIndexCourt6Values)
        {
            switch (aIndexCourt6Values)
            {
                case null:
                    return new List<int> { 1, 2, 3, 4 };
                case 1:
                    return new List<int> { 1, 3, 4 };
                case 2:
                    return new List<int> { 2 };
                case 3:
                    return new List<int> { 1, 3, 4 };
                case 4:
                    return new List<int> { 1, 3, 4 };
                case 6:
                    return new List<int> { 1, 3, 4 };
                case 5:
                    return new List<int> { 1, 3, 4 };
                default:
                    return new List<int> { 1, 2, 3, 4 };
            }
        }
        public static List<int> getCourtindexForStatsWithNonClayAndClayOnly2(int aIndexCourt3Values)
        {
            switch (aIndexCourt3Values)
            {
                case 1:
                    return new List<int> { 1, 3, 4 };
                case 2:
                    return new List<int> { 2 };
                default:
                    return new List<int> { 1, 2, 3, 4 };

            }
        }
        /// <summary>
        /// {All,'H','C','I','G' }
        /// </summary>
        public static List<int[]> ListCourtIndex
        = new List<int[]> {  {new int[]{ 1,2,3,4,5,6 } },{new int[]{ 1 } }, {new int[] { 2 } }
            , { new int[]{ 3,4,6 } }, {new int[]{ 5 } } };

    }

}
