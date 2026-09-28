using CoreBanking.Application.DTOs.Client;
using CoreBanking.Application.DTOs.Client.CreateClient;
using CoreBanking.Application.Exceptions;
using CoreBanking.Application.Interfaces.IRepositories;
using CoreBanking.Application.Interfaces.IServices;
using CoreBanking.Application.Mappers;
using CoreBanking.Domain.Constants;
using CoreBanking.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace CoreBanking.Application.Services
{
    public class ClinetService : IClinetService
    {
        private readonly ClientMapper _mapper;
        private IClientRepository _clientRepository;
        public ClinetService(IClientRepository clientRepository, ClientMapper mapper) {
            _clientRepository = clientRepository;
            _mapper = mapper;
        }
        public async Task<ClientResponseModel> CreateAsync(CreateClientRequest createClientRequest)
        {
            var client = Client.Create(createClientRequest.Name, createClientRequest.Surname, createClientRequest.Email,
                createClientRequest.Birthday, createClientRequest.PassportNumber, null);

            await _clientRepository.AddAsync(client);
            await _clientRepository.SaveChangesAsync();

            return _mapper.ClientToClientResponseModel(client);
        }

        public async Task<ClientResponseModel> GetClientByIdAsync(Guid Id)
        {
            var cl = await _clientRepository.GetByIdAsync(Id) ?? throw new EntityNotFoundException(nameof(Client), Id);
            return _mapper.ClientToClientResponseModel(cl);
        }

        public async Task<List<ClientResponseModel>> GetClientsAsync()
        {
            var clients = await _clientRepository.GetAllItemsAsync() ?? throw new EntityNotFoundException("No Client in this Table");
            return _mapper.ClientsToClientResponseModels(clients.ToList());
        }

        public async Task RemoveClientByIdAsync(Guid Id)
        {
            if (Id == BankClient.Id)
                throw new Exception("Cannot remove bank Client.");

            var cl = await _clientRepository.GetByIdAsync(Id) ?? throw new EntityNotFoundException(nameof(Client), Id);
            _clientRepository.Remove(cl);
            await _clientRepository.SaveChangesAsync();
        }

        public async Task<ClientResponseModel> UpdateClientEmailAndPassportNumberById(Guid id, UpdateClientRequest update)
        {
            var cl = await _clientRepository.GetByIdAsync(id) ?? throw new EntityNotFoundException(nameof(Client), id);

            cl.UpdateEmail(update.Email);
            cl.UpdatePassportNumber(update.PassportNumber);
            
            await _clientRepository.SaveChangesAsync();
            return _mapper.ClientToClientResponseModel(cl);
        }
    }
}
