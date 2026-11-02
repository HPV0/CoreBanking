using CoreBanking.Application.DTOs.Transaction;
using CoreBanking.Application.Exceptions;
using CoreBanking.Application.Interfaces.Common;
using CoreBanking.Application.Interfaces.IRepositories;
using CoreBanking.Application.Interfaces.IServices;
using CoreBanking.Application.Mappers;
using CoreBanking.Domain.Constants;
using CoreBanking.Domain.Entities.AccountEntities;
using CoreBanking.Domain.Entities.CurrencyEntities;
using CoreBanking.Domain.Entities.TransactionEntities;


namespace CoreBanking.Application.Services
{
    public class TransactionService : ITransactionService
    {
        ITransactionRepository _transactionRepository;
        IAccountRepository _accountRepository;
        ICurrencyRepository _currencyRepository;
        TransactionMapper _mapper;
        IMoneyCalculator _moneyCalculator;
        IAccountBalanceService _accountBalanceService;
        IUnitOfWork _unitOfWork;
        public TransactionService(TransactionMapper mapper, ITransactionRepository transactionRepository, IAccountRepository accountRepository, ICurrencyRepository currencyRepository, IMoneyCalculator moneyCalculator, IAccountBalanceService _accountBalanceManager, IUnitOfWork unitOfWork)
        {
            _transactionRepository = transactionRepository;
            _accountRepository = accountRepository;
            _currencyRepository = currencyRepository;
            _mapper = mapper;
            _moneyCalculator = moneyCalculator;
            _accountBalanceService = _accountBalanceManager;
            _unitOfWork = unitOfWork;
        }

        async Task<List<Transaction>> BuildTransactionAsync(CreateTransactionRequest createTransactionRequest)
        {

            if (createTransactionRequest.SenderId == BankAccount.Id || createTransactionRequest.ReceiverId == BankAccount.Id)
                throw new UnprocessableException("Cannot do transaction with base account");

            var senderAccount = await _accountRepository.GetByIdAsync(createTransactionRequest.SenderId)
                ?? throw new EntityNotFoundException(nameof(Account), createTransactionRequest.SenderId);

            var receiverAccount = await _accountRepository.GetByIdAsync(createTransactionRequest.ReceiverId)
                ?? throw new EntityNotFoundException(nameof(Account), createTransactionRequest.ReceiverId);

            var baseAccount = await _accountRepository.GetByIdAsync(BankAccount.Id)
                ?? throw new EntityNotFoundException(nameof(Account), BankAccount.Id);

            decimal senderExchangeRate = createTransactionRequest.SenderExchangeRate
                    ?? (await _currencyRepository.GetActiveCurrencyRateByIdAsync(senderAccount.CurrencyId)
                        ?? throw new EntityNotFoundException(nameof(CurrencyRate), senderAccount.CurrencyId)).Rate;

            decimal receiverExchangeRate = createTransactionRequest.ReceiveExchangeRate
                ?? (await _currencyRepository.GetActiveCurrencyRateByIdAsync(receiverAccount.CurrencyId)
                    ?? throw new EntityNotFoundException(nameof(CurrencyRate), receiverAccount.CurrencyId)).Rate;

            decimal sendingAmount = createTransactionRequest.Amount;

            if (senderAccount.Id == receiverAccount.Id)
                throw new UnprocessableException("Sender and Recevier accounts cannot be same.");

            if (senderAccount.CurrencyId == BaseCurrency.Id && senderExchangeRate != 1)
                throw new UnprocessableException("Sender Exchange rate for base currency cannot be other then 1");

            if (receiverAccount.CurrencyId == BaseCurrency.Id && receiverExchangeRate != 1)
                throw new UnprocessableException("Receiver Exchange rate for base currency cannot be other then 1");

            List<Transaction> Details = new();

            if (senderAccount.CurrencyId == receiverAccount.CurrencyId)
            {
                var detail = CreateTransactionDetail(senderAccount, receiverAccount, 1, 1, sendingAmount, out _);
                await _transactionRepository.AddAsync(detail);
                Details.Add(detail);
            }
            else
            {
                decimal baseAccountRecevingAmount;

                var detail1 = CreateTransactionDetail(senderAccount, baseAccount, senderExchangeRate, 1, sendingAmount, out baseAccountRecevingAmount);
                await _transactionRepository.AddAsync(detail1);
                Details.Add(detail1);

                var detail2 = CreateTransactionDetail(baseAccount, receiverAccount, 1, receiverExchangeRate, baseAccountRecevingAmount, out _);
                await _transactionRepository.AddAsync(detail2);
                Details.Add(detail2);
            }

            return Details;
        }

