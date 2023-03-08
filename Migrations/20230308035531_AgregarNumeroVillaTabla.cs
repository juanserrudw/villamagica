using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VillaMgic_API.Migrations
{
    /// <inheritdoc />
    public partial class AgregarNumeroVillaTabla : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "numeroVillas",
                columns: table => new
                {
                    VillaNo = table.Column<int>(type: "int", nullable: false),
                    VillaId = table.Column<int>(type: "int", nullable: false),
                    detallesEspecial = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaActualizacion = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_numeroVillas", x => x.VillaNo);
                    table.ForeignKey(
                        name: "FK_numeroVillas_villas_VillaId",
                        column: x => x.VillaId,
                        principalTable: "villas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "villas",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "FechaActualizacion", "Fechacreacion" },
                values: new object[] { new DateTime(2023, 3, 7, 22, 55, 31, 205, DateTimeKind.Local).AddTicks(7864), new DateTime(2023, 3, 7, 22, 55, 31, 205, DateTimeKind.Local).AddTicks(7847) });

            migrationBuilder.UpdateData(
                table: "villas",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "FechaActualizacion", "Fechacreacion" },
                values: new object[] { new DateTime(2023, 3, 7, 22, 55, 31, 205, DateTimeKind.Local).AddTicks(7870), new DateTime(2023, 3, 7, 22, 55, 31, 205, DateTimeKind.Local).AddTicks(7869) });

            migrationBuilder.CreateIndex(
                name: "IX_numeroVillas_VillaId",
                table: "numeroVillas",
                column: "VillaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "numeroVillas");

            migrationBuilder.UpdateData(
                table: "villas",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "FechaActualizacion", "Fechacreacion" },
                values: new object[] { new DateTime(2023, 3, 7, 12, 55, 56, 228, DateTimeKind.Local).AddTicks(6513), new DateTime(2023, 3, 7, 12, 55, 56, 228, DateTimeKind.Local).AddTicks(6504) });

            migrationBuilder.UpdateData(
                table: "villas",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "FechaActualizacion", "Fechacreacion" },
                values: new object[] { new DateTime(2023, 3, 7, 12, 55, 56, 228, DateTimeKind.Local).AddTicks(6516), new DateTime(2023, 3, 7, 12, 55, 56, 228, DateTimeKind.Local).AddTicks(6516) });
        }
    }
}
