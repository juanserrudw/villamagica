using VillaMgic_API.Models;

namespace VillaMgic_API.Repository.IRepository
{
    public interface IVillaRepository:IRepository<Villa> 
    {
        Task<Villa> Actualizar(Villa entidad);
    }
}
