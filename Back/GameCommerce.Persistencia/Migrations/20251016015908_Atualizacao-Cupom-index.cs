using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameCommerce.Persistencia.Migrations
{
    /// <inheritdoc />
    public partial class AtualizacaoCupomindex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Cupons_SiteInfoId_Codigo",
                table: "Cupons");

            migrationBuilder.CreateIndex(
                name: "IX_Cupons_SiteInfoId",
                table: "Cupons",
                column: "SiteInfoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Cupons_SiteInfoId",
                table: "Cupons");

            migrationBuilder.CreateIndex(
                name: "IX_Cupons_SiteInfoId_Codigo",
                table: "Cupons",
                columns: new[] { "SiteInfoId", "Codigo" },
                unique: true);
        }
    }
}
