using DiplomWebBack.Application.Usecases.Command;
using DiplomWebBack.DomainRepos.Repos;
using MediatR;

namespace DiplomWebBack.Application.Usecases.CommandHandlers
{
    public class DeleteTagCommandHandler
     : IRequestHandler<DeleteTagCommand>
    {
        private readonly ITagsRepository _repository;

        public DeleteTagCommandHandler(ITagsRepository repository)
        {
            _repository = repository;
        }

        public async Task Handle(
            DeleteTagCommand request,
            CancellationToken cancellationToken)
        {
            var tag = await _repository.GetByIdAsync(request.Id, cancellationToken);

            if (tag == null)
                throw new Exception("Tag not found");

            await _repository.DeleteAsync(tag, cancellationToken);
        }
    }
}
