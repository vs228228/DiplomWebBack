using DiplomWebBack.Api.Extensions;
using DiplomWebBack.Application.DTOs.UserActivator.Response;
using DiplomWebBack.Application.Usecases.Command.UserActivator;
using DiplomWebBack.Application.Usecases.Query.UserActivator;
using DiplomWebBack.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DiplomWebBack.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UserActivatorController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserActivatorController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<PaginatedList<UserActivatorResponse>>> GetAllUserActivatorAsync([FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            CancellationToken cancellationToken = default)
        {
            var userId = HttpContext.GetCurrentUserId();

            var list = await _mediator.Send(new GetAllUserActivatorsQuery(pageNumber, pageSize, userId), cancellationToken);

            return list;
        }

        [HttpPost("{userId}/activate-user")]
        public async Task<ActionResult> ActivateUserAsync([FromRoute] Guid userId, CancellationToken cancellationToken)
        {
            var initiatorId = HttpContext.GetCurrentUserId();

            await _mediator.Send(new ActivateUserCommand(userId, initiatorId), cancellationToken);

            return Ok();
        }

        [HttpPost("{userId}/cancel-user")]
        public async Task<ActionResult> CancellUserAsync([FromRoute] Guid userId, CancellationToken cancellationToken)
        {
            var initiatorId = HttpContext.GetCurrentUserId();

            await _mediator.Send(new CanlcelUserCommand(userId, initiatorId), cancellationToken);

            return Ok();
        }
    }
}
