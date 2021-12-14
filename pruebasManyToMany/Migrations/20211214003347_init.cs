using Microsoft.EntityFrameworkCore.Migrations;

namespace pruebasManyToMany.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Asignatura",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Asignatura", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Estudiantes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombres = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Apellido1 = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Apellido2 = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Estudiantes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EstudianteAsignaturas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EstudianteId = table.Column<int>(type: "int", nullable: false),
                    AsignaturaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EstudianteAsignaturas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EstudianteAsignaturas_Asignatura_AsignaturaId",
                        column: x => x.AsignaturaId,
                        principalTable: "Asignatura",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EstudianteAsignaturas_Estudiantes_EstudianteId",
                        column: x => x.EstudianteId,
                        principalTable: "Estudiantes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EstudianteAsignaturas_AsignaturaId",
                table: "EstudianteAsignaturas",
                column: "AsignaturaId");

            migrationBuilder.CreateIndex(
                name: "IX_EstudianteAsignaturas_EstudianteId",
                table: "EstudianteAsignaturas",
                column: "EstudianteId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EstudianteAsignaturas");

            migrationBuilder.DropTable(
                name: "Asignatura");

            migrationBuilder.DropTable(
                name: "Estudiantes");
        }
    }
}
