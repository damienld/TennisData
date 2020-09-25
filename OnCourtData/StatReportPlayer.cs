using System;
using System.Collections.Generic;
using System.Linq;

namespace OnCourtData
{
    public class StatReportPlayer
    {
        /// <summary>
        /// Create for Win %
        /// </summary>
        /// <param name="aNameStat"></param>
        /// <param name="aNbWin"></param>
        /// <param name="aNbOf"></param>
        /// <param name="aIsDisplayNbOf"></param>
        /// <param name="aNbDecimals"></param>
        public StatReportPlayer(bool aIsSeparatorAfterCell, string aNameStat, string aToolTipHeader, string aToolTip
            , int aNbWin, int aNbOf, bool aIsDisplayNbOf, int aIndexDisplay, int aNbDecimals)
        {
            IsSeparatorAfterCell = aIsSeparatorAfterCell;
            NameStat = aNameStat;
            ToolTipHeader = aToolTipHeader;
            ToolTip = aToolTip;
            NbWin = aNbWin;
            NbOf = aNbOf;
            NbDecimals = aNbDecimals;
            IsDisplayNbOf = aIsDisplayNbOf;
            ReferenceToCompare = -1;
            IndexDisplay = aIndexDisplay;
            Percentage = stat(aNbWin, aNbOf, aNbDecimals);
        }
        /// <summary>
        /// Create for Win % vs a reference
        /// </summary>
        /// <param name="aIsSeparatorAfterCell"></param>
        /// <param name="aNameStat"></param>
        /// <param name="aToolTipHeader"></param>
        /// <param name="aToolTip"></param>
        /// <param name="aNbWin"></param>
        /// <param name="aNbOf"></param>
        /// <param name="aIsDisplayNbOf"></param>
        /// <param name="aReferenceToCompare"></param>
        /// <param name="aIndexDisplay"></param>
        /// <param name="aNbDecimals"></param>
        public StatReportPlayer(bool aIsSeparatorAfterCell, string aNameStat, string aToolTipHeader, string aToolTip
            , int aNbWin
            , int aNbOf, bool aIsDisplayNbOf, double aReferenceToCompare, int aIndexDisplay, int aNbDecimals)
        {
            IsSeparatorAfterCell = aIsSeparatorAfterCell;
            NameStat = aNameStat;
            ToolTipHeader = aToolTipHeader;
            ToolTip = aToolTip;
            NbWin = aNbWin;
            NbOf = aNbOf;
            NbDecimals = aNbDecimals;
            IsDisplayNbOf = aIsDisplayNbOf;
            ReferenceToCompare = aReferenceToCompare;
            IndexDisplay = aIndexDisplay;
            Percentage = stat(aNbWin, aNbOf, aNbDecimals);
        }
        /// <summary>
        /// Create for ROI
        /// </summary>
        /// <param name="aNameStat"></param>
        /// <param name="aPercentage"></param>
        /// <param name="aNbOf"></param>
        /// <param name="aIsDisplayNbOf"></param>
        /// <param name="aNbDecimals"></param>
        public StatReportPlayer(bool aIsSeparatorAfterCell, string aNameStat, string aToolTipHeader
            , string aToolTip, bool aIsDisplayNbOf
           , double aPercentage, int aNbOf, int aIndexDisplay, int aNbDecimals, int aLevel1Color, int aLevel2Color)
        {
            IsSeparatorAfterCell = aIsSeparatorAfterCell;
            NameStat = aNameStat;
            ToolTipHeader = aToolTipHeader;
            ToolTip = aToolTip;
            NbOf = aNbOf;
            NbDecimals = aNbDecimals;
            IsDisplayNbOf = aIsDisplayNbOf;
            Percentage = aPercentage;
            IndexDisplay = aIndexDisplay;
            Level1Color = aLevel1Color;
            Level2Color = aLevel2Color;
        }
        /// <summary>
        /// Create for ROI
        /// </summary>
        /// <param name="aNameStat"></param>
        /// <param name="aPercentage"></param>
        /// <param name="aNbOf"></param>
        /// <param name="aIsDisplayNbOf"></param>
        /// <param name="aNbDecimals"></param>
        public StatReportPlayer(bool aIsSeparatorAfterCell, string aNameStat, string aToolTipHeader, string aToolTip
            , bool aIsDisplayNbOf, double aPercentage, int aNbOf, double aReferenceToCompare, int aIndexDisplay
            , int aNbDecimals, int aLevel1Color, int aLevel2Color)
        {
            IsSeparatorAfterCell = aIsSeparatorAfterCell;
            NameStat = aNameStat;
            ToolTipHeader = aToolTipHeader;
            ToolTip = aToolTip;
            NbOf = aNbOf;
            NbDecimals = aNbDecimals;
            Percentage = aPercentage;
            IndexDisplay = aIndexDisplay;
            ReferenceToCompare = aReferenceToCompare;
            IsDisplayNbOf = aIsDisplayNbOf;
            Level1Color = aLevel1Color;
            Level2Color = aLevel2Color;
        }
        /// <summary>
        /// Base 0
        /// </summary>
        public int IndexDisplay { get; set; }
        public string NameStat { get; set; }
        public string ToolTipHeader { get; set; }
        public string ToolTip { get; set; }
        public double Percentage { get; set; }
        public int NbWin { get; set; } = -1;
        public int NbOf { get; set; }
        public int NbDecimals { get; set; }
        /*public int aROI { get; set; }
        public int aNbMatchesForROI { get; set; }*/
        public bool IsSeparatorAfterCell { get; set; }
        public bool IsDisplayNbOf { get; set; }
        public double ReferenceToCompare { get; set; } = -1D;
        public double? DifferenceWithReference { get; set; } = null;
        public int Level1Color { get; set; } = 4;
        public int Level2Color { get; set; } = 8;
        public enum FormatBgColorType
        {
            None, ColorPlayer, ColorGreenRed, ColorGreenRed1LevelOnly, ColorPlayerAndGreenRed
        }
        public FormatBgColorType FormatBgColor { get; set; }

