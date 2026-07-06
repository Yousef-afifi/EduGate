using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduGate.Migrations
{
    /// <inheritdoc />
    public partial class Edited_Exam_Table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Total_Marks",
                table: "Exam",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DueDate",
                table: "Exam",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DueDate",
                table: "Exam");

            migrationBuilder.AlterColumn<int>(
                name: "Total_Marks",
                table: "Exam",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
