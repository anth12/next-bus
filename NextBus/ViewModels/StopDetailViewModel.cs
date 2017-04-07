using NextBus.Models;

namespace NextBus.ViewModels
{
    public class StopDetailViewModel : BaseViewModel
    {
        public BusStop Item { get; set; }
        public StopDetailViewModel(BusStop item = null)
        {
            if (item == null)
                item = Mock.DesignTime.SingleStop;

            Title = $"{item.Name}, {item.Locality}";
            Item = item;
        }

        int quantity = 1;
        public int Quantity
        {
            get { return quantity; }
            set { SetProperty(ref quantity, value); }
        }
    }
}