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
        {
            new DateSince("Last 6 months", DateTime.Now.AddMonths(-6))
            , new DateSince("Last 12 months", DateTime.Now.AddYears(-1))
            , new DateSince(DateTime.Now.Year+"", new DateTime(DateTime.Now.Year-1, 12, 25))
            , new DateSince((DateTime.Now.Year-1)+"", new DateTime(DateTime.Now.Year-2, 12, 25))
            , new DateSince((DateTime.Now.Year-2)+"", new DateTime(DateTime.Now.Year-3, 12, 25))
            , new DateSince((DateTime.Now.Year-3)+"", new DateTime(DateTime.Now.Year-4, 12, 25))
            , new DateSince((DateTime.Now.Year-4)+"", new DateTime(DateTime.Now.Year-5, 12, 25))
            , new DateSince((DateTime.Now.Year-5)+"", new DateTime(DateTime.Now.Year-6, 12, 25))
            };
        public static List<DateSince> ListDates_FPlayer1 = new List<DateSince>()
        {   new DateSince("Last 6 months", DateTime.Now.AddMonths(-6))
            , new DateSince("Last 12 months", DateTime.Now.AddYears(-1))
            , new DateSince(DateTime.Now.Year+"", new DateTime(DateTime.Now.Year-1, 12, 25))
            , new DateSince((DateTime.Now.Year-1)+"", new DateTime(DateTime.Now.Year-2, 12, 25))
            , new DateSince((DateTime.Now.Year-2)+"", new DateTime(DateTime.Now.Year-3, 12, 25))
            , new DateSince((DateTime.Now.Year-3)+"", new DateTime(DateTime.Now.Year-4, 12, 25))
            , new DateSince((DateTime.Now.Year-4)+"", new DateTime(DateTime.Now.Year-5, 12, 25))
            , new DateSince((DateTime.Now.Year-5)+"", new DateTime(DateTime.Now.Year-6, 12, 25))
            , new DateSince((DateTime.Now.Year-6)+"", new DateTime(DateTime.Now.Year-7, 12, 25))
            , new DateSince((DateTime.Now.Year-7)+"", new DateTime(DateTime.Now.Year-8, 12, 25))
            , new DateSince((DateTime.Now.Year-8)+"", new DateTime(DateTime.Now.Year-9, 12, 25))
            , new DateSince((DateTime.Now.Year-9)+"", new DateTime(DateTime.Now.Year-10, 12, 25))
            };
        public static List<DateSince> ListDates_FPlayer2 = new List<DateSince>()
        {   new DateSince("Last 6 months", DateTime.Now.AddMonths(-6))
            , new DateSince("Last 12 months", DateTime.Now.AddYears(-1))
            , new DateSince(DateTime.Now.Year+"", new DateTime(DateTime.Now.Year-1, 12, 25))
            , new DateSince((DateTime.Now.Year-1)+"", new DateTime(DateTime.Now.Year-2, 12, 25))
            , new DateSince((DateTime.Now.Year-2)+"", new DateTime(DateTime.Now.Year-3, 12, 25))
            , new DateSince((DateTime.Now.Year-3)+"", new DateTime(DateTime.Now.Year-4, 12, 25))
            , new DateSince((DateTime.Now.Year-4)+"", new DateTime(DateTime.Now.Year-5, 12, 25))
            , new DateSince((DateTime.Now.Year-5)+"", new DateTime(DateTime.Now.Year-6, 12, 25))
            , new DateSince((DateTime.Now.Year-6)+"", new DateTime(DateTime.Now.Year-7, 12, 25))
            , new DateSince((DateTime.Now.Year-7)+"", new DateTime(DateTime.Now.Year-8, 12, 25))
            , new DateSince((DateTime.Now.Year-8)+"", new DateTime(DateTime.Now.Year-9, 12, 25))
            , new DateSince((DateTime.Now.Year-9)+"", new DateTime(DateTime.Now.Year-10, 12, 25))
            };
        public static List<DateSince> ListDates_FTrn_P1 = new List<DateSince>()
        {   new DateSince("Last 6 months", DateTime.Now.AddMonths(-6))
            , new DateSince("Last 12 months", DateTime.Now.AddYears(-1))
            , new DateSince(DateTime.Now.Year+"", new DateTime(DateTime.Now.Year-1, 12, 25))
            , new DateSince((DateTime.Now.Year-1)+"", new DateTime(DateTime.Now.Year-2, 12, 25))
            , new DateSince((DateTime.Now.Year-2)+"", new DateTime(DateTime.Now.Year-3, 12, 25))
            , new DateSince((DateTime.Now.Year-3)+"", new DateTime(DateTime.Now.Year-4, 12, 25))
            , new DateSince((DateTime.Now.Year-4)+"", new DateTime(DateTime.Now.Year-5, 12, 25))
            , new DateSince((DateTime.Now.Year-5)+"", new DateTime(DateTime.Now.Year-6, 12, 25))
            };
        public static List<DateSince> ListDates_FTrn_P2 = new List<DateSince>()
        {   new DateSince("Last 6 months", DateTime.Now.AddMonths(-6))
            , new DateSince("Last 12 months", DateTime.Now.AddYears(-1))
            , new DateSince(DateTime.Now.Year+"", new DateTime(DateTime.Now.Year-1, 12, 25))
            , new DateSince((DateTime.Now.Year-1)+"", new DateTime(DateTime.Now.Year-2, 12, 25))
            , new DateSince((DateTime.Now.Year-2)+"", new DateTime(DateTime.Now.Year-3, 12, 25))
            , new DateSince((DateTime.Now.Year-3)+"", new DateTime(DateTime.Now.Year-4, 12, 25))
            , new DateSince((DateTime.Now.Year-4)+"", new DateTime(DateTime.Now.Year-5, 12, 25))
            , new DateSince((DateTime.Now.Year-5)+"", new DateTime(DateTime.Now.Year-6, 12, 25))
            };
        public static List<DateSince> ListDates_FFindPlayerP1 = new List<DateSince>()
        {   new DateSince("Last 6 months", DateTime.Now.AddMonths(-6))
            , new DateSince("Last 12 months", DateTime.Now.AddYears(-1))
            , new DateSince(DateTime.Now.Year+"", new DateTime(DateTime.Now.Year-1, 12, 25))
            , new DateSince((DateTime.Now.Year-1)+"", new DateTime(DateTime.Now.Year-2, 12, 25))
            , new DateSince((DateTime.Now.Year-2)+"", new DateTime(DateTime.Now.Year-3, 12, 25))
            , new DateSince((DateTime.Now.Year-3)+"", new DateTime(DateTime.Now.Year-4, 12, 25))
            , new DateSince((DateTime.Now.Year-4)+"", new DateTime(DateTime.Now.Year-5, 12, 25))
            , new DateSince((DateTime.Now.Year-5)+"", new DateTime(DateTime.Now.Year-6, 12, 25))
            };
        public static List<DateSince> ListDates_FFindPlayerP2 = new List<DateSince>()
        {   new DateSince("Last 6 months", DateTime.Now.AddMonths(-6))
            , new DateSince("Last 12 months", DateTime.Now.AddYears(-1))
            , new DateSince(DateTime.Now.Year+"", new DateTime(DateTime.Now.Year-1, 12, 25))
            , new DateSince((DateTime.Now.Year-1)+"", new DateTime(DateTime.Now.Year-2, 12, 25))
            , new DateSince((DateTime.Now.Year-2)+"", new DateTime(DateTime.Now.Year-3, 12, 25))
            , new DateSince((DateTime.Now.Year-3)+"", new DateTime(DateTime.Now.Year-4, 12, 25))
            , new DateSince((DateTime.Now.Year-4)+"", new DateTime(DateTime.Now.Year-5, 12, 25))
            , new DateSince((DateTime.Now.Year-5)+"", new DateTime(DateTime.Now.Year-6, 12, 25))
            };
        public static List<DateSince> ListDates_FPlayerStats = new List<DateSince>()
        {   new DateSince("Last 6 months", DateTime.Now.AddMonths(-6))
            , new DateSince("Last 12 months", DateTime.Now.AddYears(-1))
            , new DateSince(DateTime.Now.Year+"", new DateTime(DateTime.Now.Year-1, 12, 25))
            , new DateSince((DateTime.Now.Year-1)+"", new DateTime(DateTime.Now.Year-2, 12, 25))
            , new DateSince((DateTime.Now.Year-2)+"", new DateTime(DateTime.Now.Year-3, 12, 25))
            , new DateSince((DateTime.Now.Year-3)+"", new DateTime(DateTime.Now.Year-4, 12, 25))
            , new DateSince((DateTime.Now.Year-4)+"", new DateTime(DateTime.Now.Year-5, 12, 25))
            , new DateSince((DateTime.Now.Year-5)+"", new DateTime(DateTime.Now.Year-6, 12, 25))
            };
        public static List<DateSince> ListDates_FAces = new List<DateSince>()
        {   new DateSince(DateTime.Now.Year+"", new DateTime(DateTime.Now.Year-1, 12, 25))
            , new DateSince((DateTime.Now.Year-1)+"", new DateTime(DateTime.Now.Year-2, 12, 25))
            , new DateSince((DateTime.Now.Year-2)+"", new DateTime(DateTime.Now.Year-3, 12, 25))
            , new DateSince((DateTime.Now.Year-3)+"", new DateTime(DateTime.Now.Year-4, 12, 25))
            , new DateSince((DateTime.Now.Year-4)+"", new DateTime(DateTime.Now.Year-5, 12, 25))
            , new DateSince((DateTime.Now.Year-5)+"", new DateTime(DateTime.Now.Year-6, 12, 25))
            , new DateSince((DateTime.Now.Year-6)+"", new DateTime(DateTime.Now.Year-7, 12, 25))
            , new DateSince((DateTime.Now.Year-7)+"", new DateTime(DateTime.Now.Year-8, 12, 25))
            , new DateSince((DateTime.Now.Year-8)+"", new DateTime(DateTime.Now.Year-9, 12, 25))
            , new DateSince((DateTime.Now.Year-9)+"", new DateTime(DateTime.Now.Year-10, 12, 25))
            };

    };
}
