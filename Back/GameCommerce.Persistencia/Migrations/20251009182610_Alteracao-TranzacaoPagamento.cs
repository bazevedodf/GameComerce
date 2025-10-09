using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameCommerce.Persistencia.Migrations
{
    /// <inheritdoc />
    public partial class AlteracaoTranzacaoPagamento : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GatewayCustomerId",
                table: "TransacoesPagamento");

            migrationBuilder.DropColumn(
                name: "GatewayStatus",
                table: "TransacoesPagamento");

            migrationBuilder.RenameColumn(
                name: "GatewaySuccess",
                table: "TransacoesPagamento",
                newName: "Success");

            migrationBuilder.RenameColumn(
                name: "GatewayMessage",
                table: "TransacoesPagamento",
                newName: "Message");

            migrationBuilder.AlterColumn<string>(
                name: "PixCode",
                table: "TransacoesPagamento",
                type: "TEXT",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 500);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "TransacoesPagamento",
                type: "TEXT",
                maxLength: 20,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "TransacoesPagamento");

            migrationBuilder.RenameColumn(
                name: "Success",
                table: "TransacoesPagamento",
                newName: "GatewaySuccess");

            migrationBuilder.RenameColumn(
                name: "Message",
                table: "TransacoesPagamento",
                newName: "GatewayMessage");

            migrationBuilder.AlterColumn<string>(
                name: "PixCode",
                table: "TransacoesPagamento",
                type: "TEXT",
                maxLength: 500,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GatewayCustomerId",
                table: "TransacoesPagamento",
                type: "TEXT",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GatewayStatus",
                table: "TransacoesPagamento",
                type: "TEXT",
                maxLength: 20,
                nullable: false,
                defaultValue: "");
        }
    }
}
