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
            var client = _mapper.ToClient(createClientRequest);

            var passportNumberExist = await _clientRepository.PassportNumberExistsAsync(createClientRequest.PassportNumber);
            if (passportNumberExist)
                throw new UnprocessableException("Passport number already exist.");

            await _clientRepository.AddAsync(client);

            await _clientRepository.SaveChangesAsync();

            return _mapper.ToClientResponseModel(client);
        }

        public async Task<ClientResponseModel> GetClientByIdAsync(int Id)
        {
            var cl = await _clientRepository.GetByIdAsync(Id)
                ?? throw new EntityNotFoundException(nameof(Client), Id);

            return _mapper.ToClientResponseModel(cl);
        }

        public async Task<List<ClientResponseModel>> GetClientsAsync()
        {
            var clients = await _clientRepository.GetAllItemsAsync();

            return _mapper.ToClientResponseModels(clients.ToList());
        }

        public async Task RemoveByIdAsync(int Id)
        {
            if (Id == BankClient.Id)
                throw new Exception("Cannot remove bank Client.");

            var cl = await _clientRepository.GetByIdAsync(Id)
                ?? throw new EntityNotFoundException(nameof(Client), Id);

            _clientRepository.Remove(cl);

            await _clientRepository.SaveChangesAsync();
        }

        public async Task<ClientResponseModel> UpdateByIdAsync(int id, UpdateClientRequest update)
        {
            var cl = await _clientRepository.GetByIdAsync(id)
                ?? throw new EntityNotFoundException(nameof(Client), id);

            if (update.Email is not null)
                cl.Email = update.Email;

            if (update.PassportNumber is not null)
            {
                var passportNumberExist = await _clientRepository.PassportNumberExistsAsync(update.PassportNumber);

                if (passportNumberExist)
                    throw new UnprocessableException("Passport number already exist.");

                cl.PassportNumber = update.PassportNumber;
            }

            if (update.Name is not null)
                cl.Name = update.Name;

            if (update.Surname is not null)
                cl.Surname = update.Surname;

            if (update.Birthday is not null)
                cl.Birthday = update.Birthday;

            await _clientRepository.SaveChangesAsync();

            return _mapper.ToClientResponseModel(cl);
        }
    }
}
