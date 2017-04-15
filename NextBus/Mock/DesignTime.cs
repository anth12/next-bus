using NextBus.Helpers;
using NextBus.Models;
using System.Linq;
using NextBus.Logging;

namespace NextBus.Mock
{
    class DesignTime
    {
        public static BusStop SingleStop => AllStops.First();

        public static ObservableRangeCollection<BusStop> AllStops=> new ObservableRangeCollection<BusStop>
        {
            new BusStop { Name = "Sliema", Locality = "Sliema", Routes = Routes, Data = {  Distance = 89 }},
            new BusStop { Name = "Ferries 1", Locality = "Sliema", Data = { Distance = 398 }},
            new BusStop { Name = "Ferries 3", Locality = "Sliema", Data = {  Distance = 450 }},
            new BusStop { Name = "Ferries 2", Locality = "Sliema", Data = {  Distance = 500 }},
            new BusStop { Name = "Trofimu", Locality = "Sliema", Data = {  Distance = 900 }},
            new BusStop { Name = "Mrabat", Locality = "St Julians", Data = {  Distance = 1800 }},
            new BusStop { Name = "Paris", Locality = "Birkirkara", Data = {  Distance = 3000 }},
            new BusStop { Name = "Free Port", Locality = "Birzibuga", Data = {  Distance = 8000 }},
        };

        public static ObservableRangeCollection<Route> Routes => new ObservableRangeCollection<Route>
        {
            new Route { ArrivesAt = "3m", Destination = "Valletta", Id = "001", Name = "13"},
            new Route { ArrivesAt = "7m", Destination = "Valletta", Id = "001", Name = "14"},
            new Route { ArrivesAt = "12m", Destination = "Valletta", Id = "001", Name = "15"},
            new Route { ArrivesAt = "18m", Destination = "Valletta", Id = "001", Name = "13A"},
            new Route { ArrivesAt = "30+", Destination = "Valletta", Id = "001", Name = "225"},
        };

        public static ObservableRangeCollection<LogEntry> Logs => new ObservableRangeCollection<LogEntry>
        {
            new LogEntry { Type = LogType.Info, Title = "Application Starting", Message = ""},
            new LogEntry { Type = LogType.Warn, Title = "Application Offline", Message = "Remote server not reachable"},
            new LogEntry { Type = LogType.Info, Title = "Application Loaded", Message = ""},
            new LogEntry { Type = LogType.Error, Title = "Some random error", Message = ""},
        };

        public static ObservableRangeCollection<string> Traces => new ObservableRangeCollection<string>
        {
            "Application Starting",
            "Api timeout- first attempt",
            "Api timeout- second attempt",
            "Api timeout- third attempt",
            "Api unreachable",
            "View opened"
        };
    }
}
