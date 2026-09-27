using CoreBanking.Application.DTOs.Transaction;
using CoreBanking.Application.Exceptions;
using CoreBanking.Application.Interfaces.IRepositories;
using CoreBanking.Application.Interfaces.IServices;
using CoreBanking.Application.Mappers;
using CoreBanking.Domain.Constants;
using CoreBanking.Domain.Entities;
using CoreBanking.Domain.Entities.AccountEntities;
using CoreBanking.Domain.Entities.TransactionEntities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoreBanking.Application.Services
{
    public class TransactionService : ITransactionService
    {
        ITransactionRepository _transactionRepository;
        readonly IUnitOfWork  _unitOfWork;
        IAccountRepository _accountRepository;
        ICurrencyRepository _currencyRepository;
        TransactionMapper _mapper;
        public TransactionService(TransactionMapper mapper, ITransactionRepository transactionRepository, IAccountRepository accountRepository, ICurrencyRepository currencyRepository, IUnitOfWork unitOfWork)
        {
            _transactionRepository = transactionRepository;
            _accountRepository = accountRepository;
            _unitOfWork = unitOfWork;
            _currencyRepository = currencyRepository;
            _mapper = mapper; 
        }
        public async Task<List<TransactionDetailResponseModel>> CreateAsync(CreateTransactionRequest createTransactionRequest)
        {
            List<TransactionDetail> details;
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var senderAccount = await _accountRepository.GetByIdAsync(createTransactionRequest.SenderId) ?? throw new EntityNotFoundException(nameof(Account), createTransactionRequest.SenderId);
                var receiverAccount = await _accountRepository.GetByIdAsync(createTransactionRequest.ReceiverId) ?? throw new EntityNotFoundException(nameof(Account), createTransactionRequest.SenderId);
                var baseAccount = await _accountRepository.GetByIdAsync(BankAccount.Id) ?? throw new EntityNotFoundException(nameof(Account), createTransactionRequest.SenderId);

                decimal senderExchangeRate = createTransactionRequest.SenderExchangeRate ?? (await _currencyRepository.GetCurrencyWithCurrentRateByIdAsync(senderAccount.CurrencyId)).FindCurrentRate().Rate;
                decimal receiverExchangeRate = createTransactionRequest.ReceiveExchangeRate ?? (await _currencyRepository.GetCurrencyWithCurrentRateByIdAsync(receiverAccount.CurrencyId)).FindCurrentRate().Rate;

                var transaction = Transaction.Create(senderAccount, receiverAccount, baseAccount, senderExchangeRate, receiverExchangeRate, createTransactionRequest.Amount);
                await _transactionRepository.AddAsync(transaction);
                details = transaction.Details!.ToList();
                await _transactionRepository.SaveChangesAsync();
            } catch 
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }

            await _unitOfWork.CommitTransactionAsync();
            return _mapper.TransactionDetailsToTransactionDetailResponseModels(details);

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

        public async Task RemoveTransactionByIdAsync(Guid id)
        {
            var tr = await _transactionRepository.GetOnlyTransactionWithNoDetailsByIdAsync(id) ?? throw new EntityNotFoundException(nameof(Transaction), id);
            _transactionRepository.Remove(tr);
            await _transactionRepository.SaveChangesAsync();
        }
    }
}
