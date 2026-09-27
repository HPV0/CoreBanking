using CoreBanking.API.Validators.AddValidators;
using CoreBanking.Application.DTOs.Account;
using CoreBanking.Application.DTOs.Client;
using CoreBanking.Application.DTOs.Client.CreateClient;
using CoreBanking.Application.Interfaces.IServices;
using CoreBanking.Application.Services;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace CoreBanking.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        IAccountService _accountService;
        public AccountsController(IAccountService accountService)
        {
            _accountService = accountService;
        }
        [HttpPost]
        public async Task<ActionResult<AccountResponseModel>> CreateClientAccountAsync(CreateAccountRequest createAccountRequest, [FromServices] IValidator<CreateAccountRequest> validator)
        {
            validator.ValidateAndThrow(createAccountRequest);
            var createdClient = await _accountService.CreateAsync(createAccountRequest);
            return Ok(createdClient);
        }
        [HttpGet("Client/{id}")]
        public async Task<ActionResult<List<AccountResponseModel>>> GetAccountsByClientIdAsync(Guid id)
        {
            return Ok(await _accountService.GetAccountsByClientIdAsync(id));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AccountResponseModel>> GetAccountByIdAsync(Guid id)
        {
            return Ok(await _accountService.GetAccountByIdAsync(id));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> RemoveAccountByIdasync(Guid id)
        {
            await _accountService.RemoveAccountByIdAsync(id);
            return Ok();
        }
        [HttpPut("Withdraw/{id}")]
        public async Task<ActionResult<AccountResponseModel>> WithdrawCashByAccountIdAsync(Guid id,  CashFlowRequest cashflow, [FromServices] IValidator<CashFlowRequest> validator)
        {
            validator.ValidateAndThrow(cashflow);
            return Ok(await _accountService.WithdrawCashByAccountIdAsync(id, cashflow.Amount));
        }

        [HttpPut("Deposit/{id}")]
        public async Task<ActionResult<AccountResponseModel>> DepositCashByAccountIdAsync(Guid id, CashFlowRequest cashflow, [FromServices] IValidator<CashFlowRequest> validator)
        {
            validator.ValidateAndThrow(cashflow);
            return Ok(await _accountService.DepositCashByAccountIdAsync(id, cashflow.Amount));
        }




    }
}
