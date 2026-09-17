using AttendUp.Mvc.Data;
using AttendUp.Mvc.Data.Helpers;
using AttendUp.Mvc.Services;
using AttendUp.Mvc.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory; // Required for caching 

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.Configure<FormOptions>(o => o.ValueCountLimit = int.MaxValue);
builder.Services.AddDbContext<AttendUpContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

builder.Services.AddScoped<IStatisticsService, StatisticsService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IFamilyService, FamilyService>();

// Identity code configureren
builder.Services.AddDefaultIdentity<IdentityUser>(options => {
    options.SignIn.RequireConfirmedAccount = false;

    // HIER HEB IK DE STRENGE EISEN VOLLEDIG UITGEZET:
    options.Password.RequireDigit = false;             // Geen verplicht cijfer meer
    options.Password.RequireLowercase = false;         // Geen verplichte kleine letter
    options.Password.RequireUppercase = false;         // GEEN HOOFDLETTER MEER VERPLICHT (Verwijdert de extra foutmelding!)
    options.Password.RequireNonAlphanumeric = false;   // GEEN SPECIAAL TEKEN MEER VERPLICHT (Verwijdert de extrafoutmelding!)
    options.Password.RequiredLength = 6;               // Minimale lengte blijft op 6 staan 
    options.Password.RequiredUniqueChars = 0;          // Geen unieke tekens verplicht

    // I allowed spaces in usernames so that names like 'Jan Janssen' don't trigger validation errors.
    options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+ ";
})
.AddRoles<IdentityRole>()
.AddEntityFrameworkStores<AttendUpContext>();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/AccessDenied";
    options.SlidingExpiration = false;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(1);


    options.Events = new CookieAuthenticationEvents
    {
        OnValidatePrincipal = async context =>
        {
            if (context.Principal?.Identity?.IsAuthenticated == true &&
                (context.Principal.IsInRole("Admin") || context.Principal.IsInRole("SuperAdmin")))
            {
                //  fetch the Cache service instead of querying the database directly
                var cache = context.HttpContext.RequestServices.GetRequiredService<IMemoryCache>();

                // for the timeout from the cache (or refresh it from the DB if 5 minutes have passed)
                int timeoutMinutes = await cache.GetOrCreateAsync("BoardSessionTimeout", async entry =>
                {
                    // I cache this configuration value for 5 minutes
                    entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5);

                    var dbContext = context.HttpContext.RequestServices.GetRequiredService<AttendUpContext>();
                    var settings = await dbContext.Settings.FirstOrDefaultAsync();

                    return settings?.BoardSessionTimeoutMinutes ?? 60; // I use 60 minutes as my fallback value
                });

                // restrict how often cookies are rewritten
                var currentExpires = context.Properties.ExpiresUtc;
                var targetExpires = DateTimeOffset.UtcNow.AddMinutes(timeoutMinutes);

                // I only adjust the expiration time if it does not exist yet,
                // or if the newly calculated time significantly differs from what is currently in the cookie.
                if (!currentExpires.HasValue || Math.Abs((currentExpires.Value - targetExpires).TotalMinutes) > 1)
                {
                    context.Properties.ExpiresUtc = targetExpires;
                    context.ShouldRenew = true; // I only rewrite the cookie if the settings have actually changed
                }
            }
        }
    };
});
// I add the HttpContextAccessor to allow my database logger to read IP addresses and user names
builder.Services.AddHttpContextAccessor();

// I register my custom database logger provider into the built-in logging pipeline
builder.Logging.AddProvider(new AttendUp.Mvc.Data.Helpers.DbLoggerProvider(builder.Services.BuildServiceProvider()));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Registration}/{action=Index}/{id?}")
    .WithStaticAssets();

// Automatisch updaten van migration bij runnen project
if(!app.Environment.IsDevelopment())
{
    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;

        var context = services.GetRequiredService<AttendUpContext>();
        await context.Database.MigrateAsync();

        await DbSeeder.SeedIdentityAsync(services);
    }
}

app.Run();