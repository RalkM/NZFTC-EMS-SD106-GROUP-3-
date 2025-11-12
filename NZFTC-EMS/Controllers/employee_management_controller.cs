using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZFTC_EMS.Data;
using NZFTC_EMS.Models;

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

        // LIST
        [HttpGet("")]
        [HttpGet("index")]
        public async Task<IActionResult> Index(string q, string department)
        {
            // admin-only check (same style as leave)
            if (HttpContext.Session.GetString("Role") != "Admin")
                return RedirectToAction("AccessDenied", "website");

            ViewData["Layout"] = "~/Views/Shared/_portal.cshtml";

            var query = _context.Employees.AsQueryable();

            // filter by search text
            if (!string.IsNullOrWhiteSpace(q))
                query = query.Where(e => e.full_name.Contains(q) || e.email.Contains(q));

            // filter by department
            if (!string.IsNullOrWhiteSpace(department) && department != "All")
                query = query.Where(e => e.department == department);

            var employees = await query.ToListAsync();

            // pass the filter values back to the view
            ViewBag.Search = q;
            ViewBag.Department = department ?? "All";

            return View(
                "~/Views/website/admin/EmployeeManagement/employee_management.cshtml",
                employees
            );
        }

        // DETAILS (GET)
        [HttpGet("details/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            if (HttpContext.Session.GetString("Role") != "Admin")
                return RedirectToAction("AccessDenied", "website");

            ViewData["Layout"] = "~/Views/Shared/_portal.cshtml";

            var emp = await _context.Employees.FindAsync(id);
            if (emp == null) return NotFound();

            return View("~/Views/website/admin/EmployeeManagement/employee_details.cshtml", emp);
        }


        // CREATE (GET)
        [HttpGet("create")]
        public IActionResult Create()
        {
            ViewData["Layout"] = "~/Views/Shared/_portal.cshtml";
            return View("~/Views/website/admin/EmployeeManagement/employee_create.cshtml",
                new employee_model());
        }

        // CREATE (POST)
        [HttpPost("create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(employee_model model)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Layout"] = "~/Views/Shared/_portal.cshtml";
                return View("~/Views/website/admin/EmployeeManagement/employee_create.cshtml", model);
            }

            _context.Employees.Add(model);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        // EDIT (GET)
        [HttpGet("edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            ViewData["Layout"] = "~/Views/Shared/_portal.cshtml";

            var emp = await _context.Employees.FindAsync(id);
            if (emp == null) return NotFound();

            return View("~/Views/website/admin/EmployeeManagement/employee_edit.cshtml", emp);
        }

        // EDIT (POST)
        [HttpPost("edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, employee_model model)
        {
            if (id != model.id)
                return BadRequest();

            if (!ModelState.IsValid)
            {
                ViewData["Layout"] = "~/Views/Shared/_portal.cshtml";
                return View("~/Views/website/admin/EmployeeManagement/employee_edit.cshtml", model);
            }

            var existingEmployee = await _context.Employees.FindAsync(id);
            if (existingEmployee == null)
                return NotFound();

            // Update the fields you allow editing
            existingEmployee.full_name = model.full_name;
            existingEmployee.role = model.role;
            existingEmployee.department = model.department;
            existingEmployee.email = model.email;
            existingEmployee.basic_pay = model.basic_pay;

            _context.Update(existingEmployee);
            await _context.SaveChangesAsync();

            TempData["msg"] = "✅ Employee record updated successfully!";
            return RedirectToAction("Index"); // redirects to the employee list
        }


        // DELETE (GET)
        [HttpGet("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            ViewData["Layout"] = "~/Views/Shared/_portal.cshtml";

            var emp = await _context.Employees.FindAsync(id);
            if (emp == null) return NotFound();

            return View("~/Views/website/admin/EmployeeManagement/employee_delete.cshtml", emp);
        }

        // DELETE (POST)
        [HttpPost("delete/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var emp = await _context.Employees.FindAsync(id);
            if (emp != null)
            {
                _context.Employees.Remove(emp);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }
    }
}




