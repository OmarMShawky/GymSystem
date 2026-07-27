using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GymSystem.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class RemoveHealthRecordAge : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Age",
                table: "HealthRecords");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Age",
                table: "HealthRecords",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
