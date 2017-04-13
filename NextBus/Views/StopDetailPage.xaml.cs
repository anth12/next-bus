using NextBus.ViewModels;

using Xamarin.Forms;

namespace NextBus.Views
{
    public partial class StopDetailPage : TabbedPage
    {
        StopDetailViewModel viewModel;

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
    }
}
