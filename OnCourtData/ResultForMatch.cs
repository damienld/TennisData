using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace OnCourtData
{
    public class ResultForMatch
    {
        public enum TypeEnd { Completed, PulledOut, Retirement, Disqualified }
        [System.ComponentModel.Browsable(false)]
        public TypeEnd EndType { get; set; }
        [System.ComponentModel.Browsable(false)]
        public List<int> fListTbResultsForP1 { get; set; }
        /// <summary>
        /// 0=notplayed, 1=won by P1, 2=won by P2
        /// </summary>
        [System.ComponentModel.Browsable(false)]
        public List<int> fListTbWinners { get; set; }
        [System.ComponentModel.Browsable(false)]
        public List<int> fListTbResultsForP2 { get; set; }
        [System.ComponentModel.Browsable(false)]
        public List<int> fListSetResultsForP1 { get; set; }
        [System.ComponentModel.Browsable(false)]
        public List<int> fListSetResultsForP2 { get; set; }
        [System.ComponentModel.Browsable(false)]
        public int fNbSetsWonP1 { get; set; }
        [System.ComponentModel.Browsable(false)]
        public int fNbSetsWonP2 { get; set; }
        [System.ComponentModel.Browsable(false)]
        public int fNbGamesWonP1 { get; set; }
        [System.ComponentModel.Browsable(false)]
        public int fNbGamesWonP2 { get; set; }

        public ResultForMatch()
        {
            fListTbWinners = new List<int>() { 0, 0, 0, 0, 0 };
            fListTbResultsForP1 = new List<int>() { -1, -1, -1, -1, -1 };
            fListTbResultsForP2 = new List<int>() { -1, -1, -1, -1, -1 };
            fListSetResultsForP1 = new List<int>() { -1, -1, -1, -1, -1 };
            fListSetResultsForP2 = new List<int>() { -1, -1, -1, -1, -1 };
            fNbSetsWonP1 = 0;
            fNbSetsWonP2 = 0;
            fNbGamesWonP1 = 0;
            fNbGamesWonP2 = 0;
        }
        public void readResult(string aResultString)
        {//expl: 6-7(10) 6-0 6-2, 6-3 6-2 ret., 4-2 ret., w/o, 6-4 6-6 def.
            try
            {
                EndType = ResultForMatch.TypeEnd.Completed;
                string score = aResultString.Trim();
                string[] setsResults = score.Split(' ');
                if (setsResults.Length == 0)
                    return;
                int _nbSetsToRead = setsResults.Length;
                switch (setsResults[setsResults.Length - 1].Trim())
                {
                    case "w/o":
                        EndType = ResultForMatch.TypeEnd.PulledOut;
                        _nbSetsToRead -= 1;
                        break;
                    case "ret.":
                        EndType = ResultForMatch.TypeEnd.Retirement;
                        _nbSetsToRead -= 1;
                        break;
                    case "def.":
                        EndType = ResultForMatch.TypeEnd.Disqualified;
                        _nbSetsToRead -= 1;
                        break;
                    default:
                        break;
                }
                int _totalGamesP1 = 0;
                int _totalGamesP2 = 0;
                for (int i = 0; i <= _nbSetsToRead - 1; i++)
                {
                    int _tbResult = -1;
                    string _setResult = setsResults[i];
                    if (_setResult.Contains("(")) //read TB
                    {
                        int _indexStartTb = _setResult.IndexOf("(");
                        int _indexEndTb = _setResult.IndexOf(")");
                        string _tbResultStr = _setResult.Substring(_indexStartTb + 1, _indexEndTb - (_indexStartTb + 1));
                        _tbResult = Convert.ToInt16(_tbResultStr);
                        _setResult = _setResult.Substring(0, _indexStartTb);
                    }
                    string[] GamesResults = _setResult.Split('-');
                    int _gameP1 = Convert.ToInt16(GamesResults[0]);
                    int _gameP2 = Convert.ToInt16(GamesResults[1]);
                    //if last set, check that it was really completed 
                    if (i == _nbSetsToRead - 1)
                    {
                        if (EndType != ResultForMatch.TypeEnd.Completed && isSetCompleted(_gameP1, _gameP2))
                        {
                            NewMethod(ref _totalGamesP1, ref _totalGamesP2, i, _gameP1, _gameP2);
                        }
                        if (EndType == ResultForMatch.TypeEnd.Completed)
                        {
                            NewMethod(ref _totalGamesP1, ref _totalGamesP2, i, _gameP1, _gameP2);
                        }
                    }
                    else
                    {
                        NewMethod(ref _totalGamesP1, ref _totalGamesP2, i, _gameP1, _gameP2);
                    }
                    if (_tbResult != -1)
                    {
                        if (_gameP1 > _gameP2)
                        {
                            if (_tbResult < 6)
                                fListTbResultsForP1[i] = 7;
                            else
                                fListTbResultsForP1[i] = _tbResult + 2;
                            fListTbResultsForP2[i] = _tbResult;
                            fListTbWinners[i] = 1;
                        }
                        else
                        {
                            if (_tbResult < 6)
                                fListTbResultsForP2[i] = 7;
                            else
                                fListTbResultsForP2[i] = _tbResult + 2;
                            fListTbResultsForP1[i] = _tbResult;
                            fListTbWinners[i] = 2;
                        }
                    }
                }
                fNbGamesWonP1 = _totalGamesP1;
                fNbGamesWonP2 = _totalGamesP2;
                /*Trace.WriteLine(aResultString + " ; GamesP1:" + string.Join(".", fListSetResultsForP1.ToArray())
                    + " ; GamesP2:" + string.Join(".", fListSetResultsForP2.ToArray()) + " ; Sets:" + fNbSetsWonP1
                    + "-" + fNbSetsWonP2);*/

            }
            catch (Exception)
            {
                
            }
        }

        private void NewMethod(ref int _totalGamesP1, ref int _totalGamesP2, int i, int _gameP1, int _gameP2)
        {
            fListSetResultsForP1[i] = _gameP1;
            fListSetResultsForP2[i] = _gameP2;
            _totalGamesP1 += _gameP1;
            _totalGamesP2 += _gameP2;
            if (_gameP1 > _gameP2)
            {
                fNbSetsWonP1++;
                
            }
            else
            {
                fNbSetsWonP2++;
            }
            
        }

        private bool isSetCompleted(int aGames1, int aGames2)
        {
            if (aGames1 == aGames2)
                return false;
            else
                if (aGames1 < 6 && aGames2 < 6)
                return false;
            else return true;
        }


    }
}
