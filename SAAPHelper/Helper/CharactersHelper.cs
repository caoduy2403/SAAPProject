using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public static string FirstCharacterOfEachWord(string inputString, string separator = "")
        {
            // Split the input string into words, capitalize the first character of each word, and join them back into a string
            return string.Join(separator, inputString.Split(' ').Select(word => char.ToUpper(word[0]) + word.Substring(1)));
        }

        public static string FirstCharacterOfEachWordWithMessage(string inputString)
        {
            return FirstCharacterOfEachWord(inputString, Separator.Space);
        }

        public static string FirstCharacterOfEachWordWithVarible(string inputString)
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
    }
}
