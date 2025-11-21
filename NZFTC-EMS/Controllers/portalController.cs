using Microsoft.AspNetCore.Mvc;

namespace NZFTC_EMS.Controllers
{
    public class portalController : Controller
    {
        public IActionResult index()
        {
             return RedirectToAction("Portal", "Website");
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