//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//using NextBus.Models;

//using Xamarin.Forms;

//[assembly: Dependency(typeof(NextBus.Services.MockDataStore))]
//namespace NextBus.Services
//{
//    public class MockDataStore : IDataStore<BusStop>
//    {
//        bool isInitialized;
//        List<BusStop> items;

//        public async Task<bool> AddItemAsync(BusStop item)
//        {
//            await InitializeAsync();

//            items.Add(item);

//            return await Task.FromResult(true);
//        }

//        public async Task<bool> UpdateItemAsync(BusStop item)
//        {
//            await InitializeAsync();

//            var _item = items.Where((BusStop arg) => arg.Id == item.Id).FirstOrDefault();
//            items.Remove(_item);
//            items.Add(item);

//            return await Task.FromResult(true);
//        }

//        public async Task<bool> DeleteItemAsync(BusStop item)
//        {
//            await InitializeAsync();

//            var _item = items.Where((BusStop arg) => arg.Id == item.Id).FirstOrDefault();
//            items.Remove(_item);

//            return await Task.FromResult(true);
//        }

//        public async Task<BusStop> GetItemAsync(string id)
//        {
//            await InitializeAsync();

//            return await Task.FromResult(items.FirstOrDefault(s => s.Id == id));
//        }

//        public async Task<IEnumerable<BusStop>> GetItemsAsync(bool forceRefresh = false)
//        {
//            await InitializeAsync();

//            return await Task.FromResult(items);
//        }

//        public Task<bool> PullLatestAsync()
//        {
//            return Task.FromResult(true);
//        }


//        public Task<bool> SyncAsync()
//        {
//            return Task.FromResult(true);
//        }

//        public async Task InitializeAsync()
//        {
//            if (isInitialized)
//                return;

//            items = new List<BusStop>();
//            var _items = new List<BusStop>
//            {
//                new BusStop { Id = Guid.NewGuid().ToString(), Name = "Sliema, Sliema", Distance = 90},
//                new BusStop { Id = Guid.NewGuid().ToString(), Name = "Ferries 1, Sliema", Distance = 250},
//                new BusStop { Id = Guid.NewGuid().ToString(), Name = "Ferries 2, Sliema", Distance = 268},
//                new BusStop { Id = Guid.NewGuid().ToString(), Name = "Ferries 3, Sliema", Distance = 280},
//                new BusStop { Id = Guid.NewGuid().ToString(), Name = "Trofimu, Sliema", Distance = 500},
//                new BusStop { Id = Guid.NewGuid().ToString(), Name = "Chalet, Sliema", Distance = 1250},
//            };

//            foreach (BusStop item in _items)
//            {
//                items.Add(item);
//            }

//            isInitialized = true;
//        }
//    }
//}
