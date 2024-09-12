using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.ExtendedProperties;
using SAAPHelper.Constant;

namespace SAAPHelper.Helper
{
    public class CharactersHelper
    {
        public static int GetByteCount(string input)
        {
            int intByteCount = 0;
            Encoding sjisEnc = Encoding.GetEncoding("Shift_JIS");
            intByteCount = sjisEnc.GetByteCount(input);

            return intByteCount;
        }

        public static string CapitalizeFirstLetter(string inputString, string separator = "")
        {
            if (string.IsNullOrEmpty(inputString))
            {
                return inputString;
            }

            inputString = inputString.ToLower();
            string[] arrString = inputString.Split(' ').Select(word => FirstLetter(word))
                                                       .Where(word => !string.IsNullOrEmpty(word)).ToArray();
            // Split the input string into words, capitalize the first character of each word, and join them back into a string
            return string.Join(separator, arrString);
        }

        public static string CapitalizeFirstLetterWithMessage(string inputString)
        {
            return CapitalizeFirstLetter(inputString, Separator.Space);
        }

        public static string CapitalizeFirstLetterWithVarible(string inputString)
        {
            string[] arrayString = inputString.Trim().Split(' ');

            if (arrayString.Length < 2)
            {
                return string.Join("", arrayString);
            }

            string strResult = string.Empty;
            string firstString = arrayString[0];

            if (firstString.Length < 3 || firstString == firstString.ToLower())
            {
                string lastString = string.Join(Separator.Underscore, inputString.Split(' ').Where((word, i) => i > 1).Select(word => char.ToUpper(word[0]) + word.Substring(1)));
                strResult = string.Concat(firstString, lastString);
            }
            else
            {
                strResult = string.Join(Separator.Underscore, arrayString.Select(word => char.ToUpper(word[0]) + word.Substring(1)));
            }

            return strResult;
        }


        public static string FirstLetter(string word)
        {
            string result = string.Empty;
            if (string.IsNullOrEmpty(word))
            { 
               return result;
            }
            if (word.Length < 2)
            {
                result = word.ToUpper();
                return result;
            }

            return char.ToUpper(word[0]) + word.Substring(1);
        }

        public static List<string> GetSpecialCharacters(string txtValue)
        {
            List<string> list = new List<string>();
            var regexItem = new Regex("^[a-zA-Z0-9 ]*$");

            string list_char = string.Empty;

            foreach (char ch in txtValue)
            {
                string val = ch.ToString();
                if (!regexItem.IsMatch(val))
                {
                    list.Add(val);
                }
            }

            list = list.Select(x => x).Distinct().ToList();

            return list;
        }
    }
}
