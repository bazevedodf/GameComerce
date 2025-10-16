using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameCommerce.Persistencia.Migrations
{
    /// <inheritdoc />
    public partial class AtualizacaoCupom : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Cupons_Codigo",
                table: "Cupons");

            migrationBuilder.DropIndex(
                name: "IX_Cupons_SiteInfoId",
                table: "Cupons");

            migrationBuilder.CreateIndex(
                name: "IX_Cupons_SiteInfoId_Codigo",
                table: "Cupons",
                columns: new[] { "SiteInfoId", "Codigo" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Cupons_SiteInfoId_Codigo",
                table: "Cupons");

            migrationBuilder.CreateIndex(
                name: "IX_Cupons_Codigo",
                table: "Cupons",
                column: "Codigo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Cupons_SiteInfoId",
                table: "Cupons",
                column: "SiteInfoId");
        }
    }
}
