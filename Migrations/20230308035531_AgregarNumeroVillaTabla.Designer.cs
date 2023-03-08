﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using VillaMgic_API.Data;

#nullable disable

namespace VillaMgic_API.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20230308035531_AgregarNumeroVillaTabla")]
    partial class AgregarNumeroVillaTabla
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("VillaMgic_API.Models.NumeroVilla", b =>
                {
                    b.Property<int>("VillaNo")
                        .HasColumnType("int");

                    b.Property<DateTime>("FechaActualizacion")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("FechaCreacion")
                        .HasColumnType("datetime2");

                    b.Property<int>("VillaId")
                        .HasColumnType("int");

                    b.Property<string>("detallesEspecial")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("VillaNo");

                    b.HasIndex("VillaId");

                    b.ToTable("numeroVillas");
                });

            modelBuilder.Entity("VillaMgic_API.Models.Villa", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Amenidad")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Detalle")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("FechaActualizacion")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("Fechacreacion")
                        .HasColumnType("datetime2");

                    b.Property<string>("ImagenUrl")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("MetrosCuadrados")
                        .HasColumnType("int");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Ocupantes")
                        .HasColumnType("int");

                    b.Property<double>("Tarifa")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.ToTable("villas");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Amenidad = "",
                            Detalle = "detalles ...",
                            FechaActualizacion = new DateTime(2023, 3, 7, 22, 55, 31, 205, DateTimeKind.Local).AddTicks(7864),
                            Fechacreacion = new DateTime(2023, 3, 7, 22, 55, 31, 205, DateTimeKind.Local).AddTicks(7847),
                            ImagenUrl = "",
                            MetrosCuadrados = 50,
                            Nombre = "villa real",
                            Ocupantes = 5,
                            Tarifa = 200.0
                        },
                        new
                        {
                            Id = 2,
                            Amenidad = "",
                            Detalle = "las mejores de la zona",
                            FechaActualizacion = new DateTime(2023, 3, 7, 22, 55, 31, 205, DateTimeKind.Local).AddTicks(7870),
                            Fechacreacion = new DateTime(2023, 3, 7, 22, 55, 31, 205, DateTimeKind.Local).AddTicks(7869),
                            ImagenUrl = "",
                            MetrosCuadrados = 50,
                            Nombre = "villa el cachorro",
                            Ocupantes = 5,
                            Tarifa = 200.0
                        });
                });

            modelBuilder.Entity("VillaMgic_API.Models.NumeroVilla", b =>
                {
                    b.HasOne("VillaMgic_API.Models.Villa", "Villa")
                        .WithMany()
                        .HasForeignKey("VillaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Villa");
                });
#pragma warning restore 612, 618
        }
    }
}
