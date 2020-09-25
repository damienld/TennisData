using System.Data.Linq.Mapping;
using System.Collections.ObjectModel;
using System.Collections.Generic;

namespace OnCourtData
{
    [Table(Name = "Categories_atp")]
    public class Categorie
    {
        [Column(Name = "ID_C", IsPrimaryKey = true, IsDbGenerated = true)]
        public int Id { get; set; }

        [Column(Name = "NAME_C")]
        public string Name { get; set; }

        public override string ToString()
        {
            return this.Name;
        }
        public static Dictionary<int, string> fListCategories = new Dictionary<int, string>()
        { {1,"LH" }, {2,"ACA"}, {3,"ACD"}, {4,"For"}, {5,"Temp"}, {6,"SER"}, {7,"S&V"}, {8,"Slo"}, {9,"Sli"} };

    }

    public class CategoriesCollection : ObservableCollection<Categorie>
    {
    }
}
