using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class qwersdzcdvfbgnweas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CustomerName",
                table: "Complaints");

            migrationBuilder.AddColumn<string>(
                name: "UserID",
                table: "Complaints",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Complaints_UserID",
                table: "Complaints",
                column: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_Complaints_AspNetUsers_UserID",
                table: "Complaints",
                column: "UserID",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Complaints_AspNetUsers_UserID",
                table: "Complaints");

            migrationBuilder.DropIndex(
                name: "IX_Complaints_UserID",
                table: "Complaints");

            migrationBuilder.DropColumn(
                name: "UserID",
                table: "Complaints");

            migrationBuilder.AddColumn<string>(
                name: "CustomerName",
                table: "Complaints",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
