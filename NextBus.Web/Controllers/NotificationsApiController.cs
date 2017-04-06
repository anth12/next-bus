using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NextBus.Web.Controllers
{
    public class NotificationsApiController : Controller
    {
        public bool ObserveRouteStatus()
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create("https://tallinja.scm.azurewebsites.net/api/triggeredwebjobs/Notifications/run");
            request.Method = "POST";
            var userNamePassword = 
            var byteArray = Encoding.ASCII.GetBytes("");
            request.Headers.Add("Authorization", "Basic " + Convert.ToBase64String(byteArray));
            request.ContentLength = 0;
            try
            {
                var response = (HttpWebResponse)request.GetResponse();
            }
            catch (Exception e)
            {

            }
        }
    }
}
