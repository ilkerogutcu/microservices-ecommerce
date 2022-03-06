using System;
using MediatR;

namespace Identity.Application.Features.Queries.Users.GetCurrentUserIdQuery
{
    public class GetCurrentUserIdQuery:IRequest<Guid>
    {
        
    }
}