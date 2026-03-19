using DiplomWebBack.Application.DTOs.Tags.Responses;
using DiplomWebBack.Domain.Entities;
using MediatR;

namespace DiplomWebBack.Application.Usecases.Query.Tags
{
    public record GetPaginatedTagsQuery(int PageSize, int PageNumber, string Search): IRequest<PaginatedList<TagResponseDto>>;
}
