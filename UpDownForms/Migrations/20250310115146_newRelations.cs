using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UpDownForms.Migrations
{
    /// <inheritdoc />
    public partial class newRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Answers_Options_OptionId",
                table: "Answers");

            migrationBuilder.DropIndex(
                name: "IX_Answers_OptionId",
                table: "Answers");

            migrationBuilder.DropColumn(
                name: "OptionId",
                table: "Answers");

            migrationBuilder.CreateTable(
                name: "AnsweredOptions",
                columns: table => new
                {
                    AnswerMultipleChoiceId = table.Column<int>(type: "int", nullable: false),
                    OptionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnsweredOptions", x => new { x.AnswerMultipleChoiceId, x.OptionId });
                    table.ForeignKey(
                        name: "FK_AnsweredOptions_Answers_AnswerMultipleChoiceId",
                        column: x => x.AnswerMultipleChoiceId,
                        principalTable: "Answers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AnsweredOptions_Options_OptionId",
                        column: x => x.OptionId,
                        principalTable: "Options",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_AnsweredOptions_OptionId",
                table: "AnsweredOptions",
                column: "OptionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AnsweredOptions");

            migrationBuilder.AddColumn<int>(
                name: "OptionId",
                table: "Answers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Answers_OptionId",
                table: "Answers",
                column: "OptionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Answers_Options_OptionId",
                table: "Answers",
                column: "OptionId",
                principalTable: "Options",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
