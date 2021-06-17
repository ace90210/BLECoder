using Microsoft.EntityFrameworkCore.Migrations;

namespace RadzenTemplate.EntityFrameworkCore.SqlServer.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "JsonBlobs",
                columns: table => new
                {
                    Key = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    Data = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JsonBlobs", x => x.Key);
                });

            migrationBuilder.CreateTable(
                name: "UserConfigurations",
                columns: table => new
                {
                    UserUniqueIdentifier = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    PreferredTheme = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    LoginCount = table.Column<int>(type: "int", nullable: false),
                    SiteNickname = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserConfigurations", x => x.UserUniqueIdentifier);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "JsonBlobs");

            migrationBuilder.DropTable(
                name: "UserConfigurations");
        }
    }
}
