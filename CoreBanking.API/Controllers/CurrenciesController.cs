using CoreBanking.API.Filters.IdempotencyFilter;
using CoreBanking.Application.DTOs.Currency;
using CoreBanking.Application.Interfaces.IServices;
using FluentValidation;
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
        [Idempotent]
        public async Task<ActionResult<CurrencyResponseModel>> CreateAsync(CreateCurrencyRequest createCurrencyRequest,
            [FromServices] IValidator<CreateCurrencyRequest> validator)
        {
            validator.ValidateAndThrow(createCurrencyRequest);

            var createdClient = await _currencyService.CreateAsync(createCurrencyRequest);

            return Ok(createdClient);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CurrencyResponseModel>> GetCurrencyByIdAsync(int id)
        {
            var currency = await _currencyService.GetCurrencyByIdAsync(id);

            return Ok(currency);
        }

        [HttpGet]
        public async Task<ActionResult<List<CurrencyWithAllRatesResponseModel>>> GetAllCurrenciesAsync()
        {
            var currencies = await _currencyService.GetAllCurrenciesAsync();

            return Ok(currencies);
        }

        [HttpGet("code")]
        public async Task<ActionResult<CurrencyResponseModel>> GetCurrencyByCodeAsync([FromQuery] GetCurrencyRateByCodeRequest request,
            [FromServices] IValidator<GetCurrencyRateByCodeRequest> validator)
        {
            validator.ValidateAndThrow(request);

            var currency = await _currencyService.GetCurrencyByCodeAsync(request.Code);

            return Ok(currency);
        }

        [HttpPut]
        [Idempotent]
        public async Task<ActionResult<CurrencyResponseModel>> UpdateCurrencyRateByIdAsync(UpdateCurrencyRateByIdRequest updateCurrencyRateRequest,
            [FromServices] IValidator<UpdateCurrencyRateByIdRequest> validator)
        {
            validator.ValidateAndThrow(updateCurrencyRateRequest);

            var updatedCurrency = await _currencyService.UpdateCurrencyRateByIdAsync(updateCurrencyRateRequest);

            return Ok(updatedCurrency);
        }

        [HttpPut("code")]
        [Idempotent]
        public async Task<ActionResult<CurrencyResponseModel>> UpdateCurrencyByCodeRateAsync(UpdateCurrencyRateByCodeRequest updateCurrencyRateRequest,
            [FromServices] IValidator<UpdateCurrencyRateByCodeRequest> validator)
        {
            validator.ValidateAndThrow(updateCurrencyRateRequest);

            var updatedCurrency = await _currencyService.UpdateCurrencyRateByCodeAsync(updateCurrencyRateRequest);

            return Ok(updatedCurrency);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> RemoveByIdAsync(int id)
        {
            await _currencyService.RemoveByIdAsync(id);

            return Ok();
        }
    }
}
