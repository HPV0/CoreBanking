using CoreBanking.Application.DTOs.Client;
using CoreBanking.Application.DTOs.Client.CreateClient;
using CoreBanking.Application.Interfaces.IRepositories;
using CoreBanking.Application.Interfaces.IServices;
using CoreBanking.Application.Mappers;
using CoreBanking.Domain.Entities;
using CoreBanking.Infrastructure.Data.Context;
using CoreBanking.Infrastructure.Repositories;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CoreBanking.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        IClinetService _clientService;
        public ClientsController(IClinetService clinetService) {
            _clientService = clinetService;
        }
        [HttpPost]
        public async Task<ActionResult<ClientResponseModel>> CreateAsync(CreateClientRequest createClientRequest, [FromServices] IValidator<CreateClientRequest> validator) {
            validator.ValidateAndThrow(createClientRequest);
            var createdClient = await _clientService.CreateAsync(createClientRequest);
            return Ok(createdClient);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<ClientResponseModel>> GetClientByIdAsync(Guid id)
        {
            return Ok(await _clientService.GetClientByIdAsync(id));
        }
        [HttpGet]
        public async Task<ActionResult<List<ClientResponseModel>>> GetClientsAsync()
        {
            return Ok(await _clientService.GetClientsAsync());
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> RemoveClientByIdAsync(Guid id)
        {
            await _clientService.RemoveClientByIdAsync(id);
            return Ok();
        }
        [HttpPut("{id}")]
        public async Task<ActionResult<ClientResponseModel>> UpdateClientEmailAndPassportByIdAsync(Guid id, UpdateClientRequest update, [FromServices] IValidator<UpdateClientRequest> validator) {
            validator.ValidateAndThrow(update);
            var cl = await _clientService.UpdateClientEmailAndPassportNumberById(id, update);
            return Ok(cl);
        }
    }
}
