using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using VillaMgic_API.Models;

namespace VillaMgic_API.Data
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Villa> villas { get; set; }
        
        public DbSet<NumeroVilla> numeroVillas { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Villa>().HasData(
                new Villa()
                {
                    Id = 1,
                    Nombre = "villa real",
                    Detalle = "detalles ...",
                    ImagenUrl = "",
                    Ocupantes= 5,
                    MetrosCuadrados= 50,
                    Tarifa= 200,
                    Amenidad= "",
                    Fechacreacion= DateTime.Now,
                    FechaActualizacion= DateTime.Now
                    
                    
                }, 
            new Villa()
            {
                Id = 2,
                Nombre = "villa el cachorro",
                Detalle = "las mejores de la zona",
                ImagenUrl = "",
                Ocupantes = 5,
                MetrosCuadrados = 50,
                Tarifa = 200,
                Amenidad = "",
                Fechacreacion = DateTime.Now,
                FechaActualizacion = DateTime.Now


            }) ;
        }




    }
}
