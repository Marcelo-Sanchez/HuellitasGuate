using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using HuellitasGuate.Data;
using HuellitasGuate.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity.UI.Services;


public class Program
{
    public static async Task Main(string[] args)
    { 
    
        var builder = WebApplication.CreateBuilder(args);
        string connectionString = string.Empty;


        if (builder.Configuration.GetConnectionString("env") == "local")
        {
            connectionString = builder.Configuration.GetConnectionString("HuellitasGuateContextConnection");
        }

        else
        {
            connectionString = builder.Configuration.GetConnectionString("HuellitasGuateDevContextConnection");
        }


        builder.Services.AddDbContext<HuellitasGuateContext>(options =>
            options.UseSqlServer(connectionString));

        builder.Services.AddDefaultIdentity<HuellitasGuateUser>(options =>
        {
            options.SignIn.RequireConfirmedAccount = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireDigit = false;
            options.Password.RequireLowercase = false;
            options.Password.RequireUppercase = false;
            options.Password.RequiredLength = 3;
        })
            .AddRoles<IdentityRole>()
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

    using(var scope = app.Services.CreateScope())
    {
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();


        var roles = new[] { "Admin", "Manager", "Member" };

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
                await roleManager.CreateAsync(new IdentityRole(role));
        }
    }

    using (var scope = app.Services.CreateScope())
    {
        var userManager = 
            scope.ServiceProvider.GetRequiredService<UserManager<HuellitasGuateUser>>();

        string email = "wmsanchez11@gmail.com";
        string password = "Test1234,";

        if(await userManager.FindByEmailAsync(email) == null)
        {
            var user = new HuellitasGuateUser();
            user.UserName = email;
            user.Email = email;
            //user.EmailConfirmed = true;

            await userManager.CreateAsync(user, password);

            await userManager.AddToRoleAsync(user, "Admin");


         }
    }

        app.Run();


    }
}
