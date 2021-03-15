using Microsoft.EntityFrameworkCore.Migrations;

namespace opensis.data.Migrations.MySqlMigrations
{
    public partial class AddTwoColumnInGradeLevelsAndAddGradeEducationalStageTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_gradelevels_grade_equivalency",
                table: "gradelevels");

            migrationBuilder.DropIndex(
                name: "IX_gradelevels_isced_grade_level",
                table: "gradelevels");

            migrationBuilder.DropPrimaryKey(
                name: "PRIMARY",
                table: "grade_equivalency");

            migrationBuilder.DeleteData(
                table: "grade_equivalency",
                keyColumn: "isced_grade_level",
                keyValue: "ISCED 01");

            migrationBuilder.DeleteData(
                table: "grade_equivalency",
                keyColumn: "isced_grade_level",
                keyValue: "ISCED 02");

            migrationBuilder.DeleteData(
                table: "grade_equivalency",
                keyColumn: "isced_grade_level",
                keyValue: "ISCED 1");

            migrationBuilder.DeleteData(
                table: "grade_equivalency",
                keyColumn: "isced_grade_level",
                keyValue: "ISCED 2");

            migrationBuilder.DeleteData(
                table: "grade_equivalency",
                keyColumn: "isced_grade_level",
                keyValue: "ISCED 3");

            migrationBuilder.DeleteData(
                table: "grade_equivalency",
                keyColumn: "isced_grade_level",
                keyValue: "ISCED 4");

            migrationBuilder.DeleteData(
                table: "grade_equivalency",
                keyColumn: "isced_grade_level",
                keyValue: "ISCED 5");

            migrationBuilder.DeleteData(
                table: "grade_equivalency",
                keyColumn: "isced_grade_level",
                keyValue: "ISCED 6");

            migrationBuilder.DeleteData(
                table: "grade_equivalency",
                keyColumn: "isced_grade_level",
                keyValue: "ISCED 7");

            migrationBuilder.DeleteData(
                table: "grade_equivalency",
                keyColumn: "isced_grade_level",
                keyValue: "ISCED 8");

            migrationBuilder.DropTable("grade_equivalency");


            migrationBuilder.DropColumn(
                name: "isced_grade_level",
                table: "gradelevels");

            migrationBuilder.AddColumn<int>(
                name: "age_range_id",
                table: "gradelevels",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "equivalency_id",
                table: "gradelevels",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "isced_code",
                table: "gradelevels",
                nullable: true);


            migrationBuilder.AddColumn<string>(
                name: "grade_scale_type",
                table: "course_section",
                unicode: false,
                maxLength: 13,
                nullable: true,
                comment: "'Ungraded','Numeric','School_Scale','Teacher_Scale'");

            
            
            migrationBuilder.CreateTable(
                name: "grade_equivalency",
                columns: table => new
                {
                    equivalency_id = table.Column<int>(nullable: false),
                    grade_level_equivalency = table.Column<string>(unicode: false, maxLength: 250, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_grade_equivalency_1", x => x.equivalency_id);
                });

            migrationBuilder.CreateTable(
                name: "grade_age_range",
                columns: table => new
                {
                    age_range_id = table.Column<int>(nullable: false),
                    age_range = table.Column<string>(unicode: false, maxLength: 10, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_grade_age_range", x => x.age_range_id);
                });

           

            migrationBuilder.CreateTable(
                name: "grade_educational_stage",
                columns: table => new
                {
                    isced_code = table.Column<int>(nullable: false),
                    educational_stage = table.Column<string>(unicode: false, maxLength: 250, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_grade_educational_stage", x => x.isced_code);
                });

            migrationBuilder.InsertData(
                table: "grade_age_range",
                columns: new[] { "age_range_id", "age_range" },
                values: new object[,]
                {
                    { 0, "Below 5" },
                    { 14, "18+" },
                    { 13, "17–18" },
                    { 12, "16–17" },
                    { 11, "15–16" },
                    { 10, "14–15" },
                    { 8, "12–13" },
                    { 9, "13–14" },
                    { 6, "10–11" },
                    { 5, "9–10" },
                    { 4, "8–9" },
                    { 3, "7–8" },
                     { 2, "6–7" },
                    { 1, "5–6" },
                    { 7, "11–12" }
                });

            migrationBuilder.InsertData(
                table: "grade_educational_stage",
                columns: new[] { "isced_code", "educational_stage" },
                values: new object[,]
                {
                    { 5, "Short-cycle tertiary education" },
                    { 8, "Doctoral degree or equivalent" },
                    { 6, "Bachelor's degree or equivalent" },
                    { 4, "Post-secondary non-tertiary education" },
                    { 7, "Master's degree or equivalent" },
                    { 2, "Lower secondary education" },
                    { 1, "Primary education" },
                    { 0, "Early childhood Education" },
                    { 3, "Upper secondary education" }
                });
           
            migrationBuilder.InsertData(
                table: "grade_equivalency",
                columns: new[] { "equivalency_id", "grade_level_equivalency" },
                values: new object[,]
                {
                    { -1, "Pre-Kindergarten" },
                    { 0, "Kindergarten" },

                    { 1, "1st Grade" },
                    { 2, "2nd Grade" },
                    { 3, "3rd Grade" },
                    { 4, "4th Grade" },
                    { 5, "5th Grade" },
                    { 6, "6th Grade" },
                    { 7, "7th Grade" },
                    { 8, "8th Grade" },
                    { 9, "9th Grade" },
                    { 10, "10th Grade" },
                    { 11, "11th Grade" },
                    { 12, "12th Grade" },
                    { 13, "1st Year College" },
                    { 14, "2nd Year College" },
                    { 15, "3rd Year College" },
                    { 16, "4th Year College" },
                    { 17, "5th Year College" },
                    { 18, "6th Year College" },
                    { 19, "7th Year College" },
                    { 20, "8th Year College" }

                });

            migrationBuilder.CreateIndex(
                name: "IX_gradelevels_age_range_id",
                table: "gradelevels",
                column: "age_range_id");

            migrationBuilder.CreateIndex(
                name: "IX_gradelevels_equivalency_id",
                table: "gradelevels",
                column: "equivalency_id");

            migrationBuilder.CreateIndex(
                name: "IX_gradelevels_isced_code",
                table: "gradelevels",
                column: "isced_code");

            migrationBuilder.AddForeignKey(
                name: "FK_gradelevels_grade_age_range",
                table: "gradelevels",
                column: "age_range_id",
                principalTable: "grade_age_range",
                principalColumn: "age_range_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_gradelevels_grade_equivalency",
                table: "gradelevels",
                column: "equivalency_id",
                principalTable: "grade_equivalency",
                principalColumn: "equivalency_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_gradelevels_grade_educational_stage",
                table: "gradelevels",
                column: "isced_code",
                principalTable: "grade_educational_stage",
                principalColumn: "isced_code",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_gradelevels_grade_age_range",
                table: "gradelevels");

            migrationBuilder.DropForeignKey(
                name: "FK_gradelevels_grade_equivalency",
                table: "gradelevels");

            migrationBuilder.DropForeignKey(
                name: "FK_gradelevels_grade_educational_stage",
                table: "gradelevels");

            migrationBuilder.DropTable(
                name: "grade_age_range");

            migrationBuilder.DropTable(
                name: "grade_educational_stage");

            migrationBuilder.DropIndex(
                name: "IX_gradelevels_age_range_id",
                table: "gradelevels");

            migrationBuilder.DropIndex(
                name: "IX_gradelevels_equivalency_id",
                table: "gradelevels");

            migrationBuilder.DropIndex(
                name: "IX_gradelevels_isced_code",
                table: "gradelevels");

            migrationBuilder.DropPrimaryKey(
                name: "PK_grade_equivalency_1",
                table: "grade_equivalency");

            migrationBuilder.DeleteData(
                table: "grade_equivalency",
                keyColumn: "equivalency_id",
                keyValue: -1);

            migrationBuilder.DeleteData(
                table: "grade_equivalency",
                keyColumn: "equivalency_id",
                keyValue: 0);

            migrationBuilder.DeleteData(
                table: "grade_equivalency",
                keyColumn: "equivalency_id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "grade_equivalency",
                keyColumn: "equivalency_id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "grade_equivalency",
                keyColumn: "equivalency_id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "grade_equivalency",
                keyColumn: "equivalency_id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "grade_equivalency",
                keyColumn: "equivalency_id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "grade_equivalency",
                keyColumn: "equivalency_id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "grade_equivalency",
                keyColumn: "equivalency_id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "grade_equivalency",
                keyColumn: "equivalency_id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "grade_equivalency",
                keyColumn: "equivalency_id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "grade_equivalency",
                keyColumn: "equivalency_id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "grade_equivalency",
                keyColumn: "equivalency_id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "grade_equivalency",
                keyColumn: "equivalency_id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "grade_equivalency",
                keyColumn: "equivalency_id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "grade_equivalency",
                keyColumn: "equivalency_id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "grade_equivalency",
                keyColumn: "equivalency_id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "grade_equivalency",
                keyColumn: "equivalency_id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "grade_equivalency",
                keyColumn: "equivalency_id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "grade_equivalency",
                keyColumn: "equivalency_id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "grade_equivalency",
                keyColumn: "equivalency_id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "grade_equivalency",
                keyColumn: "equivalency_id",
                keyValue: 20);

            migrationBuilder.DropColumn(
                name: "age_range_id",
                table: "gradelevels");

            migrationBuilder.DropColumn(
                name: "equivalency_id",
                table: "gradelevels");

            migrationBuilder.DropColumn(
                name: "isced_code",
                table: "gradelevels");

            migrationBuilder.DropColumn(
                name: "equivalency_id",
                table: "grade_equivalency");

            migrationBuilder.DropColumn(
                name: "grade_level_equivalency",
                table: "grade_equivalency");

            migrationBuilder.DropColumn(
                name: "grade_scale_type",
                table: "course_section");

            migrationBuilder.AddColumn<string>(
                name: "isced_grade_level",
                table: "gradelevels",
                type: "varchar(8) CHARACTER SET utf8mb4",
                unicode: false,
                maxLength: 8,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "isced_grade_level",
                table: "grade_equivalency",
                type: "varchar(8) CHARACTER SET utf8mb4",
                unicode: false,
                maxLength: 8,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "age_range",
                table: "grade_equivalency",
                type: "varchar(5) CHARACTER SET utf8mb4",
                unicode: false,
                maxLength: 5,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "grade_description",
                table: "grade_equivalency",
                type: "varchar(50) CHARACTER SET utf8mb4",
                unicode: false,
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_grade_equivalency",
                table: "grade_equivalency",
                column: "isced_grade_level");

            migrationBuilder.InsertData(
                table: "grade_equivalency",
                columns: new[] { "isced_grade_level", "age_range", "grade_description" },
                values: new object[,]
                {
                    { "ISCED 01", "0-2", "Early childhood education" },
                    { "ISCED 02", "0-2", "Pre-primary education" },
                    { "ISCED 1", "5-7", "Primary education" },
                    { "ISCED 2", "6-10", "Lower secondary education" },
                    { "ISCED 3", "9-12", "Upper secondary education" },
                    { "ISCED 4", "10-11", "Post-secondary non-tertiary education" },
                    { "ISCED 5", "14-16", "Short-cycle tertiary education" },
                    { "ISCED 6", "17-23", "Bachelor's or equivalent" },
                    { "ISCED 7", "21-25", "Master's or equivalent" },
                    { "ISCED 8", "22-28", "Doctoral or equivalent level" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_gradelevels_isced_grade_level",
                table: "gradelevels",
                column: "isced_grade_level");

            migrationBuilder.AddForeignKey(
                name: "FK_gradelevels_grade_equivalency",
                table: "gradelevels",
                column: "isced_grade_level",
                principalTable: "grade_equivalency",
                principalColumn: "isced_grade_level",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
