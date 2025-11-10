using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZFTC_EMS.Data;
using NZFTC_EMS.Models;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NZFTC_EMS.Controllers
{
    [Route("payroll_management")]
    public class payroll_management_controller : Controller
    {
        private readonly AppDbContext _context;

        public payroll_management_controller(AppDbContext context)
        {
            _context = context;
        }


        [HttpGet("")]
        [HttpGet("index")]
        public async Task<IActionResult> Index()
        {
            // Always let admins through
            ViewData["Layout"] = "~/Views/Shared/_portal.cshtml";

            var payrolls = await _context.Payrolls.ToListAsync();
            return View("~/Views/website/admin/payroll_management.cshtml", payrolls);
        }


        [HttpPost("add")]
        public async Task<IActionResult> Add(payroll_model model)
        {
            if (ModelState.IsValid)
            {
                model.net_salary = model.base_salary - model.deductions;
                _context.Payrolls.Add(model);
                await _context.SaveChangesAsync();
                TempData["msg"] = "Payroll added successfully!";
            }
            return RedirectToAction("Index");
        }


        [HttpGet("payslip")]
        public async Task<IActionResult> Payslip(string name)
        {
            ViewData["Layout"] = "~/Views/Shared/_portal.cshtml";

            // default user
            if (string.IsNullOrEmpty(name))
                name = "John Doe";

            var payslips = await _context.Payrolls
                .Where(x => x.employee_name == name)
                .ToListAsync();

            return View("~/Views/website/employee/payslip.cshtml", payslips);
        }


        [HttpGet("download/{id}")]
        public async Task<IActionResult> Download(int id)
        {
            var record = await _context.Payrolls.FindAsync(id);
            if (record == null)
                return NotFound();

            var csv = new StringBuilder();
            csv.AppendLine("Employee Name,Base Salary,Deductions,Net Salary");
            csv.AppendLine($"{record.employee_name},{record.base_salary},{record.deductions},{record.net_salary}");

            var bytes = Encoding.UTF8.GetBytes(csv.ToString());
            return File(bytes, "text/csv", $"Payslip_{record.employee_name.Replace(" ", "_")}.csv");
        }


        [HttpGet("seed-test")]
        public async Task<IActionResult> SeedTest()
        {
            if (!_context.Payrolls.Any())
            {
                _context.Payrolls.AddRange(new[]
                {
                    new payroll_model { employee_name = "John Doe", base_salary = 4800, deductions = 200, net_salary = 4600 },
                    new payroll_model { employee_name = "Jane Smith", base_salary = 5200, deductions = 150, net_salary = 5050 },
                    new payroll_model { employee_name = "Liam Brown", base_salary = 6000, deductions = 400, net_salary = 5600 },
                });
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }
    }
}
