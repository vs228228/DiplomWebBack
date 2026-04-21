using DiplomWebBack.Api.Extensions;
using DiplomWebBack.Application.DTOs.User.Request;
using DiplomWebBack.Application.Usecases.Command.User;
using DiplomWebBack.Application.Usecases.Query.User;
using DiplomWebBack.Domain.Entities.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DiplomWebBack.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SkillsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SkillsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Отправка резюме для получения скиллов
        /// </summary>
        /// <param name="resumeFile"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        [Produces("application/json")]
        [Consumes("multipart/form-data")]
        [HttpPost("extract-skills")]
        public async Task<ActionResult<SkillExtraction>> ExtractSkills(IFormFile resumeFile, CancellationToken ct)
        {
            var userId = HttpContext.GetCurrentUserId();

            var result = await _mediator.Send(new ExtractSkillsCommand(resumeFile, userId), ct);

            return Ok(result);
        }

        /// <summary>
        /// Получение скиллов юзера
        /// </summary>
        /// <param name="resumeFile"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        [HttpGet("{userId}")]
        public async Task<ActionResult<SkillExtraction>> GetUserSkillsAsync([FromRoute] Guid userId, CancellationToken ct)
        {
            var initiatorId = HttpContext.GetCurrentUserId();

            var result = await _mediator.Send(new GetUserSkillsQuery(userId, initiatorId), ct);

            return Ok(result);
        }

        /// <summary>
        /// Добавитm скилл (если скилл с таким именем есть у юзера, то не добавит ничего)
        /// </summary>
        /// <param name="request"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        [HttpPost("{userId}")]
        public async Task<ActionResult> AddUserSkillAsync([FromRoute] Guid userId, [FromBody] AddUserSkillRequestDto request, CancellationToken ct)
        {
            var initiatorId = HttpContext.GetCurrentUserId();

            var command = new AddUserSkillCommand(userId, initiatorId, request);

            var ans = await _mediator.Send(command, ct);

            return Ok();
        }

        /// <summary>
        /// Удалить скилл
        /// </summary>
        /// <param name="SkillId"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        [HttpDelete("{userId}")]
        public async Task<ActionResult> DeleteUserSkillAsync([FromRoute] Guid userId, [FromBody] Guid SkillId, CancellationToken ct)
        {
            var initiatorId = HttpContext.GetCurrentUserId();

            var command = new DeleteUserSkillCommand(userId, initiatorId, SkillId);

            await _mediator.Send(command, ct);

            return NoContent();
        }
    }
}