        private Transaction CreateTransactionDetail(Account accountFrom, Account accountTo,
            decimal senderExchangeRate, decimal recevierExchangeRate,
            decimal sendingAmount, out decimal recevingAmount)
        {
            if (accountFrom.Id == accountTo.Id)
                throw new UnprocessableException("Sender and Recevier accounts cannot be same.");

            _accountBalanceService.Withdraw(accountFrom, sendingAmount);

            decimal finalExchangeRate = _moneyCalculator.ExchangeRateCalc(senderExchangeRate, recevierExchangeRate);

            recevingAmount = _moneyCalculator.ExchangeAmountCalc(finalExchangeRate, sendingAmount);

            _accountBalanceService.Deposit(accountTo, recevingAmount);

            Transaction detail = new Transaction { AccountFromId = accountFrom.Id, AccountToId = accountTo.Id, ExchangeRate = finalExchangeRate, Amount = sendingAmount };

            return detail;
        }

        public async Task<List<TransactionDetailResponseModel>> CreateAsync(CreateTransactionRequest createTransactionRequest)
        {
            var transactionDetails = await BuildTransactionAsync(createTransactionRequest);

            await _transactionRepository.SaveChangesAsync();

            return _mapper.ToTransactionDetailResponseModels(transactionDetails);
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
            return await _transactionRepository.GetTotalCashFlowsByAccountsAsync(timeFrom, timeTo);
        }

        public async Task<List<TransactionDetailResponseModel>> UndoTransactionByIdAsync(int id)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var originalTransactionDetails = await _transactionRepository.GetByTransactionIdAsync(id)
                ?? throw new EntityNotFoundException("Transaction", id);

                Transaction senderTransactionDetail, reciverTransactionDetail;
                GetTransactionDetailsSenderAndReceiver(originalTransactionDetails, out reciverTransactionDetail, out senderTransactionDetail);


                var reverseSenderExchangeRate = _moneyCalculator.ExchangeRateCalc(1, reciverTransactionDetail.ExchangeRate);
                var reverseReceiverExchangeRate = senderTransactionDetail.ExchangeRate;

                var reverseSendingAmount = reciverTransactionDetail.ExchangeRate * reciverTransactionDetail.Amount;
                _moneyCalculator.NormalizeMoneyAmount(ref reverseSendingAmount);

                var reverseCreateRequest = new CreateTransactionRequest
                {
                    SenderId = reciverTransactionDetail.AccountToId,
                    ReceiverId = senderTransactionDetail.AccountFromId,
                    SenderExchangeRate = reverseSenderExchangeRate,
                    ReceiveExchangeRate = reverseReceiverExchangeRate,
                    Amount = reverseSendingAmount
                };

                var reverseTransaction = await BuildTransactionAsync(reverseCreateRequest);

                await _transactionRepository.SaveChangesAsync();

                foreach (var detail in reverseTransaction)
                    _transactionRepository.Remove(detail);

                foreach (var detail in originalTransactionDetails)
                    _transactionRepository.Remove(detail);

                await _transactionRepository.SaveChangesAsync();

                await _unitOfWork.CommitTransactionAsync();

                return _mapper.ToTransactionDetailResponseModels(reverseTransaction);
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
            finally
            {
                _unitOfWork.Dispose();
            }

        }


        void GetTransactionDetailsSenderAndReceiver(List<Transaction> Details, out Transaction receiverAccount, out Transaction senderAccount)
        {
            if (Details.Count == 0)
                throw new Exception("Cannot get Sender And Receiver, when Details count is 0");

            receiverAccount = Details[0];
            senderAccount = Details[0];

            foreach (var transactionDetail in Details)
            {
                if (transactionDetail.AccountFromId != BankAccount.Id)
                {
                    senderAccount = transactionDetail;
                }
                else if (transactionDetail.AccountToId != BankAccount.Id)
                {
                    receiverAccount = transactionDetail;
                }
                else
                {
                    throw new Exception("No AccountFromId or AccountToId in transactions");
                }
            }

        }

        public async Task<TransactionResponseModel> GetTransactionByIdAsync(int id)
        {
            var transaction = await _transactionRepository.GetByTransactionIdAsync(id)
                ?? throw new EntityNotFoundException("Transaction", id);

            return _mapper.ToTransactionResponseModel(transaction);
        }

        public async Task<List<TransactionResponseModel>> GetAllTransactionsAsync()
        {
            var transactions = await _transactionRepository.GetAllTransactionsAsync();

            return _mapper.ToTransactionResponseModels(transactions);
        }

    }
}
