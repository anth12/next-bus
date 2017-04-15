using NextBus.Logging;
using NextBus.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;


namespace NextBus.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DevTools : TabbedPage
    {
        public DevTools()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            ((DevToolsViewModel) BindingContext).Reload();
        }

        private void LogListView_OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var item = args.SelectedItem as LogEntry;
            if (item == null)
                return;

            // TODO dedicated page
            DisplayAlert(item.Title, $@"Type:
{item.Type}
--------------
Source:
{item.Source}
--------------
Message:
{item.Message}
--------------
Time:
{item.DateUtc}
", "OK");

            LogListView.SelectedItem = null;
        }
    }
}
