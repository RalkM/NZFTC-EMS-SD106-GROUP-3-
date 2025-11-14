using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NZFTC_EMS.Data;
using NZFTC_EMS.Data.Entities;

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
                .FirstOrDefaultAsync(e => e.EmployeeId == id);

            if (emp == null) return NotFound();

            return View(
                "~/Views/website/admin/employee_details.cshtml",
                emp
            );
        }

        // ===========================
        // CREATE (GET)
        // ===========================
        [HttpGet("create")]
        public async Task<IActionResult> Create()
        {
            if (!IsAdmin())
                return RedirectToAction("AccessDenied", "website");

            ViewData["Layout"] = "~/Views/Shared/_portal.cshtml";

            var model = new Employee
            {
                StartDate = DateTime.UtcNow
            };

            await BuildEmployeeFormDataAsync(model);

            return View(
                "~/Views/website/admin/employee_create.cshtml",
                model
            );
        }

        // ===========================
        // CREATE (POST)
        // ===========================
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

            var emp = await _context.Employees.FindAsync(id);
            if (emp == null) return NotFound();

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
                    return View("~/Views/website/admin/employee_create.cshtml", model);
                }

                await ApplyJobPositionLogicAsync(model);

                _context.Employees.Add(model);
                await _context.SaveChangesAsync();

            var existingEmployee = await _context.Employees.FindAsync(id);
            if (existingEmployee == null)
                return NotFound();

            // PERSONAL DETAILS
            existingEmployee.FirstName = model.FirstName;
            existingEmployee.LastName = model.LastName;
            existingEmployee.Email = model.Email;
            existingEmployee.Phone = model.Phone;
            existingEmployee.Address = model.Address;
            existingEmployee.Birthday = model.Birthday;
            existingEmployee.Gender = model.Gender;

            // JOB DETAILS (from form)
            existingEmployee.Department   = model.Department;
            existingEmployee.JobTitle     = model.JobTitle;
            existingEmployee.PayFrequency = model.PayFrequency;
            existingEmployee.StartDate    = model.StartDate;

            // decide JobPositionId, PayGradeId, Role from JobPositions table
            await ApplyJobPositionLogicAsync(existingEmployee);

            _context.Update(existingEmployee);
            await _context.SaveChangesAsync();

            TempData["msg"] = "Employee record updated successfully!";
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
        [HttpPost("delete/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (!IsAdmin())
                return RedirectToAction("AccessDenied", "website");

            var emp = await _context.Employees.FindAsync(id);
            if (emp != null)
            {
                _context.Employees.Remove(emp);
                await _context.SaveChangesAsync();
            }

            TempData["msg"] = "Employee deleted.";
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


    }
    
}
