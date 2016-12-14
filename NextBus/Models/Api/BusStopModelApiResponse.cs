using Newtonsoft.Json;
using System.Collections.Generic;

namespace NextBus.Models.Api
{
    public class Route
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

    public class BusStopResponse
    {

        [JsonProperty("I")]
        public string Id { get; set; }

        [JsonProperty("N")]
        public string Name { get; set; }

        [JsonProperty("Z")]
        public string Z { get; set; }

        [JsonProperty("LA")]
        public string Latitude { get; set; }

        [JsonProperty("LO")]
        public string Longitude { get; set; }

        [JsonProperty("LOC")]
        public string Locality { get; set; }

        [JsonProperty("L")]
        public IList<Route> Routes { get; set; }
    }
    
    public class CommonRet
    {

        [JsonProperty("Result")]
        public bool Result { get; set; }

        [JsonProperty("ResultDesc")]
        public string ResultDesc { get; set; }

        [JsonProperty("ResultApiVersionInvalid")]
        public bool ResultApiVersionInvalid { get; set; }
    }

    public class BusStopModelApiResponse
    {

        [JsonProperty("CommonRet")]
        public CommonRet CommonRet { get; set; }

        [JsonProperty("Stops")]
        public IList<BusStopResponse> Stops { get; set; }
    }
}
