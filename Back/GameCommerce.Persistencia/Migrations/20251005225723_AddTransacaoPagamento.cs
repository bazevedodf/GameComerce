using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameCommerce.Persistencia.Migrations
{
    /// <inheritdoc />
    public partial class AddTransacaoPagamento : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TransacaoPagamento_Pedidos_PedidoId",
                table: "TransacaoPagamento");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TransacaoPagamento",
                table: "TransacaoPagamento");

            migrationBuilder.RenameTable(
                name: "TransacaoPagamento",
                newName: "TransacoesPagamento");

            migrationBuilder.RenameIndex(
                name: "IX_TransacaoPagamento_PedidoId",
                table: "TransacoesPagamento",
                newName: "IX_TransacoesPagamento_PedidoId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TransacoesPagamento",
                table: "TransacoesPagamento",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TransacoesPagamento_Pedidos_PedidoId",
                table: "TransacoesPagamento",
                column: "PedidoId",
                principalTable: "Pedidos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TransacoesPagamento_Pedidos_PedidoId",
                table: "TransacoesPagamento");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TransacoesPagamento",
                table: "TransacoesPagamento");

            migrationBuilder.RenameTable(
                name: "TransacoesPagamento",
                newName: "TransacaoPagamento");

            migrationBuilder.RenameIndex(
                name: "IX_TransacoesPagamento_PedidoId",
                table: "TransacaoPagamento",
                newName: "IX_TransacaoPagamento_PedidoId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TransacaoPagamento",
                table: "TransacaoPagamento",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TransacaoPagamento_Pedidos_PedidoId",
                table: "TransacaoPagamento",
                column: "PedidoId",
                principalTable: "Pedidos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
