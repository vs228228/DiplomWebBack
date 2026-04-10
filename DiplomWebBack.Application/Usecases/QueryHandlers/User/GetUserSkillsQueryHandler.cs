using DiplomWebBack.Application.Services.Interfaces;
using DiplomWebBack.Application.Usecases.Query.User;
using DiplomWebBack.Domain.CustomExceptions;
using DiplomWebBack.Domain.Entities.Responses;
using DiplomWebBack.Domain.Interfaces;
using DiplomWebBack.Domain.Repos;
using Mapster;
using MediatR;

namespace DiplomWebBack.Application.Usecases.QueryHandlers.User
{
    public class GetUserSkillsQueryHandler : IRequestHandler<GetUserSkillsQuery, SkillExtractionResponse>
    {
        private readonly IUserSkillsRepository _repository;
        private readonly IUserVerificationService _userVerificationService;

        public GetUserSkillsQueryHandler(IUserSkillsRepository repository, IUserVerificationService userVerificationService)
        {
            _repository = repository;
            _userVerificationService = userVerificationService;
        }

        public async Task<SkillExtractionResponse> Handle(GetUserSkillsQuery request, CancellationToken cancellationToken)
        {
            var initiator = await _userVerificationService.CheckIfUserValidAndGetAsync(request.InitiatorId, cancellationToken);

            if(request.UserId == request.InitiatorId)
            {
                return (await _repository.GetByUserIdAsync(initiator.Id)).Adapt<SkillExtractionResponse>();
            }

            if(false) // потом рассмотреть вариант того кто это может делать
            {
                throw new ForbiddenException("Юзер не moje");
            }

            return (await _repository.GetByUserIdAsync(request.UserId)).Adapt<SkillExtractionResponse>();
        }
    }
}
