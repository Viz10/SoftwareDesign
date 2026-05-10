using AccountService.Application.Commands;
using AccountService.Application.Queries;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Warehouse.Shared.DTOs.AccountDTO;

using LoginRequest = Warehouse.Shared.DTOs.AccountDTO.LoginRequest;
using RegisterRequest = Warehouse.Shared.DTOs.AccountDTO.RegisterRequest;


namespace AccountService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {

        private readonly IMediator mediator;
        private readonly IMapper mapper;

        public AccountController(IMediator _mediator, IMapper _mapper)
        {
            mediator = _mediator;
            mapper = _mapper;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest, CancellationToken cancellationToken)
        {
            LoginCommand loginCommand = mapper.Map<LoginCommand>(loginRequest);

            var result = await mediator.Send(loginCommand,cancellationToken);

            return result.IsSuccessful ? Ok(result.Value) : BadRequest(result.GetErrors());
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register( [FromBody] RegisterRequest registerRequest,CancellationToken cancellationToken)
        {
            RegisterAccountCommand registerAccountCommand = mapper.Map<RegisterAccountCommand>(registerRequest);

            var result = await mediator.Send(registerAccountCommand,cancellationToken);

            return result.IsSuccessful ? Ok(result.Value) : BadRequest(result.GetErrors());
        }

        [HttpGet("emails")]
        [Authorize(Roles = "Admin,Seller")]
        public async Task<IActionResult> GetAccountEmails(CancellationToken ct)
        {
            var result = await mediator.Send(new GetAccountEmailsQuery(),ct);
            return result.IsSuccessful ? Ok(result.Value) : NotFound(result.GetErrors());
        }
    }
}
