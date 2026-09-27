using CoreBanking.Application.DTOs.Client;
using CoreBanking.Application.DTOs.Client.CreateClient;
using CoreBanking.Domain.Entities;
using Riok.Mapperly.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoreBanking.Application.Mappers
{
    [Mapper]
    public partial class ClientMapper
    {
        public partial CreateClientRequest ClientToCreateClientRequest(Client client);
        public partial List<CreateClientRequest> ClientsToCreateClientRequests(List<Client> clients);

        public partial ClientResponseModel ClientToClientResponseModel(Client client);
        public partial List<ClientResponseModel> ClientsToClientResponseModels(List<Client> clients);

        //public partial void UpdateProductFromDTO(UpdateProductRequestDTO dto, Product product);
    }
}
