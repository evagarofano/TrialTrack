using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrialTrack.Migrations
{
    /// <inheritdoc />
    public partial class AddMonthlyRecruitmentTarget : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MonthlyRecruitmentTarget",
                table: "Sites",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MonthlyRecruitmentTarget",
                table: "Sites");
        }
    }
}
