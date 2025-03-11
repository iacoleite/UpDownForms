using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UpDownForms.Migrations
{
    /// <inheritdoc />
    public partial class ChangeTypeToQuestionType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "Questions");

            migrationBuilder.AlterColumn<string>(
                name: "QuestionType",
                table: "Questions",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(21)",
                oldMaxLength: 21)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "BaseQuestionType",
                table: "Questions",
                type: "varchar(21)",
                maxLength: 21,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BaseQuestionType",
                table: "Questions");

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "QuestionType",
                keyValue: null,
                column: "QuestionType",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "QuestionType",
                table: "Questions",
                type: "varchar(21)",
                maxLength: 21,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "Questions",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }
    }
}
