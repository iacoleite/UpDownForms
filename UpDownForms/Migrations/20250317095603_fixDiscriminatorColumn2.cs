using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UpDownForms.Migrations
{
    /// <inheritdoc />
    public partial class fixDiscriminatorColumn2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BaseQuestionType",
                table: "Questions");

            migrationBuilder.RenameColumn(
                name: "QuestionType",
                table: "Questions",
                newName: "QuestionMCType");

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "Questions",
                type: "varchar(21)",
                maxLength: 21,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "QuestionMCType",
                table: "Questions",
                newName: "QuestionType");

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "Questions",
                type: "longtext",
                nullable: false,
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
    }
}
