using System.Collections.Generic;
using Newtonsoft.Json;
using NextBus.Helpers;
using Plugin.Geolocator.Abstractions;

namespace NextBus.Models
{
    public class BusStop : ObservableObject
    {
        [JsonProperty("I")]
        public string Id { get; set; }

        [JsonProperty("N")]
        public string Name { get; set; }

        [JsonProperty("Z")]
        public string Z { get; set; }

        [JsonProperty("LA")]
        public double Latitude { get; set; }

        [JsonProperty("LO")]
        public double Longitude { get; set; }

        [JsonProperty("LOC")]
        public string Locality { get; set; }

        [JsonProperty("L")]
        public IList<Route> Routes { get; set; } = new List<Route>();

        [JsonIgnore]
        public Position Position => new Position { Latitude = Latitude, Longitude = Longitude};

        public CustomStopData Data { get; set; } = new CustomStopData();

    }

    public class CustomStopData : ObservableObject
    {
        public bool IsFavorite { get; set; }
        public string FavoriteIcon => IsFavorite ? "heart_full_green.png" : "heart_green.png";

        public double? Distance { get; set; }
    }
}
