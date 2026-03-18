using DiplomWebBack.Application.DTOs.User.Response;
using DiplomWebBack.Application.Usecases.Query.User;
using DiplomWebBack.Domain.CustomExceptions;
using DiplomWebBack.Domain.Repos;
using Mapster;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            return user.Adapt<UserProfileDto>();
        }
    }
}
