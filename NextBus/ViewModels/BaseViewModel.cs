using NextBus.Helpers;

namespace NextBus.ViewModels
{
    public class BaseViewModel : ObservableObject
    {
        
        public bool IsBusy { get; set; }
        

        /// <summary>
        /// Public property to set and get the title of the BusStop
        /// </summary>
        public string Title { get; set; }
    }
}

