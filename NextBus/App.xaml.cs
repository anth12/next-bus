using NextBus.Logging;
using NextBus.Logging.Appenders;
using NextBus.Tracing;
using NextBus.Views;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Microsoft.Azure.Mobile;
using Microsoft.Azure.Mobile.Analytics;
using Microsoft.Azure.Mobile.Crashes;

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

        protected override void OnStart()
        {
            base.OnStart();

            MobileCenter.Start("android=130a026a-d5ea-4915-9c9f-6d0a200e5c36;" + "ios=TODO",
                   typeof(Analytics), typeof(Crashes));
        }

        public static void SetMainPage()
        {
            LogHelper.Appenders.Add(new FileAppender());
#if DEBUG
            Trace.Listeners.Add(new InMemoryTraceListener());
            
#endif
            Trace.WriteLine("App Starting");

            Current.MainPage = new NavigationPage(new StopsListPage())
            {
                BarTextColor = Color.FromHex("002664"),
                BarBackgroundColor = Color.FromHex("e4eaf6")
            };
        }
    }
}
