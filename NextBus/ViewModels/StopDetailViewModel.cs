using NextBus.Models;
using NextBus.Services;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using NextBus.Logging;
using NextBus.Models.Messages;
using Xamarin.Forms;
using System.Threading;
using NextBus.Helpers;

namespace NextBus.ViewModels
{
    public class StopDetailViewModel : BaseViewModel, IDisposable
    {
        public DateTime? LastUpdated { get; set; }
        public bool IsOffline { get; set; }
        public BusStop Item { get; set; }
        private Timer timer { get; set; }
        public ICommand ReloadCommand { get; set; }
        public ICommand FavoriteCommand { get; set; }

        public StopDetailViewModel()
        {
            if (Application.Current == null)
                Item = Mock.DesignTime.SingleStop;
        }

        public StopDetailViewModel(BusStop item)
        {
            if (item == null)
                item = Mock.DesignTime.SingleStop;

            ReloadCommand = new Command(async()=> await Reload());
            FavoriteCommand = new Command(async ()=> await Favorite());
            Title = $"{item.Name}, {item.Locality}";
            Item = item;
            
            //timer = new Timer(()=> Reload(false), 8000, 8000);
        }
        
        public void Dispose()
        {
            timer.Dispose();
        }

        public async Task Reload(bool showLoading = true)
        {
            if(showLoading)
                IsBusy = true;
            
            try
            {
                var response = await BusStopService.GetStopDetails(Item);
                if (response != null)
                {
                    LastUpdated = DateTime.Now;
                    var data = Item.Data;
                    Item = response.Stops.First(s => s.Id == Item.Id);
                    Item.Data = data;
                }
                
                IsOffline = response == null;
            }
            catch (Exception ex)
            {
                LogHelper.Error<StopsListViewModel>(ex);
                IsOffline = true;
            }

            IsBusy = false;
        }

        public async Task Favorite()
        {
            Item.Data.IsFavorite = !Item.Data.IsFavorite;

            MessagingCenter.Send(new BusStopFavorited
            {
                BusStopId = Item.Id,
                IsFavorite = Item.Data.IsFavorite
            }, "BusStopFavorited");

            await BusStopService.SaveChanges();
        }

    }
}