using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace dhub.Migrations
{
    /// <inheritdoc />
    public partial class @new : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Surveys",
                columns: table => new
                {
                    SurveyID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Surveys", x => x.SurveyID);
                });

            migrationBuilder.CreateTable(
                name: "Questions",
                columns: table => new
                {
                    QuestionID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SurveyID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    QuestionText = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsRequired = table.Column<bool>(type: "bit", nullable: false),
                    IsHidden = table.Column<bool>(type: "bit", nullable: false),
                    QuestionType = table.Column<int>(type: "int", nullable: false),
                    Options = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Questions", x => x.QuestionID);
                    table.ForeignKey(
                        name: "FK_Questions_Surveys_SurveyID",
                        column: x => x.SurveyID,
                        principalTable: "Surveys",
                        principalColumn: "SurveyID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SurveyResponses",
                columns: table => new
                {
                    ResponseID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SurveyID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SubmissionDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SurveyResponses", x => x.ResponseID);
                    table.ForeignKey(
                        name: "FK_SurveyResponses_Surveys_SurveyID",
                        column: x => x.SurveyID,
                        principalTable: "Surveys",
                        principalColumn: "SurveyID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "QuestionResponses",
                columns: table => new
                {
                    QuestionResponseID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SurveyResponseID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    QuestionID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AnswerText = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionResponses", x => x.QuestionResponseID);
                    table.ForeignKey(
                        name: "FK_QuestionResponses_Questions_QuestionID",
                        column: x => x.QuestionID,
                        principalTable: "Questions",
                        principalColumn: "QuestionID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_QuestionResponses_SurveyResponses_SurveyResponseID",
                        column: x => x.SurveyResponseID,
                        principalTable: "SurveyResponses",
                        principalColumn: "ResponseID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AnswerOptions",
                columns: table => new
                {
                    AnswerOptionID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    QuestionResponseID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OptionText = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnswerOptions", x => x.AnswerOptionID);
                    table.ForeignKey(
                        name: "FK_AnswerOptions_QuestionResponses_QuestionResponseID",
                        column: x => x.QuestionResponseID,
                        principalTable: "QuestionResponses",
                        principalColumn: "QuestionResponseID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AnswerOptions_QuestionResponseID",
                table: "AnswerOptions",
                column: "QuestionResponseID");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionResponses_QuestionID",
                table: "QuestionResponses",
                column: "QuestionID");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionResponses_SurveyResponseID",
                table: "QuestionResponses",
                column: "SurveyResponseID");

            migrationBuilder.CreateIndex(
                name: "IX_Questions_SurveyID",
                table: "Questions",
                column: "SurveyID");

            migrationBuilder.CreateIndex(
                name: "IX_SurveyResponses_SurveyID",
                table: "SurveyResponses",
                column: "SurveyID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AnswerOptions");

            migrationBuilder.DropTable(
                name: "QuestionResponses");

            migrationBuilder.DropTable(
                name: "Questions");

            migrationBuilder.DropTable(
                name: "SurveyResponses");

            migrationBuilder.DropTable(
                name: "Surveys");
        }
    }
}
