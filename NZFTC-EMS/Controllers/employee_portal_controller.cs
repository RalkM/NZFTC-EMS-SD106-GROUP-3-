using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZFTC_EMS.Data;
using NZFTC_EMS.Models;

namespace NZFTC_EMS.Controllers
{
    [Route("employee_portal")]
    public class employee_portal_controller : Controller
    {
        private readonly AppDbContext _db;
        private readonly IWebHostEnvironment _env;

        public employee_portal_controller(AppDbContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }

        // --- Helper: get or create the current employee row ---
        private async Task<employee_model?> GetOrCreateCurrentAsync()
        {
            var role = HttpContext.Session.GetString("Role");
            var name = HttpContext.Session.GetString("Username");
            var email = HttpContext.Session.GetString("UserEmail");

            if (string.IsNullOrEmpty(role) || role != "Employee")
                return null; // you can redirect to AccessDenied if you prefer

            // try to find by email first, then by name
            var emp = await _db.Employees
                .FirstOrDefaultAsync(e =>
                    (!string.IsNullOrEmpty(email) && e.email == email) ||
                    (!string.IsNullOrEmpty(name) && e.full_name == name));

            if (emp != null) return emp;

            // nothing found – create a minimal record so Profile works
            emp = new employee_model
            {
                full_name = name ?? "Employee",
                email = email ?? $"user{Guid.NewGuid():N}@example.local",
                role = "Employee",
                department = "Unassigned",
                start_date = DateTime.Today
            };
            _db.Employees.Add(emp);
            await _db.SaveChangesAsync();
            return emp;
        }

        // GET /employee_portal/profile
        [HttpGet("profile")]
        public async Task<IActionResult> Profile()
        {
            var emp = await GetOrCreateCurrentAsync();
            if (emp == null) return RedirectToAction("AccessDenied", "website");

            ViewData["Layout"] = "~/Views/Shared/_portal.cshtml";   // employee left-nav
            return View("~/Views/website/employee/profile.cshtml", emp);
        }

        // POST /employee_portal/update/{id}
        [HttpPost("update/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, employee_model form)
        {
            var emp = await _db.Employees.FindAsync(id);
            if (emp == null) return NotFound();

            // map editable fields from the profile form
            emp.birthdate = form.birthdate;
            emp.gender = form.gender;

            emp.job_title = form.job_title;
            emp.pay_frequency = form.pay_frequency;
            emp.start_date = form.start_date;

            emp.address = form.address;
            emp.phone = form.phone;

            emp.emergency_contact_name = form.emergency_contact_name;
            emp.emergency_contact_relationship = form.emergency_contact_relationship;
            emp.emergency_contact_phone = form.emergency_contact_phone;
            emp.emergency_contact_email = form.emergency_contact_email;

            await _db.SaveChangesAsync();
            TempData["msg"] = "Profile updated.";
            return RedirectToAction("Profile");
        }

        // POST /employee_portal/upload/{id}
        [HttpPost("upload/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upload(int id, IFormFile? photo)
        {
            var emp = await _db.Employees.FindAsync(id);
            if (emp == null) return NotFound();

            if (photo != null && photo.Length > 0)
            {
                var uploads = Path.Combine(_env.WebRootPath, "uploads");
                if (!Directory.Exists(uploads)) Directory.CreateDirectory(uploads);

                var fname = $"{Guid.NewGuid():N}{Path.GetExtension(photo.FileName)}";
                var full = Path.Combine(uploads, fname);

                using (var fs = new FileStream(full, FileMode.Create))
                    await photo.CopyToAsync(fs);

                emp.photo_path = $"/uploads/{fname}";
                await _db.SaveChangesAsync();
                TempData["msg"] = "Photo updated.";
            }

            return RedirectToAction("Profile");
        }
    }
}
