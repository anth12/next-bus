using NextBus.Helpers;
using NextBus.Mock;
using NextBus.Models;
using NextBus.Services;
using PropertyChanged;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using NextBus.Models.Messages;
using NextBus.Tracing;
using NextBus.Utilities.Extensions;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
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
        public ObservableRangeCollection<BusStop> FavoriteStops { get; set; }

        public Command LoadItemsCommand { get; set; }
        public Command ReloadLocationCommand { get; set; }

        public string FilterText { get; set; }
        

        public StopsListViewModel()
        {
            Title = "Nearby Stops";

            AllStops = new ObservableRangeCollection<BusStop>();
            NearbyStops = new ObservableRangeCollection<BusStop>();
            FilteredStops = new ObservableRangeCollection<BusStop>();
            FavoriteStops = new ObservableRangeCollection<BusStop>();

            LoadItemsCommand = new Command(async () => await LoadItems());
            ReloadLocationCommand = new Command(async () => await ReloadLocation(true));

            if (Application.Current == null)
            {
                AllStops = DesignTime.AllStops;
            }

            PropertyChangedEventHanders.Add(nameof(FilterText), FilterTextChanged);

            MessagingCenter.Subscribe<BusStopFavorited>(this, "BusStopFavorited", BusStopFavorited);
        }

        ~StopsListViewModel()
        {
            MessagingCenter.Unsubscribe<BusStopFavorited>(this,"BusStopFavorited");
        }

        private void BusStopFavorited(BusStopFavorited e)
        {
            FavoriteStops.ReplaceRange(
                    AllStops.Where(s => s.Data.IsFavorite)
                );
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
                var items = await BusStopService.GetStops(
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

        public async Task ReloadLocation(bool showLoadingPrecise = false)
        {
            IsBusy = true;

            if (!CrossGeolocator.Current.IsGeolocationEnabled)
            {
                // TODO notify user
                IsBusy = false;
                return;
            }
            
            Position position = null;

            try
            {
                position = await CrossGeolocator.Current.GetLastKnownLocationAsync();
                SetCurrentPosition(position);

                if(position != null)
                    Trace.WriteLine($"Found cached location: {position.Latitude}, {position.Longitude}");
                else
                    Trace.WriteLine("Unable to obtain cached location");

            }
            catch(Exception ex) { }
            
            if (!showLoadingPrecise)
                IsBusy = false;

            try
            {
                position = await CrossGeolocator.Current.GetPositionAsync(TimeSpan.FromSeconds(8));
                SetCurrentPosition(position);

                if (position != null)
                    Trace.WriteLine($"Found location: {position.Latitude}, {position.Longitude}");
            }
            catch (TaskCanceledException ex)
            {
                Trace.WriteLine("Unable to obtain location");
                if (position == null)
                {
                    // TODO notify if no position obtainable
                }
            }
            finally
            {
                IsBusy = false;
            }

            IsBusy = false;
        }

        private void SetCurrentPosition(Position position)
        {
            if (position != null)
            {
                // Calculate stop distances
                foreach (var busStop in AllStops)
                {
                    busStop.Data.Distance = busStop.Position.DistanceFrom(position, PositionExtensions.UnitOfLength.Kilometers) * 1000;
                }

                UpdateStops();
            }
        }

        private void UpdateStops(bool filterOnly = false)
        {
            if (!filterOnly)
            {
                NearbyStops.ReplaceRange(
                    AllStops.OrderBy(s => s.Data.Distance).Take(15)
                );
                FavoriteStops.ReplaceRange(
                    AllStops.Where(s=> s.Data.IsFavorite)
                );
            }

            FilterTextChanged();
        }
    }
}