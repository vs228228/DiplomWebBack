using DiplomWebBack.Application.DTOs.Tags.Responses;
using DiplomWebBack.Application.Usecases.Query;
using DiplomWebBack.DomainRepos.Repos;
using Mapster;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiplomWebBack.Application.Usecases.QueryHandlers
{
    public class GetPaginatedTagsQueryHandler : IRequestHandler<GetPaginatedTagsQuery, IEnumerable<TagResponseDto>>
    {
        private readonly ITagsRepository _repository;

        public GetPaginatedTagsQueryHandler(ITagsRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<TagResponseDto>> Handle(
            GetPaginatedTagsQuery request,
            CancellationToken cancellationToken)
        {
            var tags = await _repository.GetAllAsync(cancellationToken);

            return tags.Adapt<IEnumerable<TagResponseDto>>();
        }
    }
}
