using DiplomWebBack.Application.DTOs.User.Response;
using MediatR;

namespace DiplomWebBack.Application.Usecases.Query.User
{
    public record GetAllUsersWithoutPaginationQuery() : IRequest<IEnumerable<UserProfileDto>>;
}
