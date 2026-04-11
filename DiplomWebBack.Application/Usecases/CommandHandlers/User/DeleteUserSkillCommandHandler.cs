using DiplomWebBack.Application.Services.Interfaces;
using DiplomWebBack.Application.Usecases.Command.User;
using DiplomWebBack.Domain.Entities.Responses;
using DiplomWebBack.Domain.Repos;
using MediatR;

namespace DiplomWebBack.Application.Usecases.CommandHandlers.User
{
    public class DeleteUserSkillCommandHandler : IRequestHandler<DeleteUserSkillCommand, Unit>
    {
        private readonly IUserVerificationService _userVerificationService;
        private readonly IUserSkillsRepository _skillsRepository;

        public DeleteUserSkillCommandHandler(IUserVerificationService userVerificationService, IUserSkillsRepository skillsRepository)
        {
            _userVerificationService = userVerificationService;
            _skillsRepository = skillsRepository;
        }

        public async Task<Unit> Handle(DeleteUserSkillCommand request, CancellationToken cancellationToken)
        {
            var user = await _userVerificationService.CheckIfUserValidAndGetAsync(request.UserId, cancellationToken);

            await _skillsRepository.RemoveSkillAsync(user.Id, request.SkillId);

            return Unit.Value;
        }
    }
}
