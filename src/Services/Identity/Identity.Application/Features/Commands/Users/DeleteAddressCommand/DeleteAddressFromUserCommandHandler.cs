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
using Olcsan.Boilerplate.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Olcsan.Boilerplate.Utilities.Results;

namespace Identity.Application.Features.Commands.Users.DeleteAddressCommand
{
    public class DeleteAddressFromUserCommandHandler : IRequestHandler<DeleteAddressFromUserCommand, IResult>
    {
        private readonly UserManager<User> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IAddressRepository _addressRepository;

        public DeleteAddressFromUserCommandHandler(UserManager<User> userManager,
            IHttpContextAccessor httpContextAccessor, IAddressRepository addressRepository)
        {
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _addressRepository = addressRepository;
        }

        [LogAspect(typeof(FileLogger), "Identity-Service")]
        [ExceptionLogAspect(typeof(FileLogger), "Identity-Service")]
        public async Task<IResult> Handle(DeleteAddressFromUserCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.AddressId.ToString()))
            {
                return new ErrorResult(Messages.DataNotFound);
            }

            if (string.IsNullOrEmpty(_httpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value))
            {
                return new ErrorResult(Messages.SignInFirst);
            }

            var currentUser =
                await _userManager.FindByEmailAsync(_httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Email)
                    ?.Value);
            if (currentUser is null) return new ErrorDataResult<UserViewModel>(Messages.SignInFirst);
            var deleteAddress = await _addressRepository.GetAsync(x =>
                x.Id.Equals(request.AddressId) && x.CreatedBy.Equals(currentUser.Id));
            if (deleteAddress is null)
            {
                return new ErrorResult(Messages.DataNotFound);
            }

            currentUser.Addresses.Remove(deleteAddress);
            await _userManager.UpdateAsync(currentUser);
            _addressRepository.Delete(deleteAddress);
            await _addressRepository.SaveChangesAsync();
            return new SuccessResult();
        }
    }
}