using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduGate.Migrations
{
    /// <inheritdoc />
    public partial class Added_Column_to_LessonMaterial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "File_Size",
                table: "Material",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Material",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "File_Size",
                table: "Material");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "Material");
        }
    }
}
