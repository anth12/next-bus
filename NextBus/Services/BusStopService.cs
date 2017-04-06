using System;
using System.Linq;
using System.Threading.Tasks;
using NextBus.Helpers;
using NextBus.Models;

namespace NextBus.Services
{
    public class BusStopService
    {
        private BusStopModelApiResponse _stops;

        public async Task<BusStopModelApiResponse> GetStops(Action loadingFromApiCallback = null)
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
                await FileHelper.PersistAsync(_stops);
            }

            return _stops;
        }

        public async Task<ComingBusApiResponse> GetStopDetails(string busStopId)
        {
            var busStop = (await GetStops())
                            .Stops.First(b => b.Id == busStopId);
            // Load the data
            return await ApiHelper.PostAsync<ComingBusApiResponse>("StopsMap/GetComingBus", new { BusStop = busStop });
        }

    }
}
