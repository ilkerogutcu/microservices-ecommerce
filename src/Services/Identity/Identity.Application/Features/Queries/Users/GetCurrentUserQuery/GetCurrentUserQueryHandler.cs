using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Identity.Application.Constants;
using Identity.Application.Features.Queries.ViewModels;
using Identity.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Olcsan.Boilerplate.Aspects.Autofac.Exception;
using Olcsan.Boilerplate.Aspects.Autofac.Logger;
using Olcsan.Boilerplate.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Olcsan.Boilerplate.Utilities.Results;

namespace Identity.Application.Features.Queries.Users.GetCurrentUserQuery
{
    public class GetCurrentUserQueryHandler : IRequestHandler<GetCurrentUserQuery, IDataResult<UserViewModel>>
    {
        private readonly UserManager<User> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

        public GetCurrentUserQueryHandler(UserManager<User> userManager, IHttpContextAccessor httpContextAccessor,
            IMapper mapper)
        {
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }

        [ExceptionLogAspect(typeof(FileLogger), "Identity-Service")]
        [LogAspect(typeof(FileLogger), "Identity-Service")]
        public async Task<IDataResult<UserViewModel>> Handle(GetCurrentUserQuery request,
            CancellationToken cancellationToken)
        {
            var currentUserEmail = _httpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value;
            if (string.IsNullOrEmpty(currentUserEmail))
            {
                return new ErrorDataResult<UserViewModel>(Messages.SignInFirst);
            }

            var currentUser = await _userManager.Users.Include(x => x.Addresses).ThenInclude(x => x.District)
                .ThenInclude(x => x.City)
                .SingleOrDefaultAsync(x => x.NormalizedEmail.Equals(currentUserEmail), cancellationToken);
            if (currentUser is null) return new ErrorDataResult<UserViewModel>(Messages.SignInFirst);

            var currentUserViewModel = _mapper.Map<UserViewModel>(currentUser);
            return new SuccessDataResult<UserViewModel>(currentUserViewModel);
        }
    }
}