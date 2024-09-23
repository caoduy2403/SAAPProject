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
        public string LineCode { get; set; }
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

    public class ExportModel
    {
        public string listID { get; set; } = string.Empty;
        public bool isBefore { get; set; } = true;
        public bool isAfter { get; set; } = true;
        public bool isCommented { get; set; } = true;
        public bool isTranslatedFile { get; set; } = true;

    }
}