        private double stat(int aNbWin, int aNbOf, int aNbDecimals)
        {
            if (aNbOf == 0)
                return 0;
            else
                return Math.Round(aNbWin * 100.0 / aNbOf, 0);
        }
        private string getHTMLResultString()
        {
            string _strPctOrDollar = "%";
            if (NbWin == -1)
                _strPctOrDollar = "€";
            string _res = Percentage.ToString() + _strPctOrDollar;
            if (ReferenceToCompare >= 0)
            {
                try
                {
                    int _value = Convert.ToInt32((Percentage - Math.Round(100 * ReferenceToCompare)));
                    _res = (_value > 0 ? "+" : "") + _value.ToString() + _strPctOrDollar;
                }
                catch (Exception e)
                {

                } 
            }
            else if(DifferenceWithReference != null)
            {
                try
                {
                    int _value = Convert.ToInt32(DifferenceWithReference.Value);
                    _res = (_value > 0 ? "+" : "") + _value.ToString() + _strPctOrDollar;
                }
                catch (Exception e)
                {

                }
            }
            return _res;
        }
        private string getHTMLTotalCountString()
        {
            string _res = "";
            if (IsDisplayNbOf)
                _res += "(" + NbOf + ")";
            return _res;
        }
        public string getHtmlForCell(int aIndexPlayerBase1)
        {
            string _titleStyle = "";
            if (ToolTip != "")
                _titleStyle = " title='" + this.ToolTip + "'";

            string _res1 = getHTMLResultString();
            string _cssColor = "";

            int _diff = Convert.ToInt32((Percentage - Math.Round(100 * ReferenceToCompare)));
            if (ReferenceToCompare < 0) //compare to 100% not to a reference
                _diff = Convert.ToInt32((Percentage - 100));
            if (DifferenceWithReference != null)
                _diff = Convert.ToInt32(DifferenceWithReference.Value);

            if (FormatBgColor == FormatBgColorType.ColorPlayer && NbOf >= 10)
            {
                _cssColor = "class='p" + aIndexPlayerBase1 + "'";
                _res1 = "<b>" + _res1 + "</b>";
            }
            else if ((FormatBgColor == FormatBgColorType.ColorGreenRed 
                || FormatBgColor == FormatBgColorType.ColorPlayerAndGreenRed) 
                && NbOf >= 10)
            {
                if (_diff >= Level2Color)
                    _cssColor = "class='green2' ";
                else if (_diff >= Level1Color)
                    _cssColor = "class='green1' ";
                else if (_diff <= -Level2Color)
                    _cssColor = "class='red2' ";
                else if (_diff <= -Level1Color)
                    _cssColor = "class='red1' ";
                string _styleUnderline = FormatBgColor == FormatBgColorType.ColorPlayerAndGreenRed? " style='text-decoration:underline;font-weight: bold;'" : "";
                   _res1 = "<div"+ _styleUnderline+">" + _res1 + "</div>";
            }
            else if ((FormatBgColor == FormatBgColorType.ColorGreenRed1LevelOnly)
                && NbOf >= 10)
            {
                if (_diff >= Level2Color)
                    _cssColor = "class='green2' ";
                else if (_diff <= -Level2Color)
                    _cssColor = "class='red2' ";
                string _styleUnderline = FormatBgColor == FormatBgColorType.ColorPlayerAndGreenRed ? " style='text-decoration:underline;font-weight: bold;'" : "";
                _res1 = "<div" + _styleUnderline + ">" + _res1 + "</div>";
            }
            string _res2 = "<div style='font-size:8px;'>" + getHTMLTotalCountString() + "</div>";
            if (NbOf == 0)
            {
                _res1 = "";
                _res2 = "";
            }
            return "<td " + _cssColor + _titleStyle
                 + ">" + _res1 + _res2 + "</td>" + (IsSeparatorAfterCell?"<td width=5></td>":"");
        }
        public string getHtmlForHeaderCell()
        {
            string _titleStyle = "";
            if (ToolTipHeader != "")
                _titleStyle = " title='" + this.ToolTipHeader + "'";
            return "<td width=40 " + _titleStyle + ">" + NameStat + "</td>" + (IsSeparatorAfterCell ? "<td width=5></td>" : "");
        }
    }

}
