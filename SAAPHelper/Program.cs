using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;
using System.Security.Policy;
using System.Data;
using SAAPHelper.Helper;
using System.ComponentModel;
using System.Net;
using System.Collections;
using System.Net.Http;
using Newtonsoft.Json;
using SAAPHelper.Constant;
using SAAPHelper.Models;
using DocumentFormat.OpenXml.Vml;
using DocumentFormat.OpenXml.ExtendedProperties;
using DocumentFormat.OpenXml.Wordprocessing;
using DocumentFormat.OpenXml.Drawing;
using DocumentFormat.OpenXml.Bibliography;

namespace SAAPHelper
{
    internal class Program
    {
       
        static void Main(string[] args)
        {
            string value = "x関数削除Sql = x関数削除Sql & \"   select * from objects_schema where type in (N'IF',N'TF', N'FN') and schema_name = \" & SQLTXT(xCRS!スキーマ) & \" and name collate \" & Gc照合順序名 & \" = \" & SQLTXT(xCRS!オブジェクト名) & vbCrLf";

            //int ori = value.IndexOf('\"');
            //int indexStart1 = value.IndexOf('\"', value.IndexOf('\"') + 1);
            //int indexStart = value.IndexOf("\"", 3);

            string afterVal = ApostropheIsNotCommented(value);

            string xText = string.Empty;

            string xJapan = " ﾃｷｽﾄ テキスト  ｴﾗｰ, エラー";
            string line = "sssa";

            int index = line.IndexOf("A", StringComparison.OrdinalIgnoreCase);
            string convert = JapanCharactersHandler.ConvertHalfToFull(xJapan);

            string result = Regex.Replace(xJapan, "ﾃｷｽﾄ", "test", RegexOptions.IgnoreCase);
            

            //var data = GetSpecialCharacters(value);
            //int IdxCmt = FuncHelper.GetStartIdxCommented(value);
            //string firstName  = CharactersHelper.CapitalizeFirstLetter("AAA 1234    CCCC 1");       
            JapanFileHelper.ConvertComment(true, true, true);

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

            Console.WriteLine("Successfully ");
            Console.ReadKey();
        }

        public static string ApostropheIsNotCommented(string txtValue)
        {
            string result = string.Empty;
           

            //txtValue = txtValue.Replace("\"", "@@");
            if (FuncHelper.chkTextIsNotCommented(txtValue))
            {
                int idxFirst = 0;
                int idxSecond = 0;

                while (idxFirst != -1 && txtValue.Length > 0)
                {
                    idxFirst = 0;
                    idxSecond = 0;
                    idxFirst = txtValue.IndexOf(Separator.DoubleQuotes);

                    if (idxFirst == -1)
                    {
                        return txtValue;
                    }

                    idxSecond = txtValue.IndexOf(Separator.DoubleQuotes, idxFirst + 1);
                    txtValue = ReplaceTextIsNotCommented(txtValue, idxFirst, idxSecond);
                   //result = txtValue.Substring()
                }
            }

            return txtValue;
        }

        public static string ReplaceTextIsNotCommented(string txtValue, int startIdx, int endIdx)
        {
            string result = string.Empty;
            string firstString = txtValue.Substring(0, startIdx-1);
            string mainString = txtValue.Substring(startIdx, endIdx +1 - startIdx);
            string lastString = txtValue.Substring(endIdx + 1);

            mainString = mainString.Replace("'","@");
            mainString = mainString.Replace("\"", "@");
            result = firstString + mainString + lastString;
            return result;
        }



    }
}
