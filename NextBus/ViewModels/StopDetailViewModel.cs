using NextBus.Models;
using NextBus.Services;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using NextBus.Converters;
using NextBus.Logging;
using NextBus.Models.Messages;
using Xamarin.Forms;
using NextBus.Helpers;
using NextBus.Tracing;

namespace NextBus.ViewModels
{
    public class StopDetailViewModel : BaseViewModel
    {
        public bool AutoRefresh { get; set; } = true;

        public DateTime LastUpdated { get; set; } = DateTime.MinValue;

        public FormattedString LastUpdatedText => new FormattedString
        {
            Spans =
            {
                new Span { Text = "Last Updated: "},
                new Span { Text = TimeConverter.Convert(LastUpdated)}
            }
        };

        public bool IsOffline { get; set; }
        public BusStop Item { get; set; }
        public ObservableRangeCollection<Route> LiveRoutes { get; set; } = new ObservableRangeCollection<Route>();
        
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
        }

        public async Task Reload(bool showLoading = true)
        {
            if(showLoading)
                IsBusy = true;

            Trace.Write("Reloading stop data");
            try
            {
                var response = await BusStopService.GetStopDetails(Item);
                if (response != null)
                {
                    LastUpdated = DateTime.Now;
                    
                    LiveRoutes.ReplaceRange(response.Stops.First(s => s.Id == Item.Id).Routes);
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