using System.Linq;
using System.Text.RegularExpressions;
using SAAPHelper.Constant;
using System;

namespace SAAPHelper.Helper
{
    public class FuncHelper
    {
        /// <summary>
        /// https://www.w3resource.com/csharp-exercises/string/csharp-string-exercise-52.php
        /// </summary>
        /// <param name="inputString"></param>
        /// <returns></returns>
        public static string CapitalizeFirstLetter(string inputString)
        {
            // Split the input string into words, capitalize the first character of each word, and join them back into a string
            return string.Join(" ", inputString.Split(' ').Select(word => char.ToUpper(word[0]) + word.Substring(1)));
        }


        public static bool chkTextIsNotCommented(string text)
        {
            bool result = false;
            string newtext = text.Trim();
          
            if (!string.IsNullOrEmpty(newtext) )
            {
                string substring = newtext.Substring(0, 1);
                if ( (substring != "'" || !substring.Contains("\'")))
                {
                    result = true;
                }
            }
            return result;
        }

        public static bool IsCommented(string text)
        {
            bool result = false;
            string newtext = text.Trim();

            if (string.IsNullOrEmpty(newtext))
            {
                return result;
            }

            string substring = newtext.Substring(0, 1);
            if ((substring == "'" || substring.Contains("\'")))
            {
                result = true;
            }

            return result;
        }

        /// <summary>
        /// test as comment
        /// </summary>
        /// <param name="txtValue"></param>
        /// <returns></returns>
        public static string RepacleTextInSQL(string txtValue)
        {
            string result = string.Empty;


            int sub = txtValue.IndexOf("\"") + 1;
            while (txtValue.Contains("\""))
            {
                
            }
            
            return result;
        }

        public static int GetStartIdxCommented(string txtValue)
        {
            int result = 0;
            if (chkTextIsNotCommented(txtValue))
            {
                txtValue = ApostropheIsNotCommented(txtValue);

                int idx = 0;
                //txtValue = txtValue.Replace("N''''","@AAA@");
                //txtValue = txtValue.Replace("N''", "@A@");
                //txtValue = txtValue.Replace("\"'\"", "@@@");
                //txtValue = txtValue.Replace("' \"", "@@@");
                //txtValue = txtValue.Replace("\"'", "@@");
                //txtValue = txtValue.Replace("\"'■", "@@@");
                //txtValue = txtValue.Replace("'サブフォーム'","@サブフォーム@");
                //txtValue = txtValue.Replace("'Subform'", "@Subform@");
                //txtValue = txtValue.Replace("'subform'", "@subform@");
                //txtValue = txtValue.Replace("'計算式'", "@計算式@");
                //txtValue = txtValue.Replace("'Calculation_formula'", "@Calculation_formula@");
                //txtValue = txtValue.Replace("'合計計算'","@合計計算@");
                //txtValue = txtValue.Replace("'Total_Calculation'", "@Total_Calculation@");

                Regex.Replace(txtValue, "N''''", "@AAA@", RegexOptions.IgnoreCase);
                Regex.Replace(txtValue, "N''", "@A@", RegexOptions.IgnoreCase);
                Regex.Replace(txtValue, "\"'\"", "@@@", RegexOptions.IgnoreCase);
                Regex.Replace(txtValue, "' \"", "@@@", RegexOptions.IgnoreCase);
                Regex.Replace(txtValue, "\"'", "@@", RegexOptions.IgnoreCase);
                Regex.Replace(txtValue, "\"'■", "@@@", RegexOptions.IgnoreCase);
                Regex.Replace(txtValue, "'サブフォーム'", "@サブフォーム@", RegexOptions.IgnoreCase);
                Regex.Replace(txtValue, "'Subform'", "@Subform@", RegexOptions.IgnoreCase);
                Regex.Replace(txtValue, "'計算式'", "@計算式@", RegexOptions.IgnoreCase);
                Regex.Replace(txtValue, "'Calculation_formula'", "@Calculation_formula@", RegexOptions.IgnoreCase);
                Regex.Replace(txtValue, "'合計計算'", "@合計計算@", RegexOptions.IgnoreCase);
                Regex.Replace(txtValue, "'Total_Calculation'", "@Total_Calculation@", RegexOptions.IgnoreCase);

                while (idx != -1 && txtValue.Length > 0)
                {
                    idx = txtValue.IndexOf("'");
                    result = result + idx;

                    if (idx == -1)
                    {
                        result = -1;
                        break;
                    }

                    //N'' Or 「"N'" & INDATA & "'"」
                    if (idx > 0 && (txtValue[idx - 1] == 'N' ))
                    {
                        result++;
                        txtValue = txtValue.Substring(idx + 1);
                        int sub = txtValue.IndexOf("'") + 1;
                        if (txtValue.Contains("'''"))
                        {
                            int count = GetLengthApostrophes(txtValue);
                            sub = sub + count - 1;
                        }

                        txtValue = txtValue.Substring(sub);
                        result = result + sub;
                    }
                    else
                    {
                        break;
                    }
                }
            }

            return result;
        }

        public static string ApostropheIsNotCommented(string txtValue)
        {
            string result = txtValue;
           
            int idxFirst = txtValue.IndexOf(Separator.DoubleQuotes);

            if (idxFirst == -1)
            {
                return result;
            }

            int idxSecond;
            result = string.Empty;

            while (idxFirst != -1 && txtValue.Length > 0)
            {
                idxFirst = txtValue.IndexOf(Separator.DoubleQuotes);
                idxSecond = txtValue.IndexOf(Separator.DoubleQuotes, idxFirst + 1);
                if (idxFirst == -1 || idxSecond == -1)
                {
                    result = result + txtValue;
                    return result;
                }

                txtValue = ReplaceTextIsNotCommented(txtValue, idxFirst, idxSecond);
                result = result + txtValue.Substring(0, idxSecond);
                txtValue = txtValue.Substring(idxSecond);
            }

            return result;
        }

        public static string ReplaceTextIsNotCommented(string txtValue, int startIdx, int endIdx)
        {
            try
            {
                string result = string.Empty;
                string firstString = txtValue.Substring(0, startIdx);
                string mainString = txtValue.Substring(startIdx, endIdx + 1 - startIdx);
                string lastString = txtValue.Substring(endIdx + 1);

                mainString = mainString.Replace("'", "@");
                mainString = mainString.Replace("\"", "@");
                result = firstString + mainString + lastString;
                return result;
            }
            catch (Exception ex)
            { 
                Console.WriteLine("Exception.ReplaceTextIsNotCommented ", ex.ToString());
                return txtValue;
            }
        }

        public static int GetLengthApostrophes(string txtValue)
        {
            int result = 0;
            int sub = txtValue.IndexOf("'");
            txtValue = txtValue.Substring(sub);
            foreach (char ch in txtValue)
            {
                if (ch == '\'')
                {
                    result += 1;
                }
                else
                {
                    return result;
                }
            }

            return result;
        }
    }
}
