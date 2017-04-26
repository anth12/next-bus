using System;
using System.Threading;
using System.Threading.Tasks;
using NextBus.Tracing;
using NextBus.ViewModels;

using Xamarin.Forms;

namespace NextBus.Views
{
    public partial class StopDetailPage : TabbedPage
    {
        private StopDetailViewModel viewModel;
        private CancellationTokenSource timerCancellation = new CancellationTokenSource();

        // Note - The Xamarin.Forms Previewer requires a default, parameterless constructor to render a page.
        public StopDetailPage()
        {
            InitializeComponent();
        }

        public StopDetailPage(StopDetailViewModel viewModel)
        {
            InitializeComponent();

            BindingContext = this.viewModel = viewModel;
        }

        protected override void OnAppearing()
        {
            Task.Run(async () =>
            {
                while (!timerCancellation.IsCancellationRequested)
                {
                    await Task.Delay(1100);

                    if (viewModel.LastUpdated.AddSeconds(10) < DateTime.Now)
                    {
                        if (viewModel.AutoRefresh)
                            viewModel.Reload(showLoading: false);
                    }
                    else
                    {
                        // Trigger redraw on Last updated label
                        Device.BeginInvokeOnMainThread(
                            () => viewModel.LastUpdated = viewModel.LastUpdated.AddMilliseconds(1));
                    }
                }
                Trace.Write("Canceling timer");
            }, timerCancellation.Token);
            base.OnAppearing();
        }

        protected override void OnDisappearing()
        {
            timerCancellation.Cancel();
            base.OnDisappearing();
        }
    }
}
