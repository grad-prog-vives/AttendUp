using AttendUp.Mvc.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AttendUp.Mvc.Data
{
    public class AttendUpContext : IdentityDbContext<IdentityUser>
    {
        public AttendUpContext(DbContextOptions<AttendUpContext> options)
            : base(options) { }

        public DbSet<Registration> Registrations { get; set; }
        public DbSet<Training> Trainings { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<SubActivity> SubActivities { get; set; }
        public DbSet<TrainingSubActivity> TrainingSubActivities { get; set; }
        public DbSet<PasswordExpiry> PasswordExpiries { get; set; }
        public DbSet<Setting> Settings { get; set; }
        public DbSet<Family> Families { get; set; }
        public DbSet<FamilyMember> FamilyMembers { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }

        public DbSet<SystemLog> SystemLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<TrainingSubActivity>()
                .HasKey(t => new { t.TrainingID, t.SubActivityID });

            SeedLocationData(modelBuilder);
            SeedSubActivityData(modelBuilder);
            SeedBrandingData(modelBuilder);
            SeedTrainingData(modelBuilder);
            SeedTrainingSubActivityData(modelBuilder);
            SeedAuditLogData(modelBuilder);
        }

        private void SeedAuditLogData(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AuditLog>().HasData(
                new AuditLog
                {
                    ID = 1,
                    User = "Admin Admin",
                    Action = "Aangemaakt",
                    Details = "(ID: 1) Training 'Winter Kickoff' aangemaakt",
                    Timestamp = new DateTime(2025, 10, 15, 14, 22, 10)
                },
                new AuditLog
                {
                    ID = 2,
                    User = "Admin Admin",
                    Action = "Aangemaakt",
                    Details = "(ID: 2) Training 'Winter Session' aangemaakt.",
                    Timestamp = new DateTime(2025, 10, 15, 14, 25, 0)
                },
                new AuditLog
                {
                    ID = 3,
                    User = "Superadmin Superadmin",
                    Action = "Aangepast",
                    Details = "(ID: 6) Training 'New Year Training' gewijzigd.",
                    Timestamp = new DateTime(2025, 12, 28, 11, 05, 43)
                },
                new AuditLog
                {
                    ID = 4,
                    User = "Admin Admin",
                    Action = "Verwijderd",
                    Details = "(ID: 99) Training 'Oude Test Training' permanent verwijderd inclusief 15 bijbehorende registraties.",
                    Timestamp = new DateTime(2026, 01, 10, 09, 15, 22)
                },
                new AuditLog
                {
                    ID = 5,
                    User = "Admin Admin",
                    Action = "Aangepast",
                    Details = "(ID: 18) Training 'Toertocht' is GEDEACTIVEERD. Reden: Slechte weersomstandigheden (storm).",
                    Timestamp = new DateTime(2026, 03, 28, 17, 40, 11)
                },
                new AuditLog
                {
                    ID = 6,
                    User = "Superadmin Superadmin",
                    Action = "Aangepast",
                    Details = "(ID: 18) Training 'Toertocht' is op ACTIEF gezet.",
                    Timestamp = new DateTime(2026, 03, 29, 07, 30, 0)
                },
                new AuditLog
                {
                    ID = 7,
                    User = "Admin Admin",
                    Action = "Aangemaakt",
                    Details = "(ID: 28) Training 'June Training' aangemaakt.",
                    Timestamp = new DateTime(2026, 05, 20, 19, 02, 15)
                },
                new AuditLog
                {
                    ID = 8,
                    User = "Admin Admin",
                    Action = "Aangemaakt",
                    Details = "(ID: 29) Training 'June Training' aangemaakt.",
                    Timestamp = new DateTime(2026, 05, 20, 19, 04, 50)
                },
                new AuditLog
                {
                    ID = 9,
                    User = "Admin Admin",
                    Action = "Aangemaakt",
                    Details = "(ID: 30) Training 'June Training' aangemaakt.",
                    Timestamp = new DateTime(2026, 05, 20, 19, 06, 12)
                },
                new AuditLog
                {
                    ID = 10,
                    User = "Admin Admin",
                    Action = "Aangemaakt",
                    Details = "(ID: 31) Training 'Summer Ride' aangemaakt.",
                    Timestamp = new DateTime(2026, 05, 20, 19, 10, 05)
                },
                new AuditLog
                {
                    ID = 11,
                    User = "SuperAdmin SuperAdmin",
                    Action = "Aanpassen",
                    Details = "(ID: 31) Training 'Summer Ride' gewijzigd.",
                    Timestamp = new DateTime(2026, 05, 22, 10, 15, 30)
                },
                new AuditLog
                {
                    ID = 12,
                    User = "Admin Admin",
                    Action = "Aangemaakt",
                    Details = "(ID: 32) Training 'July Training' aangemaakt.",
                    Timestamp = new DateTime(2026, 06, 15, 09, 30, 22)
                },
                new AuditLog
                {
                    ID = 13,
                    User = "Admin Admin",
                    Action = "Aangemaakt",
                    Details = "(ID: 33) Training 'July Training' aangemaakt.",
                    Timestamp = new DateTime(2026, 06, 15, 09, 32, 45)
                },
                new AuditLog
                {
                    ID = 14,
                    User = "Admin Admin",
                    Action = "Aangemaakt",
                    Details = "(ID: 34) Training 'July Training' aangemaakt.",
                    Timestamp = new DateTime(2026, 06, 15, 09, 35, 10)
                },
                new AuditLog
                {
                    ID = 15,
                    User = "Admin Admin",
                    Action = "Aangemaakt",
                    Details = "(ID: 35) Training 'End Ride' aangemaakt.",
                    Timestamp = new DateTime(2026, 06, 15, 09, 40, 00)
                },
                new AuditLog
                {
                    ID = 16,
                    User = "SuperAdmin SuperAdmin",
                    Action = "Aangepast",
                    Details = "(ID: 33) Training 'July Training' is GEDEACTIVEERD. Reden: Extreme hittegolf voorspeld (>35°C).",
                    Timestamp = new DateTime(2026, 07, 10, 14, 22, 18)
                },
                new AuditLog
                {
                    ID = 17,
                    User = "Admin Admin",
                    Action = "Aangemaakt",
                    Details = "(ID: 36) Training 'September Restart' aangemaakt.",
                    Timestamp = new DateTime(2026, 08, 20, 11, 14, 55)
                },
                new AuditLog
                {
                    ID = 18,
                    User = "Admin Admin",
                    Action = "Aangemaakt",
                    Details = "(ID: 37) Training 'September Training' aangemaakt.",
                    Timestamp = new DateTime(2026, 08, 20, 11, 16, 40)
                },
                new AuditLog
                {
                    ID = 19,
                    User = "Admin Admin",
                    Action = "Aangemaakt",
                    Details = "(ID: 38) Training 'September Training' aangemaakt.",
                    Timestamp = new DateTime(2026, 08, 20, 11, 18, 12)
                },
                new AuditLog
                {
                    ID = 20,
                    User = "Admin Admin",
                    Action = "Aangemaakt",
                    Details = "(ID: 39) Training 'Autumn Ride' aangemaakt.",
                    Timestamp = new DateTime(2026, 08, 20, 11, 22, 01)
                },
                new AuditLog
                {
                    ID = 21,
                    User = "SuperAdmin SuperAdmin",
                    Action = "Verwijderd",
                    Details = "(ID: 88) Training 'Test Sessie Herfst' permanent verwijderd inclusief 3 bijbehorende registraties.",
                    Timestamp = new DateTime(2026, 09, 02, 16, 45, 33)
                },
                new AuditLog
                {
                    ID = 22,
                    User = "Admin Admin",
                    Action = "Aangepast",
                    Details = "(ID: 36) Training 'September Restart' gewijzigd.",
                    Timestamp = new DateTime(2026, 09, 04, 10, 08, 19)
                }
            );
        }

        private void SeedLocationData(ModelBuilder modelBuilder) =>
            modelBuilder.Entity<Location>().HasData(
                new Location { Id = 1, Name = "Expo Roeselare", IsActive = true }
            );

        private void SeedSubActivityData(ModelBuilder modelBuilder) =>
            modelBuilder.Entity<SubActivity>().HasData(
                new SubActivity { ID = 1, Name = "Basistraining", IsActive = true },
                new SubActivity { ID = 2, Name = "Hockey", IsActive = true },
                new SubActivity { ID = 3, Name = "Toerentocht", IsActive = true }
            );

        private void SeedBrandingData(ModelBuilder modelBuilder) =>
            modelBuilder.Entity<Setting>().HasData(
                new Setting
                {
                    ID = 1,
                    ButtonColor = "#FF6600",
                    BackgroundColor = "#f0f4f8",
                    LogoPath = "/images/logo.png",
                    DefaultButtonColor = "#FF6600",
                    DefaultBackgroundColor = "#1A1A2E"
                }
            );

        private void SeedTrainingData(ModelBuilder modelBuilder) =>
            modelBuilder.Entity<Training>().HasData(
                // --- November 2025 ---
                new Training { ID = 1, Title = "Winter Kickoff", Date = new DateTime(2025, 11, 2), StartTime = new TimeSpan(10, 0, 0), EndTime = new TimeSpan(12, 0, 0), IsActive = true },
                new Training { ID = 2, Title = "Winter Session", Date = new DateTime(2025, 11, 9), StartTime = new TimeSpan(10, 0, 0), EndTime = new TimeSpan(12, 0, 0), IsActive = true },
                new Training { ID = 3, Title = "Winter Session", Date = new DateTime(2025, 11, 16), StartTime = new TimeSpan(10, 0, 0), EndTime = new TimeSpan(12, 0, 0), IsActive = true },
                new Training { ID = 4, Title = "Winter Session", Date = new DateTime(2025, 11, 23), StartTime = new TimeSpan(10, 0, 0), EndTime = new TimeSpan(12, 0, 0), IsActive = true },
                new Training { ID = 5, Title = "Winter Session", Date = new DateTime(2025, 11, 30), StartTime = new TimeSpan(10, 0, 0), EndTime = new TimeSpan(12, 0, 0), IsActive = true },
                // --- January 2026 ---
                new Training { ID = 6, Title = "New Year Training", Date = new DateTime(2026, 1, 4), StartTime = new TimeSpan(10, 0, 0), EndTime = new TimeSpan(12, 0, 0), IsActive = true },
                new Training { ID = 7, Title = "January Session", Date = new DateTime(2026, 1, 11), StartTime = new TimeSpan(10, 0, 0), EndTime = new TimeSpan(12, 0, 0), IsActive = true },
                new Training { ID = 8, Title = "January Session", Date = new DateTime(2026, 1, 18), StartTime = new TimeSpan(10, 0, 0), EndTime = new TimeSpan(12, 0, 0), IsActive = true },
                new Training { ID = 9, Title = "January Session", Date = new DateTime(2026, 1, 25), StartTime = new TimeSpan(10, 0, 0), EndTime = new TimeSpan(12, 0, 0), IsActive = true },
                // --- February 2026 ---
                new Training { ID = 10, Title = "Training", Date = new DateTime(2026, 2, 1), StartTime = new TimeSpan(10, 0, 0), EndTime = new TimeSpan(12, 0, 0), IsActive = true },
                new Training { ID = 11, Title = "Training", Date = new DateTime(2026, 2, 8), StartTime = new TimeSpan(10, 0, 0), EndTime = new TimeSpan(12, 0, 0), IsActive = true },
                new Training { ID = 12, Title = "Training", Date = new DateTime(2026, 2, 15), StartTime = new TimeSpan(10, 0, 0), EndTime = new TimeSpan(12, 0, 0), IsActive = true },
                new Training { ID = 13, Title = "Training", Date = new DateTime(2026, 2, 22), StartTime = new TimeSpan(10, 0, 0), EndTime = new TimeSpan(12, 0, 0), IsActive = true },
                // --- March 2026 ---
                new Training { ID = 14, Title = "Training", Date = new DateTime(2026, 3, 1), StartTime = new TimeSpan(10, 0, 0), EndTime = new TimeSpan(12, 0, 0), IsActive = true },
                new Training { ID = 15, Title = "Training", Date = new DateTime(2026, 3, 8), StartTime = new TimeSpan(10, 0, 0), EndTime = new TimeSpan(12, 0, 0), IsActive = true },
                new Training { ID = 16, Title = "Training", Date = new DateTime(2026, 3, 15), StartTime = new TimeSpan(10, 0, 0), EndTime = new TimeSpan(12, 0, 0), IsActive = true },
                new Training { ID = 17, Title = "Training", Date = new DateTime(2026, 3, 22), StartTime = new TimeSpan(10, 0, 0), EndTime = new TimeSpan(12, 0, 0), IsActive = true },
                new Training { ID = 18, Title = "Toertocht", Date = new DateTime(2026, 3, 29), StartTime = new TimeSpan(10, 0, 0), EndTime = new TimeSpan(13, 0, 0), IsActive = true },
                // --- April 2026 ---
                new Training { ID = 19, Title = "Training", Date = new DateTime(2026, 4, 5), StartTime = new TimeSpan(10, 0, 0), EndTime = new TimeSpan(12, 0, 0), IsActive = true },
                new Training { ID = 20, Title = "Training", Date = new DateTime(2026, 4, 12), StartTime = new TimeSpan(10, 0, 0), EndTime = new TimeSpan(12, 0, 0), IsActive = true },
                new Training { ID = 21, Title = "Training", Date = new DateTime(2026, 4, 19), StartTime = new TimeSpan(10, 0, 0), EndTime = new TimeSpan(12, 0, 0), IsActive = true },
                new Training { ID = 22, Title = "Training", Date = new DateTime(2026, 4, 26), StartTime = new TimeSpan(10, 0, 0), EndTime = new TimeSpan(12, 0, 0), IsActive = true },
                // --- May 2026 ---
                new Training { ID = 23, Title = "Training", Date = new DateTime(2026, 5, 3), StartTime = new TimeSpan(10, 0, 0), EndTime = new TimeSpan(12, 0, 0), IsActive = true },
                new Training { ID = 24, Title = "Training", Date = new DateTime(2026, 5, 10), StartTime = new TimeSpan(10, 0, 0), EndTime = new TimeSpan(12, 0, 0), IsActive = true },
                new Training { ID = 25, Title = "Training", Date = new DateTime(2026, 5, 17), StartTime = new TimeSpan(10, 0, 0), EndTime = new TimeSpan(12, 0, 0), IsActive = true },
                new Training { ID = 26, Title = "Open Ride", Date = new DateTime(2026, 5, 24), StartTime = new TimeSpan(19, 30, 0), EndTime = new TimeSpan(21, 0, 0), IsActive = true },
                new Training { ID = 27, Title = "Festival Ride", Date = new DateTime(2026, 5, 31), StartTime = new TimeSpan(13, 0, 0), EndTime = new TimeSpan(17, 0, 0), IsActive = true },
                // --- June 2026 ---
                new Training { ID = 28, Title = "June Training", Date = new DateTime(2026, 6, 7), StartTime = new TimeSpan(10, 0, 0), EndTime = new TimeSpan(12, 0, 0), IsActive = true },
                new Training { ID = 29, Title = "June Training", Date = new DateTime(2026, 6, 14), StartTime = new TimeSpan(10, 0, 0), EndTime = new TimeSpan(12, 0, 0), IsActive = true },
                new Training { ID = 30, Title = "June Training", Date = new DateTime(2026, 6, 21), StartTime = new TimeSpan(10, 0, 0), EndTime = new TimeSpan(12, 0, 0), IsActive = true },
                new Training { ID = 31, Title = "Summer Ride", Date = new DateTime(2026, 6, 28), StartTime = new TimeSpan(10, 0, 0), EndTime = new TimeSpan(13, 0, 0), IsActive = true },
                // --- July 2026 ---
                new Training { ID = 32, Title = "July Training", Date = new DateTime(2026, 7, 5), StartTime = new TimeSpan(10, 0, 0), EndTime = new TimeSpan(12, 0, 0), IsActive = true },
                new Training { ID = 33, Title = "July Training", Date = new DateTime(2026, 7, 12), StartTime = new TimeSpan(10, 0, 0), EndTime = new TimeSpan(12, 0, 0), IsActive = true },
                new Training { ID = 34, Title = "July Training", Date = new DateTime(2026, 7, 19), StartTime = new TimeSpan(10, 0, 0), EndTime = new TimeSpan(12, 0, 0), IsActive = true },
                new Training { ID = 35, Title = "End Ride", Date = new DateTime(2026, 7, 26), StartTime = new TimeSpan(10, 0, 0), EndTime = new TimeSpan(14, 0, 0), IsActive = true },
                // --- September 2026 ---
                new Training { ID = 36, Title = "September Restart", Date = new DateTime(2026, 9, 6), StartTime = new TimeSpan(10, 0, 0), EndTime = new TimeSpan(12, 0, 0), IsActive = true },
                new Training { ID = 37, Title = "September Training", Date = new DateTime(2026, 9, 13), StartTime = new TimeSpan(10, 0, 0), EndTime = new TimeSpan(12, 0, 0), IsActive = true },
                new Training { ID = 38, Title = "September Training", Date = new DateTime(2026, 9, 20), StartTime = new TimeSpan(10, 0, 0), EndTime = new TimeSpan(12, 0, 0), IsActive = true },
                new Training { ID = 39, Title = "Autumn Ride", Date = new DateTime(2026, 9, 27), StartTime = new TimeSpan(10, 0, 0), EndTime = new TimeSpan(13, 0, 0), IsActive = true },
                // --- October 2026 ---
                new Training { ID = 40, Title = "October Training", Date = new DateTime(2026, 10, 4), StartTime = new TimeSpan(10, 0, 0), EndTime = new TimeSpan(12, 0, 0), IsActive = true },
                new Training { ID = 41, Title = "October Training", Date = new DateTime(2026, 10, 11), StartTime = new TimeSpan(10, 0, 0), EndTime = new TimeSpan(12, 0, 0), IsActive = true },
                new Training { ID = 42, Title = "October Training", Date = new DateTime(2026, 10, 18), StartTime = new TimeSpan(10, 0, 0), EndTime = new TimeSpan(12, 0, 0), IsActive = true },
                new Training { ID = 43, Title = "Halloween Ride", Date = new DateTime(2026, 10, 25), StartTime = new TimeSpan(18, 0, 0), EndTime = new TimeSpan(21, 0, 0), IsActive = true },
                // --- November 2026 ---
                new Training { ID = 44, Title = "November Training", Date = new DateTime(2026, 11, 1), StartTime = new TimeSpan(10, 0, 0), EndTime = new TimeSpan(12, 0, 0), IsActive = true },
                new Training { ID = 45, Title = "November Training", Date = new DateTime(2026, 11, 8), StartTime = new TimeSpan(10, 0, 0), EndTime = new TimeSpan(12, 0, 0), IsActive = true },
                new Training { ID = 46, Title = "November Training", Date = new DateTime(2026, 11, 15), StartTime = new TimeSpan(10, 0, 0), EndTime = new TimeSpan(12, 0, 0), IsActive = true },
                new Training { ID = 47, Title = "Winter Prep", Date = new DateTime(2026, 11, 22), StartTime = new TimeSpan(10, 0, 0), EndTime = new TimeSpan(12, 0, 0), IsActive = true },
                // --- December 2026 ---
                new Training { ID = 48, Title = "December Training", Date = new DateTime(2026, 12, 6), StartTime = new TimeSpan(10, 0, 0), EndTime = new TimeSpan(12, 0, 0), IsActive = true },
                new Training { ID = 49, Title = "December Training", Date = new DateTime(2026, 12, 13), StartTime = new TimeSpan(10, 0, 0), EndTime = new TimeSpan(12, 0, 0), IsActive = true },
                new Training { ID = 50, Title = "Year End Ride", Date = new DateTime(2026, 12, 27), StartTime = new TimeSpan(10, 0, 0), EndTime = new TimeSpan(13, 0, 0), IsActive = true },
                // --- January 2027 ---
                new Training { ID = 51, Title = "Kickoff 2027", Date = new DateTime(2027, 1, 3), StartTime = new TimeSpan(10, 0, 0), EndTime = new TimeSpan(12, 0, 0), IsActive = true },
                new Training { ID = 52, Title = "January Training", Date = new DateTime(2027, 1, 10), StartTime = new TimeSpan(10, 0, 0), EndTime = new TimeSpan(12, 0, 0), IsActive = true },
                new Training { ID = 53, Title = "January Training", Date = new DateTime(2027, 1, 17), StartTime = new TimeSpan(10, 0, 0), EndTime = new TimeSpan(12, 0, 0), IsActive = true },
                new Training { ID = 54, Title = "January Training", Date = new DateTime(2027, 1, 24), StartTime = new TimeSpan(10, 0, 0), EndTime = new TimeSpan(12, 0, 0), IsActive = true }
            );

        private void SeedTrainingSubActivityData(ModelBuilder modelBuilder)
        {
            var items = new List<TrainingSubActivity>();

            for (int i = 1; i <= 54; i++)
            {
                if (i <= 25)
                {
                    items.Add(new TrainingSubActivity { TrainingID = i, SubActivityID = 1 });
                    items.Add(new TrainingSubActivity { TrainingID = i, SubActivityID = 2 });
                    items.Add(new TrainingSubActivity { TrainingID = i, SubActivityID = 3 });
                }
                else if (new[] { 26, 27, 31, 35, 39, 43, 47, 50 }.Contains(i))
                {
                    items.Add(new TrainingSubActivity { TrainingID = i, SubActivityID = 3 });
                }
                else
                {
                    items.Add(new TrainingSubActivity { TrainingID = i, SubActivityID = 1 });
                    items.Add(new TrainingSubActivity { TrainingID = i, SubActivityID = 2 });
                }
            }

            modelBuilder.Entity<TrainingSubActivity>().HasData(items);
        }
    }
}
