using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZFTC_EMS.Data;
using NZFTC_EMS.Data.Entities;

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

        // Helper: find or create the current employee row
        private async Task<Employee?> GetOrCreateCurrentAsync()
        {
            var role = HttpContext.Session.GetString("Role");
            var name = HttpContext.Session.GetString("Username");
            var email = HttpContext.Session.GetString("UserEmail");

            if (string.IsNullOrEmpty(role) || role != "Employee")
                return null;

            var emp = await _db.Employees
                .Include(e => e.PayGrade)
                .Include(e => e.JobPosition)
                .FirstOrDefaultAsync(e =>
                    (!string.IsNullOrEmpty(email) && e.Email == email) ||
                    (!string.IsNullOrEmpty(name) &&
                     (e.FirstName + " " + e.LastName) == name));

            if (emp != null) return emp;

            var firstName = "Employee";
            var lastName = "";

            if (!string.IsNullOrWhiteSpace(name))
            {
                var parts = name.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);
                firstName = parts[0];
                if (parts.Length > 1) lastName = parts[1];
            }

            emp = new Employee
            {
                FirstName = firstName,
                LastName = lastName,
                Email = !string.IsNullOrEmpty(email)
                            ? email
                            : $"user{Guid.NewGuid():N}@example.local",
                Role = "Employee",
                StartDate = DateTime.Today
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
            if (emp == null)
                return RedirectToAction("AccessDenied", "website");

            ViewData["Layout"] = "~/Views/Shared/_portal.cshtml";

            return View(
                "~/Views/website/portal/profile.cshtml",
                emp
            );
        }

        // POST /employee_portal/update/{id}
        [HttpPost("update/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, Employee form)
        {
            var emp = await GetOrCreateCurrentAsync();
            if (emp == null || emp.EmployeeId != id)
                return RedirectToAction("AccessDenied", "website");

            emp.Birthday = form.Birthday;
            emp.Gender = form.Gender;

            emp.JobTitle = form.JobTitle;
            emp.PayFrequency = form.PayFrequency;
            emp.StartDate = form.StartDate;

            emp.Address = form.Address;
            emp.Phone = form.Phone;

            emp.EmergencyContactName = form.EmergencyContactName;
            emp.EmergencyContactRelationship = form.EmergencyContactRelationship;
            emp.EmergencyContactPhone = form.EmergencyContactPhone;
            emp.EmergencyContactEmail = form.EmergencyContactEmail;

            // not touching Role, Email, JobPositionId, PayGradeId here

            await _db.SaveChangesAsync();
            TempData["msg"] = "Profile updated.";
            return RedirectToAction("Profile");
        }

        // POST /employee_portal/upload/{id}
        [HttpPost("upload/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upload(int id, IFormFile? photo)
        {
            var emp = await GetOrCreateCurrentAsync();
            if (emp == null || emp.EmployeeId != id)
                return RedirectToAction("AccessDenied", "website");

            if (photo != null && photo.Length > 0)
            {
                var uploads = Path.Combine(_env.WebRootPath, "uploads");
                if (!Directory.Exists(uploads))
                    Directory.CreateDirectory(uploads);

                var fname = $"{Guid.NewGuid():N}{Path.GetExtension(photo.FileName)}";
                var full = Path.Combine(uploads, fname);

                using (var fs = new FileStream(full, FileMode.Create))
                    await photo.CopyToAsync(fs);

                emp.PhotoPath = $"/uploads/{fname}";
                await _db.SaveChangesAsync();
                TempData["msg"] = "Photo updated.";
            }

            return RedirectToAction("Profile");
        }
    }
}
