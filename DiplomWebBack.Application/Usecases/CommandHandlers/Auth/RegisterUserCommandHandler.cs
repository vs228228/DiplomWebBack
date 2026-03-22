using DiplomWebBack.Application.Usecases.Command.Auth;
using DiplomWebBack.Domain.CustomExceptions;
using DiplomWebBack.Domain.Entities;
using DiplomWebBack.Domain.Interfaces;
using DiplomWebBack.Domain.Repos;
using MediatR;

namespace DiplomWebBack.Application.Usecases.CommandHandlers.Auth
{
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, Unit>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IUserActivatorRepository _userActivatorRepository;

        public RegisterUserCommandHandler(IUserRepository userRepository, IPasswordHasher passwordHasher, IUserActivatorRepository userActivator)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _userActivatorRepository = userActivator;
        }

        public async Task<Unit> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            if(await _userRepository.GetByEmailAsync(request.dto.Email, cancellationToken) is not null)
            {
                throw new BadRequestException("Пользователь с таким email уже есть");
            }

            if(await _userRepository.GetByEmailAsync(request.dto.Email, cancellationToken, false, true) is not null)
            {
                throw new BadRequestException("Пользователь с таким email уже есть, однако он удалён");
            }

            if (await _userRepository.GetByEmailAsync(request.dto.Email, cancellationToken, false, false, true) is not null)
            {
                throw new BadRequestException("Пользователь с таким email уже есть, однако он не подтверждён админом/менеджером");
            }

            var user = new Domain.Entities.User()
            {
                Id = Guid.NewGuid(),
                CreatedAt = DateTime.UtcNow,
                Email = request.dto.Email,
                IsActive = false,
                IsDelete = false,
                Name = request.dto.Name,
                Surname = request.dto.Surname,
                Patronymic = request.dto.Patronymic,
                PhoneNumber = request.dto.PhoneNumber,
                Role = Domain.Enums.UserRole.Employee,
                PasswordHash = _passwordHasher.HashPassword(request.dto.Password),

            };

            await _userRepository.AddAsync(user);

            var activator = new Domain.Entities.UserActivator()
            {
                Id = Guid.NewGuid(),
                User = user,
                UserId = user.Id,
            };

            await _userActivatorRepository.AddAsync(activator, cancellationToken);

            return Unit.Value;
        }
    }
}
