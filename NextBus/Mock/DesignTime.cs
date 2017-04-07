using NextBus.Helpers;
using NextBus.Models;
using System.Linq;

namespace NextBus.Mock
{
    class DesignTime
    {
        public static BusStop SingleStop => AllStops.First();

        public static ObservableRangeCollection<BusStop> AllStops=> new ObservableRangeCollection<BusStop>
        {
            new BusStop { Name = "Sliema", Locality = "Sliema", Distance = 89, Routes = Routes},
            new BusStop { Name = "Ferries 1", Locality = "Sliema", Distance = 398 },
            new BusStop { Name = "Ferries 3", Locality = "Sliema", Distance = 450 },
            new BusStop { Name = "Ferries 2", Locality = "Sliema", Distance = 500 },
            new BusStop { Name = "Trofimu", Locality = "Sliema", Distance = 900 },
            new BusStop { Name = "Mrabat", Locality = "St Julians", Distance = 1800 },
            new BusStop { Name = "Paris", Locality = "Birkirkara", Distance = 3000 },
            new BusStop { Name = "Free Port", Locality = "Birzibuga", Distance = 8000 },
        };

        public static ObservableRangeCollection<Route> Routes => new ObservableRangeCollection<Route>
        {
            new Route { ArrivesAt = "3m", Destination = "Valletta", Id = "001", Name = "13"},
            new Route { ArrivesAt = "7m", Destination = "Valletta", Id = "001", Name = "14"},
            new Route { ArrivesAt = "12m", Destination = "Valletta", Id = "001", Name = "15"},
            new Route { ArrivesAt = "18m", Destination = "Valletta", Id = "001", Name = "13A"},
            new Route { ArrivesAt = "30+", Destination = "Valletta", Id = "001", Name = "225"},
        };
    }
}
