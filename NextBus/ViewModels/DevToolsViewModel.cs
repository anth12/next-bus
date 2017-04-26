using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using NextBus.Logging;
using Xamarin.Forms;
using NextBus.Helpers;
using NextBus.Models;
using NextBus.Services;
using NextBus.Tracing;

namespace NextBus.ViewModels
{
    public class DevToolsViewModel : BaseViewModel
    {
        public ObservableRangeCollection<LogEntry> Logs { get; set; } = new ObservableRangeCollection<LogEntry>();
        public ObservableRangeCollection<string> Traces { get; set; } = new ObservableRangeCollection<string>();

        public ICommand ReloadCommand { get; set; }
        public ICommand ClearLogsCommand { get; set; }
        public ICommand ClearTraceCommand { get; set; }
        public ICommand ClearDataCommand { get; set; }
        

        public DevToolsViewModel()
        {
            if (Application.Current == null)
            {
                Logs = Mock.DesignTime.Logs;
                Traces = Mock.DesignTime.Traces;
            }

            ReloadCommand = new Command(async()=> await Reload());
            ClearLogsCommand = new Command(async()=> await ClearLogs());
            ClearTraceCommand = new Command(async ()=> await ClearTrace());
            ClearDataCommand = new Command(async ()=> await ClearData());
            Title = "Dev Tools";
        }

        public async Task Reload()
        {
            IsBusy = true;

            try
            {
                var logs = await LogHelper.Appenders.First().ReadAllAsync();
                Logs.ReplaceRange(logs);

                var traces = Trace.Listeners.First().GetValues();
                Traces.ReplaceRange(traces);
            }
            finally
            {
                IsBusy = false;
            }
        }

        public async Task ClearLogs()
        {
            IsBusy = true;
            try
            {
                foreach (var logAppender in LogHelper.Appenders)
                {
                    await logAppender.ClearAll();
                }
            }
            finally
            {
                await Reload();
            }
        }
        public async Task ClearTrace()
        {
            IsBusy = true;
            try
            {
                foreach (var listener in Trace.Listeners)
                {
                    listener.Clear();
                }
            }
            finally
            {
                await Reload();
            }
        }

        public async Task ClearData()
        {
            IsBusy = true;
            try
            {
                await FileHelper.DeleteAsync<BusStopModelApiResponse>();
            }
            finally
            {
                IsBusy = false;
            }
        }

    }
}