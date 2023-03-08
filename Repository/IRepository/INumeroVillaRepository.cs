using VillaMgic_API.Models;

namespace VillaMgic_API.Repository.IRepository
{
    public interface INumeroVillaRepository:IRepository<NumeroVilla> 
    {
        Task<NumeroVilla> Actualizar(NumeroVilla entidad);
    }
}
