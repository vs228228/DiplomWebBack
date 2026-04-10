using DiplomWebBack.Application.Services.Interfaces;
using DiplomWebBack.Application.Usecases.Command.User;
using DiplomWebBack.Domain.CustomExceptions;
using DiplomWebBack.Domain.Repos;
using MediatR;

namespace DiplomWebBack.Application.Usecases.CommandHandlers.User
{
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, Unit>
    {
        private readonly IUserVerificationService _userVerificationService;
        private readonly IUserRepository _userRepository;

        public UpdateUserCommandHandler(IUserVerificationService userVerificationService, IUserRepository userRepository)
        {
            _userVerificationService = userVerificationService;
            _userRepository = userRepository;
        }

        public async Task<Unit> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            Domain.Entities.User user = null!;

            if(request.InitiatorId == request.UserId)
            {
                user = await _userVerificationService.CheckIfUserValidAndGetAsync(request.InitiatorId, cancellationToken);
            }
            else
            {
                var initiator = await _userVerificationService.CheckIfUserValidAndGetAsync(request.InitiatorId, cancellationToken);

                if(!(initiator.Role is Domain.Enums.UserRole.Admin) && !(initiator.Role is Domain.Enums.UserRole.Manager))
                {
                    throw new ForbiddenException("Нет доступа к редактированию пользователей");
                }
                
                user = await _userVerificationService.CheckIfUserValidAndGetAsync(request.UserId, cancellationToken);
            }

            user.Surname = request.Request.Surname;
            user.Name = request.Request.Name;
            user.Patronymic = request.Request.Patronymic;
            user.PhoneNumber = request.Request.PhoneNumber;
            user.Position = request.Request.Position;

            await _userRepository.UpdateAsync(user);

            return Unit.Value;

        }
    }
}
