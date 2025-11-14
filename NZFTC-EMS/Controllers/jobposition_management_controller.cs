using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NZFTC_EMS.Data;
using NZFTC_EMS.Data.Entities;

namespace NZFTC_EMS.Controllers
{
    [Route("jobposition_management")]
    public class jobposition_management_controller : Controller
    {
        private readonly AppDbContext _context;

        public jobposition_management_controller(AppDbContext context)
        {
            _context = context;
        }

        private bool IsAdmin()
            => HttpContext.Session.GetString("Role") == "Admin";

        // LIST
[HttpGet("")]
[HttpGet("index")]
public async Task<IActionResult> Index(string? q, string? department)
{
    if (!IsAdmin())
        return RedirectToAction("AccessDenied", "website");

    ViewData["Layout"] = "~/Views/Shared/_portal.cshtml";

    var query = _context.JobPositions
        .Include(j => j.PayGrade)
        .AsQueryable();

    if (!string.IsNullOrWhiteSpace(q))
    {
        query = query.Where(j =>
            j.Name.Contains(q) ||
            (j.Department != null && j.Department.Contains(q)) ||
            (j.Description != null && j.Description.Contains(q)));
    }

    if (!string.IsNullOrWhiteSpace(department) && department != "All")
    {
        query = query.Where(j => j.Department == department);
    }

    var positions = await query
        .OrderBy(j => j.Department)
        .ThenBy(j => j.Name)
        .ToListAsync();

    // build department filter options
    var departments = await _context.JobPositions
        .Where(j => j.Department != null)
        .Select(j => j.Department!)
        .Distinct()
        .OrderBy(d => d)
        .ToListAsync();

    ViewBag.DepartmentOptions = departments;
    ViewBag.Search = q;
    ViewBag.Department = department ?? "All";

    return View("~/Views/website/admin/jobposition_management.cshtml", positions);
}


        private async Task BuildFormDataAsync(JobPosition model)
        {
            var paygrades = await _context.PayGrades
                .Where(pg => pg.IsActive)
                .OrderBy(pg => pg.Name)
                .ToListAsync();

            ViewBag.PayGrades = paygrades
                .Select(pg => new SelectListItem
                {
                    Text = $"{pg.Name} - {pg.Description}",
                    Value = pg.PayGradeId.ToString(),
                    Selected = (model.PayGradeId == pg.PayGradeId)
                })
                .ToList();

            ViewBag.AccessRoles = new List<SelectListItem>
            {
                new("Employee", "Employee"){ Selected = model.AccessRole == "Employee" || string.IsNullOrEmpty(model.AccessRole) },
                new("Admin", "Admin"){ Selected = model.AccessRole == "Admin" }
            };
        }

        // CREATE (GET)
        [HttpGet("create")]
        public async Task<IActionResult> Create()
        {
            if (!IsAdmin())
                return RedirectToAction("AccessDenied", "website");

            ViewData["Layout"] = "~/Views/Shared/_portal.cshtml";

            var model = new JobPosition
            {
                IsActive = true,
                AccessRole = "Employee"
            };

            await BuildFormDataAsync(model);

            return View("~/Views/website/admin/jobposition_create.cshtml", model);
        }

        // CREATE (POST)
        [HttpPost("create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(JobPosition model)
        {
            if (!IsAdmin())
                return RedirectToAction("AccessDenied", "website");

            if (!ModelState.IsValid)
            {
                ViewData["Layout"] = "~/Views/Shared/_portal.cshtml";
                await BuildFormDataAsync(model);
                return View("~/Views/website/admin/jobposition_create.cshtml", model);
            }

            _context.JobPositions.Add(model);
            await _context.SaveChangesAsync();

            TempData["msg"] = "Job position created.";
            return RedirectToAction("Index");
        }

        // EDIT (GET)
        [HttpGet("edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            if (!IsAdmin())
                return RedirectToAction("AccessDenied", "website");

            ViewData["Layout"] = "~/Views/Shared/_portal.cshtml";

            var model = await _context.JobPositions.FindAsync(id);
            if (model == null) return NotFound();

            await BuildFormDataAsync(model);

            return View("~/Views/website/admin/jobposition_edit.cshtml", model);
        }

        // EDIT (POST)
        [HttpPost("edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, JobPosition model)
        {
            if (!IsAdmin())
                return RedirectToAction("AccessDenied", "website");

            if (id != model.JobPositionId)
                return BadRequest();

            if (!ModelState.IsValid)
            {
                ViewData["Layout"] = "~/Views/Shared/_portal.cshtml";
                await BuildFormDataAsync(model);
                return View("~/Views/website/admin/jobposition_edit.cshtml", model);
            }

            var existing = await _context.JobPositions.FindAsync(id);
            if (existing == null) return NotFound();

            existing.Name        = model.Name;
            existing.Department  = model.Department;
            existing.Description = model.Description;
            existing.PayGradeId  = model.PayGradeId;
            existing.AccessRole  = model.AccessRole;
            existing.IsActive    = model.IsActive;

            _context.Update(existing);
            await _context.SaveChangesAsync();

            TempData["msg"] = "Job position updated.";
            return RedirectToAction("Index");
        }

        // DELETE (GET)
        [HttpGet("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (!IsAdmin())
                return RedirectToAction("AccessDenied", "website");

            ViewData["Layout"] = "~/Views/Shared/_portal.cshtml";

            var model = await _context.JobPositions
                .Include(j => j.PayGrade)
                .FirstOrDefaultAsync(j => j.JobPositionId == id);

            if (model == null) return NotFound();

            return View("~/Views/website/admin/jobposition_delete.cshtml", model);
        }

        // DELETE (POST)
        [HttpPost("delete/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (!IsAdmin())
                return RedirectToAction("AccessDenied", "website");

            var existing = await _context.JobPositions.FindAsync(id);
            if (existing == null) return RedirectToAction("Index");

            bool inUse = await _context.Employees
                .AnyAsync(e => e.JobPositionId == id);

            if (inUse)
            {
                TempData["msg"] = "❌ Cannot delete – job position is used by employees.";
                return RedirectToAction("Index");
            }

            _context.JobPositions.Remove(existing);
            await _context.SaveChangesAsync();

            TempData["msg"] = "Job position deleted.";
            return RedirectToAction("Index");
        }
    }
}
