using NextBus.Helpers;
using NextBus.Models;

namespace NextBus.Mock
{
    class DesignTime
    {
        public static ObservableRangeCollection<BusStop> AllStops=> new ObservableRangeCollection<BusStop>
        {
            new BusStop { Name = "Sliema", Locality = "Sliema", Distance = 89 },
            new BusStop { Name = "Ferries 1", Locality = "Sliema", Distance = 398 },
            new BusStop { Name = "Ferries 3", Locality = "Sliema", Distance = 450 },
            new BusStop { Name = "Ferries 2", Locality = "Sliema", Distance = 500 },
            new BusStop { Name = "Trofimu", Locality = "Sliema", Distance = 900 },
            new BusStop { Name = "Mrabat", Locality = "St Julians", Distance = 1800 },
            new BusStop { Name = "Paris", Locality = "Birkirkara", Distance = 3000 },
            new BusStop { Name = "Free Port", Locality = "Birzibuga", Distance = 8000 },
        };
    }
}
