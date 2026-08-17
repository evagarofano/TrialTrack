using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrialTrack.Migrations
{
    /// <inheritdoc />
    public partial class AddUniqueProtocolNumber : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ProtocolNumber",
                table: "Studies",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Studies_ProtocolNumber",
                table: "Studies",
                column: "ProtocolNumber",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Studies_ProtocolNumber",
                table: "Studies");

            migrationBuilder.AlterColumn<string>(
                name: "ProtocolNumber",
                table: "Studies",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
