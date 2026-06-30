using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduGate.Migrations
{
    /// <inheritdoc />
    public partial class fixed_name : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Exam_Course_Coures_Id",
                table: "Exam");

            migrationBuilder.RenameColumn(
                name: "Coures_Id",
                table: "Exam",
                newName: "Course_Id");

            migrationBuilder.RenameIndex(
                name: "IX_Exam_Coures_Id",
                table: "Exam",
                newName: "IX_Exam_Course_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Exam_Course_Course_Id",
                table: "Exam",
                column: "Course_Id",
                principalTable: "Course",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Exam_Course_Course_Id",
                table: "Exam");

            migrationBuilder.RenameColumn(
                name: "Course_Id",
                table: "Exam",
                newName: "Coures_Id");

            migrationBuilder.RenameIndex(
                name: "IX_Exam_Course_Id",
                table: "Exam",
                newName: "IX_Exam_Coures_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Exam_Course_Coures_Id",
                table: "Exam",
                column: "Coures_Id",
                principalTable: "Course",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
