using DiplomWebBack.Application.DTOs.User.Response;
using DiplomWebBack.Application.Usecases.Query.Projects;
using DiplomWebBack.Application.Usecases.Query.User;
using DiplomWebBack.Domain.Entities;
using DiplomWebBack.Domain.Repos;
using Mapster;
using MediatR;

namespace DiplomWebBack.Application.Usecases.QueryHandlers.User
{
    public class GetAllUsersWithoutPaginationQueryHandler : IRequestHandler<GetAllUsersWithoutPaginationQuery, PaginatedList<UserProfileDto>>
    {
        private readonly IUserRepository _userRepository;

        public GetAllUsersWithoutPaginationQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }


        public async Task<PaginatedList<UserProfileDto>> Handle(GetAllUsersWithoutPaginationQuery request, CancellationToken cancellationToken)
        {
            var users = await _userRepository.GetAllAsync(request.Dto.PageNumber, request.Dto.PageSize, cancellationToken);

            return users.Adapt<PaginatedList<UserProfileDto>>();
        }
    }
}
