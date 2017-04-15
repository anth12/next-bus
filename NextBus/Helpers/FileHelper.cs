using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NextBus.Logging;
using NextBus.Tracing;
using UnifiedStorage;
using Xamarin.Forms;

namespace NextBus.Helpers
{
    internal static class FileHelper
    {
        public static async Task PersistAsync<TData>(TData data)
        {
            var fileName = typeof (TData).Name + ".json";

            Trace.WriteLine($"Persisting {fileName}");
            try
            {
                var json = JsonConvert.SerializeObject(data);

                var fileSystem = DependencyService.Get<IFileSystem>();
                var file = await fileSystem.LocalStorage.GetFileAsync(fileName);

                using (var stream = await file.OpenAsync(FileAccessOption.ReadWrite))
                {
                    var buffer = Encoding.UTF8.GetBytes(json);
                    await stream.WriteAsync(buffer, 0, buffer.Length);
                    stream.SetLength(buffer.Length);
                    //using (var streamWriter = new StreamWriter(stream))
                    //{
                    //    await streamWriter.WriteAsync(buffer);
                    //    stream.SetLength(j);
                    //}
                }
                
                Trace.WriteLine($"Persisted {fileName}");
            }
            catch (Exception ex)
            {
                Trace.WriteLine($"Persistence error {fileName}");
                LogHelper.Error($"Error persisting {fileName}", ex);
            }
        }

        public static async Task<TData> LoadAsync<TData>()
            where TData : class
        {
            var fileName = typeof(TData).Name + ".json";

            try
            {
                var fileSystem = DependencyService.Get<IFileSystem>();


                var file = await fileSystem.LocalStorage.GetFileAsync(fileName);

                if (file != null)
                {
                    var exists = await file.ExistsAsync();

                    if (exists == false)
                    {
                        Trace.WriteLine($"File does not exist: {fileName}");
                        return null;
                    }

                    using (var stream = await file.OpenAsync(FileAccessOption.ReadOnly))
                    {
                        using (var streamReader = new StreamReader(stream))
                        {
                            var json = streamReader.ReadToEnd();

                            if (!string.IsNullOrEmpty(json))
                            {
                                return JsonConvert.DeserializeObject<TData>(json);
                            }
                        }

                    }
                }

            }
            catch (Exception ex)
            {
                LogHelper.Error<TData>($"Error loading {fileName} from disk", ex);
            }

            return null;
        }
    }
}
