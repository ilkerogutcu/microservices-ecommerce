using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Identity.Application.Features.Queries.Users.GetCurrentUserIdQuery
{
    public class GetCurrentUserIdQueryHandler : IRequestHandler<GetCurrentUserIdQuery, Guid>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GetCurrentUserIdQueryHandler(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Guid> Handle(GetCurrentUserIdQuery request, CancellationToken cancellationToken)
        {
            var userId = _httpContextAccessor?.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return await Task.FromResult(string.IsNullOrEmpty(userId) ? default : Guid.Parse(userId));
        }
    }
}