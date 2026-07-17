using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CanvasPhase3.Entities
{
    /// <inheritdoc />
    public partial class InitialSetupMod1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "administrators",
                columns: table => new
                {
                    uid = table.Column<string>(type: "character varying(8)", maxLength: 8, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("administrators_pkey", x => x.uid);
                });

            migrationBuilder.CreateTable(
                name: "departments",
                columns: table => new
                {
                    depid = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    subjabbrv = table.Column<string>(type: "character varying(4)", maxLength: 4, nullable: true),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("departments_pkey", x => x.depid);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    uid = table.Column<string>(name: " uid", type: "character varying(8)", maxLength: 8, nullable: false),
                    firstname = table.Column<string>(name: " firstname", type: "character varying(100)", maxLength: 100, nullable: false),
                    lastname = table.Column<string>(name: " lastname", type: "character varying(100)", maxLength: 100, nullable: false),
                    dob = table.Column<DateOnly>(name: " dob", type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("users_pkey", x => x.uid);
                });

            migrationBuilder.CreateTable(
                name: "courses",
                columns: table => new
                {
                    catalogid = table.Column<string>(type: "character varying(5)", maxLength: 5, nullable: false),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    number = table.Column<short>(type: "smallint", nullable: true),
                    depid = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("courses_pkey", x => x.catalogid);
                    table.ForeignKey(
                        name: "courses_depid_fkey",
                        column: x => x.depid,
                        principalTable: "departments",
                        principalColumn: "depid");
                });

            migrationBuilder.CreateTable(
                name: "professors",
                columns: table => new
                {
                    uid = table.Column<string>(type: "character varying(8)", maxLength: 8, nullable: false),
                    employerdep = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("professors_pkey", x => x.uid);
                    table.ForeignKey(
                        name: "professors_employerdep_fkey",
                        column: x => x.employerdep,
                        principalTable: "departments",
                        principalColumn: "depid");
                });

            migrationBuilder.CreateTable(
                name: "students",
                columns: table => new
                {
                    uid = table.Column<string>(type: "character varying(8)", maxLength: 8, nullable: false),
                    majordep = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("students_pkey", x => x.uid);
                    table.ForeignKey(
                        name: "students_majordep_fkey",
                        column: x => x.majordep,
                        principalTable: "departments",
                        principalColumn: "depid");
                });

            migrationBuilder.CreateTable(
                name: "classes",
                columns: table => new
                {
                    classid = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    catalogid = table.Column<string>(type: "character varying(5)", maxLength: 5, nullable: false),
                    semesteryear = table.Column<short>(type: "smallint", nullable: true),
                    semesterterm = table.Column<string>(type: "character varying(6)", maxLength: 6, nullable: true),
                    location = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    starttime = table.Column<TimeOnly>(type: "time without time zone", nullable: true),
                    endtime = table.Column<TimeOnly>(type: "time without time zone", nullable: true),
                    profid = table.Column<string>(type: "character varying(8)", maxLength: 8, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("classes_pkey", x => new { x.classid, x.catalogid });
                    table.UniqueConstraint("AK_classes_classid", x => x.classid);
                    table.ForeignKey(
                        name: "classes_catalogid_fkey",
                        column: x => x.catalogid,
                        principalTable: "courses",
                        principalColumn: "catalogid");
                    table.ForeignKey(
                        name: "classes_profid_fkey",
                        column: x => x.profid,
                        principalTable: "professors",
                        principalColumn: "uid");
                });

            migrationBuilder.CreateTable(
                name: "assignmentcategories",
                columns: table => new
                {
                    categoryid = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    classid = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    gradingweight = table.Column<short>(type: "smallint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("assignmentcategories_pkey", x => new { x.categoryid, x.classid });
                    table.UniqueConstraint("AK_assignmentcategories_categoryid", x => x.categoryid);
                    table.ForeignKey(
                        name: "assignmentcategories_classid_fkey",
                        column: x => x.classid,
                        principalTable: "classes",
                        principalColumn: "classid");
                });

            migrationBuilder.CreateTable(
                name: "enrollment",
                columns: table => new
                {
                    classid = table.Column<int>(type: "integer", nullable: false),
                    uid = table.Column<string>(type: "character varying(8)", maxLength: 8, nullable: false),
                    grade = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("enrollment_pkey", x => new { x.classid, x.uid });
                    table.ForeignKey(
                        name: "enrollment_classid_fkey",
                        column: x => x.classid,
                        principalTable: "classes",
                        principalColumn: "classid");
                    table.ForeignKey(
                        name: "enrollment_uid_fkey",
                        column: x => x.uid,
                        principalTable: "students",
                        principalColumn: "uid");
                });

            migrationBuilder.CreateTable(
                name: "assignments",
                columns: table => new
                {
                    assignmentid = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    categoryid = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    score = table.Column<int>(type: "integer", nullable: true),
                    maxscore = table.Column<int>(type: "integer", nullable: true),
                    content = table.Column<string>(type: "text", nullable: true),
                    duedate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("assignments_pkey", x => new { x.assignmentid, x.categoryid });
                    table.UniqueConstraint("AK_assignments_assignmentid", x => x.assignmentid);
                    table.ForeignKey(
                        name: "assignments_categoryid_fkey",
                        column: x => x.categoryid,
                        principalTable: "assignmentcategories",
                        principalColumn: "categoryid");
                });

            migrationBuilder.CreateTable(
                name: "assignmentsubmissions",
                columns: table => new
                {
                    submissiontime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    studentid = table.Column<string>(type: "character varying(8)", maxLength: 8, nullable: false),
                    assignmentid = table.Column<int>(type: "integer", nullable: false),
                    score = table.Column<int>(type: "integer", nullable: true),
                    content = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("assignmentsubmissions_pkey", x => new { x.submissiontime, x.studentid, x.assignmentid });
                    table.ForeignKey(
                        name: "assignmentsubmissions_assignmentid_fkey",
                        column: x => x.assignmentid,
                        principalTable: "assignments",
                        principalColumn: "assignmentid");
                    table.ForeignKey(
                        name: "assignmentsubmissions_studentid_fkey",
                        column: x => x.studentid,
                        principalTable: "students",
                        principalColumn: "uid");
                });

            migrationBuilder.CreateIndex(
                name: "assignmentcategories_categoryid_key",
                table: "assignmentcategories",
                column: "categoryid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_assignmentcategories_classid",
                table: "assignmentcategories",
                column: "classid");

            migrationBuilder.CreateIndex(
                name: "IX_assignments_categoryid",
                table: "assignments",
                column: "categoryid");

            migrationBuilder.CreateIndex(
                name: "unique_assignmentid",
                table: "assignments",
                column: "assignmentid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_assignmentsubmissions_assignmentid",
                table: "assignmentsubmissions",
                column: "assignmentid");

            migrationBuilder.CreateIndex(
                name: "IX_assignmentsubmissions_studentid",
                table: "assignmentsubmissions",
                column: "studentid");

            migrationBuilder.CreateIndex(
                name: "IX_classes_catalogid",
                table: "classes",
                column: "catalogid");

            migrationBuilder.CreateIndex(
                name: "IX_classes_profid",
                table: "classes",
                column: "profid");

            migrationBuilder.CreateIndex(
                name: "unique_classid",
                table: "classes",
                column: "classid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_courses_depid",
                table: "courses",
                column: "depid");

            migrationBuilder.CreateIndex(
                name: "departments_subjabbrv_key",
                table: "departments",
                column: "subjabbrv",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_enrollment_uid",
                table: "enrollment",
                column: "uid");

            migrationBuilder.CreateIndex(
                name: "IX_professors_employerdep",
                table: "professors",
                column: "employerdep");

            migrationBuilder.CreateIndex(
                name: "IX_students_majordep",
                table: "students",
                column: "majordep");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "administrators");

            migrationBuilder.DropTable(
                name: "assignmentsubmissions");

            migrationBuilder.DropTable(
                name: "enrollment");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "assignments");

            migrationBuilder.DropTable(
                name: "students");

            migrationBuilder.DropTable(
                name: "assignmentcategories");

            migrationBuilder.DropTable(
                name: "classes");

            migrationBuilder.DropTable(
                name: "courses");

            migrationBuilder.DropTable(
                name: "professors");

            migrationBuilder.DropTable(
                name: "departments");
        }
    }
}
