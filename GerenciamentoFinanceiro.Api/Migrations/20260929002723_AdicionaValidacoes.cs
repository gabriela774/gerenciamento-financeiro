using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GerenciamentoFinanceiro.Api.Migrations
{
    /// <inheritdoc />
    public partial class AdicionaValidacoes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lancamentos_Categorias_CategoriaId",
                table: "Lancamentos");

            migrationBuilder.AlterColumn<Guid>(
                name: "CategoriaId",
                table: "Lancamentos",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Nome",
                table: "Categorias",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Lancamento_Tipo",
                table: "Lancamentos",
                sql: "\"Tipo\" IN ('entrada', 'saida')");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Lancamento_Valor_Positivo",
                table: "Lancamentos",
                sql: "\"Valor\" > 0");

            migrationBuilder.CreateIndex(
                name: "IX_Categorias_Nome",
                table: "Categorias",
                column: "Nome",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Lancamentos_Categorias_CategoriaId",
                table: "Lancamentos",
                column: "CategoriaId",
                principalTable: "Categorias",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lancamentos_Categorias_CategoriaId",
                table: "Lancamentos");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Lancamento_Tipo",
                table: "Lancamentos");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Lancamento_Valor_Positivo",
                table: "Lancamentos");

            migrationBuilder.DropIndex(
                name: "IX_Categorias_Nome",
                table: "Categorias");

            migrationBuilder.AlterColumn<Guid>(
                name: "CategoriaId",
                table: "Lancamentos",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<string>(
                name: "Nome",
                table: "Categorias",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AddForeignKey(
                name: "FK_Lancamentos_Categorias_CategoriaId",
                table: "Lancamentos",
                column: "CategoriaId",
                principalTable: "Categorias",
                principalColumn: "Id");
        }
    }
}
