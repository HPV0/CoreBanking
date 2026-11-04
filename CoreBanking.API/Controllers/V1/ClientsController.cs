using Asp.Versioning;
using CoreBanking.API.Filters.IdempotencyFilter;
using CoreBanking.Application.DTOs.Client;
using CoreBanking.Application.DTOs.Client.CreateClient;
using CoreBanking.Application.Interfaces.IServices;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace CoreBanking.API.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        private readonly IClinetService _clientService;

        public ClientsController(IClinetService clientService)
        {
            _clientService = clientService;
        }

        [HttpPost]
        [Idempotent]
        public async Task<ActionResult<ClientResponseModel>> CreateAsync(
            CreateClientRequest request,
            [FromServices] IValidator<CreateClientRequest> validator)
        {
            validator.ValidateAndThrow(request);

            var createdClient = await _clientService.CreateAsync(request);

            return Ok(createdClient);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ClientResponseModel>> GetClientByIdAsync(int id)
        {
            var client = await _clientService.GetClientByIdAsync(id);

            return Ok(client);
        }

        [HttpGet]
        public async Task<ActionResult<List<ClientResponseModel>>> GetClientsAsync()
        {
            var clients = await _clientService.GetClientsAsync();

            return Ok(clients);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> RemoveClientByIdAsync(int id)
        {
            await _clientService.RemoveByIdAsync(id);

            return Ok();
        }

        [HttpPut("{id}")]
        [Idempotent]
        public async Task<ActionResult<ClientResponseModel>> UpdateByIdAsync(
            int id,
            UpdateClientRequest request,
            [FromServices] IValidator<UpdateClientRequest> validator)
        {
            validator.ValidateAndThrow(request);

            var updatedClient = await _clientService
                .UpdateByIdAsync(id, request);

            return Ok(updatedClient);
        }
    }


}