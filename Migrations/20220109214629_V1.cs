using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebAPI.Migrations
{
    public partial class V1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Klubovi",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Naziv = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Klubovi", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Organizatori",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Naziv = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Sredstva = table.Column<double>(type: "float", nullable: false),
                    Sportski_objekat = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Organizatori", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Takmicari",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ime = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    Prezime = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Pol = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    Sport = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Kategorija = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Takmicari", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Takmicenja",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Naziv = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Sport = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Kategorija = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Datum_odrzavanja = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OrganizatorID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Takmicenja", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Takmicenja_Organizatori_OrganizatorID",
                        column: x => x.OrganizatorID,
                        principalTable: "Organizatori",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Registracije",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Datum_registracije = table.Column<DateTime>(type: "datetime2", nullable: false),
                    KlubID = table.Column<int>(type: "int", nullable: true),
                    TakmicarID = table.Column<int>(type: "int", nullable: true),
                    TakmicenjeID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Registracije", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Registracije_Klubovi_KlubID",
                        column: x => x.KlubID,
                        principalTable: "Klubovi",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Registracije_Takmicari_TakmicarID",
                        column: x => x.TakmicarID,
                        principalTable: "Takmicari",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Registracije_Takmicenja_TakmicenjeID",
                        column: x => x.TakmicenjeID,
                        principalTable: "Takmicenja",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Registracije_KlubID",
                table: "Registracije",
                column: "KlubID");

            migrationBuilder.CreateIndex(
                name: "IX_Registracije_TakmicarID",
                table: "Registracije",
                column: "TakmicarID");

            migrationBuilder.CreateIndex(
                name: "IX_Registracije_TakmicenjeID",
                table: "Registracije",
                column: "TakmicenjeID");

            migrationBuilder.CreateIndex(
                name: "IX_Takmicenja_OrganizatorID",
                table: "Takmicenja",
                column: "OrganizatorID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Registracije");

            migrationBuilder.DropTable(
                name: "Klubovi");

            migrationBuilder.DropTable(
                name: "Takmicari");

            migrationBuilder.DropTable(
                name: "Takmicenja");

            migrationBuilder.DropTable(
                name: "Organizatori");
        }
    }
}
