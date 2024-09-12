using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using SAAPHelper.Models;
using DocumentFormat.OpenXml.Office.CustomUI;
using System.Text.RegularExpressions;

namespace SAAPHelper.Helper
{
    public class TranslateHelper
    {
        public static string TranslateText(string text, string sourceLang = "ja", string targetLang = "en")
        {
            string result = string.Empty;
            result = APITranslate(text, sourceLang, targetLang).GetAwaiter().GetResult();
            return result;
        }
        /// <summary>
        /// https://www.youtube.com/watch?v=fFDHfkz9KbE
        /// </summary>
        /// <param name="text"></param>
        /// <param name="sourceLang"></param>
        /// <param name="targetLang"></param>
        /// <returns></returns>
        public static async Task<string> APITranslate(string text, string sourceLang = "ja", string targetLang = "en")
        {
            string result = text;
            try
            {
                HttpClient httpClient = new HttpClient();
                if (string.IsNullOrEmpty(text))
                {
                    return result;
                }

                if (CharactersHelper.GetByteCount(text) > 500)
                {
                    return result;
                }

                string url = $"http://api.mymemory.translated.net/get?q={Uri.EscapeDataString(text)}&langpair={sourceLang}|{targetLang}";
                HttpResponseMessage response = await httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();
                string responseJson = await response.Content.ReadAsStringAsync();
                var trans = JsonConvert.DeserializeObject<TranslationResponse>(responseJson);
                if (trans.ResponseStatus == 200)
                {
                    foreach (var match in trans.Matches)
                    {
                        if (!JapanCharactersHandler.IsJapaneseMatch(match.Translation))
                        {
                            result = match.Translation;
                            //Regex reg = new Regex("[*'\",_&#^@:]");
                            //result = reg.Replace(result, string.Empty);

                            Regex rgx = new Regex("[^a-zA-Z1-9 -]");
                            result = rgx.Replace(result, string.Empty).Trim();

                            return result;
                        }
                    }
                   
                    return result;
                }
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception TranslateHelper.TranslateText ", ex.Message);
                return result;
            }
        }

        /// <summary>
        /// "vi", "en" "ja"
        /// https://laptrinhvb.net/bai-viet/devexpress/---Csharp----Su-dung-Google-Translate-API-de-dich-van-ban-/c251d0c2ee8acb6c.html
        /// https://www.codeproject.com/Tips/5247661/Google-Translate-API-Usage-in-Csharp
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string TranslateTextXXX(string input)
        {
            string url = String.Format("https://translate.googleapis.com/translate_a/single?client=gtx&sl={0}&tl={1}&dt=t&q={2}","ja", "en", Uri.EscapeUriString(input));
            HttpClient httpClient = new HttpClient();
            string result = httpClient.GetStringAsync(url).Result;
            //var jsonData = JsonConvert.DeserializeObjectNewtonsoft.Json
            var jsonData = JsonConvert.DeserializeObject<List<dynamic>>(result);
            var translationItems = jsonData[0];
            var data = JsonConvert.SerializeObject(translationItems[0]);
            var listData = JsonConvert.DeserializeObject<List<dynamic>>(data);

            string txtEL = listData[0].ToString();
            string txtJP = listData[1].ToString();

            string translation = "";
            foreach (object item in translationItems)
            {
                IEnumerable translationLineObject = item as IEnumerable;
                IEnumerator translationLineString = translationLineObject.GetEnumerator();
                translationLineString.MoveNext();
                //var resultCurrent = translationLineString.Current.ToString();
                translation += string.Format(" {0}", translationLineString.Current.ToString());

            }
            if (translation.Length > 1) { translation = translation.Substring(1); };
            return translation;
        }
    }
}
