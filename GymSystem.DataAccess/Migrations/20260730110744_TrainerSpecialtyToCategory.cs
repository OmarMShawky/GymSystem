using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GymSystem.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class TrainerSpecialtyToCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // 1. Add the new column first, so the old Specialty values can still be mapped across.
            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "Trainers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            // 2. Backfill by matching the old specialty text to a category name, ignoring
            //    spacing (e.g. "CardioFitness" -> "Cardio Fitness").
            migrationBuilder.Sql(@"
                UPDATE t
                SET t.CategoryId = c.Id
                FROM Trainers t
                INNER JOIN Categories c
                    ON REPLACE(c.Name, ' ', '') = REPLACE(t.Specialty, ' ', '');");

            // 3. Specialties with no matching category (e.g. Bodybuilding) fall back to the
            //    first category so the required FK can be satisfied. Review these afterwards.
            migrationBuilder.Sql(@"
                UPDATE Trainers
                SET CategoryId = (SELECT TOP 1 Id FROM Categories ORDER BY Id)
                WHERE CategoryId = 0;");

            // 4. Only now is the old column safe to drop.
            migrationBuilder.DropColumn(
                name: "Specialty",
                table: "Trainers");

            migrationBuilder.CreateIndex(
                name: "IX_Trainers_CategoryId",
                table: "Trainers",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Trainers_Categories_CategoryId",
                table: "Trainers",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Trainers_Categories_CategoryId",
                table: "Trainers");

            migrationBuilder.DropIndex(
                name: "IX_Trainers_CategoryId",
                table: "Trainers");

            migrationBuilder.AddColumn<string>(
                name: "Specialty",
                table: "Trainers",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "");

            // Restore the specialty text from the linked category name.
            migrationBuilder.Sql(@"
                UPDATE t
                SET t.Specialty = REPLACE(c.Name, ' ', '')
                FROM Trainers t
                INNER JOIN Categories c ON c.Id = t.CategoryId;");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "Trainers");
        }
    }
}
