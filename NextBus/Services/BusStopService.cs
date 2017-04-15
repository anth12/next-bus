using NextBus.Helpers;
using NextBus.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace NextBus.Services
{
    public class BusStopService
    {
        private static BusStopModelApiResponse _stops;

        public static async Task SaveChanges()
        {
            await FileHelper.PersistAsync(_stops);
        }

        public static async Task<BusStopModelApiResponse> GetStops(Action loadingFromApiCallback = null)
        {
            if (_stops != null)
                return _stops;

            // Attempt to load from disk
            _stops = await FileHelper.LoadAsync<BusStopModelApiResponse>();

            if (_stops != null)
                return _stops;

            loadingFromApiCallback?.Invoke();

            // Load the data
            _stops = await ApiHelper.PostAsync<BusStopModelApiResponse>("/StopsMap/GetBusStops");

            if (_stops != null)
            {
                // Write the data to disk
                await SaveChanges();
            }

            return _stops;
        }

        public static async Task<ComingBusApiResponse> GetStopDetails(string busStopId)
        {
            var busStop = (await GetStops())
                            .Stops.First(b => b.Id == busStopId);

            return await GetStopDetails(busStop);
        }

        public static async Task<ComingBusApiResponse> GetStopDetails(BusStop busStop)
        {
            // Load the data
            return await ApiHelper.PostAsync<ComingBusApiResponse>("/StopsMap/GetComingBus", new { BusStop = busStop });
        }

    }
}
