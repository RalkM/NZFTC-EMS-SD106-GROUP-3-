using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.ComponentModel.DataAnnotations;
using System.IO;
using NZFTC_EMS.Data;
using NZFTC_EMS.Data.Entities;
using NZFTC_EMS.Models.ViewModels;

namespace NZFTC_EMS.Controllers
{

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
  // ===== helper: current employee =====
private async Task<Employee?> GetCurrentEmployeeAsync()
{
    // Get the logged-in employee id from session (set in WebsiteController.Login)
    int? employeeId = HttpContext.Session.GetInt32("EmployeeId");
    if (employeeId == null)
        return null; // not logged in

    // Load that specific employee (include related data if you need it)
    return await _context.Employees
        .Include(e => e.PayGrade)       // uncomment if you need pay info here
        .Include(e => e.LeaveBalances)  // etc, optional
        .FirstOrDefaultAsync(e => e.EmployeeId == employeeId.Value);
}

        // ===== VIEW PROFILE =====
[HttpGet("profile")]
public async Task<IActionResult> Profile()
{
    ViewData["Layout"] = "~/Views/Shared/_portal.cshtml";

    var emp = await GetCurrentEmployeeAsync();
    if (emp == null)
        return RedirectToAction("Authentication", "Website");

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

// 🔹 Pay fields – wired to stored data
string payFrequency = emp.PayFrequency.ToString();
string earningsRate = "";

if (emp.PayGrade != null)
{
    var rate = emp.PayGrade.BaseRate;
    earningsRate = rate.ToString("C");
}
else
{
    earningsRate = "Not set";
}

string profileImageUrl;
    if (emp.PhotoBytes != null && emp.PhotoBytes.Length > 0)
    {
        var base64 = Convert.ToBase64String(emp.PhotoBytes);
        profileImageUrl = $"data:image/jpeg;base64,{base64}";
    }
    else
    {
        // fallback avatar file from wwwroot
        profileImageUrl = "/img/default-avatar.png";
    }

var vm = new EmployeeProfileVm
{
    EmployeeId = emp.EmployeeId,
    FullName   = $"{emp.FirstName} {emp.LastName}",

    Birthday   = emp.Birthday,
    Gender     = emp.Gender ?? "",

    Email      = emp.Email ?? "",
    Phone      = emp.Phone ?? "",
    Address    = emp.Address ?? "",

    Department   = emp.Department ?? "",
    JobTitle     = jobTitle,
    StartDate    = emp.StartDate,
    PayFrequency = payFrequency,
    EarningsRate = earningsRate,

    EmergencyName         = emergency?.FullName ?? "",
    EmergencyRelationship = emergency?.Relationship ?? "",
    EmergencyPhone        = emergency?.Phone ?? "",
    EmergencyEmail        = emergency?.Email ?? "",

    ProfileImageUrl = profileImageUrl
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
        return RedirectToAction("Authentication", "Website");

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
    emp.Email   = model.Email;
    emp.Phone   = model.Phone;
    emp.Address = model.Address;

    // emergency contact stuff ... (keep as is)

    // Photo upload -> store in DB
    if (model.ProfilePhoto != null && model.ProfilePhoto.Length > 0)
    {
        using var ms = new MemoryStream();
        await model.ProfilePhoto.CopyToAsync(ms);
        emp.PhotoBytes = ms.ToArray();
    }

    await _context.SaveChangesAsync();

    TempData["msg"] = "Profile updated successfully.";
    return RedirectToAction("Profile");
}

    }
}



