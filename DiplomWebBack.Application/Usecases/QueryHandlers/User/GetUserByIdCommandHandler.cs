using DiplomWebBack.Application.DTOs.User.Response;
using DiplomWebBack.Application.Usecases.Query.User;
using DiplomWebBack.Domain.CustomExceptions;
using DiplomWebBack.Domain.Enums;
using DiplomWebBack.Domain.Repos;
using Mapster;
using MediatR;

namespace DiplomWebBack.Application.Usecases.QueryHandlers.User
{
    public class GetUserByIdCommandHandler : IRequestHandler<GetUserByIdCommand, UserProfileDto>
    {
        private readonly IUserRepository _userRepository;

        public GetUserByIdCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<UserProfileDto> Handle(GetUserByIdCommand request, CancellationToken cancellationToken)
        {
            var initiator = await _userRepository.GetByIdAsync(request.InitiatorId, cancellationToken);

            if(initiator is null)
            {
                throw new UnauthorizedException("Инициатор не найден");
            }

            var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);

            if (user is null)
            {
                throw new NotFoundException($"Пользователь с id {request.UserId} не найден");
            }

            var ans =  user.Adapt<UserProfileDto>();

            if(initiator.Role == UserRole.Admin)
            {
                ans.CanEdit = true;
            }
            else if(initiator.Id == user.Id)
            {
                ans.CanEdit = true;
            }
            else if (initiator.Role == UserRole.Manager && user.Role == UserRole.Employee)
            {
                ans.CanEdit = true;
            }
            else
            {
                ans.CanEdit = false;
            }

            return ans;
        }
    }
}
