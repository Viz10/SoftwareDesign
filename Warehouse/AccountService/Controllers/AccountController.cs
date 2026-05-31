using AccountService.Application.Commands;
using AccountService.Application.Queries;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using LoginRequest = Warehouse.Shared.DTOs.AccountDTO.LoginRequest;
using RegisterRequest = Warehouse.Shared.DTOs.AccountDTO.RegisterRequest;
using SaveAccountEmailRequest = Warehouse.Shared.DTOs.AccountDTO.SaveAccountEmailRequest;

namespace AccountService.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class AccountController(IMediator _mediator, IMapper _mapper) : ControllerBase
    {

        private readonly IMediator mediator = _mediator;
        private readonly IMapper mapper = _mapper;

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest, CancellationToken cancellationToken)
        {
            LoginCommand loginCommand = mapper.Map<LoginCommand>(loginRequest);
            var result = await mediator.Send(loginCommand,cancellationToken); /// both handler and validator return Result
            return result.IsSuccessful ? Ok(result) : BadRequest(result); /// errors could come from either validation pipeline or handler
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterRequest registerRequest,CancellationToken cancellationToken)
        {
            RegisterAccountCommand registerAccountCommand = mapper.Map<RegisterAccountCommand>(registerRequest);
            var result = await mediator.Send(registerAccountCommand,cancellationToken);
            return result.IsSuccessful ? Ok(result) : BadRequest(result);
        }

        [HttpGet("emails")]
        [Authorize(Roles = "Admin,Seller")]
        public async Task<IActionResult> GetAccountEmails(CancellationToken ct)
        {
            var result = await mediator.Send(new GetAccountEmailsQuery(),ct);
            return result.IsSuccessful ? Ok(result) : NotFound(result);
        }

        [HttpPost("internal/save-email")]
        [Authorize(Roles = "Admin,Seller")]
        public async Task<IActionResult> SaveEmail([FromBody] SaveAccountEmailRequest req,CancellationToken ct)
        {
            SaveAccountEmailCommand command = mapper.Map<SaveAccountEmailCommand>(req);
            var result = await mediator.Send(command, ct);
            return result.IsSuccessful ? Ok(result) : BadRequest(result);
        }
    }
}
