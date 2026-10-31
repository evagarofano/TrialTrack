using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrialTrack.Migrations
{
    /// <inheritdoc />
    public partial class AddSiteRecruitmentTarget : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RecruitmentTarget",
                table: "Sites",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RecruitmentTarget",
                table: "Sites");
        }
    }
}
