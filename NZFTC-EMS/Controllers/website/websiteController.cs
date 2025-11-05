using Microsoft.AspNetCore.Mvc;

namespace NZFTC_EMS.Controllers
{
    public class websiteController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
