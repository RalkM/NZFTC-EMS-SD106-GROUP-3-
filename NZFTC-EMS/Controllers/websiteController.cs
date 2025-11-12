using Microsoft.AspNetCore.Mvc;

namespace NZFTC_EMS.Controllers
{
    public class WebsiteController : Controller
    {
        public IActionResult Index() => View(); // uses _Layout.cshtml

        public IActionResult Authentication()
        {
            ViewData["Layout"] = "~/Views/Shared/_auth.cshtml"; // switch layout only for this page
            return View("~/Views/Login/login.cshtml"); // renders Views/website/Authentication.cshtml
        }

        [HttpGet]
        public IActionResult Login()
        {
            ViewData["Layout"] = "~/Views/Shared/_auth.cshtml"; // switch layout only for this page
            return View("~/Views/Login/login.cshtml"); // renders Views/website/Authentication.cshtml
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(string username, string password)
        {
            if (!string.IsNullOrWhiteSpace(username) && !string.IsNullOrWhiteSpace(password))
            {
                // TEMP: decide role. Replace with real DB/Identity later.
                var role = username.Equals("admin", StringComparison.OrdinalIgnoreCase) ? "Admin" : "Employee";
                HttpContext.Session.SetString("UserName", username);
                HttpContext.Session.SetString("Role", role);
                return RedirectToAction("Portal");
            }

            ViewData["Layout"] = "~/Views/Shared/_auth.cshtml";
            ModelState.AddModelError(string.Empty, "Invalid username or password.");
            return View();


        }

        public IActionResult Portal()
        {
            var user = HttpContext.Session.GetString("UserName");
            if (string.IsNullOrEmpty(user))
                return RedirectToAction("Login");

            ViewData["Layout"] = "~/Views/Shared/_portal.cshtml"; // <- different layout
            return View("~/Views/website/portal.cshtml"); // Views/website/Portal.cshtml (or return View("~/Views/SomeFolder/Portal.cshtml"))
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }




    }
}
