using SAAPHelper.Constant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SAAPHelper.Helper
{
    public class JapanCharactersHandler
    {
        static Dictionary<char, char> _dicFullToHalf;
        static Dictionary<char, char> _dicHalfToFull;

        static JapanCharactersHandler()
        {
            _dicFullToHalf = new Dictionary<char, char>();
            _dicHalfToFull = new Dictionary<char, char>();
            string fullWidthChars = "アイウエオカキクケコサシスセソタチツテトナニヌネノハヒフヘホマミムメモヤユヨラリルレロワヲンッァィゥェォャュョ゙゚ー０１２３４５６７８９ＡＢＣＤＥＦＧＨＩＪＫＬＭＮＯＰＱＲＳＴＵＶＷＸＹＺａｂｃｄｅｆｇｈｉｊｋｌｍｎｏｐｑｒｓｔｕｖｗｘｙｚ";
            string halfWidthChars = "ｱｲｳｴｵｶｷｸｹｺｻｼｽｾｿﾀﾁﾂﾃﾄﾅﾆﾇﾈﾉﾊﾋﾌﾍﾎﾏﾐﾑﾒﾓﾔﾕﾖﾗﾘﾙﾚﾛﾜｦﾝｯｧｨｩｪｫｬｭｮﾞﾟｰ0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            for (int i = 0; i < fullWidthChars.Length; i++)
            {
                char full = fullWidthChars[i];
                char half = halfWidthChars[i];

                _dicFullToHalf.Add(full, half);
                _dicHalfToFull.Add(half, full);
            }
        }

        //String message = 'EnglishLetters - みずずほダイレクト';
        //String regex = '[\u3040-\u30ff\u3400-\u4dbf\u4e00-\u9fff\uf900-\ufaff\uff66-\uff9f]';
        //Pattern regexPattern = Pattern.compile(regex);
        //Matcher regexMatcher = regexPattern.matcher(message);
        //system.assert(regexMatcher.find());
        //var romaji = GetCharsInRange(searchKeyword, 0x0020, 0x007E);
        //var hiragana = GetCharsInRange(searchKeyword, 0x3040, 0x309F);
        //var katakana = GetCharsInRange(searchKeyword, 0x30A0, 0x30FF);
        //var kanji = GetCharsInRange(searchKeyword, 0x4E00, 0x9FBF);
        /// <summary>
        /// https://stackoverflow.com/questions/15805859/detect-japanese-character-input-and-romajis-ascii
        /// https://salesforce.stackexchange.com/questions/176498/check-if-a-string-contains-a-japanese-letter
        /// </summary>
        /// <param name="text"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        private static IEnumerable<char> GetCharsInRange(string text, int min, int max)
        {
            return text.Where(e => e >= min && e <= max);
        }


        public static bool IsJapaneseMatch(string textValue)
        {
            // Create a Regex
            Regex rg = new Regex(Constants.pattern);
            return rg.IsMatch(textValue);
        }

       

        public static bool IsContainedHalfWidth(string textValue)
        {
            bool isHalfWidth = false;
            foreach (char ch in textValue)
            {
                if (_dicHalfToFull.ContainsKey(ch))
                {
                    isHalfWidth = true;
                    break;
                }
            }
            return isHalfWidth;
        }



        public static string ConvertFullToHalf(string txtValue)
        {
            StringBuilder sbResult = new StringBuilder(txtValue);
            for (int i = 0; i < txtValue.Length; i++)
            {
                if (_dicFullToHalf.ContainsKey(txtValue[i]))
                {
                    sbResult[i] = _dicFullToHalf[txtValue[i]];
                }
            }
            return sbResult.ToString();
        }

        public static string ConvertHalfToFull(string txtValue)
        {
            StringBuilder sbResult = new StringBuilder(txtValue);
            for (int i = 0; i < txtValue.Length; i++)
            {
                if (_dicHalfToFull.ContainsKey(txtValue[i]))
                {
                    sbResult[i] = _dicHalfToFull[txtValue[i]];
                }
            }
            return sbResult.ToString();
        }

        #region LINQ

        public static bool IsContainedHalfWidthLINQ(string textValue)
        {
            return textValue.Any(ch => _dicHalfToFull.ContainsKey(ch));
        }

        public static string ConvertHalfToFullLINQ(string txtValue)
        {
            StringBuilder sbResult = new StringBuilder(txtValue);
            for (int i = 0; i < txtValue.Length; i++)
            {
                if (_dicHalfToFull.ContainsKey(txtValue[i]))
                {
                    sbResult[i] = _dicHalfToFull[txtValue[i]];
                }
            }
            return sbResult.ToString();
        }

        #endregion



    }
}
