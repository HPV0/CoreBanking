using CoreBanking.Application.DTOs.Account;
using CoreBanking.Application.DTOs.Transaction;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoreBanking.Application.Interfaces.IServices
{
    public interface ITransactionService
    {
        public Task<List<TransactionDetailResponseModel>> CreateAsync(CreateTransactionRequest createTransactionRequest);
        public Task<List<AccountTotalWithdrawalsResponseModel>> GetTotalWithdrawalsByAccountsAsync(DateTime timeFrom, DateTime timeTo);
        public Task<List<AccountCashFlowResponseModel>> GetTotalCashFlowsByAccountsAsync(DateTime timeFrom, DateTime timeTo);
        public Task<List<AccountTotalDepositesResponseModel>> GetAccountsTotalDepositBetweenDatesAsync(DateTime timeFrom, DateTime timeTo);
        public Task RemoveTransactionByIdAsync(Guid id);
    }
}
