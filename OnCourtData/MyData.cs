using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnCourtData
{
    public static class MyData
    {
        public static List<string> fListAltitudeSitesATP =
            new List<long> { 589, 7161, 7776, 319, 306, 6169, 314, 7389, 576, 3352, 252, 213, 5063, 7795, 3994, 7075, 5016
            , 9158, 7102, 3406, 8996,1536}
            .ConvertAll(m=>m.ToString());
        //Gstaad;bogota;Medellin;Curitiba,Guada, Monterrey, MAdrod
        public static List<string> fListAltitudeSitesWTA =
            new List<long> { 1094, 408, 712, 894, 2002, 1039, 1536 }.ConvertAll(m => m.ToString());
        //Site Id: Morelos;Guadala;Segovia;Leon;Bengalaru/Bangalore; 
        public static List<string> fListAltitudeSitesATPHard =
            new List<long> { 6963, 6284, 783, 1539, 7808 , 8371}.ConvertAll(m => m.ToString());
        public static List<string> fListWindSitesATP =
            new List<long> { 301, 403, 499 }.ConvertAll(m => m.ToString());
        public static List<string> fListWindSitesWTA =
            new List<long> { 1049, 301, 235, 902, 837, 1050, 505, 415, 1003 }.ConvertAll(m => m.ToString());
    }
}
