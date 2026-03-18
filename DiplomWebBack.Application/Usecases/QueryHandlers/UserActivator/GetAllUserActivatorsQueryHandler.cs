using DiplomWebBack.Application.DTOs.UserActivator.Response;
using DiplomWebBack.Application.Usecases.Query.UserActivator;
using DiplomWebBack.Domain.CustomExceptions;
using DiplomWebBack.Domain.Entities;
using DiplomWebBack.Domain.Repos;
using Mapster;
using MediatR;

namespace DiplomWebBack.Application.Usecases.QueryHandlers.UserActivator
{
    public class GetAllUserActivatorsQueryHandler : IRequestHandler<GetAllUserActivatorsQuery, PaginatedList<UserActivatorResponse>>
    {
        private readonly IUserActivatorRepository _userActivatorRepository;
        private readonly IUserRepository _userRepository;

        public GetAllUserActivatorsQueryHandler(IUserActivatorRepository userActivatorRepository, IUserRepository userRepository)
        {
            _userActivatorRepository = userActivatorRepository;
            _userRepository = userRepository;
        }

        public async Task<PaginatedList<UserActivatorResponse>> Handle(GetAllUserActivatorsQuery request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);

            if(user is null)
            {
                throw new BadRequestException("Инициатор не найден");
            }

            if(user.Role != Domain.Enums.UserRole.Admin && user.Role != Domain.Enums.UserRole.Manager)
            {
                throw new ForbiddenException("Нет доступа к этому ресурсу");
            }

            var list = await _userActivatorRepository.GetAllAsync(request.pageNumber, request.pageSize, cancellationToken);

            return list.Adapt<PaginatedList<UserActivatorResponse>>();
        }
    }
}
