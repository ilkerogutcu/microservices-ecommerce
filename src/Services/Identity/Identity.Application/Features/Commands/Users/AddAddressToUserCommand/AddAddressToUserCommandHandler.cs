using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Identity.Application.Constants;
using Identity.Application.Features.Queries.ViewModels;
using Identity.Application.Interfaces.Repositories;
using Identity.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Olcsan.Boilerplate.Aspects.Autofac.Exception;
using Olcsan.Boilerplate.Aspects.Autofac.Logger;
using Olcsan.Boilerplate.Aspects.Autofac.Validation;
using Olcsan.Boilerplate.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Olcsan.Boilerplate.Utilities.Results;

namespace Identity.Application.Features.Commands.Users.AddAddressToUserCommand
{
    public class AddAddressToUserCommandHandler : IRequestHandler<AddAddressToUserCommand, IResult>
    {
        private readonly IDistrictRepository _districtRepository;
        private readonly UserManager<User> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AddAddressToUserCommandHandler(IDistrictRepository districtRepository, UserManager<User> userManager,
            IHttpContextAccessor httpContextAccessor)
        {
            _districtRepository = districtRepository;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        [LogAspect(typeof(FileLogger), "Identity-Service")]
        [ExceptionLogAspect(typeof(FileLogger), "Identity-Service")]
        [ValidationAspect(typeof(AddAddressToUserCommandValidator))]
        public async Task<IResult> Handle(AddAddressToUserCommand request, CancellationToken cancellationToken)
        {
            var district = await _districtRepository.GetAsync(x => x.Id.Equals(request.DistrictId));
            if (district is null)
            {
                return new ErrorResult(Messages.DataNotFound);
            }

            if (string.IsNullOrEmpty(_httpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value))
            {
                return new ErrorDataResult<UserViewModel>(Messages.SignInFirst);
            }

            var currentUser =
                await _userManager.FindByEmailAsync(_httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Email)
                    ?.Value);
            if (currentUser is null) return new ErrorDataResult<UserViewModel>(Messages.SignInFirst);
            var address = new Address
            {
                DistrictId = district.Id,
                Zip = request.Zip,
                FirstName = request.FirstName,
                LastName = request.LastName,
                PhoneNumber = request.PhoneNumber,
                AddressLine = request.AddressLine,
                AddressTitle = request.AddressTitle,
                CreatedBy = currentUser.Id,
                CreatedDate = DateTime.Now
            };
            currentUser.Addresses.Add(address);
            await _userManager.UpdateAsync(currentUser);
            return new SuccessResult();
        }
    }
}