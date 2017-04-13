﻿using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using NextBus.Helpers;

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
        public string Latitude { get; set; }

        [JsonProperty("LO")]
        public string Longitude { get; set; }

        [JsonProperty("LOC")]
        public string Locality { get; set; }

        [JsonProperty("L")]
        public IList<Route> Routes { get; set; }

        public Coordinates Coordinates => new Coordinates(Latitude, Longitude);

        public decimal? Distance { get; set; }

        public bool IsFavorite { get; set; }

        public string FavoriteIcon => IsFavorite ? "heart_full_green.png" : "heart_green.png";
    }
}