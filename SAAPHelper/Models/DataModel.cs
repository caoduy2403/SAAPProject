using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAAPHelper.Models
{
    public class DataExcelModel
    {
        public int No { get; set; }
        public string JA { get; set; }
        public string EN { get; set; }
        
        public string Variable { get; set; }
        public string Message { get; set; }
        public int LineCode { get; set; }
        public string LineText { get; set; }

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

        /// <summary>
        /// 200: OK
        /// </summary>
        [JsonProperty(PropertyName = "matches")]
        public List<MatchResponse> Matches { get; set; }
    }

    public class MatchResponse
    {
        [JsonProperty(PropertyName = "Segment")]
        public string Segment { get; set; }

        [JsonProperty(PropertyName = "translation")]
        public string Translation { get; set; }
    }

    public class ResponseData
    {
        [JsonProperty(PropertyName = "translatedText")]
        public string TranslatedText { get; set; }

        //[JsonProperty(PropertyName = "match")]
        //public string Match { get; set; }
    }
}
