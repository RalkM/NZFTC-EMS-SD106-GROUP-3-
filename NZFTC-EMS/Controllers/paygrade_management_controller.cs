using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZFTC_EMS.Data;
using NZFTC_EMS.Data.Entities;

namespace NZFTC_EMS.Controllers
{
    [Route("paygrade_management")]
    public class paygrade_management_controller : Controller
    {
        private readonly AppDbContext _context;

        public paygrade_management_controller(AppDbContext context)
        {
            _context = context;
        }

        private bool IsAdmin()
        {
            return HttpContext.Session.GetString("Role") == "Admin";
        }

        // LIST
        [HttpGet("")]
        [HttpGet("index")]
        public async Task<IActionResult> Index()
        {
            if (!IsAdmin())
                return RedirectToAction("AccessDenied", "website");

            ViewData["Layout"] = "~/Views/Shared/_portal.cshtml";

            var grades = await _context.PayGrades
                .OrderBy(p => p.Name)
                .ToListAsync();

            return View(
                "~/Views/website/admin/paygrade_management.cshtml",
                grades
            );
        }

        // CREATE (GET)
        [HttpGet("create")]
        public IActionResult Create()
        {
            if (!IsAdmin())
                return RedirectToAction("AccessDenied", "website");

            ViewData["Layout"] = "~/Views/Shared/_portal.cshtml";

            var model = new PayGrade
            {
                IsActive = true,
                BaseRate = 0,
                RateType = RateType.Hourly    // assuming enum Hourly/Salary
            };

            return View(
                "~/Views/website/admin/paygrade_create.cshtml",
                model
            );
        }

        // CREATE (POST)
        [HttpPost("create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PayGrade model)
        {
            if (!IsAdmin())
                return RedirectToAction("AccessDenied", "website");

            if (!ModelState.IsValid)
            {
                ViewData["Layout"] = "~/Views/Shared/_portal.cshtml";
                return View(
                    "~/Views/website/admin/paygrade_create.cshtml",
                    model
                );
            }

            // simple uniqueness check on Name
            bool exists = await _context.PayGrades
                .AnyAsync(p => p.Name == model.Name);

            if (exists)
            {
                ModelState.AddModelError("Name", "A pay grade with this name already exists.");
                ViewData["Layout"] = "~/Views/Shared/_portal.cshtml";
                return View(
                    "~/Views/website/admin/paygrade_create.cshtml",
                    model
                );
            }

            _context.PayGrades.Add(model);
            await _context.SaveChangesAsync();

            TempData["msg"] = "Pay grade created successfully.";
            return RedirectToAction("Index");
        }

        // EDIT (GET)
        [HttpGet("edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            if (!IsAdmin())
                return RedirectToAction("AccessDenied", "website");

            ViewData["Layout"] = "~/Views/Shared/_portal.cshtml";

            var grade = await _context.PayGrades.FindAsync(id);
            if (grade == null) return NotFound();

            return View(
                "~/Views/website/admin/paygrade_edit.cshtml",
                grade
            );
        }

        // EDIT (POST)
        [HttpPost("edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, PayGrade model)
        {
            if (!IsAdmin())
                return RedirectToAction("AccessDenied", "website");

            if (id != model.PayGradeId)
                return BadRequest();

            if (!ModelState.IsValid)
            {
                ViewData["Layout"] = "~/Views/Shared/_portal.cshtml";
                return View(
                    "~/Views/website/admin/paygrade_edit.cshtml",
                    model
                );
            }

            var existing = await _context.PayGrades.FindAsync(id);
            if (existing == null) return NotFound();

            // check name uniqueness (exclude self)
            bool exists = await _context.PayGrades
                .AnyAsync(p => p.PayGradeId != id && p.Name == model.Name);

            if (exists)
            {
                ModelState.AddModelError("Name", "A pay grade with this name already exists.");
                ViewData["Layout"] = "~/Views/Shared/_portal.cshtml";
                return View(
                    "~/Views/website/admin/paygrade_edit.cshtml",
                    model
                );
            }

            existing.Name = model.Name;
            existing.Description = model.Description;   // <-- add this
            existing.BaseRate = model.BaseRate;
            existing.RateType = model.RateType;
            existing.IsActive = model.IsActive;

            _context.Update(existing);
            await _context.SaveChangesAsync();

            TempData["msg"] = "Pay grade updated.";
            return RedirectToAction("Index");
        }

        // DELETE (GET)
        [HttpGet("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (!IsAdmin())
                return RedirectToAction("AccessDenied", "website");

            ViewData["Layout"] = "~/Views/Shared/_portal.cshtml";

            var grade = await _context.PayGrades.FindAsync(id);
            if (grade == null) return NotFound();

            return View(
                "~/Views/website/admin/paygrade_delete.cshtml",
                grade
            );
        }

        // DELETE (POST)
        [HttpPost("delete/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (!IsAdmin())
                return RedirectToAction("AccessDenied", "website");

            var grade = await _context.PayGrades.FindAsync(id);
            if (grade == null) return RedirectToAction("Index");

            // prevent deleting if used by any employees
            bool inUse = await _context.Employees
                .AnyAsync(e => e.PayGradeId == id);

            if (inUse)
            {
                TempData["msg"] = "❌ Cannot delete this pay grade because it is in use by employees.";
                return RedirectToAction("Index");
            }

            _context.PayGrades.Remove(grade);
            await _context.SaveChangesAsync();

            TempData["msg"] = "Pay grade deleted.";
            return RedirectToAction("Index");
        }
    }
}
