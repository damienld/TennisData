﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq.Mapping;
using System.Collections.ObjectModel;

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
    }

    public class CategoriesCollection : ObservableCollection<Categorie>
    {
    }
}
