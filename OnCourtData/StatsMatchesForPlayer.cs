using OnCourtData;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace OnCourtData
{
    public class StatsListMatchesForPlayer
    {
        public List<MatchDetailsWithOdds> fListMatches { get; set; }
        public StatsListMatchesForPlayer(List<MatchDetailsWithOdds> aListMatches, long aIdPlayer)
        {
            try
            {
                fListMatches = aListMatches;
                foreach (MatchDetailsWithOdds m in fListMatches)
                    if (m.ProcessedResult == null)
                        m.readResult();
                Win = fListMatches.Where(m => m.isCountAsWinForStats(aIdPlayer)).Count();
                Loss = fListMatches.Where(m => m.isCountAsLossForStats(aIdPlayer)).Count();
                List<MatchDetailsWithOdds> _listMatchPlayerFirst = fListMatches.Where(m => m.Id1 == aIdPlayer).ToList();
                List<MatchDetailsWithOdds> _listMatchPlayerSecond = fListMatches.Where(m => m.Id2 == aIdPlayer).ToList();
                SetsWon = _listMatchPlayerFirst.Sum(m => m.ProcessedResult.fNbSetsWonP1) + _listMatchPlayerSecond.Sum(m => m.ProcessedResult.fNbSetsWonP2);
                SetsLost = _listMatchPlayerFirst.Sum(m => m.ProcessedResult.fNbSetsWonP2) + _listMatchPlayerSecond.Sum(m => m.ProcessedResult.fNbSetsWonP1);
                SetXWon = new List<int>();
                SetXLost = new List<int>();
                for (int i = 0; i <= 4; i++)
                {
                    SetXWon.Add(fListMatches.Where
                        (m => m.isCountAsSetXWonForStats(i, aIdPlayer)).Count());
                    SetXLost.Add(fListMatches.Where
                        (m => m.isCountAsSetXLostForStats(i, aIdPlayer)).Count());
                }
                Trace.WriteLine(Win + "-" + Loss + "; Sets:" + SetsWon + "-" + SetsLost + "; Set1:" + SetXWon[0] + "-" + SetXLost[0]);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
        public override string ToString()
        {
            return (Win + "-" + Loss + "; Sets:" + SetsWon + "-" + SetsLost + "; Set1:" + SetXWon[0] + "-" + SetXLost[0]);
        }
        public int Win {get; set;}
        public int Loss { get; set; }
        public int SetsWon { get; set; }
        public int SetsLost { get; set; }
        public List<int> SetXWon { get; set; }
        public List<int> SetXLost { get; set; }
        public int SetBetting2to0 { get; set; }
        public int SetBetting2to1 { get; set; }
        public int SetBetting0to2 { get; set; }
        public int SetBetting1to2 { get; set; }
        public int SetBetting3to0 { get; set; }
        public int SetBetting3to1 { get; set; }
        public int SetBetting3to2 { get; set; }
        public int SetBetting0to3 { get; set; }
        public int SetBetting1to3 { get; set; }
        public int SetBetting2to3 { get; set; }

    }
}
