using System;
using NextBus.Models;
using NextBus.ViewModels;
using Xamarin.Forms;

namespace NextBus.Views
{
    public partial class StopsListPage : TabbedPage
    {
        public StopsListPage()
        {
            InitializeComponent();

            CurrentPageChanged += OnCurrentPageChanged;
        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var item = args.SelectedItem as BusStop;
            if (item == null)
                return;

            await Navigation.PushAsync(new StopDetailPage(new StopDetailViewModel(item)));

            // Manually deselect BusStop
            NearbyListView.SelectedItem = null;
            SearchListView.SelectedItem = null;
            FavoritesListView.SelectedItem = null;
        }
        
        protected override void OnAppearing()
        {
            base.OnAppearing();

            var viewModel = (StopsListViewModel) BindingContext;

            if (viewModel.NearbyStops.Count == 0)
                viewModel.LoadItemsCommand.Execute(null);
        }


        private DateTime lastClick = DateTime.Today;
        private int devToolsClicks = 0;

        private void OnCurrentPageChanged(object sender, EventArgs eventArgs)
        {
            if (lastClick > DateTime.Now.AddMilliseconds(-1000) && Children.IndexOf(CurrentPage) != 1)
            {
                devToolsClicks++;
#if DEBUG
                if (devToolsClicks > 2)
#else
                if (devToolsClicks > 4)
#endif
                {
                    Navigation.PushAsync(new DevTools());
                }
            }
            else
            {
                devToolsClicks = 0;
            }


            lastClick = DateTime.Now;
        }
    }
}
