using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NextBus.Logging;
using NextBus.Tracing;

namespace NextBus.Helpers
{
    public class ApiHelper
    {
        private static int requestCount = 0;

        public static Task<TType> PostAsync<TType>(string apiPath, int retries = 2)
        {
            return PostAsync<TType>(apiPath, "", retries);
        }

        public static Task<TType> PostAsync<TType>(string apiPath, object data, int retries = 2)
        {
            var json = JsonConvert.SerializeObject(data);
            return PostAsync<TType>(apiPath, json, retries);
        }

        /// <summary>
        /// Invokes a Http Get to the API endpoint & deserialize's the json response
        /// </summary>
        public static async Task<TType> PostAsync<TType>(string apiPath, string payload, int retries = 2)
        {
            Trace.Write($"Http Request {requestCount++}");
            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.TryAddWithoutValidation("x-api-version", LocalSettings.Current.ApiVersion);
                    client.DefaultRequestHeaders.TryAddWithoutValidation("content-type", "application/json; charset=utf-8");

                    var content = new StringContent(payload, Encoding.UTF8, "application/json");
                    var response = await client.PostAsync(LocalSettings.Current.ApiEndpoing + apiPath, content);

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        var json = await response.Content.ReadAsStringAsync();

                        if (string.IsNullOrWhiteSpace(json))
                        {
                            return default(TType);
                        }

                        return JsonConvert.DeserializeObject<TType>(json);
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error<ApiHelper>("Error executing Http request", ex);
            }


            if (retries > 0)
            {
                // Wait & Retry
                await Task.Delay(200);
                return await PostAsync<TType>(apiPath, payload, retries - 1);
            }

            return default(TType);
        }
    }
}
