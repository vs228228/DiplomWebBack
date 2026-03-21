using DiplomWebBack.Application.DTOs.User.Request;
using DiplomWebBack.Application.DTOs.User.Response;
using DiplomWebBack.Domain.Entities;
using MediatR;

namespace DiplomWebBack.Application.Usecases.Query.User
{
    public record GetAllUsersWithoutPaginationQuery(UserPaginatedRequestDto Dto) : IRequest<PaginatedList<UserProfileDto>>;
}
