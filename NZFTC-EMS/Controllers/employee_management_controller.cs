using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NZFTC_EMS.Data;
using NZFTC_EMS.Data.Entities;
using System.Security.Cryptography;

namespace NZFTC_EMS.Controllers
{
    [Route("employee_management")]
    public class employee_management_controller : Controller
    {
        private readonly AppDbContext _context;

        public employee_management_controller(AppDbContext context)
        {
            _context = context;
        }

        private bool IsAdmin() => HttpContext.Session.GetString("Role") == "Admin";

        // ===========================
        // LIST
        // ===========================
        [HttpGet("")]
        [HttpGet("index")]
        public async Task<IActionResult> Index(string? q, string? department)
        {
            if (!IsAdmin())
                return RedirectToAction("AccessDenied", "website");

            ViewData["Layout"] = "~/Views/Shared/_portal.cshtml";

            var query = _context.Employees
                .Include(e => e.PayGrade)
                .Include(e => e.JobPosition)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(q))
            {
                query = query.Where(e =>
                    e.FirstName.Contains(q) ||
                    e.LastName.Contains(q) ||
                    e.Email.Contains(q));
            }

            if (!string.IsNullOrWhiteSpace(department) && department != "All")
            {
                query = query.Where(e =>
                    e.JobPosition != null &&
                    e.JobPosition.Department == department);
            }

            var employees = await query.ToListAsync();

            // dept filter options from JobPositions
            var departments = await _context.JobPositions
                .Where(j => j.IsActive && j.Department != null)
                .Select(j => j.Department!)
                .Distinct()
                .OrderBy(d => d)
                .ToListAsync();

            ViewBag.DepartmentOptions = departments;
            ViewBag.Search = q;
            ViewBag.Department = department ?? "All";

            return View(
                "~/Views/website/admin/employee_management.cshtml",
                employees
            );
        }

        // ===========================
        // DETAILS
        // ===========================
        [HttpGet("details/{id}")]
public async Task<IActionResult> Details(int id)
{
    if (!IsAdmin())
        return RedirectToAction("AccessDenied", "website");

    ViewData["Layout"] = "~/Views/Shared/_portal.cshtml";

    var emp = await _context.Employees
        .Include(e => e.PayGrade)
        .Include(e => e.JobPosition)
        .Include(e => e.EmergencyContacts)
        .FirstOrDefaultAsync(e => e.EmployeeId == id);

    if (emp == null) return NotFound();

    // map nav data into NotMapped helpers (optional, if your view uses them)
    if (emp.JobPosition != null)
    {
        emp.JobTitle   ??= emp.JobPosition.Name;
        emp.Department ??= emp.JobPosition.Department;
        emp.Role       ??= emp.JobPosition.AccessRole;
    }
    if (emp.PayGrade != null && string.IsNullOrEmpty(emp.PayFrequency))
    {
        emp.PayFrequency = emp.PayGrade.RateType.ToString();
    }

    // primary emergency contact into helpers (if view uses them)
    var contact = emp.EmergencyContacts.FirstOrDefault();
    if (contact != null)
    {
        emp.EmergencyContactName         = contact.FullName;
        emp.EmergencyContactRelationship = contact.Relationship;
        emp.EmergencyContactPhone        = contact.Phone;
        emp.EmergencyContactEmail        = contact.Email;
    }

    return View(
        "~/Views/website/admin/employee_details.cshtml",
        emp
    );
}


