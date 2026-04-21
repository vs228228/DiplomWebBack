using DiplomWebBack.Application.Services.Interfaces;
using DiplomWebBack.Application.Usecases.Command.User;
using DiplomWebBack.Domain.CustomExceptions;
using DiplomWebBack.Domain.Entities.Responses;
using DiplomWebBack.Domain.Enums;
using DiplomWebBack.Domain.Repos;
using MediatR;

namespace DiplomWebBack.Application.Usecases.CommandHandlers.User
{
    public class AddUserSkillCommandHandler : IRequestHandler<AddUserSkillCommand, Guid>
    {
        private readonly IUserVerificationService _userVerificationService;
        private readonly IUserSkillsRepository _skillsRepository;

        public AddUserSkillCommandHandler(IUserVerificationService userVerificationService, IUserSkillsRepository skillsRepository)
        {
            _userVerificationService = userVerificationService;
            _skillsRepository = skillsRepository;
        }

        public async Task<Guid> Handle(AddUserSkillCommand request, CancellationToken cancellationToken)
        {
            var initiator = await _userVerificationService.CheckIfUserValidAndGetAsync(request.InitiatorId, cancellationToken);

            if(request.UserId != request.InitiatorId && initiator.Role != UserRole.Manager && initiator.Role != UserRole.Admin)
            {
                throw new ForbiddenException("Нет прав");
            }

            var user = await _userVerificationService.CheckIfUserValidAndGetAsync(request.UserId, cancellationToken);

            var skills = new SkillExtraction()
            {
                TotalFound = 1,
                Skills = new List<SkillItem> { new SkillItem { Level = request.Request.Level, Name = request.Request.Name, Years = request.Request.Years } }
            };

            await _skillsRepository.SaveAsync(user.Id, skills);

            return skills.Skills[0].Id;
        }
    }
}
