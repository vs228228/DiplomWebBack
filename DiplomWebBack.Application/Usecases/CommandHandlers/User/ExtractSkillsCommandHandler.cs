using DiplomWebBack.Application.DTOs.User.Response;
using DiplomWebBack.Application.Services.Interfaces;
using DiplomWebBack.Application.Usecases.Command.User;
using DiplomWebBack.Domain.CustomExceptions;
using DiplomWebBack.Domain.Enums;
using DiplomWebBack.Domain.Interfaces;
using DiplomWebBack.Domain.Repos;
using Mapster;
using MediatR;

namespace DiplomWebBack.Application.Usecases.CommandHandlers.User
{
    public class ExtractSkillsCommandHandler : IRequestHandler<ExtractSkillsCommand, SkillResponseDto>
    {
        private readonly IExternalApiService _externalApi;
        private readonly IUserSkillsRepository _repository;
        private readonly IUserVerificationService _userVerificationService;

        public ExtractSkillsCommandHandler(IExternalApiService externalApi, IUserSkillsRepository repository, IUserVerificationService userVerificationService)
        {
            _externalApi = externalApi;
            _repository = repository;
            _userVerificationService = userVerificationService;
        }

        public async Task<SkillResponseDto> Handle(ExtractSkillsCommand request, CancellationToken cancellationToken)
        {
            var initiator = await _userVerificationService.CheckIfUserValidAndGetAsync(request.InitiatorId, cancellationToken);

            if(request.UserId != request.InitiatorId && initiator.Role == UserRole.Employee)
            {
                throw new ForbiddenException("Нет доступа к обновлению резюме этого юзера");
            }

            if (request.ResumeFile == null || request.ResumeFile.Length == 0)
            {
                throw new BadRequestException("Файл пуст");
            }

            var user = await _userVerificationService.CheckIfUserValidAndGetAsync(request.UserId, cancellationToken);

            var extractionResult = await _externalApi.ExtractSkillsAsync(request.ResumeFile, cancellationToken);

            await _repository.SaveAsync(request.UserId, extractionResult);

            return extractionResult.Adapt<SkillResponseDto>();
        }
    }
}
