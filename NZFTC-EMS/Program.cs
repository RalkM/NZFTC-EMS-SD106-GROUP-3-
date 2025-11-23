using Microsoft.EntityFrameworkCore;
using NZFTC_EMS.Data;
using NZFTC_EMS.Services.Leave;
using NZFTC_EMS.Services.Payroll;

var builder = WebApplication.CreateBuilder(args);

// ============================================================
// 1) Register DbContext
// ============================================================
builder.Services.AddDbContext<AppDbContext>(options =>
{
    var cs = builder.Configuration.GetConnectionString("DefaultConnection");
    if (string.IsNullOrWhiteSpace(cs))
        throw new InvalidOperationException("Missing ConnectionStrings:DefaultConnection");

    options.UseMySql(cs, ServerVersion.AutoDetect(cs));
});

// ============================================================
// 2) Register ALL Services (Leave + Payroll)
// ============================================================

// Payroll
builder.Services.AddScoped<ITaxService, TaxService>();
builder.Services.AddScoped<IPayrollService, PayrollService>();;


// Leave
builder.Services.AddScoped<LeaveService>();
builder.Services.AddScoped<LeavePolicyService>();
builder.Services.AddScoped<LeaveReportService>();

// ============================================================
// 3) MVC + Session
// ============================================================
builder.Services.AddHttpContextAccessor();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
builder.Services.AddControllersWithViews();

var app = builder.Build();

// ============================================================
// 4) Database Connectivity Check (Dev Only)
// ============================================================
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    Console.WriteLine("EF CanConnect: " + db.Database.CanConnect());
    // db.Database.Migrate();  // Optional
}

// ============================================================
// 5) Quick health endpoint
// ============================================================
app.MapGet("/db-check", async (AppDbContext db) =>
{
    var can = await db.Database.CanConnectAsync();
    var employees = await db.Employees.CountAsync();
    return Results.Json(new { CanConnect = can, Employees = employees });
});

// ============================================================
// 6) Middleware Pipeline
// ============================================================
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Website/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseSession();
app.UseAuthorization();

// ============================================================
// 7) Default Route
// ============================================================
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Website}/{action=Index}/{id?}"
);

app.Run();
