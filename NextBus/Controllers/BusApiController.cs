using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using NextBus.Models;
using NextBus.Models.Api;

namespace NextBus.Controllers
{
    public class BusApiController : Controller
    {
        private IHostingEnvironment _hostingEnvironment;
        private IMemoryCache _memoryCache;

        public BusApiController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        private const string ApiEndpoint = "https://www.publictransport.com.mt/appws/";
        private const string ApiVersion = "1.3.1";
        private string RawBusStopDataPath => _hostingEnvironment.WebRootPath + "\\App_Data\\bus-stops-raw.json";
        private string BusStopDataPath => _hostingEnvironment.WebRootPath + "\\App_Data\\bus-stops.json";

        private IList<BusStopResponse> _busStops;
        private IList<BusStopResponse> BusStops
        {
            get
            {
                if (_busStops != null)
                    return _busStops;

                var json = System.IO.File.ReadAllText(RawBusStopDataPath);
                return _busStops = JsonConvert.DeserializeObject<BusStopModelApiResponse>(json).Stops;
            }
        }

        public async Task<string> UpdateStops()
        {
            using (var webClient = new HttpClient())
            {
                webClient.DefaultRequestHeaders.TryAddWithoutValidation("x-api-version", ApiVersion);
                webClient.DefaultRequestHeaders.TryAddWithoutValidation("content-type", "application/json; charset=utf-8");

                var response = await webClient.PostAsync(ApiEndpoint + "StopsMap/GetBusStops", new StringContent(""));

                var content = await response.Content.ReadAsStringAsync();
                // Write the raw reponse data
                System.IO.File.WriteAllText(RawBusStopDataPath, content);

                var data = JsonConvert.DeserializeObject<BusStopModelApiResponse>(content);

                var usefulData = data.Stops.Select(stop => new BusStop
                {
                    Id = stop.Id,
                    Name = stop.Name + ", " + stop.Locality,
                    Latitude = stop.Latitude,
                    Longitude = stop.Longitude
                });

                var usefulDataJson = JsonConvert.SerializeObject(usefulData);
                System.IO.File.WriteAllText(BusStopDataPath, usefulDataJson);

                return GetStops();
            }
        }

        // /BusApi/GetStops
        public string GetStops()
        {
            return System.IO.File.ReadAllText(BusStopDataPath);
        }

        public IEnumerable<NearbyBus> GetNextBusTimes(string id)
        {
            return Cache("ComingBus-" + id, () =>
            {

                var busStop = BusStops.First(b => b.Id == id);
                using (var webClient = new HttpClient())
                {
                    webClient.DefaultRequestHeaders.TryAddWithoutValidation("x-api-version", ApiVersion);
                    webClient.DefaultRequestHeaders
                        .Accept
                        .Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    //webClient.DefaultRequestHeaders.TryAddWithoutValidation("content-type", "application/json; charset=UTF-8");
                    //webClient.DefaultRequestHeaders.TryAddWithoutValidation("Accept", "application/json, text/javascript, */*; q=0.01");

                    var payload = JsonConvert.SerializeObject(new { BusStop = busStop });
                    var response = webClient.PostAsync(ApiEndpoint + "StopsMap/GetComingBus", new StringContent(payload,
                        Encoding.UTF8,
                        "application/json")).Result;

                    var content = response.Content.ReadAsStringAsync().Result;

                    var data = JsonConvert.DeserializeObject<ComingBusApiResponse>(content);

                    return data.Stops.First(s=> s.Id == id).Routes.Select(route => new NearbyBus
                    {
                        Name = route.Name,
                        Destination = route.Destination,
                        ArrivalTime = route.ArrivesAt,
                    });
                }

            });
        }

        private static readonly List<Tuple<string, DateTime, object>> CacheSource = new List<Tuple<string, DateTime, object>>();

        private TType Cache<TType>(string cacheKey, Func<TType> loadFunc) where TType : class
        {
            // Remove expired
            CacheSource.RemoveAll(c => c.Item2 < DateTime.Now);

            var value = CacheSource.FirstOrDefault(c => c.Item1 == cacheKey);

            if(value != null)
                return value as TType;

            var result = loadFunc();
            CacheSource.Add(new Tuple<string, DateTime, object>(cacheKey, DateTime.Now.AddSeconds(10), result));

            return result;
        }

    }
}