        // CREATE (GET)
[HttpGet("create")]
public async Task<IActionResult> Create()
{
    if (!IsAdmin())
        return RedirectToAction("AccessDenied", "website");

    ViewData["Layout"] = "~/Views/Shared/_portal.cshtml";

    var model = new Employee
    {
        StartDate = DateTime.UtcNow   // default date
    };

    await BuildEmployeeFormDataAsync(model);

    return View(
        "~/Views/website/admin/employee_create.cshtml",
        model
    );
}

// CREATE (POST)
[HttpPost("create")]
[ValidateAntiForgeryToken]
public async Task<IActionResult> Create(Employee model)
{
    if (!IsAdmin())
        return RedirectToAction("AccessDenied", "website");

    if (!ModelState.IsValid)
    {
        ViewData["Layout"] = "~/Views/Shared/_portal.cshtml";
        await BuildEmployeeFormDataAsync(model);
        return View(
            "~/Views/website/admin/employee_create.cshtml",
            model
        );
    }

    await ApplyJobPositionLogicAsync(model);

    // optional primary emergency contact
    var hasContactData =
    !string.IsNullOrWhiteSpace(model.EmergencyContactName) ||
    !string.IsNullOrWhiteSpace(model.EmergencyContactPhone) ||
    !string.IsNullOrWhiteSpace(model.EmergencyContactEmail);

    if (hasContactData)
    {
        model.EmergencyContacts.Add(new EmployeeEmergencyContact
        {
            FullName     = model.EmergencyContactName ?? string.Empty,
            Relationship = model.EmergencyContactRelationship,
            Phone        = model.EmergencyContactPhone,
            Email        = model.EmergencyContactEmail
        });
    }

    // 🔹 generate random unique NZFTCxxxxxx code
    model.EmployeeCode = await GenerateUniqueEmployeeCodeAsync();

    _context.Employees.Add(model);
    await _context.SaveChangesAsync();

    TempData["msg"] = "Employee created successfully.";
    return RedirectToAction("Index");
    }


