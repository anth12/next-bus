using Newtonsoft.Json;
using NextBus.Helpers;

namespace NextBus.Models
{
    public class Route : ObservableObject
    {
        [JsonProperty("I")]
        public string Id { get; set; }

        [JsonProperty("N")]
        public string Name { get; set; }

        [JsonProperty("D")]
        public string Destination { get; set; }

        [JsonProperty("AT")]
        public string ArrivesAt { get; set; }

        [JsonProperty("O")]
        public string O { get; set; }

        [JsonProperty("RA")]
        public bool RA { get; set; }
    }
}
