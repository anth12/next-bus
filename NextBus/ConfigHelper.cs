using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextBus
{
    public class LocalSettings
    {
#if DEBUG
        /*
         * Development Configuration
         * Please ensure live settings are not committed to source control here
         */

        public static LocalSettingsModel Current = new LocalSettingsModel
        {
            ApiEndpoing = "https://www.publictransport.com.mt/appws",
            ApiVersion = "1.4.5"
        };
#else
/*
 * LIVE Configuration
 * This may be used in development but values should not be messed with.
 */
        public static LocalSettingsModel Current = new LocalSettingsModel
        {
            ApiEndpoing = "https://www.publictransport.com.mt/appws",
            ApiVersion = "1.4.5",
            //EnableDeveloperTools = false
        };
#endif

    }

    public class LocalSettingsModel
    {
        /// <summary>
        /// Primary App API endpoint
        /// </summary>
        public string ApiEndpoing { get; set; }

        public string ApiVersion { get; set; }
    }
}
