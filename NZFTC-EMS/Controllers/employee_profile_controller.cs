using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.ComponentModel.DataAnnotations;
using System.IO;
using NZFTC_EMS.Data;
using NZFTC_EMS.Data.Entities;

namespace NZFTC_EMS.Controllers
{
    public class EmployeeProfileVm
    {
        public int EmployeeId { get; set; }
        public string FullName { get; set; } = "";

        // Personal
        public DateTime? Birthday { get; set; }
        public string Gender { get; set; } = "";

        // Contact
        public string Email { get; set; } = "";
        public string Phone { get; set; } = "";
        public string Address { get; set; } = "";

        // Employment
        public string Department { get; set; } = "";
        public string JobTitle { get; set; } = "";
        public DateTime? StartDate { get; set; }
        public string PayFrequency { get; set; } = "";   // hook to DB later if needed
        public string EarningsRate { get; set; } = "";   // hook to DB later if needed

        // Emergency
        public string EmergencyName { get; set; } = "";
        public string EmergencyRelationship { get; set; } = "";
        public string EmergencyPhone { get; set; } = "";
        public string EmergencyEmail { get; set; } = "";

        public string ProfileImageUrl { get; set; } = "";
    }

    public class EmployeeProfileEditVm
    {
        [Required, EmailAddress]
        public string Email { get; set; } = "";

        [Required]
        public string Phone { get; set; } = "";

        [Required]
        public string Address { get; set; } = "";

        public string? EmergencyName { get; set; }
        public string? EmergencyRelationship { get; set; }
        public string? EmergencyPhone { get; set; }
        public string? EmergencyEmail { get; set; }

        public IFormFile? ProfilePhoto { get; set; }
    }

    [Route("employee_profile")]
    public class employee_profile_controller : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public employee_profile_controller(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // ===== helper: current employee =====
        private async Task<Employee?> GetCurrentEmployeeAsync()
        {
            // Try session username first
            var employeeName = HttpContext.Session.GetString("Username");

            if (!string.IsNullOrWhiteSpace(employeeName))
            {
                var parts = employeeName.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);
                var first = parts.Length > 0 ? parts[0] : employeeName;
                var last = parts.Length > 1 ? parts[1] : "";

                var empByName = await _context.Employees
                    .FirstOrDefaultAsync(e => e.FirstName == first && e.LastName == last);

                if (empByName != null)
                    return empByName;
            }

            // Dev fallback: first employee in table
            return await _context.Employees
                .OrderBy(e => e.EmployeeId)
                .FirstOrDefaultAsync();
        }

        // ===== VIEW PROFILE =====
        [HttpGet("profile")]
        public async Task<IActionResult> Profile()
        {
            ViewData["Layout"] = "~/Views/Shared/_portal.cshtml";

            var emp = await GetCurrentEmployeeAsync();
            if (emp == null)
                return Content("Employee not found.");

            var emergency = await _context.EmployeeEmergencyContacts
                .FirstOrDefaultAsync(ec => ec.EmployeeId == emp.EmployeeId);

            // Job title same logic as admin side
            string jobTitle = emp.JobTitle ?? "";
            if (string.IsNullOrEmpty(jobTitle) && emp.JobPositionId.HasValue)
            {
                var jp = await _context.JobPositions
                    .FirstOrDefaultAsync(j => j.JobPositionId == emp.JobPositionId.Value);
                if (jp != null)
                    jobTitle = jp.Name;
            }

            // Pay fields – left empty for now (you can wire to paygrade/payroll later)
            string payFrequency = "";
            string earningsRate = "";

            var vm = new EmployeeProfileVm
            {
                EmployeeId = emp.EmployeeId,
                FullName = $"{emp.FirstName} {emp.LastName}",

                Birthday = emp.Birthday,
                Gender = emp.Gender ?? "",

                Email = emp.Email ?? "",
                Phone = emp.Phone ?? "",
                Address = emp.Address ?? "",

                Department = emp.Department ?? "",
                JobTitle = jobTitle,
                StartDate = emp.StartDate,
                PayFrequency = payFrequency,
                EarningsRate = earningsRate,

                EmergencyName = emergency?.FullName ?? "",
                EmergencyRelationship = emergency?.Relationship ?? "",
                EmergencyPhone = emergency?.Phone ?? "",
                EmergencyEmail = emergency?.Email ?? "",

                ProfileImageUrl = $"/uploads/employees/{emp.EmployeeId}.jpg"
            };

            return View("~/Views/website/employee/profile.cshtml", vm);
        }

        // ===== EDIT PROFILE (GET) =====
        [HttpGet("edit")]
        public async Task<IActionResult> Edit()
        {
            ViewData["Layout"] = "~/Views/Shared/_portal.cshtml";

            var emp = await GetCurrentEmployeeAsync();
            if (emp == null)
                return Content("Employee not found.");

            var emergency = await _context.EmployeeEmergencyContacts
                .FirstOrDefaultAsync(ec => ec.EmployeeId == emp.EmployeeId);

            var vm = new EmployeeProfileEditVm
            {
                Email = emp.Email ?? "",
                Phone = emp.Phone ?? "",
                Address = emp.Address ?? "",
                EmergencyName = emergency?.FullName,
                EmergencyRelationship = emergency?.Relationship,
                EmergencyPhone = emergency?.Phone,
                EmergencyEmail = emergency?.Email
            };

            return View("~/Views/website/employee/profile_edit.cshtml", vm);
        }

        // ===== EDIT PROFILE (POST) =====
        [HttpPost("edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EmployeeProfileEditVm model)
        {
            ViewData["Layout"] = "~/Views/Shared/_portal.cshtml";

            var emp = await GetCurrentEmployeeAsync();
            if (emp == null)
                return Content("Employee not found.");

            if (!ModelState.IsValid)
                return View("~/Views/website/employee/profile_edit.cshtml", model);

            // allowed fields only
            emp.Email = model.Email;
            emp.Phone = model.Phone;
            emp.Address = model.Address;

            var emergency = await _context.EmployeeEmergencyContacts
                .FirstOrDefaultAsync(ec => ec.EmployeeId == emp.EmployeeId);

            if (emergency == null)
            {
                emergency = new EmployeeEmergencyContact
                {
                    EmployeeId = emp.EmployeeId
                };
                _context.EmployeeEmergencyContacts.Add(emergency);
            }

            emergency.FullName = model.EmergencyName ?? "";
            emergency.Relationship = model.EmergencyRelationship ?? "";
            emergency.Phone = model.EmergencyPhone ?? "";
            emergency.Email = model.EmergencyEmail ?? "";

            // Photo upload
            if (model.ProfilePhoto != null && model.ProfilePhoto.Length > 0)
            {
                var folder = Path.Combine(_env.WebRootPath, "uploads", "employees");
                Directory.CreateDirectory(folder);

                var filePath = Path.Combine(folder, $"{emp.EmployeeId}.jpg");
                using (var stream = System.IO.File.Create(filePath))
                {
                    await model.ProfilePhoto.CopyToAsync(stream);
                }
            }

            await _context.SaveChangesAsync();

            TempData["msg"] = "Profile updated successfully.";
            return RedirectToAction("Profile");
        }
    }
}



