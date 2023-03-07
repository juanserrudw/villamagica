using VillaMgic_API.Models.Dto;

namespace VillaMgic_API.Data
{
    public static class VillaStore
    {
        public static List<VillaDto> villaList =new List<VillaDto>
        {
              new VillaDto{Id=1, Nombre="vista el rancho", Ocupantes=3, MetrosCuadrados=50},
              new VillaDto{Id=2, Nombre="vista al rio", Ocupantes=4, MetrosCuadrados=80}
        };
    }
}
