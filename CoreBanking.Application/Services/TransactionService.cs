using CoreBanking.Application.DTOs.Transaction;
using CoreBanking.Application.Exceptions;
using CoreBanking.Application.Interfaces.IRepositories;
using CoreBanking.Application.Interfaces.IServices;
using CoreBanking.Application.Mappers;
using CoreBanking.Domain.Constants;
using CoreBanking.Domain.Entities.AccountEntities;
using CoreBanking.Domain.Entities.TransactionEntities;
using CoreBanking.Domain.ValueObjects;


namespace CoreBanking.Application.Services
{
    public class TransactionService : ITransactionService
    {
        ITransactionRepository _transactionRepository;
        IAccountRepository _accountRepository;
        ICurrencyRepository _currencyRepository;
        TransactionMapper _mapper;
        public TransactionService(TransactionMapper mapper, ITransactionRepository transactionRepository, IAccountRepository accountRepository, ICurrencyRepository currencyRepository)
        {
            _transactionRepository = transactionRepository;
            _accountRepository = accountRepository;
            _currencyRepository = currencyRepository;
            _mapper = mapper;
        }

        async Task<Transaction> BuildTransactionAsync(CreateTransactionRequest createTransactionRequest) {
            var senderAccount = await _accountRepository.GetByIdAsync(createTransactionRequest.SenderId)
                ?? throw new EntityNotFoundException(nameof(Account), createTransactionRequest.SenderId);
            var receiverAccount = await _accountRepository.GetByIdAsync(createTransactionRequest.ReceiverId)
                ?? throw new EntityNotFoundException(nameof(Account), createTransactionRequest.SenderId);
            var baseAccount = await _accountRepository.GetByIdAsync(BankAccount.Id)
                ?? throw new EntityNotFoundException(nameof(Account), createTransactionRequest.SenderId);

            decimal senderExchangeRate = createTransactionRequest.SenderExchangeRate
                    ?? (await _currencyRepository.GetCurrencyWithCurrentRateByIdAsync(senderAccount.CurrencyId)).FindCurrentRate().Rate;

            decimal receiverExchangeRate = createTransactionRequest.ReceiveExchangeRate
                ?? (await _currencyRepository.GetCurrencyWithCurrentRateByIdAsync(receiverAccount.CurrencyId)).FindCurrentRate().Rate;

            var transaction = Transaction.Create(senderAccount, receiverAccount, baseAccount, senderExchangeRate, receiverExchangeRate, createTransactionRequest.Amount);

            return transaction;
        }

        public async Task<TransactionResponseModel> CreateAsync(CreateTransactionRequest createTransactionRequest)
        {
            var transaction = await BuildTransactionAsync(createTransactionRequest);

            await _transactionRepository.AddAsync(transaction);

            await _transactionRepository.SaveChangesAsync();

            return _mapper.ToTransactionResponseModel(transaction);
        }

        public async Task<List<AccountTotalDepositesResponseModel>> GetAccountsTotalDepositBetweenDatesAsync(DateTime timeFrom, DateTime timeTo)
        {
            return await _transactionRepository.GetAccountsTotalDepositBetweenDatesAsync(timeFrom, timeTo);
        }

        public async Task<List<AccountTotalWithdrawalsResponseModel>> GetTotalWithdrawalsByAccountsAsync(DateTime timeFrom, DateTime timeTo)
        {
            return await _transactionRepository.GetTotalWithdrawalsByAccountsAsync(timeFrom, timeTo);
        }

        public async Task<List<AccountCashFlowResponseModel>> GetTotalCashFlowsByAccountsAsync(DateTime timeFrom, DateTime timeTo)
        {
            var accountsDepoistes = await GetAccountsTotalDepositBetweenDatesAsync(timeFrom, timeTo);
            var accountsWithdrawals = await GetTotalWithdrawalsByAccountsAsync(timeFrom, timeTo);

            return accountsDepoistes
                .Select(deposit =>
                {
                    var withdrawal = accountsWithdrawals
                        .FirstOrDefault(w => w.AccountId == deposit.AccountId);

                    return new AccountCashFlowResponseModel
                    {
                        Id = deposit.AccountId,
                        CurrencyCode = deposit.CurrencyCode,
                        BalanceChangeAmount = deposit.TotalDeposites - withdrawal.TotalWithdrawals,
                        IncomesAmount = deposit.TotalDeposites,
                        SpendingAmount = withdrawal.TotalWithdrawals
                    };
                })
                .ToList();
        }

        public async Task<TransactionResponseModel> UndoTransactionByIdAsync(Guid id)
        {
            var originalTransaction = await _transactionRepository.GetByIdAsync(id) 
                ?? throw new EntityNotFoundException(nameof(Transaction), id);

            TransactionDetail senderTransactionDetail, reciverTransactionDetail;
            originalTransaction.GetTransactionDetailsSenderAndReceiver(out reciverTransactionDetail,out senderTransactionDetail);


            var backwordSenderExchangeRate = Money.ExchangeCalc(1, reciverTransactionDetail.ExchangeRate);
            var backwordReceiverExchangeRate = senderTransactionDetail.ExchangeRate;

            var backwordSendingAmount = reciverTransactionDetail.ExchangeRate * reciverTransactionDetail.Amount;
            Money.NormalizeMoneyAmount(ref backwordSendingAmount);

            var backwordCreateRequest = new CreateTransactionRequest {
                SenderId = reciverTransactionDetail.AccountToId,
                ReceiverId = senderTransactionDetail.AccountFromId,
                SenderExchangeRate = backwordSenderExchangeRate,
                ReceiveExchangeRate = backwordReceiverExchangeRate,
                Amount = backwordSendingAmount
            };

            var backwordTransaction = await BuildTransactionAsync(backwordCreateRequest);
            
            await _transactionRepository.AddAsync(backwordTransaction);

            await _transactionRepository.SoftDeleteTransactionAsync(backwordTransaction);

            await _transactionRepository.SoftDeleteTransactionAsync(originalTransaction);

            await _transactionRepository.SaveChangesAsync();

            return _mapper.ToTransactionResponseModel(backwordTransaction);
            
        }

        public async Task<TransactionResponseModel> GetTransactionByIdAsync(Guid id)
        {
            var transaction = await _transactionRepository.GetByIdAsync(id)
                ?? throw new EntityNotFoundException(nameof(Transaction), id);

            return _mapper.ToTransactionResponseModel(transaction);
        }

        public async Task<List<TransactionResponseModel>> GetAllTransactionsAsync()
        {
            var transactions = await _transactionRepository.GetAllItemsAsync();

            return _mapper.ToTransactionResponseModels(transactions.ToList());
        }

    }
}
