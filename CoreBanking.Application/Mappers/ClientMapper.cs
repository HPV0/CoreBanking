using CoreBanking.Application.DTOs.Client;
using CoreBanking.Application.DTOs.Client.CreateClient;
using CoreBanking.Domain.Entities;
using Riok.Mapperly.Abstractions;

namespace CoreBanking.Application.Mappers
{
    [Mapper]
    public partial class ClientMapper
    {
        public partial CreateClientRequest ToCreateClientRequest(Client client);
        public partial List<CreateClientRequest> ToCreateClientRequests(List<Client> clients);

        public partial ClientResponseModel ToClientResponseModel(Client client);
        public partial List<ClientResponseModel> ToClientResponseModels(List<Client> clients);
    }
}
