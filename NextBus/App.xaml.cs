using NextBus.Views;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace NextBus
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            SetMainPage();
        }

        public static void SetMainPage()
        {
            Current.MainPage = new NavigationPage(new StopsListPage())
            {
                BarTextColor = Color.FromHex("002664"),
                BarBackgroundColor = Color.FromHex("e4eaf6")
            };
        }
    }
}
