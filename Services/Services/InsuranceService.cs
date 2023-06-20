using Data.Interfaces;
using Data.Repository;
using Domain.Entities;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
  

    public class InsuranceService : IInsuranceService
    {
        private readonly IInsuranceRepository _insuranceRepository;

        public InsuranceService(IInsuranceRepository insuranceRepository)
        {
            _insuranceRepository = insuranceRepository;
        }

        public Task CreateAsync(Poliza poliza)
        {
            if (poliza.FechaInicioVigencia > DateTime.UtcNow || poliza.FechaFinVigencia < DateTime.UtcNow)
            {
                throw new Exception("La póliza debe estar vigente");
            }

            return _insuranceRepository.CreateAsync(poliza);
        }


        public Task<List<Poliza>> GetAllAsync()
        {
           return _insuranceRepository.GetAllAsync();
        }

        public Task<Poliza?> GetByIdAsync(string id)
        {
            return _insuranceRepository.GetByIdAsync(id);
        }

        public Task<Poliza?> GetBySpecificAsync(string atributo)
        {
            return _insuranceRepository.GetBySpecificAsync(atributo);
        }

        public Task UpdateAsync(string id, Poliza poliza)
        {
           return _insuranceRepository.UpdateAsync(id, poliza);
        }

        public Task DeleteAsync(string id)
        {
            return _insuranceRepository.DeleteAsync(id);
        }

        #region Private Methods
        private bool IsPolizaValid(Poliza poliza)
        {
            return poliza.FechaInicioVigencia <= DateTime.Now && poliza.FechaFinVigencia >= DateTime.Now;
        }
        #endregion


    }
}
