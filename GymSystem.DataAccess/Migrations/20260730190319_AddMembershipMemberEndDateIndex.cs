using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GymSystem.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddMembershipMemberEndDateIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Memberships_MemberId",
                table: "Memberships");

            migrationBuilder.CreateIndex(
                name: "IX_Memberships_MemberId_EndDate",
                table: "Memberships",
                columns: new[] { "MemberId", "EndDate" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Memberships_MemberId_EndDate",
                table: "Memberships");

            migrationBuilder.CreateIndex(
                name: "IX_Memberships_MemberId",
                table: "Memberships",
                column: "MemberId");
        }
    }
}
