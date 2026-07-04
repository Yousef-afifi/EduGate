using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduGate.Migrations
{
    /// <inheritdoc />
    public partial class Updated_Password_Column : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Password_Hash",
                table: "Teacher",
                newName: "Password");

            migrationBuilder.RenameColumn(
                name: "Password_Hash",
                table: "Account",
                newName: "Password");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Password",
                table: "Teacher",
                newName: "Password_Hash");

            migrationBuilder.RenameColumn(
                name: "Password",
                table: "Account",
                newName: "Password_Hash");
        }
    }
}
