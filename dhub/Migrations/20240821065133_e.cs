using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace dhub.Migrations
{
    /// <inheritdoc />
    public partial class e : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QuestionResponses_Questions_QuestionID",
                table: "QuestionResponses");

            migrationBuilder.AddColumn<string>(
                name: "QuestionText",
                table: "QuestionResponses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_QuestionResponses_Questions_QuestionID",
                table: "QuestionResponses",
                column: "QuestionID",
                principalTable: "Questions",
                principalColumn: "QuestionID",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QuestionResponses_Questions_QuestionID",
                table: "QuestionResponses");

            migrationBuilder.DropColumn(
                name: "QuestionText",
                table: "QuestionResponses");

            migrationBuilder.AddForeignKey(
                name: "FK_QuestionResponses_Questions_QuestionID",
                table: "QuestionResponses",
                column: "QuestionID",
                principalTable: "Questions",
                principalColumn: "QuestionID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
