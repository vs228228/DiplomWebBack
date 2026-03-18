using DiplomWebBack.Application.DTOs.Tags.Responses;
using DiplomWebBack.Application.Usecases.Query.Tags;
using DiplomWebBack.Domain.Entities;
using DiplomWebBack.DomainRepos.Repos;
using Mapster;
using MediatR;

namespace DiplomWebBack.Application.Usecases.QueryHandlers.Tags
{
    public class GetPaginatedTagsQueryHandler : IRequestHandler<GetPaginatedTagsQuery, PaginatedList<TagResponseDto>>
    {
        private readonly ITagsRepository _repository;

        public GetPaginatedTagsQueryHandler(ITagsRepository repository)
        {
            _repository = repository;
        }

        public async Task<PaginatedList<TagResponseDto>> Handle(
            GetPaginatedTagsQuery request,
            CancellationToken cancellationToken)
        {
            var tags = await _repository.GetAllAsync(request.PageSize, request.PageNumber, cancellationToken);

            return tags.Adapt<PaginatedList<TagResponseDto>>();
        }
    }
}
