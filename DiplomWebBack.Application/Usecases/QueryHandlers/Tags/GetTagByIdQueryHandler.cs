using DiplomWebBack.Application.DTOs.Tags.Responses;
using DiplomWebBack.Application.Usecases.Query.Tags;
using DiplomWebBack.Domain.CustomExceptions;
using DiplomWebBack.DomainRepos.Repos;
using Mapster;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiplomWebBack.Application.Usecases.QueryHandlers.Tags
{
    public class GetTagByIdQueryHandler
    : IRequestHandler<GetTagByIdQuery, TagResponseDto?>
    {
        private readonly ITagsRepository _repository;

        public GetTagByIdQueryHandler(ITagsRepository repository)
        {
            _repository = repository;
        }

        public async Task<TagResponseDto?> Handle(
            GetTagByIdQuery request,
            CancellationToken cancellationToken)
        {
            var tag = await _repository.GetByIdAsync(request.Id, cancellationToken);

            if (tag is null)
            {
                throw new NotFoundException("Тег с таким id не найден");
            }
                

            return tag.Adapt<TagResponseDto>();
        }
    }
}
