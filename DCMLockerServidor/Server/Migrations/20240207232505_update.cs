using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DCMLockerServidor.Server.Migrations
{
    /// <inheritdoc />
    public partial class update : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "__efmigrationshistory",
                columns: table => new
                {
                    MigrationId = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false, collation: "utf8mb4_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ProductVersion = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: false, collation: "utf8mb4_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.MigrationId);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_unicode_ci");

            migrationBuilder.CreateTable(
                name: "empresas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nombre = table.Column<string>(type: "text", nullable: true, collation: "utf8mb4_unicode_520_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Active = table.Column<sbyte>(type: "tinyint", nullable: true),
                    TokenEmpresa = table.Column<string>(type: "text", nullable: true, collation: "utf8mb4_unicode_520_ci")
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_unicode_520_ci");

            migrationBuilder.CreateTable(
                name: "sizes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nombre = table.Column<string>(type: "text", nullable: true, collation: "utf8mb4_unicode_520_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Alto = table.Column<int>(type: "int", nullable: true),
                    Ancho = table.Column<int>(type: "int", nullable: true),
                    Profundidad = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_unicode_520_ci");

            migrationBuilder.CreateTable(
                name: "lockers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    NroSerieLocker = table.Column<string>(type: "text", nullable: true, collation: "utf8mb4_unicode_520_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Empresa = table.Column<int>(type: "int", nullable: true),
                    LastUpdateTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    Status = table.Column<string>(type: "text", nullable: true, collation: "utf8mb4_unicode_520_ci")
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Lockers_Empresa",
                        column: x => x.Empresa,
                        principalTable: "empresas",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_unicode_520_ci");

            migrationBuilder.CreateTable(
                name: "boxes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IdFisico = table.Column<int>(type: "int", nullable: true),
                    IdLocker = table.Column<int>(type: "int", nullable: false),
                    IdSize = table.Column<int>(type: "int", nullable: true),
                    Box = table.Column<int>(type: "int", nullable: true),
                    Puerta = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    Ocupacion = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    Libre = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    LastUpdateTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    Status = table.Column<string>(type: "text", nullable: true, collation: "utf8mb4_unicode_520_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Enable = table.Column<bool>(type: "tinyint(1)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Boxes_IdLocker",
                        column: x => x.IdLocker,
                        principalTable: "lockers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Boxes_IdSize",
                        column: x => x.IdSize,
                        principalTable: "sizes",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_unicode_520_ci");

            migrationBuilder.CreateTable(
                name: "tokens",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IdLocker = table.Column<int>(type: "int", nullable: true),
                    IdSize = table.Column<int>(type: "int", nullable: true),
                    IdBox = table.Column<int>(type: "int", nullable: true),
                    Token = table.Column<string>(type: "longtext", nullable: true, collation: "utf8mb4_unicode_520_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FechaCreacion = table.Column<DateTime>(type: "datetime", nullable: true),
                    FechaInicio = table.Column<DateTime>(type: "datetime", nullable: true),
                    FechaFin = table.Column<DateTime>(type: "datetime", nullable: true),
                    Contador = table.Column<int>(type: "int", nullable: true),
                    Confirmado = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    Modo = table.Column<string>(type: "text", nullable: true, collation: "utf8mb4_unicode_520_ci")
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tokens_IdBox",
                        column: x => x.IdBox,
                        principalTable: "boxes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Tokens_IdLocker",
                        column: x => x.IdLocker,
                        principalTable: "lockers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Tokens_IdSize",
                        column: x => x.IdSize,
                        principalTable: "sizes",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_unicode_520_ci");

            migrationBuilder.CreateIndex(
                name: "FK_Boxes_IdLocker",
                table: "boxes",
                column: "IdLocker");

            migrationBuilder.CreateIndex(
                name: "FK_Boxes_IdSize",
                table: "boxes",
                column: "IdSize");

            migrationBuilder.CreateIndex(
                name: "FK_Lockers_Empresa",
                table: "lockers",
                column: "Empresa");

            migrationBuilder.CreateIndex(
                name: "Id_UNIQUE",
                table: "lockers",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "FK_Tokens_IdBox",
                table: "tokens",
                column: "IdBox");

            migrationBuilder.CreateIndex(
                name: "FK_Tokens_IdLocker",
                table: "tokens",
                column: "IdLocker");

            migrationBuilder.CreateIndex(
                name: "FK_Tokens_IdSize",
                table: "tokens",
                column: "IdSize");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "__efmigrationshistory");

            migrationBuilder.DropTable(
                name: "tokens");

            migrationBuilder.DropTable(
                name: "boxes");

            migrationBuilder.DropTable(
                name: "lockers");

            migrationBuilder.DropTable(
                name: "sizes");

            migrationBuilder.DropTable(
                name: "empresas");
        }
    }
}
