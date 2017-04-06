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
        }
        
        protected override void OnAppearing()
        {
            base.OnAppearing();

            var viewModel = (StopsListViewModel) BindingContext;

            if (viewModel.NearbyStops.Count == 0)
                viewModel.LoadItemsCommand.Execute(null);
        }
        
    }
}
