using Microsoft.AspNetCore.Mvc;

namespace NZFTC_EMS.Controllers
{
    public class portalController : Controller
    {
        public IActionResult index()
        {
            ViewData["Layout"] = "~/Views/Shared/_portal.cshtml"; // switch layout only for this page
            return View("~/Views/website/portal.cshtml"); // renders Views/website/Authentication.cshtml
        }
        public IActionResult Support()
        {
            return RedirectToAction("Index", "Support_Management");
        }
    }
}