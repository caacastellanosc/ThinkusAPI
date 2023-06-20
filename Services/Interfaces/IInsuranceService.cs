using Domain.Entities;
using DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IInsuranceService
    {

        Task<List<Poliza>> GetAllAsync();
        Task<Poliza?> GetByIdAsync(string id);
        Task<Poliza?> GetBySpecificAsync(string atributo);
        Task CreateAsync(Poliza poliza);
        Task UpdateAsync(string id, Poliza poliza);
        Task DeleteAsync(string id);

    }
}
