using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAAPHelper.Constant
{
    public class Constants
    {
        public const string pathFolder = "C:\\Users\\AZIP\\Desktop\\SAAPLibrary";
        public const string OutputName = "GetText";
        public const string ConvertName = "Converted";
        public const string CommentName = "ReviewedNew";
        /// <summary>
        /// regex to check japanese
        /// </summary>
        public const string pattern = "[\u3040-\u30ff\u3400-\u4dbf\u4e00-\u9fff\uf900-\ufaff\uff66-\uff9f]";

        /// <summary>
        /// get words from Split
        /// </summary>
        public static char[] delimiterChars = { ' ', ',', '.', ':', '(', ')', '{', '}', '=', '!', ',', '[', ']', '「', '」', '↑', '↓', '%', '）', '（', '<', '>', '：', '\'', '】', '【', '『', '』' };
        public static readonly string[] _redFlag = { "ﾃｷｽﾄ", "テキスト", "ｴﾗｰ", "エラー" };
    }

    public class ExtensionFile
    {
        /// <summary>
        /// Text
        /// </summary>
        public static readonly string TEXT = ".txt";
    }

    public class Separator
    {
        /// <summary>
        /// Seperator
        /// </summary>
        public static readonly string Space = " ";
        public static readonly string Underscore = "_";
        public static readonly string Commas = ";";
        public static readonly string Tab = "\t";
        public static readonly string QuestionMark = "?";
        public static readonly string Exclamation = "!";
        public static readonly string Colons = ":";
        public static readonly string SemiColons = ";";
        public static readonly string DoubleQuotes = "\"";
    }
}
