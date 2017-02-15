using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace NextBus.Web.Controllers
{
    public class HomeController : Controller
    {
        private IHostingEnvironment env;

        public HomeController(IHostingEnvironment env)
        {
            this.env = env;
        }
        
        public IActionResult Index()
        {
            if(!Request.Host.Host.Contains("localhost") && !Request.IsHttps)
                return RedirectPermanent("https://" + Request.Host + Request.Path);

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
