using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduGate.Migrations
{
    /// <inheritdoc />
    public partial class Update_Student_Answer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentAnswer_Choice_Choice_Id",
                table: "StudentAnswer");

            migrationBuilder.AlterColumn<int>(
                name: "Choice_Id",
                table: "StudentAnswer",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "File_Name",
                table: "StudentAnswer",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "File_Path",
                table: "StudentAnswer",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "File_Size",
                table: "StudentAnswer",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "File_Type",
                table: "StudentAnswer",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentAnswer_Choice_Choice_Id",
                table: "StudentAnswer",
                column: "Choice_Id",
                principalTable: "Choice",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentAnswer_Choice_Choice_Id",
                table: "StudentAnswer");

            migrationBuilder.DropColumn(
                name: "File_Name",
                table: "StudentAnswer");

            migrationBuilder.DropColumn(
                name: "File_Path",
                table: "StudentAnswer");

            migrationBuilder.DropColumn(
                name: "File_Size",
                table: "StudentAnswer");

            migrationBuilder.DropColumn(
                name: "File_Type",
                table: "StudentAnswer");

            migrationBuilder.AlterColumn<int>(
                name: "Choice_Id",
                table: "StudentAnswer",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentAnswer_Choice_Choice_Id",
                table: "StudentAnswer",
                column: "Choice_Id",
                principalTable: "Choice",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
