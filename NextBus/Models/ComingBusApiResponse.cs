using System.Collections.Generic;
using Newtonsoft.Json;
using NextBus.Helpers;

namespace NextBus.Models
{
    public class ComingBusApiResponse : ObservableObject
    {
        [JsonProperty("CommonRet")]
        public CommonResponse Result { get; set; }

        [JsonProperty("Stops")]
        public IList<BusStop> Stops { get; set; }

        [JsonProperty("IsNoNearbyStopsFound")]
        public bool IsNoNearbyStopsFound { get; set; }

        [JsonProperty("Routes")]
        public object Routes { get; set; }
    }
}
