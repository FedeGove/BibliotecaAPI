using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace BibliotecaAPI.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Utenti",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nome = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Utenti", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Libri",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Titolo = table.Column<string>(type: "text", nullable: false),
                    Autore = table.Column<string>(type: "text", nullable: false),
                    Anno = table.Column<int>(type: "integer", nullable: false),
                    Genere = table.Column<string>(type: "text", nullable: false),
                    Disponibile = table.Column<bool>(type: "boolean", nullable: false),
                    DataRestituzione = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UtenteId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Libri", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Libri_Utenti_UtenteId",
                        column: x => x.UtenteId,
                        principalTable: "Utenti",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Libri_UtenteId",
                table: "Libri",
                column: "UtenteId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Libri");

            migrationBuilder.DropTable(
                name: "Utenti");
        }
    }
}
