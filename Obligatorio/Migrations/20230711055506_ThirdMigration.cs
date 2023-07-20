using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Obligatorio.Migrations
{
    /// <inheritdoc />
    public partial class ThirdMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Pelicula",
                table: "Horarios");

            migrationBuilder.DropColumn(
                name: "Sala",
                table: "Horarios");

            migrationBuilder.AddColumn<int>(
                name: "PeliculaId",
                table: "Horarios",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SalaId",
                table: "Horarios",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Horarios_PeliculaId",
                table: "Horarios",
                column: "PeliculaId");

            migrationBuilder.CreateIndex(
                name: "IX_Horarios_SalaId",
                table: "Horarios",
                column: "SalaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Horarios_Peliculas_PeliculaId",
                table: "Horarios",
                column: "PeliculaId",
                principalTable: "Peliculas",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Horarios_Salas_SalaId",
                table: "Horarios",
                column: "SalaId",
                principalTable: "Salas",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Horarios_Peliculas_PeliculaId",
                table: "Horarios");

            migrationBuilder.DropForeignKey(
                name: "FK_Horarios_Salas_SalaId",
                table: "Horarios");

            migrationBuilder.DropIndex(
                name: "IX_Horarios_PeliculaId",
                table: "Horarios");

            migrationBuilder.DropIndex(
                name: "IX_Horarios_SalaId",
                table: "Horarios");

            migrationBuilder.DropColumn(
                name: "PeliculaId",
                table: "Horarios");

            migrationBuilder.DropColumn(
                name: "SalaId",
                table: "Horarios");

            migrationBuilder.AddColumn<int>(
                name: "Pelicula",
                table: "Horarios",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Sala",
                table: "Horarios",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
