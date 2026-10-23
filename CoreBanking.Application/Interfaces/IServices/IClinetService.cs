using CoreBanking.Application.DTOs.Client;
using CoreBanking.Application.DTOs.Client.CreateClient;

namespace CoreBanking.Application.Interfaces.IServices
{
    public interface IClinetService
    {
        public Task<ClientResponseModel> CreateAsync(CreateClientRequest createClientRequest);
        public Task<ClientResponseModel> GetClientByIdAsync(Guid Id);
        public Task<List<ClientResponseModel>> GetClientsAsync();
        public Task RemoveByIdAsync(Guid Id);

        public Task<ClientResponseModel> UpdateByIdAsync(Guid id, UpdateClientRequest update);
    }
}
