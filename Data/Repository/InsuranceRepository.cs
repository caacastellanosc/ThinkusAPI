using Data.Interfaces;
using Data.Models;
using Domain.Entities;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repository
{
    public class InsuranceRepository:IInsuranceRepository
    {
        private readonly IMongoCollection<Poliza> _polizaCollection;

        public InsuranceRepository(
            IOptions<InsuranceStoreDatabaseSettings> bookStoreDatabaseSettings)
        {
            var mongoClient = new MongoClient(
                bookStoreDatabaseSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                bookStoreDatabaseSettings.Value.DatabaseName);

            _polizaCollection = mongoDatabase.GetCollection<Poliza>(
                bookStoreDatabaseSettings.Value.InsuranceCollectionName);
        }

        public async Task<List<Poliza>> GetAllAsync() =>
            await _polizaCollection.Find(_ => true).ToListAsync();

        public async Task<Poliza?> GetByIdAsync(string id) =>
            await _polizaCollection.Find(x => x.Id == id).FirstOrDefaultAsync();
        public async Task<Poliza?> GetBySpecificAsync(string atributo)
        {
            var filter = Builders<Poliza>.Filter.Eq(x => x.PlacaAutomotor, atributo) |
                         Builders<Poliza>.Filter.Eq(x => x.NumeroPoliza, atributo);

            return await _polizaCollection.Find(filter).FirstOrDefaultAsync();
        }


        public async Task CreateAsync(Poliza newpoliza) =>
            await _polizaCollection.InsertOneAsync(newpoliza);

        public async Task UpdateAsync(string id, Poliza updatepoliza) =>
            await _polizaCollection.ReplaceOneAsync(x => x.Id == id, updatepoliza);

        public async Task DeleteAsync(string id) =>
            await _polizaCollection.DeleteOneAsync(x => x.Id == id);

  
    }
}
