using Microsoft.EntityFrameworkCore;
using NZFTC_EMS.Data;

var builder = WebApplication.CreateBuilder(args);

// 1) Register DbContext (SQL Server)
builder.Services.AddDbContext<AppDbContext>(options =>
{
    var cs = builder.Configuration.GetConnectionString("DefaultConnection");
    if (string.IsNullOrWhiteSpace(cs))
        throw new InvalidOperationException("Missing ConnectionStrings:DefaultConnection");
    options.UseMySql(cs, ServerVersion.AutoDetect(cs));
});


// Other services
builder.Services.AddHttpContextAccessor();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
builder.Services.AddControllersWithViews();

var app = builder.Build();

// 2) (Dev) check connectivity AFTER build
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    Console.WriteLine("EF CanConnect: " + db.Database.CanConnect());
    // Optional auto-migrate (dev only)
    // db.Database.Migrate();
}

// 3) Quick health endpoint: http://localhost:<port>/db-check
app.MapGet("/db-check", async (AppDbContext db) =>
{
    var can = await db.Database.CanConnectAsync();
    var employees = await db.Employees.CountAsync();
    return Results.Json(new { CanConnect = can, Employees = employees });
});

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

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Website}/{action=Index}/{id?}"
);

app.Run();
