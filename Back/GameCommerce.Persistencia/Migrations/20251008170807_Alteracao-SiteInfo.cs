using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameCommerce.Persistencia.Migrations
{
    /// <inheritdoc />
    public partial class AlteracaoSiteInfo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApiKey",
                table: "SiteInfo",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BaseUrl",
                table: "SiteInfo",
                type: "TEXT",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "SiteInfo",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ApiKey", "BaseUrl" },
                values: new object[] { null, null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApiKey",
                table: "SiteInfo");

            migrationBuilder.DropColumn(
                name: "BaseUrl",
                table: "SiteInfo");
        }
    }
}
