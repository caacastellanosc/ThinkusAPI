using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface IInsuranceRepository
    {

        Task<List<Poliza>> GetAllAsync();
        Task<Poliza?> GetByIdAsync(string id);
        Task<Poliza?> GetBySpecificAsync(string atributo);
        Task CreateAsync(Poliza poliza);
        Task UpdateAsync(string id, Poliza poliza);
        Task DeleteAsync(string id);
    }
}
