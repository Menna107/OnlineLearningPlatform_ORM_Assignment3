using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ORM_Assignment3.Migrations
{
    /// <inheritdoc />
    public partial class seedInitialData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "CategoryId", "Description", "Name", "ParentCategoryId" },
                values: new object[] { 1, null, "Category1", null });

            migrationBuilder.InsertData(
                table: "Instructors",
                columns: new[] { "InstructorId", "Bio", "Email", "FullName", "JoinDate" },
                values: new object[,]
                {
                    { 1, null, "instructor1@gmail.com", "Instructor1", new DateTime(2023, 6, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, null, "instructor2@gmail.com", "Instructor2", new DateTime(2024, 9, 11, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                table: "Students",
                columns: new[] { "StudentId", "DOB", "Email", "FullName" },
                values: new object[,]
                {
                    { 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "student1@gmail.com", "Student1" },
                    { 2, new DateTime(2001, 2, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "student2@gmail.com", "Student2" }
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "CategoryId", "Description", "Name", "ParentCategoryId" },
                values: new object[] { 2, null, "Category2", 1 });

            migrationBuilder.InsertData(
                table: "Courses",
                columns: new[] { "CourseId", "CategoryId", "Description", "InstructorId", "IsPublished", "Level", "Price", "PublishedAt", "Title" },
                values: new object[,]
                {
                    { 2, 1, "Description for Course2", 2, false, 1, 800m, null, "Course2" },
                    { 1, 2, "Description for Course1", 1, true, 0, 300m, null, "Course1" }
                });

            migrationBuilder.InsertData(
                table: "Enrollments",
                columns: new[] { "CourseId", "StudentId", "EnrollmentDate", "FinalGrade", "Progress", "Status" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 50m, 0 },
                    { 1, 2, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 95m, 100m, 1 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Courses",
                keyColumn: "CourseId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Enrollments",
                keyColumns: new[] { "CourseId", "StudentId" },
                keyValues: new object[] { 1, 1 });

            migrationBuilder.DeleteData(
                table: "Enrollments",
                keyColumns: new[] { "CourseId", "StudentId" },
                keyValues: new object[] { 1, 2 });

            migrationBuilder.DeleteData(
                table: "Courses",
                keyColumn: "CourseId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Instructors",
                keyColumn: "InstructorId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Students",
                keyColumn: "StudentId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Students",
                keyColumn: "StudentId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Instructors",
                keyColumn: "InstructorId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 1);
        }
    }
}
