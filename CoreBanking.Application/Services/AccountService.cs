using CoreBanking.Application.DTOs.Account;
using CoreBanking.Application.Exceptions;
using CoreBanking.Application.Interfaces.Common;
using CoreBanking.Application.Interfaces.IRepositories;
using CoreBanking.Application.Interfaces.IServices;
using CoreBanking.Application.Mappers;
using CoreBanking.Domain.Constants;
using CoreBanking.Domain.Entities;
using CoreBanking.Domain.Entities.AccountEntities;
using CoreBanking.Domain.Entities.CurrencyEntities;

namespace CoreBanking.Application.Services
{
    public class AccountService : IAccountService
    {
        private IAccountRepository _accountRepository;

        private IClientRepository _clientRepository;

        private ICurrencyRepository _currencyRepository;

        private AccountMapper _mapper;
        private IMoneyCalculator _moneyCalculator;
        private IAccountBalanceService _accountBalanceService;


        public AccountService(IAccountRepository accountRepository, IClientRepository clientRepository, ICurrencyRepository currencyRepository, AccountMapper mapper, IMoneyCalculator moneyCalculator, IAccountBalanceService accountBalanceManager)
        {
            _accountRepository = accountRepository;
            _clientRepository = clientRepository;
            _currencyRepository = currencyRepository;
            _mapper = mapper;
            _moneyCalculator = moneyCalculator;
            _accountBalanceService = accountBalanceManager;
        }

        public async Task<AccountResponseModel> CreateAsync(CreateAccountRequest createAccountRequest)
        {
            bool clientExists = await _clientRepository.IsExistsAsync(createAccountRequest.ClientId);
            if (!clientExists)
                throw new EntityNotFoundException(nameof(Client), createAccountRequest.ClientId);

            var currency = await _currencyRepository.GetOnlyCurrencyByCodeAsync(createAccountRequest.CurrencyCode)
                ?? throw new EntityNotFoundException(nameof(Currency), createAccountRequest.CurrencyCode); ;

            var newAccount = new Account
            {
                Currency = currency,
                ClientId = createAccountRequest.ClientId,
                Balance = new AccountBalance { Amount = 0 }
            };

            await _accountRepository.AddAsync(newAccount);

            await _accountRepository.SaveChangesAsync();

            return _mapper.ToAccountResponseModel(newAccount);
        }

        public async Task<AccountResponseModel> DepositCashByAccountIdAsync(int id, decimal amount)
        {
            var account = await _accountRepository.GetByIdAsync(id)
                ?? throw new EntityNotFoundException(nameof(Account), id);

            _accountBalanceService.Deposit(account, amount);

            await _accountRepository.SaveChangesAsync();

            return _mapper.ToAccountResponseModel(account);
        }

        public async Task<AccountResponseModel> GetAccountByIdAsync(int id)
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

        public async Task<List<AccountResponseModel>> GetAccountsByClientIdAsync(int id)
        {
            var client = await _clientRepository.GetByIdAsync(id)
                ?? throw new EntityNotFoundException(nameof(Client), id);

            var accounts = await _accountRepository.GetAccountsByClientIdAsync(id);

            return _mapper.ToAccountResponseModels(accounts);

        }

        public async Task RemoveAccountByIdAsync(int id)
        {
            if (id == BankAccount.Id)
                throw new Exception("Cannot remove bank account.");

            var acc = await _accountRepository.GetByIdAsync(id)
                ?? throw new EntityNotFoundException(nameof(Account), id);

            if (acc.Balance.Amount != 0)
                throw new UnprocessableException("Account isn't empty, so it cannot be removed");

            _accountRepository.Remove(acc);

            await _accountRepository.SaveChangesAsync();
        }

        public async Task<AccountResponseModel> WithdrawCashByAccountIdAsync(int id, decimal amount)
        {
            var account = await _accountRepository.GetByIdAsync(id)
                ?? throw new EntityNotFoundException(nameof(Account), id);

            _accountBalanceService.Withdraw(account, amount);

            await _accountRepository.SaveChangesAsync();

            return _mapper.ToAccountResponseModel(account);
        }
    }
}
