using CoreBanking.API.Filters.IdempotencyFilter;
using CoreBanking.Application.DTOs.Transaction;
using CoreBanking.Application.Interfaces.IServices;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

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
        [Idempotent]
        public async Task<ActionResult<List<TransactionDetailResponseModel>>> CreateAsync(CreateTransactionRequest createTransactionRequest, 
            [FromServices] IValidator<CreateTransactionRequest> validator)
        {
            validator.ValidateAndThrow(createTransactionRequest);

            var createdTransactionDetails = await _transactionService.CreateAsync(createTransactionRequest);
            
            return Ok(createdTransactionDetails);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TransactionResponseModel>> GetTransactionById(Guid id)
        {
            var transaction = await _transactionService.GetTransactionByIdAsync(id);

            return Ok(transaction);
        }


        [HttpGet("all")]
        public async Task<ActionResult<List<TransactionResponseModel>>> GetAllTransactionsAsync()
        {
            var transactions = await _transactionService.GetAllTransactionsAsync();

            return Ok(transactions);
        }

        [HttpGet("withdrawals")]
        public async Task<ActionResult<List<AccountTotalWithdrawalsResponseModel>>> GetTotalWithdrawalsByAccountsAsync([FromQuery] DateRangeRequest timeRange, 
            [FromServices] IValidator<DateRangeRequest> validator)
        {
            validator.ValidateAndThrow(timeRange);

            var accountsTotalWithdrawals = await _transactionService.GetTotalWithdrawalsByAccountsAsync(timeRange.timeFrom, timeRange.timeTo);
            
            return Ok(accountsTotalWithdrawals);
        }

        [HttpGet("deposits")]
        public async Task<ActionResult<List<AccountTotalWithdrawalsResponseModel>>> GetTotalDepositsByAccountsAsync([FromQuery] DateRangeRequest timeRange,
            [FromServices] IValidator<DateRangeRequest> validator)
        {
            validator.ValidateAndThrow(timeRange);

            var accountsTotalDeposites = await _transactionService.GetAccountsTotalDepositBetweenDatesAsync(timeRange.timeFrom, timeRange.timeTo);
            
            return Ok(accountsTotalDeposites);
        }

        [HttpGet("cashFlow")]
        public async Task<ActionResult<List<AccountCashFlowResponseModel>>> GetTotalCashFlowsByAccountsAsync([FromQuery] DateRangeRequest timeRange,
            [FromServices] IValidator<DateRangeRequest> validator)
        {
            validator.ValidateAndThrow(timeRange);

            var accountsTotalCashFlow = await _transactionService.GetTotalCashFlowsByAccountsAsync(timeRange.timeFrom, timeRange.timeTo);
            
            return Ok(accountsTotalCashFlow);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> RemoveTransactionByIdAsync(Guid id)
        {
            await _transactionService.RemoveTransactionByIdAsync(id);
            
            return Ok();
        }
    }
}
