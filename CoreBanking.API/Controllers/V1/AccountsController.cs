using Asp.Versioning;
using CoreBanking.API.Filters.IdempotencyFilter;
using CoreBanking.Application.DTOs.Account;
using CoreBanking.Application.Interfaces.IServices;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace CoreBanking.API.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        IAccountService _accountService;

        public AccountsController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost]
        [Idempotent]
        public async Task<ActionResult<AccountResponseModel>> CreateAsync(
            CreateAccountRequest createAccountRequest,
            [FromServices] IValidator<CreateAccountRequest> validator)
        {
            validator.ValidateAndThrow(createAccountRequest);

            var createdClient = await _accountService.CreateAsync(createAccountRequest);

            return Ok(createdClient);
        }

        [HttpGet("client/{id}")]
        public async Task<ActionResult<List<AccountResponseModel>>> GetAccountsByClientIdAsync(int id)
        {
            var accounts = await _accountService.GetAccountsByClientIdAsync(id);

            return Ok(accounts);
        }

        [HttpGet]
        public async Task<ActionResult<List<AccountResponseModel>>> GetAccountsAsync()
        {
            var accounts = await _accountService.GetAccountsAsync();

            return Ok(accounts);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AccountResponseModel>> GetAccountByIdAsync(int id)
        {
            var account = await _accountService.GetAccountByIdAsync(id);

            return Ok(account);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> RemoveAccountByIdasync(int id)
        {
            await _accountService.RemoveAccountByIdAsync(id);

            return Ok();
        }

        [HttpPut("withdraw/{id}")]
        [Idempotent]
        public async Task<ActionResult<AccountResponseModel>> WithdrawCashByAccountIdAsync(int id, CashFlowRequest cashflow, [FromServices] IValidator<CashFlowRequest> validator)
        {
            validator.ValidateAndThrow(cashflow);

            var updatedAccount = await _accountService.WithdrawCashByAccountIdAsync(id, cashflow.Amount);

            return Ok(updatedAccount);
        }

        [HttpPut("deposit/{id}")]
        [Idempotent]
        public async Task<ActionResult<AccountResponseModel>> DepositCashByAccountIdAsync(int id, CashFlowRequest cashflow,
            [FromServices] IValidator<CashFlowRequest> validator)
        {
            validator.ValidateAndThrow(cashflow);

            var updatedAccount = await _accountService.DepositCashByAccountIdAsync(id, cashflow.Amount);

            return Ok(updatedAccount);
        }

    }
}
