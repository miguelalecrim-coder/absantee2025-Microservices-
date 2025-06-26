﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class PendingChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AssociationsProjectCollaborator");

            migrationBuilder.DropTable(
                name: "AssociationTrainingModuleCollaborators");

            migrationBuilder.DropTable(
                name: "Collaborators");

            migrationBuilder.DropTable(
                name: "HolidayPeriods");

            migrationBuilder.DropTable(
                name: "HRManagers");

            migrationBuilder.DropTable(
                name: "TrainingModules_Periods");

            migrationBuilder.DropTable(
                name: "TrainingPeriods");

            migrationBuilder.DropTable(
                name: "TrainingSubjects");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "HolidayPlans");

            migrationBuilder.DropTable(
                name: "TrainingModules");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AssociationsProjectCollaborator",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CollaboratorId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProjectId = table.Column<Guid>(type: "uuid", nullable: false),
                    PeriodDate_FinalDate = table.Column<DateOnly>(type: "date", nullable: false),
                    PeriodDate_InitDate = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssociationsProjectCollaborator", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AssociationTrainingModuleCollaborators",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CollaboratorId = table.Column<Guid>(type: "uuid", nullable: false),
                    TrainingModuleId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssociationTrainingModuleCollaborators", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Collaborators",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    PeriodDateTime__finalDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    PeriodDateTime__initDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Collaborators", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HolidayPlans",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CollaboratorId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HolidayPlans", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HRManagers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    PeriodDateTime__finalDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    PeriodDateTime__initDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HRManagers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TrainingModules",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TrainingSubjectId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainingModules", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TrainingPeriods",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PeriodDate_FinalDate = table.Column<DateOnly>(type: "date", nullable: false),
                    PeriodDate_InitDate = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainingPeriods", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TrainingSubjects",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Subject = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainingSubjects", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    Names = table.Column<string>(type: "text", nullable: false),
                    Surnames = table.Column<string>(type: "text", nullable: false),
                    PeriodDateTime__finalDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    PeriodDateTime__initDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HolidayPeriods",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    HolidayPlanDataModelId = table.Column<Guid>(type: "uuid", nullable: true),
                    PeriodDate_FinalDate = table.Column<DateOnly>(type: "date", nullable: false),
                    PeriodDate_InitDate = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HolidayPeriods", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HolidayPeriods_HolidayPlans_HolidayPlanDataModelId",
                        column: x => x.HolidayPlanDataModelId,
                        principalTable: "HolidayPlans",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TrainingModules_Periods",
                columns: table => new
                {
                    TrainingModuleDataModelId = table.Column<Guid>(type: "uuid", nullable: false),
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    _finalDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    _initDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainingModules_Periods", x => new { x.TrainingModuleDataModelId, x.Id });
                    table.ForeignKey(
                        name: "FK_TrainingModules_Periods_TrainingModules_TrainingModuleDataM~",
                        column: x => x.TrainingModuleDataModelId,
                        principalTable: "TrainingModules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HolidayPeriods_HolidayPlanDataModelId",
                table: "HolidayPeriods",
                column: "HolidayPlanDataModelId");
        }
    }
}
