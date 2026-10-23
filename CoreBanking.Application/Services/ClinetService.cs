using CoreBanking.Application.DTOs.Client;
using CoreBanking.Application.DTOs.Client.CreateClient;
using CoreBanking.Application.Exceptions;
using CoreBanking.Application.Interfaces.IRepositories;
using CoreBanking.Application.Interfaces.IServices;
using CoreBanking.Application.Mappers;
using CoreBanking.Domain.Constants;
using CoreBanking.Domain.Entities;

namespace CoreBanking.Application.Services
{
    public class ClinetService : IClinetService
    {
        private readonly ClientMapper _mapper;

        private IClientRepository _clientRepository;
        
        public ClinetService(IClientRepository clientRepository, ClientMapper mapper)
        {
            _clientRepository = clientRepository;
            _mapper = mapper;
        }
        
        public async Task<ClientResponseModel> CreateAsync(CreateClientRequest createClientRequest)
        {
            var client = Client.Create(createClientRequest.Name, createClientRequest.Surname, createClientRequest.Email,
                createClientRequest.Birthday, createClientRequest.PassportNumber, null);

            await _clientRepository.AddAsync(client);

            await _clientRepository.SaveChangesAsync();

            return _mapper.ToClientResponseModel(client);
        }

        public async Task<ClientResponseModel> GetClientByIdAsync(Guid Id)
        {
            var cl = await _clientRepository.GetByIdAsync(Id) 
                ?? throw new EntityNotFoundException(nameof(Client), Id);
            
            return _mapper.ToClientResponseModel(cl);
        }

        public async Task<List<ClientResponseModel>> GetClientsAsync()
        {
            var clients = await _clientRepository.GetAllItemsAsync() 
                ?? throw new EntityNotFoundException("No Client in this Table");
            
            return _mapper.ToClientResponseModels(clients.ToList());
        }

        public async Task RemoveByIdAsync(Guid Id)
        {
            if (Id == BankClient.Id)
                throw new Exception("Cannot remove bank Client.");

            var cl = await _clientRepository.GetByIdAsync(Id)
                ?? throw new EntityNotFoundException(nameof(Client), Id);
            
            _clientRepository.Remove(cl);
            
            await _clientRepository.SaveChangesAsync();
        }

        public async Task<ClientResponseModel> UpdateByIdAsync(Guid id, UpdateClientRequest update)
        {
            var cl = await _clientRepository.GetByIdAsync(id) 
                ?? throw new EntityNotFoundException(nameof(Client), id);

            if (update.Email is not null)
                cl.UpdateEmail(update.Email);
            
            if (update.PassportNumber is not null)
                cl.UpdatePassportNumber(update.PassportNumber);

            if (update.Name is not null)
                cl.UpdateName(update.Name);

            if (update.Surname is not null)
                cl.UpdateSurname(update.Surname);

            if (update.Birthday is not null)
                cl.UpdateBirthday(update.Birthday);

            await _clientRepository.SaveChangesAsync();

            return _mapper.ToClientResponseModel(cl);
        }
    }
}
