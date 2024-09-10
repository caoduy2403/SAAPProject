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

        public static string ConvertFulToHalf(string txtValue)
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

       
    }
}
