using DiplomWebBack.Application.DTOs.User.Response;
using DiplomWebBack.Application.Usecases.Query.Projects;
using DiplomWebBack.Application.Usecases.Query.User;
using DiplomWebBack.Domain.Repos;
using Mapster;
using MediatR;

namespace DiplomWebBack.Application.Usecases.QueryHandlers.User
{
    public class GetAllUsersWithoutPaginationQueryHandler : IRequestHandler<GetAllUsersWithoutPaginationQuery, IEnumerable<UserProfileDto>>
    {
        private readonly IUserRepository _userRepository;

        public GetAllUsersWithoutPaginationQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }


        public async Task<IEnumerable<UserProfileDto>> Handle(GetAllUsersWithoutPaginationQuery request, CancellationToken cancellationToken)
        {
            var users = await _userRepository.GetAllAsync(cancellationToken);

            return users.Adapt<IEnumerable<UserProfileDto>>();
        }
    }
}
