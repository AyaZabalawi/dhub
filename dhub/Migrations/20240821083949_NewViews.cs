using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace dhub.Migrations
{
    /// <inheritdoc />
    public partial class NewViews : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "QuestionResponseDetails",
                columns: table => new
                {
                    QuestionResponseDetailId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    QuestionResponseID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    QuestionID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    QuestionText = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AnswerText = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionResponseDetails", x => x.QuestionResponseDetailId);
                    table.ForeignKey(
                        name: "FK_QuestionResponseDetails_QuestionResponses_QuestionResponseID",
                        column: x => x.QuestionResponseID,
                        principalTable: "QuestionResponses",
                        principalColumn: "QuestionResponseID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_QuestionResponseDetails_Questions_QuestionID",
                        column: x => x.QuestionID,
                        principalTable: "Questions",
                        principalColumn: "QuestionID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_QuestionResponseDetails_QuestionID",
                table: "QuestionResponseDetails",
                column: "QuestionID");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionResponseDetails_QuestionResponseID",
                table: "QuestionResponseDetails",
                column: "QuestionResponseID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "QuestionResponseDetails");
        }
    }
}
