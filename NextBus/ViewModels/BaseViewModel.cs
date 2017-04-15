using NextBus.Helpers;
using System.Threading;

namespace NextBus.ViewModels
{
    public class BaseViewModel : ObservableObject
    {
        protected BaseViewModel()
        {
            Context = SynchronizationContext.Current;
        }

        private readonly SynchronizationContext Context;

        public bool IsBusy { get; set; }
        

        /// <summary>
        /// Public property to set and get the title of the BusStop
        /// </summary>
        public string Title { get; set; }
    }
}

