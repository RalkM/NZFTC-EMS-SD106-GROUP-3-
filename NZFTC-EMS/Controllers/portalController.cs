using Microsoft.AspNetCore.Mvc;

namespace NZFTC_EMS.Controllers
{
    public class portalController : Controller
    {
           public IActionResult Support()
        {
            ViewData["Layout"] = "~/Views/Shared/_portal.cshtml"; // switch layout only for this page
            return View("~/Views/website/admin/support_management.cshtml"); // renders Views/website/Authentication.cshtml
        }
    }
}