using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class changedtokenwithUSerid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isDeleted",
                table: "Products");

            migrationBuilder.RenameColumn(
                name: "Token",
                table: "Favorites",
                newName: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Favorites",
                newName: "Token");

            migrationBuilder.AddColumn<bool>(
                name: "isDeleted",
                table: "Products",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
