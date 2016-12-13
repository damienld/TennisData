using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OnCourtData
{
    public class DateSince
    {
        
        public string DisplayValue { get; set; }
        public DateTime ActualValue { get; set; }
        public DateSince(DateTime aActualValue)
        {
            DisplayValue = aActualValue.Year.ToString();
            ActualValue = aActualValue;
        }
        public DateSince(string aDisplayValue, DateTime aActualValue)
        {
            DisplayValue = aDisplayValue;
            ActualValue = aActualValue;
        }
        public static List<DateSince> ListDates = new List<DateSince>()
        { new DateSince("Last 12 months", DateTime.Now.AddYears(-1))
            , new DateSince(DateTime.Now.Year+"", new DateTime(DateTime.Now.Year-1, 12, 25))
            , new DateSince((DateTime.Now.Year-1)+"", new DateTime(DateTime.Now.Year-2, 12, 25))
            , new DateSince((DateTime.Now.Year-2)+"", new DateTime(DateTime.Now.Year-3, 12, 25))
            , new DateSince((DateTime.Now.Year-3)+"", new DateTime(DateTime.Now.Year-4, 12, 25))
            };
    };
}
