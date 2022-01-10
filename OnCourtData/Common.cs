using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OnCourtData
{
    public class Common
    {
        public static double GetAge(DateTime? dob, DateTime? today)
        {

            if (dob != null && today != null)
            {
                TimeSpan diff = (today.Value - dob.Value);
                return Math.Round(diff.TotalDays / 365.25, 1);
            }
            else
                return -1;

        }

        public static void getRankingIntervallFromRank(int aRank, ref int aRank1, ref int aRank2)
        {
            int gap1 = 0;
            if (aRank < 11)
                gap1 = 5;
            else if (aRank < 20)
                gap1 = 8;      //exp: rk=12 => 4-20
            else if (aRank < 25)
                gap1 = 9;
            else if (aRank < 30)
                gap1 = 10;
            else if (aRank < 35)
                gap1 = 12;
            else if (aRank < 40)
                gap1 = 13;
            else if (aRank < 50)
                gap1 = 15;
            else if (aRank < 60)
                gap1 = 18;
            else if (aRank < 80)
                gap1 = 20;
            else if (aRank < 100)
                gap1 = 30;
            else if (aRank < 150)
                gap1 = 40;
            else if (aRank < 200)
                gap1 = 50;
            else gap1 = 100;
            if (aRank == -1)
            {
                aRank1 = 200;
                aRank2 = 400;
            }
            else
            {
                aRank1 = Math.Max(aRank - gap1, 1);
                aRank2 = aRank + gap1;
            }
        }
    }
}
