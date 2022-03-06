using Identity.Application.Features.Queries.Users.ViewModels;
using Identity.Application.Features.Queries.ViewModels;
using MediatR;
using Olcsan.Boilerplate.Utilities.Results;

namespace Identity.Application.Features.Queries.Users.GetCurrentUserQuery
{
    public class GetCurrentUserQuery: IRequest<IDataResult<UserViewModel>>
    {
    }
}