using CoreBanking.Application.DTOs.Account;
using CoreBanking.Application.DTOs.Currency;
using CoreBanking.Application.Interfaces.IServices;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CoreBanking.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CurrenciesController : ControllerBase
    {
        ICurrencyService _currencyService;
        public CurrenciesController(ICurrencyService currencyService)
        {
            _currencyService = currencyService;
        }

        [HttpPost]
        public async Task<ActionResult<CurrencyResponseModel>> CreateAsync(CreateCurrencyRequest createCurrencyRequest, [FromServices] IValidator<CreateCurrencyRequest> validator)
        {
            validator.ValidateAndThrow(createCurrencyRequest);
            var createdClient = await _currencyService.CreateAsync(createCurrencyRequest);
            return Ok(createdClient);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CurrencyResponseModel>> GetCurrencyByIdAsync(Guid id)
        {
            return Ok(await _currencyService.GetCurrencyByIdAsync(id));
        }

        [HttpGet("Code")]
        public async Task<ActionResult<CurrencyResponseModel>> GetCurrencyByCodeAsync(GetCurrencyRateByCodeRequest code, [FromServices] IValidator<GetCurrencyRateByCodeRequest> validator)
        {
            validator.ValidateAndThrow(code);
            return Ok(await _currencyService.GetRateCurrencyByCodeAsync(code.Code));
        }

        [HttpPut]
        public async Task<ActionResult<CurrencyResponseModel>> UpdateCurrencyRateByIdAsync(UpdateCurrencyRateByIdRequest updateCurrencyRateRequest, [FromServices] IValidator<UpdateCurrencyRateByIdRequest> validator)
        {
            validator.ValidateAndThrow(updateCurrencyRateRequest);
            return Ok(await _currencyService.UpdateCurrencyRateByIdAsync(updateCurrencyRateRequest));
        }

        [HttpPut("Code")]
        public async Task<ActionResult<CurrencyResponseModel>> UpdateCurrencyByCodeRateAsync(UpdateCurrencyRateByCodeRequest updateCurrencyRateRequest, [FromServices] IValidator<UpdateCurrencyRateByCodeRequest> validator)
        {
            validator.ValidateAndThrow(updateCurrencyRateRequest);
            return Ok(await _currencyService.UpdateCurrencyRateByCodeAsync(updateCurrencyRateRequest));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> RemoveByIdAsync(Guid id)
        {
            await _currencyService.RemoveByIdAsync(id);
            return Ok();
        }
    }
}
