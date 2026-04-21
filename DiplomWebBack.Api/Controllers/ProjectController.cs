using DiplomWebBack.Api.Extensions;
using DiplomWebBack.Application.DTOs.Project.Request;
using DiplomWebBack.Application.DTOs.Project.Response;
using DiplomWebBack.Application.DTOs.User.Response;
using DiplomWebBack.Application.Usecases.Command.Projects;
using DiplomWebBack.Application.Usecases.Query.Projects;
using DiplomWebBack.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProjectController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProjectController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Получение проекта
    /// </summary>
    /// <param name="projectId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet("{projectId}")]
    public async Task<ActionResult<ProjectResponseDto>> GetProjectByIdAsync([FromRoute] Guid projectId, CancellationToken cancellationToken)
    {
        var initiatorId = HttpContext.GetCurrentUserId();

        var project = await _mediator.Send(new GetProjectByIdQuery(projectId, initiatorId));

        return Ok(project);
    }

    /// <summary>
    ///  Получение всех проектов пагинация. Фильтры добавим чуть позже
    /// </summary>
    /// <param name="pageNumber"></param>
    /// <param name="pageSize"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<ActionResult<PaginatedList<ProjectResponseDto>>> GetAllProjectsAsync([FromQuery] GetAllProjectsRequestDto request, CancellationToken cancellationToken = default)
    {
        var userId = HttpContext.GetCurrentUserId();

        var projects = await _mediator.Send(new GetAllProjectsQuery(request), cancellationToken);

        return Ok(projects);
    }

    /// <summary>
    /// Создание нового проекта
    /// </summary>
    /// <param name="project"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [Produces("application/json")]
    [Consumes("multipart/form-data")]
    [HttpPost]
    public async Task<ActionResult<Guid>> CreateProjectAsync([FromForm] ProjectCreateRequestDto project, CancellationToken cancellationToken)
    {
        var userId = HttpContext.GetCurrentUserId();

        var result = await _mediator.Send(new CreateProjectCommand(project, userId), cancellationToken);

        return Ok(result);
    }

    /// <summary>
    /// Удаление проекта
    /// </summary>
    /// <param name="projectId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpDelete("{projectId}")]
    public async Task<ActionResult> DeleteProjectAsync([FromRoute] Guid projectId, CancellationToken cancellationToken)
    {
        var userId = HttpContext.GetCurrentUserId();

        await _mediator.Send(new DeleteProjectCommand(projectId, userId), cancellationToken);

        return NoContent();
    }

    /// <summary>
    /// Обновление всех сотрудников проекта
    /// </summary>
    /// <param name="projectId"></param>
    /// <param name="dto"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPatch("{projectId}/users")]
    public async Task<ActionResult> UpdateUsersAsync([FromRoute] Guid projectId, [FromBody] UpdateProjectUsersRequestDto dto, CancellationToken cancellationToken)
    {
        var userId = HttpContext.GetCurrentUserId();

        await _mediator.Send(new UpdateProjectUsersCommand(dto, projectId, userId), cancellationToken);

        return Ok();
    }

    /// <summary>
    /// Обновление тегов проекта
    /// </summary>
    /// <param name="projectId"></param>
    /// <param name="dto"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPatch("{projectId}/tags")]
    public async Task<ActionResult> UpdateTagsAsync([FromRoute] Guid projectId, [FromBody] UpdateProjectTagsRequestDto dto, CancellationToken cancellationToken)
    {
        var userId = HttpContext.GetCurrentUserId();

        await _mediator.Send(new UpdateProjectTagsCommand(dto, projectId, userId), cancellationToken);

        return Ok();
    }

    /// <summary>
    /// Обновление основной инфы проекта(тайтл + дескрипшен)
    /// </summary>
    /// <param name="projectId"></param>
    /// <param name="dto"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPatch("{projectId}/info")]
    public async Task<ActionResult> UpdateInfoAsync([FromRoute] Guid projectId, [FromBody] UpdateProjectInfoRequestDto dto, CancellationToken cancellationToken)
    {
        var userId = HttpContext.GetCurrentUserId();

        await _mediator.Send(new UpdateProjectInfoCommand(dto, projectId, userId), cancellationToken);

        return Ok();
    }

    /// <summary>
    /// Получение всех сотрудников проекта
    /// </summary>
    /// <param name="projectId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet("{projectId}/users")]
    public async Task<ActionResult<IEnumerable<UserProjectResponseDto>>> GetUsersByProjectIdAsync([FromRoute] Guid projectId, CancellationToken cancellationToken)
    {
        var userId = HttpContext.GetCurrentUserId();

        var users = await _mediator.Send(new GetProjectUsersQuery(projectId, userId), cancellationToken);

        return Ok(users);
    }

    /// <summary>
    /// Получение всех обычных работяг проекта
    /// </summary>
    /// <param name="projectId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet("{projectId}/employees")]
    public async Task<ActionResult<IEnumerable<UserProjectResponseDto>>> GetEmployeesByProjectIdAsync([FromRoute]Guid projectId, CancellationToken cancellationToken)
    {
        var userId = HttpContext.GetCurrentUserId();

        var employees = await _mediator.Send(new GetProjectEmployeesQuery(projectId, userId), cancellationToken);

        return Ok(employees);
    }

    /// <summary>
    /// Получение всех менеджеров проекта
    /// </summary>
    /// <param name="projectId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet("{projectId}/managers")]
    public async Task<ActionResult<IEnumerable<UserProjectResponseDto>>> GetManagersByProjectIdAsync([FromRoute ]Guid projectId, CancellationToken cancellationToken)
    {
        var userId = HttpContext.GetCurrentUserId();

        var managers = await _mediator.Send(new GetProjectManagersQuery(projectId, userId), cancellationToken);

        return Ok(managers);
    }

    /// <summary>
    /// Получение всех тегов проекта
    /// </summary>
    /// <param name="projectId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet("{projectId}/tags")]
    public async Task<ActionResult<IEnumerable<Tag>>> GetTagsByProjectIdAsync([FromRoute] Guid projectId, CancellationToken cancellationToken)
    {
        var userId = HttpContext.GetCurrentUserId();

        var tags = await _mediator.Send(new GetProjectTagsQuery(projectId, userId), cancellationToken);

        return Ok(tags);
    }
}