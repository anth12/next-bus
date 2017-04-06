using System.Collections.Generic;
using Newtonsoft.Json;
using NextBus.Helpers;

namespace NextBus.Models
{
    public class BusStopModelApiResponse : ObservableObject
    {

        [JsonProperty("CommonRet")]
        public CommonResponse Result { get; set; }

        [JsonProperty("Stops")]
        public IList<BusStop> Stops { get; set; }
    }
}
