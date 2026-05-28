using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BibliotecaAPI.Migrations
{
    /// <inheritdoc />
    public partial class RimuoviLibriInPrestito : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Libri_Utenti_UtenteId",
                table: "Libri");

            migrationBuilder.DropIndex(
                name: "IX_Libri_UtenteId",
                table: "Libri");

            migrationBuilder.DropColumn(
                name: "UtenteId",
                table: "Libri");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UtenteId",
                table: "Libri",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Libri_UtenteId",
                table: "Libri",
                column: "UtenteId");

            migrationBuilder.AddForeignKey(
                name: "FK_Libri_Utenti_UtenteId",
                table: "Libri",
                column: "UtenteId",
                principalTable: "Utenti",
                principalColumn: "Id");
        }
    }
}