        // ===========================
        // EDIT (GET)
        // ===========================
        [HttpGet("edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            if (!IsAdmin())
                return RedirectToAction("AccessDenied", "website");

            ViewData["Layout"] = "~/Views/Shared/_portal.cshtml";

            var emp = await _context.Employees
            .Include(e => e.JobPosition)
            .Include(e => e.PayGrade)
            .Include(e => e.EmergencyContacts)
            .FirstOrDefaultAsync(e => e.EmployeeId == id);
            if (emp == null) return NotFound();

            // map primary emergency contact into NotMapped fields
            var contact = emp.EmergencyContacts.FirstOrDefault();
            if (contact != null)
            {
                emp.EmergencyContactName         = contact.FullName;
                emp.EmergencyContactRelationship = contact.Relationship;
                emp.EmergencyContactPhone        = contact.Phone;
                emp.EmergencyContactEmail        = contact.Email;
            }


            await BuildEmployeeFormDataAsync(emp);

            return View("~/Views/website/admin/employee_edit.cshtml", emp);
        }

        // ===========================
        // EDIT (POST)
        // ===========================
        [HttpPost("edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Employee model)
        {
            if (!IsAdmin())
                return RedirectToAction("AccessDenied", "website");

            if (id != model.EmployeeId)
                return BadRequest();

            if (!ModelState.IsValid)
            {
                ViewData["Layout"] = "~/Views/Shared/_portal.cshtml";
                await BuildEmployeeFormDataAsync(model);
                return View("~/Views/website/admin/employee_edit.cshtml", model);
            }

            var existingEmployee = await _context.Employees
                .Include(e => e.EmergencyContacts)
                .FirstOrDefaultAsync(e => e.EmployeeId == id);

            if (existingEmployee == null)
                return NotFound();

            // PERSONAL DETAILS
            existingEmployee.FirstName = model.FirstName;
            existingEmployee.LastName  = model.LastName;
            existingEmployee.Email     = model.Email;
            existingEmployee.Phone     = model.Phone;
            existingEmployee.Address   = model.Address;
            existingEmployee.Birthday  = model.Birthday;
            existingEmployee.Gender    = model.Gender;

            // JOB DETAILS (NotMapped helpers + stored StartDate)
            existingEmployee.Department   = model.Department;
            existingEmployee.JobTitle     = model.JobTitle;
            existingEmployee.PayFrequency = model.PayFrequency;
            existingEmployee.StartDate    = model.StartDate;

            // EMERGENCY CONTACT -> related table
            var hasContactData =
                !string.IsNullOrWhiteSpace(model.EmergencyContactName) ||
                !string.IsNullOrWhiteSpace(model.EmergencyContactPhone) ||
                !string.IsNullOrWhiteSpace(model.EmergencyContactEmail);

            var existingContact = existingEmployee.EmergencyContacts.FirstOrDefault();

            if (!hasContactData)
            {
                // user cleared the form → delete existing contact
                if (existingContact != null)
                    _context.EmployeeEmergencyContacts.Remove(existingContact);
            }
            else
            {
                if (existingContact == null)
                {
                    existingContact = new EmployeeEmergencyContact
                    {
                        EmployeeId = existingEmployee.EmployeeId
                    };
                    existingEmployee.EmergencyContacts.Add(existingContact);
                }

                existingContact.FullName     = model.EmergencyContactName ?? string.Empty;
                existingContact.Relationship = model.EmergencyContactRelationship;
                existingContact.Phone        = model.EmergencyContactPhone;
                existingContact.Email        = model.EmergencyContactEmail;
            }

            // set JobPositionId / PayGradeId / Role based on Department + JobTitle
            await ApplyJobPositionLogicAsync(existingEmployee);

            _context.Update(existingEmployee);
            await _context.SaveChangesAsync();

            TempData["msg"] = "Employee record updated successfully.";
            return RedirectToAction("Index");
        }


        // ===========================
        // DELETE (GET)
        // ===========================
        [HttpGet("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (!IsAdmin())
                return RedirectToAction("AccessDenied", "website");

            ViewData["Layout"] = "~/Views/Shared/_portal.cshtml";

            var emp = await _context.Employees
                .Include(e => e.JobPosition)
                .Include(e => e.PayGrade)
                .FirstOrDefaultAsync(e => e.EmployeeId == id);

            if (emp == null) return NotFound();

            return View(
                "~/Views/website/admin/employee_delete.cshtml",
                emp
            );
        }

        // ===========================
        // DELETE (POST)
        // ===========================
        [HttpPost("DeleteConfirmed/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (!IsAdmin())
                return RedirectToAction("AccessDenied", "website");

            // Load employee + all dependent rows we want to clean up
            var emp = await _context.Employees
                .Include(e => e.PayrollSummaries)
                .Include(e => e.LeaveRequests)
                .Include(e => e.EmergencyContacts)
                .Include(e => e.LeaveBalances)
                .FirstOrDefaultAsync(e => e.EmployeeId == id);

            if (emp == null)
            {
                TempData["msg"] = "Employee not found.";
                return RedirectToAction("Index");
            }

            // Remove dependents first so FK RESTRICT doesn't block us
            if (emp.PayrollSummaries.Any())
                _context.EmployeePayrollSummaries.RemoveRange(emp.PayrollSummaries);

            if (emp.LeaveRequests.Any())
                _context.LeaveRequests.RemoveRange(emp.LeaveRequests);

            if (emp.EmergencyContacts.Any())
                _context.EmployeeEmergencyContacts.RemoveRange(emp.EmergencyContacts);

            if (emp.LeaveBalances.Any())
                _context.EmployeeLeaveBalances.RemoveRange(emp.LeaveBalances);

            // Now remove the employee
            _context.Employees.Remove(emp);

            await _context.SaveChangesAsync();

            TempData["msg"] = "Employee and related records deleted.";
            return RedirectToAction("Index");
        }

        // ===========================
        // HELPER: build dropdown data from JobPositions table
        // ===========================
        private async Task BuildEmployeeFormDataAsync(Employee emp)
        {
            // Departments from JobPositions
            var departments = await _context.JobPositions
                .Where(j => j.IsActive && j.Department != null)
                .Select(j => j.Department!)
                .Distinct()
                .OrderBy(d => d)
                .ToListAsync();

            string currentDept = emp.Department;

            if (string.IsNullOrEmpty(currentDept) && emp.JobPositionId.HasValue)
            {
                var jp = await _context.JobPositions.FindAsync(emp.JobPositionId.Value);
                currentDept = jp?.Department;
            }

            if (string.IsNullOrEmpty(currentDept))
                currentDept = departments.FirstOrDefault() ?? string.Empty;

            emp.Department = currentDept;

            ViewBag.Departments = departments
                .Select(d => new SelectListItem
                {
                    Text = d,
                    Value = d,
                    Selected = (d == currentDept)
                })
                .ToList();

            // Job titles for selected department
            var jobs = await _context.JobPositions
                .Where(j => j.IsActive && j.Department == currentDept)
                .OrderBy(j => j.Name)
                .ToListAsync();

            string currentTitle = emp.JobTitle;

            if (string.IsNullOrEmpty(currentTitle) && emp.JobPositionId.HasValue)
            {
                var jp = jobs.FirstOrDefault(j => j.JobPositionId == emp.JobPositionId.Value);
                currentTitle = jp?.Name;
            }

            if (string.IsNullOrEmpty(currentTitle))
                currentTitle = jobs.FirstOrDefault()?.Name ?? string.Empty;

            emp.JobTitle = currentTitle;

            ViewBag.JobTitles = jobs
                .Select(j => new SelectListItem
                {
                    Text = j.Name,
                    Value = j.Name,
                    Selected = (j.Name == currentTitle)
                })
                .ToList();

            // Pay frequency options
            var allFreqs = new[] { "Weekly", "Fortnightly", "Monthly" };
            ViewBag.PayFrequencies = allFreqs
                .Select(f => new SelectListItem
                {
                    Text = f,
                    Value = f,
                    Selected = (f == emp.PayFrequency)
                })
                .ToList();
        }

        // ===========================
        // HELPER: decide JobPositionId + PayGradeId + Role from JobPositions
        // ===========================
        private async Task ApplyJobPositionLogicAsync(Employee emp)
        {
            if (string.IsNullOrEmpty(emp.Department) || string.IsNullOrEmpty(emp.JobTitle))
                return;

            var jp = await _context.JobPositions
                .Include(j => j.PayGrade)
                .FirstOrDefaultAsync(j =>
                    j.IsActive &&
                    j.Department == emp.Department &&
                    j.Name == emp.JobTitle);

            if (jp == null)
                return;

            // link to job position + pay grade
            emp.JobPositionId = jp.JobPositionId;
            emp.PayGradeId = jp.PayGradeId;

            // access role: Admin / Employee
            if (!string.IsNullOrEmpty(jp.AccessRole))
                emp.Role = jp.AccessRole;
        }

    // ===========================
// JSON: Job metadata for cascading dropdowns
// ===========================
[HttpGet("JobMeta")]   // <--- IMPORTANT: route name "JobMeta"
public async Task<IActionResult> JobMeta()
{
    // same admin check as your other actions
    if (HttpContext.Session.GetString("Role") != "Admin")
        return Unauthorized();

    var data = await _context.JobPositions
        .Include(j => j.PayGrade)
        .Where(j => j.IsActive && j.Department != null)
        .Select(j => new
        {
            id           = j.JobPositionId,
            department   = j.Department!,               // "HR", "Finance", etc.
            jobTitle     = j.Name,                      // "HR Manager"
            payGradeName = j.PayGrade != null ? j.PayGrade.Name : null,
            baseRate     = j.PayGrade != null ? j.PayGrade.BaseRate : 0m,
            rateType     = j.PayGrade != null ? j.PayGrade.RateType.ToString() : null
        })
        .ToListAsync();

    return Json(data);
    }
    private async Task<string> GenerateUniqueEmployeeCodeAsync()
    {
        string code;
        bool exists;

        do
        {
            int num = RandomNumberGenerator.GetInt32(0, 1_000_000); // 000000–999999
            code = $"NZFTC{num:D6}";
            exists = await _context.Employees.AnyAsync(e => e.EmployeeCode == code);
        }
        while (exists);

        return code;
    }
    }
    }
