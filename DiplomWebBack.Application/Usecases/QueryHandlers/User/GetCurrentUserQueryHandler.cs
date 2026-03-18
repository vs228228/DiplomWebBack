using DiplomWebBack.Application.DTOs.User.Response;
using DiplomWebBack.Application.Usecases.Query.User;
using DiplomWebBack.Domain.CustomExceptions;
using DiplomWebBack.Domain.Repos;
using Mapster;
using MediatR;

namespace DiplomWebBack.Application.Usecases.QueryHandlers.User
{
    internal class GetCurrentUserQueryHandler : IRequestHandler<GetCurrentUserQuery, UserProfileDto>
    {
        private readonly IUserRepository _userRepository;

        public GetCurrentUserQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<UserProfileDto> Handle(GetCurrentUserQuery request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.Id, cancellationToken);

            if(user is null)
            {
                throw new NotFoundException("Пользователь не найден");
            }

            var userProfile = user.Adapt<UserProfileDto>();

            return userProfile;
        }
    }
}
