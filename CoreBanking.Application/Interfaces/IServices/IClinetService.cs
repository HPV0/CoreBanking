using CoreBanking.Application.DTOs.Client;
using CoreBanking.Application.DTOs.Client.CreateClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoreBanking.Application.Interfaces.IServices
{
    public interface IClinetService
    {
        public Task<ClientResponseModel> CreateAsync(CreateClientRequest createClientRequest);
        public Task<ClientResponseModel> GetClientByIdAsync(Guid Id);
        public Task<List<ClientResponseModel>> GetClientsAsync();
        public Task RemoveClientByIdAsync(Guid Id);

        public Task<ClientResponseModel> UpdateClientEmailAndPassportNumberById(Guid id, UpdateClientRequest update);
    }
}
