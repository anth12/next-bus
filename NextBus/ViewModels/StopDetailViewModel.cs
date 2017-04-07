using NextBus.Models;
using NextBus.Services;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace NextBus.ViewModels
{
    public class StopDetailViewModel : BaseViewModel
    {
        public DateTime? LastUpdated { get; set; }
        public bool IsOffline { get; set; }
        public BusStop Item { get; set; }
        public ICommand ReloadCommand { get; set; }

        public StopDetailViewModel(BusStop item = null)
        {
            if (item == null)
                item = Mock.DesignTime.SingleStop;

            ReloadCommand = new Command(async()=> await Reload());
            Title = $"{item.Name}, {item.Locality}";
            Item = item;
        }

        public async Task Reload(bool showLoading = true)
        {
            if(showLoading)
                IsBusy = true;

            var service = new BusStopService();

            try
            {
                var response = await service.GetStopDetails(Item);
                if (response != null)
                {
                    LastUpdated = DateTime.Now;
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

    }
}