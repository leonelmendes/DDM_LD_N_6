using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace iTaskAPI.Migrations
{
    /// <inheritdoc />
    public partial class AlteraOrdemExecucaoParaInt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            /*migrationBuilder.AlterColumn<int>(
                name: "OrdemExecucao",
                table: "Tarefas",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);*/


        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "OrdemExecucao",
                table: "Tarefas",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");
        }
    }
}
