using NextBus.Helpers;
using NextBus.Mock;
using NextBus.Models;
using NextBus.Services;
using PropertyChanged;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace NextBus.ViewModels
{
    [ImplementPropertyChanged]
    public class StopsListViewModel : BaseViewModel
    {
        public bool LoadingStopsFromApi { get; set; }
        public ObservableRangeCollection<BusStop> AllStops { get; set; }
        public ObservableRangeCollection<BusStop> NearbyStops { get; set; }
        public ObservableRangeCollection<BusStop> FilteredStops { get; set; }
        public Command LoadItemsCommand { get; set; }
        public Command ReloadLocationCommand { get; set; }

        public string FilterText { get; set; }
        

        public StopsListViewModel()
        {
            Title = "Nearby Stops";
            AllStops = new ObservableRangeCollection<BusStop>();
            NearbyStops = new ObservableRangeCollection<BusStop>();
            FilteredStops = new ObservableRangeCollection<BusStop>();
            LoadItemsCommand = new Command(async () => await LoadItems());
            ReloadLocationCommand = new Command(async () => await ReloadLocation());

            if (Application.Current == null)
            {
                AllStops = DesignTime.AllStops;
            }

            PropertyChangedEventHanders.Add(nameof(FilterText), FilterTextChanged);
        }

        private void FilterTextChanged()
        {
            if (string.IsNullOrEmpty(FilterText))
            {
                FilteredStops.Clear();
            }
            else
            {
                var query = FilterText.ToLower();
                FilteredStops.ReplaceRange(
                    AllStops.Where(s => s.Name.ToLower().Contains(query) || s.Locality.ToLower().Contains(query)).Take(25));
            }
        }

        public async Task LoadItems()
        {
            if (IsBusy || Application.Current == null)
                return;

            IsBusy = true;
            
            try
            {
                var service = new BusStopService();

                var items = await service.GetStops(
                    ()=> LoadingStopsFromApi = true
                );
                LoadingStopsFromApi = false;
                
                AllStops.ReplaceRange(items.Stops);

                await ReloadLocation();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                // Message maybe?
                MessagingCenter.Send(new MessagingCenterAlert
                {
                    Title = "Error",
                    Message = "Unable to load items.",
                    Cancel = "OK"
                }, "message");
            }
            finally
            {
                IsBusy = false;
                UpdateStops();
            }
        }

        public async Task ReloadLocation()
        {
            IsBusy = true;

            await Task.Delay(1000);
            IsBusy = false;
            // TODO use last cached value then update async
            //var position = await IoC.GeoLocator.GetPositionAsync(timeoutMilliseconds: 5000);

            //if (position != null)
            //{
            //    var currentLocation = new Coordinates(position.Latitude, position.Longitude);
            //    // Calculate stop distances
            //    foreach (var busStop in items.Stops)
            //    {
            //        busStop.Distance = (int) busStop.Coordinates.DistanceFrom(currentLocation);
            //    }
            //}
            UpdateStops();
        }

        private void UpdateStops(bool filterOnly = false)
        {
            if (!filterOnly)
            {
                NearbyStops.ReplaceRange(AllStops.OrderBy(s => s.Distance).Take(15));
            }

            FilterTextChanged();
        }
    }
}