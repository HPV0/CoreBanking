using CoreBanking.Application.DTOs.Account;
using CoreBanking.Application.Exceptions;
using CoreBanking.Application.Interfaces.IRepositories;
using CoreBanking.Application.Interfaces.IServices;
using CoreBanking.Application.Mappers;
using CoreBanking.Domain.Constants;
using CoreBanking.Domain.Entities;
using CoreBanking.Domain.Entities.AccountEntities;
using CoreBanking.Domain.Entities.CurrencyEntities;
using CoreBanking.Domain.ValueObjects;

namespace CoreBanking.Application.Services
{
    public class AccountService : IAccountService
    {
        private IAccountRepository _accountRepository;

        private IClientRepository _clientRepository;
        
        private ICurrencyRepository _currencyRepository;
        
        private AccountMapper _mapper;
        
        public AccountService(IAccountRepository accountRepository, IClientRepository clientRepository, ICurrencyRepository currencyRepository, AccountMapper mapper)
        {
            _accountRepository = accountRepository;
            _clientRepository = clientRepository;
            _currencyRepository = currencyRepository;
            _mapper = mapper;
        }
        
        public async Task<AccountResponseModel> CreateAsync(CreateAccountRequest createAccountRequest)
        {
            if (await _accountRepository.ClientHasAccountWithCurrencyCodeAsync(createAccountRequest.ClientId, createAccountRequest.CurrencyCode))
                throw new UnprocessableException("Client already have account with this currency.");

            var client = await _clientRepository.GetByIdAsync(createAccountRequest.ClientId) ?? throw new EntityNotFoundException(nameof(Client), createAccountRequest.ClientId);
            var currency = await _currencyRepository.GetCurrencyWithCurrentRateByCodeAsync(createAccountRequest.CurrencyCode) ?? throw new EntityNotFoundException(nameof(Currency), createAccountRequest.CurrencyCode); ;
            
            var newAccount = Account.Create(currency, client);
            
            client.AddAccount(newAccount);
            
            await _accountRepository.AddAsync(newAccount);
            
            await _accountRepository.SaveChangesAsync();
            
            return _mapper.ToAccountResponseModel(newAccount);
        }

        public async Task<AccountResponseModel> DepositCashByAccountIdAsync(Guid id, decimal amount)
        {
            var account = await _accountRepository.GetByIdAsync(id)
                ?? throw new EntityNotFoundException(nameof(Account), id);
            
            account.Deposit(Money.Create(amount, account.Currency.CurrencyCode));
            
            await _accountRepository.SaveChangesAsync();
            
            return _mapper.ToAccountResponseModel(account);
        }

        public async Task<AccountResponseModel> GetAccountByIdAsync(Guid id)
        {
            var account = await _accountRepository.GetByIdAsync(id)
                ?? throw new EntityNotFoundException(nameof(Account), id);
            
            return _mapper.ToAccountResponseModel(account);
        }

        public async Task<List<AccountResponseModel>?> GetAccountsAsync()
        {
            var accounts = await _accountRepository.GetAllItemsAsync();

            return _mapper.ToAccountResponseModels(accounts.ToList());
        }

        public async Task<List<AccountResponseModel>> GetAccountsByClientIdAsync(Guid id)
        {
            var client = await _clientRepository.GetByIdAsync(id) 
                ?? throw new EntityNotFoundException(nameof(Client), id);

            var accounts = await _accountRepository.GetAccountsByClientIdAsync(id);
            
            return _mapper.ToAccountResponseModels(accounts);

        }

        public async Task RemoveAccountByIdAsync(Guid id)
        {
            if (id == BankAccount.Id)
                throw new Exception("Cannot remove bank account.");

            var acc = await _accountRepository.GetOnlyAccountByIdAsync(id) 
                ?? throw new EntityNotFoundException(nameof(Account), id);
            
            _accountRepository.Remove(acc);
            
            await _accountRepository.SaveChangesAsync();
        }

        public async Task<AccountResponseModel> WithdrawCashByAccountIdAsync(Guid id, decimal amount)
        {
            var account = await _accountRepository.GetByIdAsync(id) 
                ?? throw new EntityNotFoundException(nameof(Account), id);

            account.Withdraw(Money.Create(amount, account.Currency.CurrencyCode));
            
            await _accountRepository.SaveChangesAsync();
            
            return _mapper.ToAccountResponseModel(account);
        }
    }
}
