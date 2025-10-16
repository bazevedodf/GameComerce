using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace GameCommerce.Persistencia.Migrations
{
    /// <inheritdoc />
    public partial class AdicionandoMarketingTag : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MarketingTags",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Tipo = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Nome = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Identificador = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    TagId = table.Column<string>(type: "TEXT", nullable: false),
                    Ativo = table.Column<bool>(type: "INTEGER", nullable: false),
                    SiteInfoId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MarketingTags", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MarketingTags_SiteInfo_SiteInfoId",
                        column: x => x.SiteInfoId,
                        principalTable: "SiteInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "MarketingTags",
                columns: new[] { "Id", "Ativo", "Identificador", "Nome", "SiteInfoId", "TagId", "Tipo" },
                values: new object[,]
                {
                    { 1, true, "campanha_principal", "GTM Campanha Principal", 1, "GTM-ABCD123", "google-tag-manager" },
                    { 2, true, "campanha_principal", "Facebook Pixel Principal", 1, "123456789012345", "facebook-pixel" },
                    { 3, true, "campanha_natal", "TikTok Campanha Natal", 1, "ABCDEF123456", "tiktok-pixel" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_MarketingTags_SiteInfoId",
                table: "MarketingTags",
                column: "SiteInfoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MarketingTags");

            migrationBuilder.AddColumn<int>(
                name: "CategoriaId1",
                table: "Produtos",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Produtos_CategoriaId1",
                table: "Produtos",
                column: "CategoriaId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Produtos_Categorias_CategoriaId1",
                table: "Produtos",
                column: "CategoriaId1",
                principalTable: "Categorias",
                principalColumn: "Id");
        }
    }
}
