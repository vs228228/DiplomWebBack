using DiplomWebBack.Application.DTOs.Tags.Responses;
using DiplomWebBack.Application.Usecases.Command;
using DiplomWebBack.Domain.CustomExceptions;
using DiplomWebBack.Domain.Entities;
using DiplomWebBack.DomainRepos.Repos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiplomWebBack.Application.Usecases.CommandHandlers
{
    public class AddTagCommandHandler
    : IRequestHandler<AddTagCommand, Guid>
    {
        private readonly ITagsRepository _repository;

        public AddTagCommandHandler(ITagsRepository repository)
        {
            _repository = repository;
        }

        public async Task<Guid> Handle(
            AddTagCommand request,
            CancellationToken cancellationToken)
        {
            if(await _repository.GetByTitleAsync(request.Title, cancellationToken) is not null)
            {
                throw new BadRequestException("Тег с таким заголовком уже существует");
            }

            var tag = new Tag
            {
                Id = Guid.NewGuid(),
                Title = request.Title
            };

            await _repository.AddAsync(tag, cancellationToken);

            return tag.Id;
        }
    }
}
