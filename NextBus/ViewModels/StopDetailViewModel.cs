using NextBus.Models;
using NextBus.Services;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Input;
using NextBus.Models.Messages;
using Xamarin.Forms;

namespace NextBus.ViewModels
{
    public class StopDetailViewModel : BaseViewModel
    {
        public DateTime? LastUpdated { get; set; }
        public bool IsOffline { get; set; }
        public BusStop Item { get; set; }
        public ICommand ReloadCommand { get; set; }
        public ICommand FavoriteCommand { get; set; }

        public StopDetailViewModel() : this(null)
        { }

        public StopDetailViewModel(BusStop item)
        {
            if (item == null)
                item = Mock.DesignTime.SingleStop;

            ReloadCommand = new Command(async()=> await Reload());
            FavoriteCommand = new Command(async ()=> await Favorite());
            Title = $"{item.Name}, {item.Locality}";
            Item = item;
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
                    // TODO merge
                    Item = response.Stops.First(s => s.Id == Item.Id);
                }
                
                IsOffline = response == null;
            }
            catch (Exception ex)
            {
                IsOffline = true;
            }

            IsBusy = false;
        }

        public async Task Favorite()
        {
            Item.IsFavorite = !Item.IsFavorite;

            MessagingCenter.Send(new BusStopFavorited
            {
                BusStopId = Item.Id,
                IsFavorite = Item.IsFavorite
            }, "BusStopFavorited");

            await BusStopService.SaveChanges();
        }

    }
}