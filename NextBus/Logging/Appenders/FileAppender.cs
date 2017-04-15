using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnifiedStorage;
using Xamarin.Forms;

namespace NextBus.Logging.Appenders
{
    public class FileAppender : ILogAppender
    {
        private static object _lock = new object();

        public async Task Write(LogEntry log)
        {
            var json = JsonConvert.SerializeObject(log) + ",";

            var fileSystem = DependencyService.Get<IFileSystem>();
            var file =
                await fileSystem.LocalStorage.GetFileAsync($@"logs\{DateTime.Today.ToString("dd-MM-yy")}.log");

            using (var stream = await file.OpenAsync(FileAccessOption.ReadWrite))
            {
                stream.Seek(stream.Length, SeekOrigin.Begin);

                var buffer = Encoding.UTF8.GetBytes(json);
                await stream.WriteAsync(buffer, 0, buffer.Length);
            }
        }

        public async Task ClearAll()
        {
            var fileSystem = DependencyService.Get<IFileSystem>();
            var files = await fileSystem.LocalStorage.GetFilesAsync(@"logs\*.log");

            foreach (var file in files)
            {
                await file.DeleteAsync();
            }
        }

        public async Task<IEnumerable<LogEntry>> ReadAllAsync()
        {
            var result = new List<LogEntry>();

            var fileSystem = DependencyService.Get<IFileSystem>();
            var files = await fileSystem.LocalStorage.GetFilesAsync(@"logs\*.log");

            foreach (var file in files)
            {
                using (var stream = await file.OpenAsync(FileAccessOption.ReadOnly))
                {
                    using (var streamReader = new StreamReader(stream))
                    {
                        var json = await streamReader.ReadToEndAsync();

                        // Remove trailing comma
                        if (json.EndsWith(","))
                        {
                            json = json.Substring(0, json.Length - 1);
                        }
                        // Format the json as an array
                        json = $"[{json}]";
                        
                        result.AddRange(JsonConvert.DeserializeObject<List<LogEntry>>(json));
                    }
                }
            }

            return result;
        }
    }
}
