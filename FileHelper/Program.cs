using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClosedXML;
using ClosedXML.Excel;
using System.IO;
using System.Data;
using System.ComponentModel;
using FileHelper.Extension;
using System.Collections;
using System.Net.Http;
using GoogleTranslateFreeApi;
using DocumentFormat.OpenXml.Office2010.Ink;
using Newtonsoft.Json;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Reflection;


namespace FileHelper
{
    public class Product
    {
        public int ID { get; set; }
        public string Category { get; set; }
    }

    internal class Program
    {
        //https://www.c-sharpcorner.com/article/export-and-import-excel-file-using-closedxml-in-asp-net-mvc/
        private const string pathFolder = "C:\\Users\\AZIP\\Desktop\\SAAPLibrary";
        //private const string baseUrl = "http://api.mymemory.translated.net";
        static void Main(string[] args)
        {
            //string[] arrayString = {"Gc Digital Processing Type_Card Type M","Gc Digital Processing Type_List Type M",};


            Console.WriteLine(" BEGIN ");

            DataTable dt = new DataTable();
            dt.Columns.Add("ID", typeof(int));
            dt.Columns.Add("Category", typeof(string));
            dt.Rows.Add(1, "Electronics");
            dt.Rows.Add(2, "Books");

            object resultModel = FuncSelect(dt.Rows[0]);


            var result = (from rw in dt.AsEnumerable()
                          select GetItemV1<Product>(rw)).ToList();

            foreach (var item in result)
            { 
                Console.WriteLine("ID = {0} Category {1}\n", item.ID, item.Category);
            }

            //foreach (string str in arrayString)
            //{
            //    string message = FirstCharacterOfEachWordWithMessage(str);
            //    string varible = FirstCharacterOfEachWordWithVarible(str);

                //    Console.WriteLine("Message {0} \nVarible {1} \n", message, varible);
                //}

            Console.WriteLine(" END ");

            Console.ReadKey();
        }

        private List<T> GetListByDataTable<T>(DataTable dt)
        {
            var result = (from rw in dt.AsEnumerable()
                          select GetItem<T>(rw)).ToList();

            return result;
        }

        private static object FuncSelect(DataRow dr)
        {
            object oResult = new
            {
                ID = dr.Field<int>("ID"),
                Category = dr.Field<string>("Category")
            };

            return oResult;
        }

        private static T GetItemV1<T>(DataRow dr)
        {
            Type temp = typeof(T);
            T obj = Activator.CreateInstance<T>();

            foreach (DataColumn col in dr.Table.Columns)
            {
                var prop = obj.GetType().GetProperty(col.ColumnName);
                if (prop != null && dr[col] != DBNull.Value)
                    prop.SetValue(obj, dr[col]);
            }
            return obj;
        }

        private static T GetItem<T>(DataRow dr)
        {
            Type temp = typeof(T);
            T obj = Activator.CreateInstance<T>();

            foreach (DataColumn column in dr.Table.Columns)
            {
                foreach (PropertyInfo pro in temp.GetProperties())
                {
                    if (pro.Name == column.ColumnName)
                        pro.SetValue(obj, dr[column.ColumnName], null);
                    else
                        continue;
                }
            }
            return obj;
        }

        private static void LinqMethod(DataTable dt)
        {
            var list = dt.AsEnumerable().Select(row =>
                new
                {
                    ID = row.Field<int>("ID"),
                    Category = row.Field<string>("Category")

                }).ToList();
        }

        public static readonly string Space = " ";
        public static readonly string Underscore = "_";
        public static readonly string Commas = ";";
        public static readonly string Tab = "\t";
        public static readonly string QuestionMark = "?";
        public static readonly string Exclamation = "!";
        public static readonly string Colons = ":";
        public static readonly string SemiColons = ";";

        public static string FirstCharacterOfEachWord(string inputString, string separator)
        {
            // Split the input string into words, capitalize the first character of each word, and join them back into a string
            return string.Join(separator.ToString(), inputString.Split(' ').Select(word => char.ToUpper(word[0]) + word.Substring(1)));
        }

        public static string FirstCharacterOfEachWordWithMessage(string inputString)
        {
            // Split the input string into words, capitalize the first character of each word, and join them back into a string
            //return FirstCharacterOfEachWord(inputString, Separator.Space);
            return string.Join(" ", inputString.Split(' ').Select(word => char.ToUpper(word[0]) + word.Substring(1)));
        }


        public static string FirstCharacterOfEachWordWithVarible(string inputString)
        {
            string[] arrayString = inputString.Trim().Split(' ');
            if (arrayString.Length < 2)
            {
                return string.Join(" ", arrayString);
            }

            string strResult = string.Empty;
            string firstString = arrayString[0];

            if (firstString.Length < 3 || firstString == firstString.ToLower())
            {
                string lastString = string.Join(Underscore, inputString.Split(' ').Where((word, i) => i > 1).Select(word => char.ToUpper(word[0]) + word.Substring(1)));
                strResult = string.Concat(firstString, lastString);
            }
            else {
                strResult = string.Join(Underscore, arrayString.Select(word => char.ToUpper(word[0]) + word.Substring(1)));
            }
            return strResult;
        }

        public static async Task<string> MainAsync(string text, string sourceLang = "ja", string targetLang = "en")
        {
            try
            {
                HttpClient httpClient = new HttpClient();
                if (string.IsNullOrEmpty(text))
                {
                    return string.Empty;
                }

                if (GetByteCount(text) > 500)
                {
                    return string.Empty;
                }

                string url = $"http://api.mymemory.translated.net/get?q={Uri.EscapeDataString(text)}&langpair={sourceLang}|{targetLang}";
                HttpResponseMessage response = await httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();
                string responseJson = await response.Content.ReadAsStringAsync();
                var trans = JsonConvert.DeserializeObject<TranslationResponse>(responseJson);

                if (trans.ResponseStatus == 200)
                {
                    return trans.ResponseData.TranslatedText;
                }
                return string.Empty;
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }

        public static int GetByteCount(string input)
        {
            int intByteCount = 0;
            System.Text.Encoding sjisEnc = System.Text.Encoding.GetEncoding("Shift_JIS");
            intByteCount = sjisEnc.GetByteCount(input);
            return intByteCount;

        }

        public class TranslationResponse
        {
            [JsonProperty(PropertyName = "responseData")]
            public ResponseData ResponseData { get; set; }
         
            /// <summary>
            /// 200: OK
            /// </summary>
           [JsonProperty(PropertyName = "responseStatus")]
            public int ResponseStatus { get; set; }
        }

        public class ResponseData
        {
            [JsonProperty(PropertyName = "translatedText")]
            public string TranslatedText { get; set; }

            //[JsonProperty(PropertyName = "match")]
            //public string Match { get; set; }
        }

    }
}
