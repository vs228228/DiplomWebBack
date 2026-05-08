using DiplomWebBack.Api.Extensions;
using DiplomWebBack.Application.DTOs.WorkLogs.Request;
using DiplomWebBack.Application.DTOs.WorkLogs.Response;
using DiplomWebBack.Application.Usecases.Command.WorkLogs;
using DiplomWebBack.Application.Usecases.Query.WorkLogs;
using DiplomWebBack.Application.Usecases.QueryHandlers.WorkLogs;
using DiplomWebBack.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DiplomWebBack.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class WorkLogsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public WorkLogsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<List<WorkLogMainInfoResponse>>> GetWorkLogsAsync([FromQuery] GetWorkLogsRequest request, CancellationToken cancellationToken)
        {
            var query = new GetWorkLogsQuery(request);

            var ans = await _mediator.Send(query, cancellationToken);

            return Ok(ans);
        }

        [HttpPost]
        public async Task<ActionResult<Guid>> CreateWorkLogAsync([FromBody] CreateWorkLogRequest request, CancellationToken cancellationToken)
        {
            var userId = HttpContext.GetCurrentUserId();

            var command = new CreateWorkLogCommand(userId, request);

            var id = await _mediator.Send(command, cancellationToken);

            return Ok(id);
        }
/*
        [HttpPut("{workLogId}")]
        public async Task<ActionResult> UpdateWorkLogByIdAsync([FromRoute] Guid workLogId, UpdateWorkLogReqeust request, CancellationToken cancellationToken)
        {
            var initiatorId = HttpContext.GetCurrentUserId();

            var command = new UpdateWorkLogCommand(workLogId, initiatorId, request);

            await _mediator.Send(command, cancellationToken);

            return Ok();
        } */

        [HttpDelete("{workLogId}")]
        public async Task<ActionResult> DeleteWorkLogByIdAsync([FromRoute] Guid workLogId, CancellationToken cancellationToken)
        {
            var initiatorId = HttpContext.GetCurrentUserId();

            var command = new DeleteWorkLogCommand(workLogId, initiatorId);

            await _mediator.Send(command, cancellationToken);

            return NoContent();
        }
    }
}
