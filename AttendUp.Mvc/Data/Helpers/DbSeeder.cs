using AttendUp.Mvc.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AttendUp.Mvc.Data.Helpers
{
    public static class DbSeeder
    {
        public static async Task SeedIdentityAsync(IServiceProvider services)
        {
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = services.GetRequiredService<UserManager<IdentityUser>>();

            foreach (var role in new[] { "Admin", "SuperAdmin", "Member" })
            {
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new IdentityRole(role));
            }

            await EnsureUserInRoleAsync(userManager, "Admin", "Admin", "admin@attendup.com", "welkom", "Admin");
            await EnsureUserInRoleAsync(userManager, "Superadmin", "Superadmin", "superadmin@attendup.com", "geheim", "SuperAdmin");
            await EnsureUserInRoleAsync(userManager, "Member", "Member", "member@attendup.com", "welkom", "Member");
        }

        private static async Task EnsureUserInRoleAsync(UserManager<IdentityUser> userManager, string firstName, string lastName, string email, string password, string role)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user == null)
            {
                user = new IdentityUser { UserName = $"{firstName}.{lastName}", Email = email, EmailConfirmed = true };
                await userManager.CreateAsync(user, password);
            }
            if (!await userManager.IsInRoleAsync(user, role))
                await userManager.AddToRoleAsync(user, role);
        }

        public static async Task SeedRegistrationsAsync(AttendUpContext context)
        {
            if (await context.Registrations.AnyAsync()) return;

            var trainings = await context.Trainings
                .Where(t => t.ID >= 1 && t.ID <= 25)
                .ToDictionaryAsync(t => t.ID);

            var registrations = new List<Registration>();

            for (int t = 1; t <= 25; t++)
            {
                var training = trainings[t];
                var baseTime = training.Date.Date.AddMinutes(570); // 09:30

                int count = 10 + (t % 25);
                int subActivityId = t % 5 == 0 ? 3 : t % 2 == 0 ? 2 : 1;
                int interval = 180 / count; // integer division, matches SQL behaviour

                for (int r = 1; r <= count; r++)
                {
                    registrations.Add(new Registration
                    {
                        FirstName = $"User{r}",
                        LastName = $"Test{t}",
                        TrainingID = t,
                        SubActivityID = subActivityId,
                        LocationId = 1,
                        RegisteredAt = baseTime.AddMinutes(r * interval),
                        IsValid = true
                    });
                }
            }

            await context.Registrations.AddRangeAsync(registrations);
            await context.SaveChangesAsync();
        }
    }
}
