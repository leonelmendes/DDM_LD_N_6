using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace iTaskAPI.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TiposTarefa",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nome = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TiposTarefa", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Utilizadores",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nome = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    Password = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Utilizadores", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Gestores",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Departamento = table.Column<string>(type: "text", nullable: false),
                    IsAdmin = table.Column<bool>(type: "boolean", nullable: false),
                    IdUtilizador = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Gestores", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Gestores_Utilizadores_IdUtilizador",
                        column: x => x.IdUtilizador,
                        principalTable: "Utilizadores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Programadores",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NivelExperiencia = table.Column<string>(type: "text", nullable: false),
                    IdGestor = table.Column<int>(type: "integer", nullable: false),
                    IdUtilizador = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Programadores", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Programadores_Gestores_IdGestor",
                        column: x => x.IdGestor,
                        principalTable: "Gestores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Programadores_Utilizadores_IdUtilizador",
                        column: x => x.IdUtilizador,
                        principalTable: "Utilizadores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Tarefas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdGestor = table.Column<int>(type: "integer", nullable: false),
                    IdProgramador = table.Column<int>(type: "integer", nullable: false),
                    IdTipoTarefa = table.Column<int>(type: "integer", nullable: false),
                    OrdemExecucao = table.Column<string>(type: "text", nullable: true),
                    Descricao = table.Column<string>(type: "text", nullable: true),
                    DataPrevistaInicio = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DataPrevistaFim = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    StoryPoints = table.Column<int>(type: "integer", nullable: true),
                    DataRealInicio = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DataRealFim = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DataCriacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EstadoAtual = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tarefas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tarefas_Gestores_IdGestor",
                        column: x => x.IdGestor,
                        principalTable: "Gestores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Tarefas_Programadores_IdProgramador",
                        column: x => x.IdProgramador,
                        principalTable: "Programadores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Tarefas_TiposTarefa_IdTipoTarefa",
                        column: x => x.IdTipoTarefa,
                        principalTable: "TiposTarefa",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Gestores_IdUtilizador",
                table: "Gestores",
                column: "IdUtilizador",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Programadores_IdGestor",
                table: "Programadores",
                column: "IdGestor");

            migrationBuilder.CreateIndex(
                name: "IX_Programadores_IdUtilizador",
                table: "Programadores",
                column: "IdUtilizador",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tarefas_IdGestor",
                table: "Tarefas",
                column: "IdGestor");

            migrationBuilder.CreateIndex(
                name: "IX_Tarefas_IdProgramador",
                table: "Tarefas",
                column: "IdProgramador");

            migrationBuilder.CreateIndex(
                name: "IX_Tarefas_IdTipoTarefa",
                table: "Tarefas",
                column: "IdTipoTarefa");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tarefas");

            migrationBuilder.DropTable(
                name: "Programadores");

            migrationBuilder.DropTable(
                name: "TiposTarefa");

            migrationBuilder.DropTable(
                name: "Gestores");

            migrationBuilder.DropTable(
                name: "Utilizadores");
        }
    }
}
