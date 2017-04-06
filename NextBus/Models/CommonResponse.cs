using Newtonsoft.Json;
using NextBus.Helpers;

namespace NextBus.Models
{
    public class CommonResponse : ObservableObject
    {
        [JsonProperty("Result")]
        public bool Result { get; set; }

        [JsonProperty("ResultDesc")]
        public string ResultDesc { get; set; }

        [JsonProperty("ResultApiVersionInvalid")]
        public bool ResultApiVersionInvalid { get; set; }
    }
}
