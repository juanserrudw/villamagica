using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace VillaMgic_API.Migrations
{
    /// <inheritdoc />
    public partial class AlimentarTablaVilla : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_villlas",
                table: "villlas");

            migrationBuilder.RenameTable(
                name: "villlas",
                newName: "villas");

            migrationBuilder.AddPrimaryKey(
                name: "PK_villas",
                table: "villas",
                column: "Id");

            migrationBuilder.InsertData(
                table: "villas",
                columns: new[] { "Id", "Amenidad", "Detalle", "FechaActualizacion", "Fechacreacion", "ImagenUrl", "MetrosCuadrados", "Nombre", "Ocupantes", "Tarifa" },
                values: new object[,]
                {
                    { 1, "", "detalles ...", new DateTime(2023, 3, 7, 12, 55, 56, 228, DateTimeKind.Local).AddTicks(6513), new DateTime(2023, 3, 7, 12, 55, 56, 228, DateTimeKind.Local).AddTicks(6504), "", 50, "villa real", 5, 200.0 },
                    { 2, "", "las mejores de la zona", new DateTime(2023, 3, 7, 12, 55, 56, 228, DateTimeKind.Local).AddTicks(6516), new DateTime(2023, 3, 7, 12, 55, 56, 228, DateTimeKind.Local).AddTicks(6516), "", 50, "villa el cachorro", 5, 200.0 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_villas",
                table: "villas");

            migrationBuilder.DeleteData(
                table: "villas",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "villas",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.RenameTable(
                name: "villas",
                newName: "villlas");

            migrationBuilder.AddPrimaryKey(
                name: "PK_villlas",
                table: "villlas",
                column: "Id");
        }
    }
}
