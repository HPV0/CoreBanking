using CoreBanking.Application.DTOs.Client;
using CoreBanking.Application.DTOs.Client.CreateClient;

namespace CoreBanking.Application.Interfaces.IServices
{
    public interface IClinetService
    {
        public Task<ClientResponseModel> CreateAsync(CreateClientRequest createClientRequest);
        public Task<ClientResponseModel> GetClientByIdAsync(int Id);
        public Task<List<ClientResponseModel>> GetClientsAsync();
        public Task RemoveByIdAsync(int Id);
        public Task<ClientResponseModel> UpdateByIdAsync(int id, UpdateClientRequest update);
    }
}
