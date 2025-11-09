using Microsoft.EntityFrameworkCore;
using NZFTC_EMS.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase("NZFTC_EMS_TestDB"));

builder.Services.AddHttpContextAccessor();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddControllersWithViews();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Website/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();


app.UseSession();


app.Use(async (context, next) =>
{
    
    if (string.IsNullOrEmpty(context.Session.GetString("Role")))
    {
        context.Session.SetString("Role", "Employee");   
        context.Session.SetString("Username", "John Doe");
    }
    await next();
});

app.UseAuthorization();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Website}/{action=Login}/{id?}"
);

app.Run();
