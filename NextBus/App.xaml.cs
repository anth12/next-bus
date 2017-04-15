using System;
using System.Linq;
using NextBus.Logging;
using NextBus.Logging.Appenders;
using NextBus.Tracing;
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
