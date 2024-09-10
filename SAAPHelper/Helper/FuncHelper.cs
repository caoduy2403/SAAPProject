using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAAPHelper.Helper
{
    public class FuncHelper
    {
        /// <summary>
        /// https://www.w3resource.com/csharp-exercises/string/csharp-string-exercise-52.php
        /// </summary>
        /// <param name="inputString"></param>
        /// <returns></returns>
        public static string FirstCharacterOfEachWord(string inputString)
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
                int idx = 0;
                txtValue = txtValue.Replace("N''''","@AAA@");
                txtValue = txtValue.Replace("N''", "@A@");
                txtValue = txtValue.Replace("'サブフォーム'","@サブフォーム@");

                txtValue = txtValue.Replace("'計算式'", "@計算式@");
                txtValue = txtValue.Replace("'Calculation_formula'", "@Calculation_formula@");
                txtValue = txtValue.Replace("'Subform'", "@Subform@");
                txtValue = txtValue.Replace("\"'\"", "@@@");
                txtValue = txtValue.Replace("\"'■", "@@@");


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
