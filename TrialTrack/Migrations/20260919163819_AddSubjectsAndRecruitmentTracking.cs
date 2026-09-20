using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrialTrack.Migrations
{
    /// <inheritdoc />
    public partial class AddSubjectsAndRecruitmentTracking : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateOnly>(
                name: "FinalRandomisationDate",
                table: "Studies",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<DateOnly>(
                name: "RecruitmentStartDate",
                table: "Studies",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RecruitmentTarget",
                table: "Studies",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateOnly>(
                name: "ScreeningCloseDate",
                table: "Studies",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ScreeningWindowDays",
                table: "Studies",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Subjects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SubjectNumber = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SiteId = table.Column<int>(type: "int", nullable: false),
                    RecruitmentStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PreScreenDate = table.Column<DateOnly>(type: "date", nullable: true),
                    ScreeningDate = table.Column<DateOnly>(type: "date", nullable: true),
                    RandomisationDate = table.Column<DateOnly>(type: "date", nullable: true),
                    ScreenFailDate = table.Column<DateOnly>(type: "date", nullable: true),
                    ScreenFailReason = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subjects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Subjects_Sites_SiteId",
                        column: x => x.SiteId,
                        principalTable: "Sites",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Subjects_SiteId",
                table: "Subjects",
                column: "SiteId");

            migrationBuilder.CreateIndex(
                name: "IX_Subjects_SubjectNumber",
                table: "Subjects",
                column: "SubjectNumber",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Subjects");

            migrationBuilder.DropColumn(
                name: "FinalRandomisationDate",
                table: "Studies");

            migrationBuilder.DropColumn(
                name: "RecruitmentStartDate",
                table: "Studies");

            migrationBuilder.DropColumn(
                name: "RecruitmentTarget",
                table: "Studies");

            migrationBuilder.DropColumn(
                name: "ScreeningCloseDate",
                table: "Studies");

            migrationBuilder.DropColumn(
                name: "ScreeningWindowDays",
                table: "Studies");
        }
    }
}
