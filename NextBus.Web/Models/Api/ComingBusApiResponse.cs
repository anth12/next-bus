using System.Collections.Generic;
using Newtonsoft.Json;

namespace NextBus.Web.Models.Api
{
    
    public class ComingBusApiResponse
    {

        [JsonProperty("CommonRet")]
        public CommonRet CommonRet { get; set; }

        [JsonProperty("Stops")]
        public IList<BusStopResponse> Stops { get; set; }

        [JsonProperty("IsNoNearbyStopsFound")]
        public bool IsNoNearbyStopsFound { get; set; }

        [JsonProperty("Routes")]
        public object Routes { get; set; }
    }
}
