using OneSignal.CSharp.SDK;
using OneSignal.CSharp.SDK.Resources;
using OneSignal.CSharp.SDK.Resources.Notifications;
using System;
using System.Collections.Generic;

namespace NextBus.App
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = new OneSignalClient("Yzg2MDViODAtNzE5OS00OTc0LTlmMmItNDhhYTYyOWVjZWE2");

            

            var options = new NotificationCreateOptions()
            {
                AppId = new Guid("d88197f0-c247-4085-a72f-a7ac7aade44a"),
                IncludedSegments = new List<string> { "All" },
                Filters = new List<INotificationFilter>
                {
                    new NotificationFilterField
                    {
                        Field = NotificationFilterFieldTypeEnum.Tag,
                        Key = "routeId",
                        Value = "2010001"
                    }
                }
            };

            options.Contents.Add(LanguageCodes.English, "Route 2020001");

            client.Notifications.Create(options);
        }
    }
}
