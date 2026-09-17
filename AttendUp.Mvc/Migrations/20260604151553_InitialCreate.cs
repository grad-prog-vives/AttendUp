using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AttendUp.Mvc.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AuditLogs",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    User = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Action = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Details = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditLogs", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Location",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Location", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PasswordExpiries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastPasswordChanged = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsBlockedDueToExpiry = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PasswordExpiries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Settings",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ButtonColor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BackgroundColor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LogoPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DefaultButtonColor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DefaultBackgroundColor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EventBannerPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RegistrationWindowMinutes = table.Column<int>(type: "int", nullable: false),
                    BoardSessionTimeoutMinutes = table.Column<int>(type: "int", nullable: false),
                    StoreIpAddresses = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Settings", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "SubActivity",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubActivity", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "SystemLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LogLevel = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Exception = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IpAddress = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemLogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Training",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StartTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    EndTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    DeactivationReason = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Training", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Families",
                columns: table => new
                {
                    FamilyId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FamilyName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OwnerPersonId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Families", x => x.FamilyId);
                    table.ForeignKey(
                        name: "FK_Families_AspNetUsers_OwnerPersonId",
                        column: x => x.OwnerPersonId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Registration",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TrainingID = table.Column<int>(type: "int", nullable: false),
                    SubActivityID = table.Column<int>(type: "int", nullable: false),
                    LocationId = table.Column<int>(type: "int", nullable: false),
                    RegisteredAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    isValid = table.Column<bool>(type: "bit", nullable: false),
                    IpAddress = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Registration", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Registration_Location_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Location",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Registration_SubActivity_SubActivityID",
                        column: x => x.SubActivityID,
                        principalTable: "SubActivity",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Registration_Training_TrainingID",
                        column: x => x.TrainingID,
                        principalTable: "Training",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TrainingSubActivity",
                columns: table => new
                {
                    TrainingID = table.Column<int>(type: "int", nullable: false),
                    SubActivityID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainingSubActivity", x => new { x.TrainingID, x.SubActivityID });
                    table.ForeignKey(
                        name: "FK_TrainingSubActivity_SubActivity_SubActivityID",
                        column: x => x.SubActivityID,
                        principalTable: "SubActivity",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TrainingSubActivity_Training_TrainingID",
                        column: x => x.TrainingID,
                        principalTable: "Training",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FamilyMembers",
                columns: table => new
                {
                    FamilyMemberId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FamilyId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FamilyMembers", x => x.FamilyMemberId);
                    table.ForeignKey(
                        name: "FK_FamilyMembers_Families_FamilyId",
                        column: x => x.FamilyId,
                        principalTable: "Families",
                        principalColumn: "FamilyId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AuditLogs",
                columns: new[] { "ID", "Action", "Details", "Timestamp", "User" },
                values: new object[,]
                {
                    { 1, "Aangemaakt", "(ID: 1) Training 'Winter Kickoff' aangemaakt", new DateTime(2025, 10, 15, 14, 22, 10, 0, DateTimeKind.Unspecified), "Admin Admin" },
                    { 2, "Aangemaakt", "(ID: 2) Training 'Winter Session' aangemaakt.", new DateTime(2025, 10, 15, 14, 25, 0, 0, DateTimeKind.Unspecified), "Admin Admin" },
                    { 3, "Aangepast", "(ID: 6) Training 'New Year Training' gewijzigd.", new DateTime(2025, 12, 28, 11, 5, 43, 0, DateTimeKind.Unspecified), "Superadmin Superadmin" },
                    { 4, "Verwijderd", "(ID: 99) Training 'Oude Test Training' permanent verwijderd inclusief 15 bijbehorende registraties.", new DateTime(2026, 1, 10, 9, 15, 22, 0, DateTimeKind.Unspecified), "Admin Admin" },
                    { 5, "Aangepast", "(ID: 18) Training 'Toertocht' is GEDEACTIVEERD. Reden: Slechte weersomstandigheden (storm).", new DateTime(2026, 3, 28, 17, 40, 11, 0, DateTimeKind.Unspecified), "Admin Admin" },
                    { 6, "Aangepast", "(ID: 18) Training 'Toertocht' is op ACTIEF gezet.", new DateTime(2026, 3, 29, 7, 30, 0, 0, DateTimeKind.Unspecified), "Superadmin Superadmin" },
                    { 7, "Aangemaakt", "(ID: 28) Training 'June Training' aangemaakt.", new DateTime(2026, 5, 20, 19, 2, 15, 0, DateTimeKind.Unspecified), "Admin Admin" },
                    { 8, "Aangemaakt", "(ID: 29) Training 'June Training' aangemaakt.", new DateTime(2026, 5, 20, 19, 4, 50, 0, DateTimeKind.Unspecified), "Admin Admin" },
                    { 9, "Aangemaakt", "(ID: 30) Training 'June Training' aangemaakt.", new DateTime(2026, 5, 20, 19, 6, 12, 0, DateTimeKind.Unspecified), "Admin Admin" },
                    { 10, "Aangemaakt", "(ID: 31) Training 'Summer Ride' aangemaakt.", new DateTime(2026, 5, 20, 19, 10, 5, 0, DateTimeKind.Unspecified), "Admin Admin" },
                    { 11, "Aanpassen", "(ID: 31) Training 'Summer Ride' gewijzigd.", new DateTime(2026, 5, 22, 10, 15, 30, 0, DateTimeKind.Unspecified), "SuperAdmin SuperAdmin" },
                    { 12, "Aangemaakt", "(ID: 32) Training 'July Training' aangemaakt.", new DateTime(2026, 6, 15, 9, 30, 22, 0, DateTimeKind.Unspecified), "Admin Admin" },
                    { 13, "Aangemaakt", "(ID: 33) Training 'July Training' aangemaakt.", new DateTime(2026, 6, 15, 9, 32, 45, 0, DateTimeKind.Unspecified), "Admin Admin" },
                    { 14, "Aangemaakt", "(ID: 34) Training 'July Training' aangemaakt.", new DateTime(2026, 6, 15, 9, 35, 10, 0, DateTimeKind.Unspecified), "Admin Admin" },
                    { 15, "Aangemaakt", "(ID: 35) Training 'End Ride' aangemaakt.", new DateTime(2026, 6, 15, 9, 40, 0, 0, DateTimeKind.Unspecified), "Admin Admin" },
                    { 16, "Aangepast", "(ID: 33) Training 'July Training' is GEDEACTIVEERD. Reden: Extreme hittegolf voorspeld (>35°C).", new DateTime(2026, 7, 10, 14, 22, 18, 0, DateTimeKind.Unspecified), "SuperAdmin SuperAdmin" },
                    { 17, "Aangemaakt", "(ID: 36) Training 'September Restart' aangemaakt.", new DateTime(2026, 8, 20, 11, 14, 55, 0, DateTimeKind.Unspecified), "Admin Admin" },
                    { 18, "Aangemaakt", "(ID: 37) Training 'September Training' aangemaakt.", new DateTime(2026, 8, 20, 11, 16, 40, 0, DateTimeKind.Unspecified), "Admin Admin" },
                    { 19, "Aangemaakt", "(ID: 38) Training 'September Training' aangemaakt.", new DateTime(2026, 8, 20, 11, 18, 12, 0, DateTimeKind.Unspecified), "Admin Admin" },
                    { 20, "Aangemaakt", "(ID: 39) Training 'Autumn Ride' aangemaakt.", new DateTime(2026, 8, 20, 11, 22, 1, 0, DateTimeKind.Unspecified), "Admin Admin" },
                    { 21, "Verwijderd", "(ID: 88) Training 'Test Sessie Herfst' permanent verwijderd inclusief 3 bijbehorende registraties.", new DateTime(2026, 9, 2, 16, 45, 33, 0, DateTimeKind.Unspecified), "SuperAdmin SuperAdmin" },
                    { 22, "Aangepast", "(ID: 36) Training 'September Restart' gewijzigd.", new DateTime(2026, 9, 4, 10, 8, 19, 0, DateTimeKind.Unspecified), "Admin Admin" }
                });

            migrationBuilder.InsertData(
                table: "Location",
                columns: new[] { "Id", "IsActive", "Name" },
                values: new object[] { 1, true, "Expo Roeselare" });

            migrationBuilder.InsertData(
                table: "Settings",
                columns: new[] { "ID", "BackgroundColor", "BoardSessionTimeoutMinutes", "ButtonColor", "DefaultBackgroundColor", "DefaultButtonColor", "EventBannerPath", "LogoPath", "RegistrationWindowMinutes", "StoreIpAddresses" },
                values: new object[] { 1, "#f0f4f8", 60, "#FF6600", "#1A1A2E", "#FF6600", null, "/images/logo.png", 30, false });

            migrationBuilder.InsertData(
                table: "SubActivity",
                columns: new[] { "ID", "IsActive", "Name" },
                values: new object[,]
                {
                    { 1, true, "Basistraining" },
                    { 2, true, "Hockey" },
                    { 3, true, "Toerentocht" }
                });

            migrationBuilder.InsertData(
                table: "Training",
                columns: new[] { "ID", "Date", "DeactivationReason", "EndTime", "IsActive", "StartTime", "Title" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 11, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new TimeSpan(0, 12, 0, 0, 0), true, new TimeSpan(0, 10, 0, 0, 0), "Winter Kickoff" },
                    { 2, new DateTime(2025, 11, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new TimeSpan(0, 12, 0, 0, 0), true, new TimeSpan(0, 10, 0, 0, 0), "Winter Session" },
                    { 3, new DateTime(2025, 11, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new TimeSpan(0, 12, 0, 0, 0), true, new TimeSpan(0, 10, 0, 0, 0), "Winter Session" },
                    { 4, new DateTime(2025, 11, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new TimeSpan(0, 12, 0, 0, 0), true, new TimeSpan(0, 10, 0, 0, 0), "Winter Session" },
                    { 5, new DateTime(2025, 11, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new TimeSpan(0, 12, 0, 0, 0), true, new TimeSpan(0, 10, 0, 0, 0), "Winter Session" },
                    { 6, new DateTime(2026, 1, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new TimeSpan(0, 12, 0, 0, 0), true, new TimeSpan(0, 10, 0, 0, 0), "New Year Training" },
                    { 7, new DateTime(2026, 1, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new TimeSpan(0, 12, 0, 0, 0), true, new TimeSpan(0, 10, 0, 0, 0), "January Session" },
                    { 8, new DateTime(2026, 1, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new TimeSpan(0, 12, 0, 0, 0), true, new TimeSpan(0, 10, 0, 0, 0), "January Session" },
                    { 9, new DateTime(2026, 1, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new TimeSpan(0, 12, 0, 0, 0), true, new TimeSpan(0, 10, 0, 0, 0), "January Session" },
                    { 10, new DateTime(2026, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new TimeSpan(0, 12, 0, 0, 0), true, new TimeSpan(0, 10, 0, 0, 0), "Training" },
                    { 11, new DateTime(2026, 2, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new TimeSpan(0, 12, 0, 0, 0), true, new TimeSpan(0, 10, 0, 0, 0), "Training" },
                    { 12, new DateTime(2026, 2, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new TimeSpan(0, 12, 0, 0, 0), true, new TimeSpan(0, 10, 0, 0, 0), "Training" },
                    { 13, new DateTime(2026, 2, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new TimeSpan(0, 12, 0, 0, 0), true, new TimeSpan(0, 10, 0, 0, 0), "Training" },
                    { 14, new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new TimeSpan(0, 12, 0, 0, 0), true, new TimeSpan(0, 10, 0, 0, 0), "Training" },
                    { 15, new DateTime(2026, 3, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new TimeSpan(0, 12, 0, 0, 0), true, new TimeSpan(0, 10, 0, 0, 0), "Training" },
                    { 16, new DateTime(2026, 3, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new TimeSpan(0, 12, 0, 0, 0), true, new TimeSpan(0, 10, 0, 0, 0), "Training" },
                    { 17, new DateTime(2026, 3, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new TimeSpan(0, 12, 0, 0, 0), true, new TimeSpan(0, 10, 0, 0, 0), "Training" },
                    { 18, new DateTime(2026, 3, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new TimeSpan(0, 13, 0, 0, 0), true, new TimeSpan(0, 10, 0, 0, 0), "Toertocht" },
                    { 19, new DateTime(2026, 4, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new TimeSpan(0, 12, 0, 0, 0), true, new TimeSpan(0, 10, 0, 0, 0), "Training" },
                    { 20, new DateTime(2026, 4, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new TimeSpan(0, 12, 0, 0, 0), true, new TimeSpan(0, 10, 0, 0, 0), "Training" },
                    { 21, new DateTime(2026, 4, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new TimeSpan(0, 12, 0, 0, 0), true, new TimeSpan(0, 10, 0, 0, 0), "Training" },
                    { 22, new DateTime(2026, 4, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new TimeSpan(0, 12, 0, 0, 0), true, new TimeSpan(0, 10, 0, 0, 0), "Training" },
                    { 23, new DateTime(2026, 5, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new TimeSpan(0, 12, 0, 0, 0), true, new TimeSpan(0, 10, 0, 0, 0), "Training" },
                    { 24, new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new TimeSpan(0, 12, 0, 0, 0), true, new TimeSpan(0, 10, 0, 0, 0), "Training" },
                    { 25, new DateTime(2026, 5, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new TimeSpan(0, 12, 0, 0, 0), true, new TimeSpan(0, 10, 0, 0, 0), "Training" },
                    { 26, new DateTime(2026, 5, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new TimeSpan(0, 21, 0, 0, 0), true, new TimeSpan(0, 19, 30, 0, 0), "Open Ride" },
                    { 27, new DateTime(2026, 5, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new TimeSpan(0, 17, 0, 0, 0), true, new TimeSpan(0, 13, 0, 0, 0), "Festival Ride" },
                    { 28, new DateTime(2026, 6, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new TimeSpan(0, 12, 0, 0, 0), true, new TimeSpan(0, 10, 0, 0, 0), "June Training" },
                    { 29, new DateTime(2026, 6, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new TimeSpan(0, 12, 0, 0, 0), true, new TimeSpan(0, 10, 0, 0, 0), "June Training" },
                    { 30, new DateTime(2026, 6, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new TimeSpan(0, 12, 0, 0, 0), true, new TimeSpan(0, 10, 0, 0, 0), "June Training" },
                    { 31, new DateTime(2026, 6, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new TimeSpan(0, 13, 0, 0, 0), true, new TimeSpan(0, 10, 0, 0, 0), "Summer Ride" },
                    { 32, new DateTime(2026, 7, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new TimeSpan(0, 12, 0, 0, 0), true, new TimeSpan(0, 10, 0, 0, 0), "July Training" },
                    { 33, new DateTime(2026, 7, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new TimeSpan(0, 12, 0, 0, 0), true, new TimeSpan(0, 10, 0, 0, 0), "July Training" },
                    { 34, new DateTime(2026, 7, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new TimeSpan(0, 12, 0, 0, 0), true, new TimeSpan(0, 10, 0, 0, 0), "July Training" },
                    { 35, new DateTime(2026, 7, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new TimeSpan(0, 14, 0, 0, 0), true, new TimeSpan(0, 10, 0, 0, 0), "End Ride" },
                    { 36, new DateTime(2026, 9, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new TimeSpan(0, 12, 0, 0, 0), true, new TimeSpan(0, 10, 0, 0, 0), "September Restart" },
                    { 37, new DateTime(2026, 9, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new TimeSpan(0, 12, 0, 0, 0), true, new TimeSpan(0, 10, 0, 0, 0), "September Training" },
                    { 38, new DateTime(2026, 9, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new TimeSpan(0, 12, 0, 0, 0), true, new TimeSpan(0, 10, 0, 0, 0), "September Training" },
                    { 39, new DateTime(2026, 9, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new TimeSpan(0, 13, 0, 0, 0), true, new TimeSpan(0, 10, 0, 0, 0), "Autumn Ride" },
                    { 40, new DateTime(2026, 10, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new TimeSpan(0, 12, 0, 0, 0), true, new TimeSpan(0, 10, 0, 0, 0), "October Training" },
                    { 41, new DateTime(2026, 10, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new TimeSpan(0, 12, 0, 0, 0), true, new TimeSpan(0, 10, 0, 0, 0), "October Training" },
                    { 42, new DateTime(2026, 10, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new TimeSpan(0, 12, 0, 0, 0), true, new TimeSpan(0, 10, 0, 0, 0), "October Training" },
                    { 43, new DateTime(2026, 10, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new TimeSpan(0, 21, 0, 0, 0), true, new TimeSpan(0, 18, 0, 0, 0), "Halloween Ride" },
                    { 44, new DateTime(2026, 11, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new TimeSpan(0, 12, 0, 0, 0), true, new TimeSpan(0, 10, 0, 0, 0), "November Training" },
                    { 45, new DateTime(2026, 11, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new TimeSpan(0, 12, 0, 0, 0), true, new TimeSpan(0, 10, 0, 0, 0), "November Training" },
                    { 46, new DateTime(2026, 11, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new TimeSpan(0, 12, 0, 0, 0), true, new TimeSpan(0, 10, 0, 0, 0), "November Training" },
                    { 47, new DateTime(2026, 11, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new TimeSpan(0, 12, 0, 0, 0), true, new TimeSpan(0, 10, 0, 0, 0), "Winter Prep" },
                    { 48, new DateTime(2026, 12, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new TimeSpan(0, 12, 0, 0, 0), true, new TimeSpan(0, 10, 0, 0, 0), "December Training" },
                    { 49, new DateTime(2026, 12, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new TimeSpan(0, 12, 0, 0, 0), true, new TimeSpan(0, 10, 0, 0, 0), "December Training" },
                    { 50, new DateTime(2026, 12, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new TimeSpan(0, 13, 0, 0, 0), true, new TimeSpan(0, 10, 0, 0, 0), "Year End Ride" },
                    { 51, new DateTime(2027, 1, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new TimeSpan(0, 12, 0, 0, 0), true, new TimeSpan(0, 10, 0, 0, 0), "Kickoff 2027" },
                    { 52, new DateTime(2027, 1, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new TimeSpan(0, 12, 0, 0, 0), true, new TimeSpan(0, 10, 0, 0, 0), "January Training" },
                    { 53, new DateTime(2027, 1, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new TimeSpan(0, 12, 0, 0, 0), true, new TimeSpan(0, 10, 0, 0, 0), "January Training" },
                    { 54, new DateTime(2027, 1, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new TimeSpan(0, 12, 0, 0, 0), true, new TimeSpan(0, 10, 0, 0, 0), "January Training" }
                });

            migrationBuilder.InsertData(
                table: "TrainingSubActivity",
                columns: new[] { "SubActivityID", "TrainingID" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 2, 1 },
                    { 3, 1 },
                    { 1, 2 },
                    { 2, 2 },
                    { 3, 2 },
                    { 1, 3 },
                    { 2, 3 },
                    { 3, 3 },
                    { 1, 4 },
                    { 2, 4 },
                    { 3, 4 },
                    { 1, 5 },
                    { 2, 5 },
                    { 3, 5 },
                    { 1, 6 },
                    { 2, 6 },
                    { 3, 6 },
                    { 1, 7 },
                    { 2, 7 },
                    { 3, 7 },
                    { 1, 8 },
                    { 2, 8 },
                    { 3, 8 },
                    { 1, 9 },
                    { 2, 9 },
                    { 3, 9 },
                    { 1, 10 },
                    { 2, 10 },
                    { 3, 10 },
                    { 1, 11 },
                    { 2, 11 },
                    { 3, 11 },
                    { 1, 12 },
                    { 2, 12 },
                    { 3, 12 },
                    { 1, 13 },
                    { 2, 13 },
                    { 3, 13 },
                    { 1, 14 },
                    { 2, 14 },
                    { 3, 14 },
                    { 1, 15 },
                    { 2, 15 },
                    { 3, 15 },
                    { 1, 16 },
                    { 2, 16 },
                    { 3, 16 },
                    { 1, 17 },
                    { 2, 17 },
                    { 3, 17 },
                    { 1, 18 },
                    { 2, 18 },
                    { 3, 18 },
                    { 1, 19 },
                    { 2, 19 },
                    { 3, 19 },
                    { 1, 20 },
                    { 2, 20 },
                    { 3, 20 },
                    { 1, 21 },
                    { 2, 21 },
                    { 3, 21 },
                    { 1, 22 },
                    { 2, 22 },
                    { 3, 22 },
                    { 1, 23 },
                    { 2, 23 },
                    { 3, 23 },
                    { 1, 24 },
                    { 2, 24 },
                    { 3, 24 },
                    { 1, 25 },
                    { 2, 25 },
                    { 3, 25 },
                    { 3, 26 },
                    { 3, 27 },
                    { 1, 28 },
                    { 2, 28 },
                    { 1, 29 },
                    { 2, 29 },
                    { 1, 30 },
                    { 2, 30 },
                    { 3, 31 },
                    { 1, 32 },
                    { 2, 32 },
                    { 1, 33 },
                    { 2, 33 },
                    { 1, 34 },
                    { 2, 34 },
                    { 3, 35 },
                    { 1, 36 },
                    { 2, 36 },
                    { 1, 37 },
                    { 2, 37 },
                    { 1, 38 },
                    { 2, 38 },
                    { 3, 39 },
                    { 1, 40 },
                    { 2, 40 },
                    { 1, 41 },
                    { 2, 41 },
                    { 1, 42 },
                    { 2, 42 },
                    { 3, 43 },
                    { 1, 44 },
                    { 2, 44 },
                    { 1, 45 },
                    { 2, 45 },
                    { 1, 46 },
                    { 2, 46 },
                    { 3, 47 },
                    { 1, 48 },
                    { 2, 48 },
                    { 1, 49 },
                    { 2, 49 },
                    { 3, 50 },
                    { 1, 51 },
                    { 2, 51 },
                    { 1, 52 },
                    { 2, 52 },
                    { 1, 53 },
                    { 2, 53 },
                    { 1, 54 },
                    { 2, 54 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Families_OwnerPersonId",
                table: "Families",
                column: "OwnerPersonId");

            migrationBuilder.CreateIndex(
                name: "IX_FamilyMembers_FamilyId",
                table: "FamilyMembers",
                column: "FamilyId");

            migrationBuilder.CreateIndex(
                name: "IX_Registration_LocationId",
                table: "Registration",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Registration_SubActivityID",
                table: "Registration",
                column: "SubActivityID");

            migrationBuilder.CreateIndex(
                name: "IX_Registration_TrainingID",
                table: "Registration",
                column: "TrainingID");

            migrationBuilder.CreateIndex(
                name: "IX_TrainingSubActivity_SubActivityID",
                table: "TrainingSubActivity",
                column: "SubActivityID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "AuditLogs");

            migrationBuilder.DropTable(
                name: "FamilyMembers");

            migrationBuilder.DropTable(
                name: "PasswordExpiries");

            migrationBuilder.DropTable(
                name: "Registration");

            migrationBuilder.DropTable(
                name: "Settings");

            migrationBuilder.DropTable(
                name: "SystemLogs");

            migrationBuilder.DropTable(
                name: "TrainingSubActivity");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Families");

            migrationBuilder.DropTable(
                name: "Location");

            migrationBuilder.DropTable(
                name: "SubActivity");

            migrationBuilder.DropTable(
                name: "Training");

            migrationBuilder.DropTable(
                name: "AspNetUsers");
        }
    }
}
