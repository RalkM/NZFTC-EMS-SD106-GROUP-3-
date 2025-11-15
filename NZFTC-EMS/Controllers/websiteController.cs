using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZFTC_EMS.Data;
using NZFTC_EMS.Data.Entities;
using System.Security.Cryptography;
using System.Text;
using System.Linq;

namespace NZFTC_EMS.Controllers
{
    public class WebsiteController : Controller
    {
        private readonly AppDbContext _context;

        public WebsiteController(AppDbContext context)
        {
            _context = context;
        }

        // When someone hits /Website, send them to Authentication
        public IActionResult Index() => RedirectToAction("Authentication");

        // ============ PORTAL ============
        // URL: /Website/Portal
        public IActionResult Portal()
        {
            // use UserId as the "logged in" flag
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
                return RedirectToAction("Authentication");

            ViewData["Layout"] = "~/Views/Shared/_portal.cshtml";
            return View("~/Views/website/portal.cshtml");
        }

        // ===== PASSWORD HELPERS =====
        private void CreatePasswordHash(string password, out byte[] hash, out byte[] salt)
        {
            using var hmac = new HMACSHA256();
            salt = hmac.Key; // random key as salt
            hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        }

        private bool VerifyPassword(string password, byte[] storedHash, byte[] storedSalt)
        {
            using var hmac = new HMACSHA256(storedSalt);
            var computed = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            return computed.SequenceEqual(storedHash);
        }

        // ============ AUTHENTICATION (GET) ============
        // URL: /Website/Authentication
        [HttpGet]
        public IActionResult Authentication()
        {
            // already logged in → go to portal
            if (HttpContext.Session.GetInt32("UserId") != null)
                return RedirectToAction("Portal");

            ViewData["Layout"] = "~/Views/Shared/_auth.cshtml";
            return View("~/Views/Login/login.cshtml");   // your Login folder view
        }

        // ============ AUTHENTICATION (POST) ============ 
        // (If you want the form to post here instead of Login, you can use this.
        //  Right now we’ll keep the posting on Login to match your original.)
        //
        // If you prefer to use this instead of Login POST, change your form
        // asp-action to "Authentication" and uncomment this block.
        //
        /*
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Authentication(string email, string password)
        {
            ViewData["Layout"] = "~/Views/Shared/_auth.cshtml";

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                ModelState.AddModelError("", "Email and password are required.");
                return View("~/Views/Login/login.cshtml");
            }

            var employee = await _context.Employees
                .Include(e => e.JobPosition)
                .FirstOrDefaultAsync(e => e.Email == email);

            if (employee == null)
            {
                ModelState.AddModelError("", "Invalid email or password.");
                return View("~/Views/Login/login.cshtml");
            }

            // FIRST-TIME LOGIN: no password set yet
            if (employee.PasswordHash == null || employee.PasswordHash.Length == 0)
            {
                CreatePasswordHash(password, out var hash, out var salt);
                employee.PasswordHash = hash;
                employee.PasswordSalt = salt;
                await _context.SaveChangesAsync();

                TempData["msg"] = "Password created. Please log in again.";
                return RedirectToAction("Authentication");
            }

            // NORMAL LOGIN: verify password
            if (employee.PasswordSalt == null ||
                !VerifyPassword(password, employee.PasswordHash!, employee.PasswordSalt))
            {
                ModelState.AddModelError("", "Invalid email or password.");
                return View("~/Views/Login/login.cshtml");
            }

            // ✅ SUCCESS → set session
            HttpContext.Session.SetInt32("UserId", employee.EmployeeId);
            HttpContext.Session.SetString("UserEmail", employee.Email);
            HttpContext.Session.SetString("FirstName", employee.FirstName);
            HttpContext.Session.SetString("LastName", employee.LastName);
            HttpContext.Session.SetString("FullName", $"{employee.FirstName} {employee.LastName}");
            HttpContext.Session.SetString("Role",
                employee.JobPosition?.AccessRole ?? "Employee");

            return RedirectToAction("Portal");
        }
        */

        // ============ LOGIN (GET) ============
        [HttpGet]
        public IActionResult Login()
        {
            // use auth layout only for this page
            ViewData["Layout"] = "~/Views/Shared/_auth.cshtml";
            return View("~/Views/Login/login.cshtml");
        }

        // ============ LOGIN (POST) ============
        // This is where the form currently posts (asp-action="Login")
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string email, string password)
        {
            ViewData["Layout"] = "~/Views/Shared/_auth.cshtml";

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                ModelState.AddModelError("", "Email and password are required.");
                // ❌ FAIL → show same page with error
                return View("~/Views/Login/login.cshtml");
            }

            var employee = await _context.Employees
                .Include(e => e.JobPosition)
                .FirstOrDefaultAsync(e => e.Email == email);

            if (employee == null)
            {
                ModelState.AddModelError("", "Invalid email or password.");
                // ❌ FAIL → show same page with error
                return View("~/Views/Login/login.cshtml");
            }

            // FIRST-TIME LOGIN: no password set yet
            if (employee.PasswordHash == null || employee.PasswordHash.Length == 0)
            {
                // Save the chosen password (setup)
                CreatePasswordHash(password, out var hash, out var salt);
                employee.PasswordHash = hash;
                employee.PasswordSalt = salt;
                await _context.SaveChangesAsync();

                TempData["msg"] = "Password created. Please log in again.";
                // Clear the form and show the login page again
                return RedirectToAction("Authentication");
            }

            // NORMAL LOGIN: verify password
            if (employee.PasswordSalt == null ||
                !VerifyPassword(password, employee.PasswordHash!, employee.PasswordSalt))
            {
                ModelState.AddModelError("", "Invalid email or password.");
                // ❌ FAIL → show same page with error
                return View("~/Views/Login/login.cshtml");
            }

            // ✅ SUCCESS → set session (for Portal)
            HttpContext.Session.SetInt32("UserId", employee.EmployeeId);
            HttpContext.Session.SetString("UserEmail", employee.Email);
            HttpContext.Session.SetString("FirstName", employee.FirstName);
            HttpContext.Session.SetString("LastName", employee.LastName);
            HttpContext.Session.SetString("FullName", $"{employee.FirstName} {employee.LastName}");
            HttpContext.Session.SetString("Role",
                employee.JobPosition?.AccessRole ?? "Employee");

            // ✅ SUCCESS → go to /Website/Portal
            return RedirectToAction("Portal");
        }

        // ============ LOGOUT ============
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
