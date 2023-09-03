using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using HuellitasGuate.Data;
using HuellitasGuate.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity.UI.Services;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("HuellitasGuateContextConnection") ?? throw new InvalidOperationException("Connection string 'HuellitasGuateContextConnection' not found.");

builder.Services.AddDbContext<HuellitasGuateContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<HuellitasGuateUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<HuellitasGuateContext>();

builder.Services.AddScoped<IEmailSender, EmailSender>();
// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();;
app.MapRazorPages();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
