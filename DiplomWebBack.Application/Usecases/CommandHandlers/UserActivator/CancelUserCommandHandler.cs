using DiplomWebBack.Application.Usecases.Command.UserActivator;
using DiplomWebBack.Domain.CustomExceptions;
using DiplomWebBack.Domain.Repos;
using MediatR;

namespace DiplomWebBack.Application.Usecases.CommandHandlers.UserActivator
{
    public class CancelUserCommandHandler : IRequestHandler<CanlcelUserCommand, Unit>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserActivatorRepository _userActivatorRepository;

        public CancelUserCommandHandler(IUserRepository userRepository, IUserActivatorRepository userActivatorRepository)
        {
            _userRepository = userRepository;
            _userActivatorRepository = userActivatorRepository;
        }

        public async Task<Unit> Handle(CanlcelUserCommand request, CancellationToken cancellationToken)
        {
            var initiator = await _userRepository.GetByIdAsync(request.InitiatorId, cancellationToken);

            if (initiator is null)
            {
                throw new BadRequestException("Инициатор не найден");
            }

            if (initiator.Role is not Domain.Enums.UserRole.Manager && initiator.Role is not Domain.Enums.UserRole.Admin)
            {
                throw new ForbiddenException("Нет доступа на данное действие");
            }

            var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken, true, true, true);

            if (user is null)
            {
                throw new NotFoundException("Пользователь для деакцивации не найден");
            }

            user.IsActive = false;
            user.IsDelete = true;
            user.Email = user.Email + Guid.NewGuid();

            await _userRepository.UpdateAsync(user);

            var userActivator = await _userActivatorRepository.GetByUserIdAsync(request.UserId, cancellationToken, true);

            userActivator.AprovedById = initiator.Id;
            userActivator.AprovedDate = DateTime.UtcNow;

            await _userActivatorRepository.UpdateAsync(userActivator);

            return Unit.Value;
        }
    }
}
