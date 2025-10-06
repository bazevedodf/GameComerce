using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace GameCommerce.Persistencia.Migrations
{
    /// <inheritdoc />
    public partial class Inicial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SiteInfo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nome = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Dominio = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    LogoUrl = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    Cnpj = table.Column<string>(type: "TEXT", maxLength: 20, nullable: true),
                    Address = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    Email = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true),
                    Instagram = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                    Facebook = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                    Whatsapp = table.Column<string>(type: "TEXT", maxLength: 20, nullable: true),
                    YouTube = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SiteInfo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Categorias",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Slug = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Descricao = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    Imagem = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                    Icon = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    SiteInfoId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categorias", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Categorias_SiteInfo_SiteInfoId",
                        column: x => x.SiteInfoId,
                        principalTable: "SiteInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Cupons",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Codigo = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Valido = table.Column<bool>(type: "INTEGER", nullable: false),
                    ValorDesconto = table.Column<decimal>(type: "TEXT", precision: 10, scale: 2, nullable: true),
                    TipoDesconto = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    MensagemErro = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    SiteInfoId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cupons", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cupons_SiteInfo_SiteInfoId",
                        column: x => x.SiteInfoId,
                        principalTable: "SiteInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Produtos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nome = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Descricao = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: false),
                    Preco = table.Column<decimal>(type: "TEXT", precision: 10, scale: 2, nullable: false),
                    PrecoOriginal = table.Column<decimal>(type: "TEXT", precision: 10, scale: 2, nullable: false),
                    Desconto = table.Column<int>(type: "INTEGER", nullable: false),
                    Imagem = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                    SiteInfoId = table.Column<int>(type: "INTEGER", nullable: false),
                    CategoriaId = table.Column<int>(type: "INTEGER", nullable: false),
                    Avaliacao = table.Column<decimal>(type: "TEXT", precision: 3, scale: 1, nullable: false),
                    TotalAvaliacoes = table.Column<int>(type: "INTEGER", nullable: false),
                    Tags = table.Column<string>(type: "TEXT", nullable: false),
                    Ativo = table.Column<bool>(type: "INTEGER", nullable: false),
                    EmDestaque = table.Column<bool>(type: "INTEGER", nullable: false),
                    Entrega = table.Column<int>(type: "INTEGER", nullable: false),
                    CategoriaId1 = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Produtos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Produtos_Categorias_CategoriaId",
                        column: x => x.CategoriaId,
                        principalTable: "Categorias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Produtos_Categorias_CategoriaId1",
                        column: x => x.CategoriaId1,
                        principalTable: "Categorias",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Produtos_SiteInfo_SiteInfoId",
                        column: x => x.SiteInfoId,
                        principalTable: "SiteInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Subcategorias",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Slug = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    CategoriaId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subcategorias", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Subcategorias_Categorias_CategoriaId",
                        column: x => x.CategoriaId,
                        principalTable: "Categorias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Pedidos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Email = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    Telefone = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    Total = table.Column<decimal>(type: "TEXT", precision: 10, scale: 2, nullable: false),
                    Frete = table.Column<decimal>(type: "TEXT", precision: 10, scale: 2, nullable: false),
                    Status = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    DescontoAplicado = table.Column<decimal>(type: "TEXT", precision: 10, scale: 2, nullable: true),
                    DataCriacao = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CupomId = table.Column<int>(type: "INTEGER", nullable: true),
                    SiteInfoId = table.Column<int>(type: "INTEGER", nullable: false),
                    MeioPagamento = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pedidos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pedidos_Cupons_CupomId",
                        column: x => x.CupomId,
                        principalTable: "Cupons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Pedidos_SiteInfo_SiteInfoId",
                        column: x => x.SiteInfoId,
                        principalTable: "SiteInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ItensPedido",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PedidoId = table.Column<int>(type: "INTEGER", nullable: false),
                    ProdutoId = table.Column<int>(type: "INTEGER", nullable: false),
                    Quantidade = table.Column<int>(type: "INTEGER", nullable: false),
                    PrecoUnitario = table.Column<decimal>(type: "TEXT", precision: 10, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItensPedido", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ItensPedido_Pedidos_PedidoId",
                        column: x => x.PedidoId,
                        principalTable: "Pedidos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ItensPedido_Produtos_ProdutoId",
                        column: x => x.ProdutoId,
                        principalTable: "Produtos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "SiteInfo",
                columns: new[] { "Id", "Address", "Cnpj", "Dominio", "Email", "Facebook", "Instagram", "LogoUrl", "Nome", "Whatsapp", "YouTube" },
                values: new object[] { 1, "Rua dos Games, 123 - São Paulo, SP", "12.345.678/0001-99", "localhost:4200", "contato@gamecommerce.com.br", "https://facebook.com/gamecommerce", "https://instagram.com/gamecommerce", "/uploads/logo-site.png", "Game Commerce", "(11) 99999-9999", null });

            migrationBuilder.InsertData(
                table: "Categorias",
                columns: new[] { "Id", "Descricao", "Icon", "Imagem", "Name", "SiteInfoId", "Slug" },
                values: new object[,]
                {
                    { 1, "Explore, crie e brilhe no Fortnite com estilo!", "/uploads/categorias/fortnite-ico.png", "/uploads/categorias/categoria-fortnite.png", "FORTNITE", 1, "fortnite" },
                    { 2, "Ação rápida e intensa em batalhas épicas.", "/uploads/categorias/freefire-ico.png", "/uploads/categorias/categoria-free-fire.webp", "FREE FIRE", 1, "free-fire" },
                    { 3, "Ação tática com estilo e precisão.", "/uploads/categorias/valorant-ico.png", "/uploads/categorias/categoria-valorant.jpg", "VALORANT", 1, "valorant" },
                    { 4, "Batalhas estratégicas em um mundo de fantasia.", "/uploads/categorias/legue-of-legends-ico.png", "/uploads/categorias/categoria-legue-of-legend.png", "LEAGUE OF LEGENDS", 1, "league-of-legends" },
                    { 5, "Explore, crie e brilhe no Roblox com estilo!", "/uploads/categorias/robux-ico.png", "/uploads/categorias/categoria-roblox.webp", "ROBLOX", 1, "roblox" },
                    { 6, "Lutas rápidas e divertidas com amigos.", "/uploads/categorias/brawstars-ico.png", "/uploads/categorias/categoria-brawl-stars.webp", "BRAWL STARS", 1, "brawl-stars" }
                });

            migrationBuilder.InsertData(
                table: "Subcategorias",
                columns: new[] { "Id", "CategoriaId", "Name", "Slug" },
                values: new object[,]
                {
                    { 1, 1, "CONTAS FORTNITE", "contas-fortnite" },
                    { 2, 1, "BUNDLES FORTNITE", "bundles-fortnite" },
                    { 3, 2, "CONTAS FREEFIRE", "contas-freefire" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Categorias_SiteInfoId",
                table: "Categorias",
                column: "SiteInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_Cupons_Codigo",
                table: "Cupons",
                column: "Codigo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Cupons_SiteInfoId",
                table: "Cupons",
                column: "SiteInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_ItensPedido_PedidoId",
                table: "ItensPedido",
                column: "PedidoId");

            migrationBuilder.CreateIndex(
                name: "IX_ItensPedido_ProdutoId",
                table: "ItensPedido",
                column: "ProdutoId");

            migrationBuilder.CreateIndex(
                name: "IX_Pedidos_CupomId",
                table: "Pedidos",
                column: "CupomId");

            migrationBuilder.CreateIndex(
                name: "IX_Pedidos_SiteInfoId",
                table: "Pedidos",
                column: "SiteInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_Produtos_CategoriaId",
                table: "Produtos",
                column: "CategoriaId");

            migrationBuilder.CreateIndex(
                name: "IX_Produtos_CategoriaId1",
                table: "Produtos",
                column: "CategoriaId1");

            migrationBuilder.CreateIndex(
                name: "IX_Produtos_SiteInfoId",
                table: "Produtos",
                column: "SiteInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_Subcategorias_CategoriaId",
                table: "Subcategorias",
                column: "CategoriaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ItensPedido");

            migrationBuilder.DropTable(
                name: "Subcategorias");

            migrationBuilder.DropTable(
                name: "Pedidos");

            migrationBuilder.DropTable(
                name: "Produtos");

            migrationBuilder.DropTable(
                name: "Cupons");

            migrationBuilder.DropTable(
                name: "Categorias");

            migrationBuilder.DropTable(
                name: "SiteInfo");
        }
    }
}
