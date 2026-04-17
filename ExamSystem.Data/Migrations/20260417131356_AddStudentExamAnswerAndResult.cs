using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExamSystem.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddStudentExamAnswerAndResult : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "student_exam_answers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ExamId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    QuestionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OptionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StudentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsCorrect = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_student_exam_answers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_student_exam_answers_exams_ExamId",
                        column: x => x.ExamId,
                        principalTable: "exams",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_student_exam_answers_question_options_OptionId",
                        column: x => x.OptionId,
                        principalTable: "question_options",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_student_exam_answers_questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "questions",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_student_exam_answers_students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "students",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "student_exam_results",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ExamId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StudentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StudentScore = table.Column<int>(type: "int", nullable: false),
                    ExamScore = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_student_exam_results", x => x.Id);
                    table.ForeignKey(
                        name: "FK_student_exam_results_exams_ExamId",
                        column: x => x.ExamId,
                        principalTable: "exams",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_student_exam_results_students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "students",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_student_exam_answers_ExamId_QuestionId_StudentId",
                table: "student_exam_answers",
                columns: new[] { "ExamId", "QuestionId", "StudentId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_student_exam_answers_OptionId",
                table: "student_exam_answers",
                column: "OptionId");

            migrationBuilder.CreateIndex(
                name: "IX_student_exam_answers_QuestionId",
                table: "student_exam_answers",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_student_exam_answers_StudentId",
                table: "student_exam_answers",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_student_exam_results_ExamId_StudentId",
                table: "student_exam_results",
                columns: new[] { "ExamId", "StudentId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_student_exam_results_StudentId",
                table: "student_exam_results",
                column: "StudentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "student_exam_answers");

            migrationBuilder.DropTable(
                name: "student_exam_results");
        }
    }
}
