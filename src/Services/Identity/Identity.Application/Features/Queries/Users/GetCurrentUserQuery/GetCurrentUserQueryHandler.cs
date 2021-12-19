using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Identity.Application.Constants;
using Identity.Application.Features.Queries.ViewModels;
using Identity.Application.Interfaces.Repositories;
using Identity.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
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

        public async Task<IDataResult<UserViewModel>> Handle(GetCurrentUserQuery request,
            CancellationToken cancellationToken)
        {
            var currentUser = await _userManager.FindByEmailAsync(_httpContextAccessor?.HttpContext.User
                .FindFirst(ClaimTypes.Email)?.Value);
            if (currentUser is null)
            {
                return new ErrorDataResult<UserViewModel>(Messages.SignInFirst);
            }

            var currentUserViewModel = _mapper.Map<UserViewModel>(currentUser);
            return new SuccessDataResult<UserViewModel>(currentUserViewModel);
        }
    }
}