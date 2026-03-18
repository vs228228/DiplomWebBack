using DiplomWebBack.Application.DTOs.Tags.Responses;
using DiplomWebBack.Application.Usecases.Command;
using DiplomWebBack.Application.Usecases.Command.Tags;
using DiplomWebBack.Application.Usecases.Query;
using DiplomWebBack.Application.Usecases.Query.Tags;
using DiplomWebBack.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DiplomWebBack.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TagsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TagsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Получить все теги
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<PaginatedList<TagResponseDto>>> GetPaginatedTagsAsync([FromQuery] int pageNumber = 1, int pageSize = 10,
            CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(
                new GetPaginatedTagsQuery(pageSize, pageNumber),
                cancellationToken);

            return Ok(result);
        }

        /// <summary>
        /// Получить один тег
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<TagResponseDto>> GetTagByIdAsync(
            [FromRoute] Guid id,
            CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(
                new GetTagByIdQuery(id),
                cancellationToken);

            return Ok(result);
        }

        /// <summary>
        /// Добавить тег
        /// </summary>
        /// <param name="Title"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<Guid>> AddTagAsync(
            [FromQuery] string Title,
            CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(
                new AddTagCommand(Title),
                cancellationToken);

            return Ok(result);
        }

        /// <summary>
        /// Удалить тег
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTagAsync(
            [FromRoute] Guid id,
            CancellationToken cancellationToken = default)
        {
            await _mediator.Send(
                new DeleteTagCommand(id),
                cancellationToken);

            return NoContent();
        }
    }
}
