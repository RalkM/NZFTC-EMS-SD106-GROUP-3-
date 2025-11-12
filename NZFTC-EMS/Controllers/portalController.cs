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
           var role = HttpContext.Session.GetString("Role") ?? "Employee";
        if (string.Equals(role, "Admin", StringComparison.OrdinalIgnoreCase))
            return RedirectToAction("Index", "Support_Management"); // admin page
        return Redirect("/support"); // client page
        }
    }
}