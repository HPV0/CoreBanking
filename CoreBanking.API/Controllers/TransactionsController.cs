using CoreBanking.Application.DTOs.Account;
using CoreBanking.Application.DTOs.Currency;
using CoreBanking.Application.DTOs.Transaction;
using CoreBanking.Application.Interfaces.IServices;
using CoreBanking.Application.Services;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Model;

namespace CoreBanking.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        ITransactionService _transactionService;
        public TransactionsController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }
        [HttpPost]
        public async Task<ActionResult<List<TransactionDetailResponseModel>>> CreateAsync(CreateTransactionRequest createTransactionRequest, [FromServices] IValidator<CreateTransactionRequest> validator)
        {
            validator.ValidateAndThrow(createTransactionRequest);
            var createdTransactionDetails = await _transactionService.CreateAsync(createTransactionRequest);
            return Ok(createdTransactionDetails);
        }

        [HttpGet("Withdrawals")]
        public async Task<ActionResult<List<AccountTotalWithdrawalsResponseModel>>> GetTotalWithdrawalsByAccountsAsync([FromQuery]  DateRangeRequest timeRange, [FromServices] IValidator<DateRangeRequest> validator)
        {
            validator.ValidateAndThrow(timeRange);
            var AccountsTotalWithdrawals = await _transactionService.GetTotalWithdrawalsByAccountsAsync(timeRange.timeFrom, timeRange.timeTo);
            return Ok(AccountsTotalWithdrawals);
        }
        [HttpGet("Deposits")]
        public async Task<ActionResult<List<AccountTotalWithdrawalsResponseModel>>> GetTotalDepositsByAccountsAsync([FromQuery] DateRangeRequest timeRange, [FromServices] IValidator<DateRangeRequest> validator)
        {
            validator.ValidateAndThrow(timeRange);
            var AccountsTotalDeposites = await _transactionService.GetAccountsTotalDepositBetweenDatesAsync(timeRange.timeFrom, timeRange.timeTo);
            return Ok(AccountsTotalDeposites);
        }
        [HttpGet("CashFlow")]
        public async Task<ActionResult<List<AccountCashFlowResponseModel>>> GetTotalCashFlowsByAccountsAsync([FromQuery]  DateRangeRequest timeRange, [FromServices] IValidator<DateRangeRequest> validator)
        {
            validator.ValidateAndThrow(timeRange);
            var AccountsTotalCashFlow = await _transactionService.GetTotalCashFlowsByAccountsAsync(timeRange.timeFrom, timeRange.timeTo);
            return Ok(AccountsTotalCashFlow);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> RemoveTransactionByIdAsync(Guid id)
        {
            await _transactionService.RemoveTransactionByIdAsync(id);
            return Ok();
        }
    }
}
