using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class ChangedLocationToCity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Locations_LocationId",
                table: "Products");

            migrationBuilder.DropTable(
                name: "Locations");

            migrationBuilder.RenameColumn(
                name: "LocationId",
                table: "Products",
                newName: "CityId");

            migrationBuilder.RenameIndex(
                name: "IX_Products_LocationId",
                table: "Products",
                newName: "IX_Products_CityId");

            migrationBuilder.CreateTable(
                name: "Cities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cities", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Cities",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 27, "Bakı" },
                    { 28, "Gəncə" },
                    { 29, "Sumqayıt" },
                    { 30, "Mingəçevir" },
                    { 31, "Naxçıvan" },
                    { 32, "Şirvan" },
                    { 33, "Şəki" },
                    { 34, "Lənkəran" },
                    { 35, "Xaçmaz" },
                    { 36, "Göyçay" },
                    { 37, "Quba" },
                    { 38, "Ağcabədi" },
                    { 39, "İmişli" },
                    { 40, "Bərdə" },
                    { 41, "Sabirabad" },
                    { 42, "Xızı" },
                    { 43, "Yevlax" },
                    { 44, "Qusar" },
                    { 45, "Yardımlı" },
                    { 46, "Zaqatala" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Cities_CityId",
                table: "Products",
                column: "CityId",
                principalTable: "Cities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Cities_CityId",
                table: "Products");

            migrationBuilder.DropTable(
                name: "Cities");

            migrationBuilder.RenameColumn(
                name: "CityId",
                table: "Products",
                newName: "LocationId");

            migrationBuilder.RenameIndex(
                name: "IX_Products_CityId",
                table: "Products",
                newName: "IX_Products_LocationId");

            migrationBuilder.CreateTable(
                name: "Locations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locations", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Locations",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 27, "Bakı" },
                    { 28, "Gəncə" },
                    { 29, "Sumqayıt" },
                    { 30, "Mingəçevir" },
                    { 31, "Naxçıvan" },
                    { 32, "Şirvan" },
                    { 33, "Şəki" },
                    { 34, "Lənkəran" },
                    { 35, "Xaçmaz" },
                    { 36, "Göyçay" },
                    { 37, "Quba" },
                    { 38, "Ağcabədi" },
                    { 39, "İmişli" },
                    { 40, "Bərdə" },
                    { 41, "Sabirabad" },
                    { 42, "Xızı" },
                    { 43, "Yevlax" },
                    { 44, "Qusar" },
                    { 45, "Yardımlı" },
                    { 46, "Zaqatala" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Locations_LocationId",
                table: "Products",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
