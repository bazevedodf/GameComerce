using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameCommerce.Persistencia.Migrations
{
    /// <inheritdoc />
    public partial class AtualizandoMarketingTag : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "MarketingTags",
                keyColumn: "Id",
                keyValue: 1,
                column: "Identificador",
                value: "g-roblox");

            migrationBuilder.UpdateData(
                table: "MarketingTags",
                keyColumn: "Id",
                keyValue: 2,
                column: "Identificador",
                value: "f-roblox");

            migrationBuilder.UpdateData(
                table: "MarketingTags",
                keyColumn: "Id",
                keyValue: 3,
                column: "Identificador",
                value: "tk-roblox");

            migrationBuilder.CreateIndex(
                name: "IX_MarketingTags_Identificador",
                table: "MarketingTags",
                column: "Identificador",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_MarketingTags_Identificador",
                table: "MarketingTags");

            migrationBuilder.UpdateData(
                table: "MarketingTags",
                keyColumn: "Id",
                keyValue: 1,
                column: "Identificador",
                value: "campanha_principal");

            migrationBuilder.UpdateData(
                table: "MarketingTags",
                keyColumn: "Id",
                keyValue: 2,
                column: "Identificador",
                value: "campanha_principal");

            migrationBuilder.UpdateData(
                table: "MarketingTags",
                keyColumn: "Id",
                keyValue: 3,
                column: "Identificador",
                value: "campanha_natal");
        }
    }
}
