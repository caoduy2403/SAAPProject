using System;
using System.Text.RegularExpressions;
using System.IO;
using SAAPHelper.Helper;
using SAAPHelper.Constant;
using DocumentFormat.OpenXml.Drawing;
namespace SAAPHelper
{
    internal class Program
    {
       
        static void Main(string[] args)
        {
            //string value = "\" and name collate \" & Gc照合順序名 & \" = \" ";
            string value = "xSQL = xSQL & \" Where 種類 = 'テーブル'\"";

            //int ori = value.IndexOf('\"');
            //int indexStart1 = value.IndexOf('\"', value.IndexOf('\"') + 1);
            //int indexStart = value.IndexOf("\"", 3);

            int afterVal = FuncHelper.GetStartIdxCommented(value);

            string xText = string.Empty;

            string xJapan = " ﾃｷｽﾄ テキスト  ｴﾗｰ, エラー";
            string line = "sssa";

            int index = line.IndexOf("A", StringComparison.OrdinalIgnoreCase);
            string convert = JapanCharactersHandler.ConvertHalfToFull(xJapan);

            string result = Regex.Replace(xJapan, "ﾃｷｽﾄ", "test", RegexOptions.IgnoreCase);

            //var data = GetSpecialCharacters(value);
            //int IdxCmt = FuncHelper.GetStartIdxCommented(value);
            //string firstName  = CharactersHelper.CapitalizeFirstLetter("AAA 1234    CCCC 1");       
            JapanFileHelper.RemoveComment(false, false, true);


            DateTime StartTime = DateTime.Now;

            Console.WriteLine("=======================================================");
            Console.WriteLine("[START]", StartTime);

            string[] fileArray = Directory.GetFiles(Constants.pathFolder, "*.txt");
            foreach (string fileUrl in fileArray)
            {
                if (!fileUrl.Contains(Constants.OutputName) && !fileUrl.Contains(Constants.ConvertName))
                {
                    Console.WriteLine("fileName {0}", System.IO.Path.GetFileName(fileUrl));

                    string filename = System.IO.Path.GetFileName(fileUrl);
                    //JapanFileHelper.GetJapaneseTextFile(fileUrl);
                    //JapanFileHelper.ConvertFile(fileUrl);
                }
            }

            Console.WriteLine("[END]", (DateTime.Now - StartTime));
            Console.WriteLine("=======================================================");

            Console.WriteLine("Successfully ");
            Console.ReadKey();
        }
    }
}
