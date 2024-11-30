using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace vrwebapi.Migrations
{
    /// <inheritdoc />
    public partial class intial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "courses",
                columns: table => new
                {
                    courseid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    coursename = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_courses", x => x.courseid);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    userid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.userid);
                });

            migrationBuilder.CreateTable(
                name: "classes",
                columns: table => new
                {
                    classid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    courseid = table.Column<int>(type: "int", nullable: false),
                    classname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_classes", x => x.classid);
                    table.ForeignKey(
                        name: "FK_classes_courses_courseid",
                        column: x => x.courseid,
                        principalTable: "courses",
                        principalColumn: "courseid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "students",
                columns: table => new
                {
                    studentid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    userid = table.Column<int>(type: "int", nullable: false),
                    rollno = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    cgpa = table.Column<float>(type: "real", nullable: false),
                    program = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_students", x => x.studentid);
                    table.ForeignKey(
                        name: "FK_students_users_userid",
                        column: x => x.userid,
                        principalTable: "users",
                        principalColumn: "userid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "teachers",
                columns: table => new
                {
                    teacherid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    userid = table.Column<int>(type: "int", nullable: false),
                    department = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_teachers", x => x.teacherid);
                    table.ForeignKey(
                        name: "FK_teachers_users_userid",
                        column: x => x.userid,
                        principalTable: "users",
                        principalColumn: "userid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "assignments",
                columns: table => new
                {
                    aid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    courseid = table.Column<int>(type: "int", nullable: false),
                    classid = table.Column<int>(type: "int", nullable: false),
                    aname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    dated = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    duedate = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    time = table.Column<TimeOnly>(type: "time", nullable: false),
                    file = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_assignments", x => x.aid);
                    table.ForeignKey(
                        name: "FK_assignments_classes_classid",
                        column: x => x.classid,
                        principalTable: "classes",
                        principalColumn: "classid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_assignments_courses_courseid",
                        column: x => x.courseid,
                        principalTable: "courses",
                        principalColumn: "courseid",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "enrollments",
                columns: table => new
                {
                    enrollmentid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    courseid = table.Column<int>(type: "int", nullable: false),
                    classid = table.Column<int>(type: "int", nullable: false),
                    studentid = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_enrollments", x => x.enrollmentid);
                    table.ForeignKey(
                        name: "FK_enrollments_classes_classid",
                        column: x => x.classid,
                        principalTable: "classes",
                        principalColumn: "classid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_enrollments_courses_courseid",
                        column: x => x.courseid,
                        principalTable: "courses",
                        principalColumn: "courseid",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_enrollments_students_studentid",
                        column: x => x.studentid,
                        principalTable: "students",
                        principalColumn: "studentid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "teacherassigneds",
                columns: table => new
                {
                    tsid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    classid = table.Column<int>(type: "int", nullable: false),
                    teacherid = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_teacherassigneds", x => x.tsid);
                    table.ForeignKey(
                        name: "FK_teacherassigneds_classes_classid",
                        column: x => x.classid,
                        principalTable: "classes",
                        principalColumn: "classid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_teacherassigneds_teachers_teacherid",
                        column: x => x.teacherid,
                        principalTable: "teachers",
                        principalColumn: "teacherid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "submissions",
                columns: table => new
                {
                    sid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    aid = table.Column<int>(type: "int", nullable: false),
                    studentid = table.Column<int>(type: "int", nullable: false),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    issubmit = table.Column<bool>(type: "bit", nullable: false),
                    file = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_submissions", x => x.sid);
                    table.ForeignKey(
                        name: "FK_submissions_assignments_aid",
                        column: x => x.aid,
                        principalTable: "assignments",
                        principalColumn: "aid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_submissions_students_studentid",
                        column: x => x.studentid,
                        principalTable: "students",
                        principalColumn: "studentid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "grades",
                columns: table => new
                {
                    gid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    sid = table.Column<int>(type: "int", nullable: false),
                    grades = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    comments = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_grades", x => x.gid);
                    table.ForeignKey(
                        name: "FK_grades_submissions_sid",
                        column: x => x.sid,
                        principalTable: "submissions",
                        principalColumn: "sid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_assignments_classid",
                table: "assignments",
                column: "classid");

            migrationBuilder.CreateIndex(
                name: "IX_assignments_courseid",
                table: "assignments",
                column: "courseid");

            migrationBuilder.CreateIndex(
                name: "IX_classes_courseid",
                table: "classes",
                column: "courseid");

            migrationBuilder.CreateIndex(
                name: "IX_enrollments_classid",
                table: "enrollments",
                column: "classid");

            migrationBuilder.CreateIndex(
                name: "IX_enrollments_courseid",
                table: "enrollments",
                column: "courseid");

            migrationBuilder.CreateIndex(
                name: "IX_enrollments_studentid",
                table: "enrollments",
                column: "studentid");

            migrationBuilder.CreateIndex(
                name: "IX_grades_sid",
                table: "grades",
                column: "sid");

            migrationBuilder.CreateIndex(
                name: "IX_students_userid",
                table: "students",
                column: "userid");

            migrationBuilder.CreateIndex(
                name: "IX_submissions_aid",
                table: "submissions",
                column: "aid");

            migrationBuilder.CreateIndex(
                name: "IX_submissions_studentid",
                table: "submissions",
                column: "studentid");

            migrationBuilder.CreateIndex(
                name: "IX_teacherassigneds_classid",
                table: "teacherassigneds",
                column: "classid");

            migrationBuilder.CreateIndex(
                name: "IX_teacherassigneds_teacherid",
                table: "teacherassigneds",
                column: "teacherid");

            migrationBuilder.CreateIndex(
                name: "IX_teachers_userid",
                table: "teachers",
                column: "userid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "enrollments");

            migrationBuilder.DropTable(
                name: "grades");

            migrationBuilder.DropTable(
                name: "teacherassigneds");

            migrationBuilder.DropTable(
                name: "submissions");

            migrationBuilder.DropTable(
                name: "teachers");

            migrationBuilder.DropTable(
                name: "assignments");

            migrationBuilder.DropTable(
                name: "students");

            migrationBuilder.DropTable(
                name: "classes");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "courses");
        }
    }
}
