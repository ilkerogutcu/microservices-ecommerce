using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Identity.Application.Constants;
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

namespace Identity.Application.Features.Commands.Users.UpdateAddressCommand
{
    public class UpdateAddressFromUserCommandHandler : IRequestHandler<UpdateAddressFromUserCommand, IResult>
    {
        private readonly IDistrictRepository _districtRepository;
        private readonly UserManager<User> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IAddressRepository _addressRepository;
        private readonly IMapper _mapper;

        public UpdateAddressFromUserCommandHandler(IDistrictRepository districtRepository,
            UserManager<User> userManager,
            IHttpContextAccessor httpContextAccessor, IAddressRepository addressRepository, IMapper mapper)
        {
            _districtRepository = districtRepository;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _addressRepository = addressRepository;
            _mapper = mapper;
        }

        [LogAspect(typeof(FileLogger), "Identity-Service")]
        [ExceptionLogAspect(typeof(FileLogger), "Identity-Service")]
        [ValidationAspect(typeof(UpdateAddressFromUserCommandValidator))]
        public async Task<IResult> Handle(UpdateAddressFromUserCommand request, CancellationToken cancellationToken)
        {
            var currentUser = await _userManager.FindByEmailAsync(_httpContextAccessor.HttpContext.User
                .FindFirst(ClaimTypes.Email)
                ?.Value);
            if (currentUser is null) return new ErrorResult(Messages.SignInFirst);

            var address = await _addressRepository.GetAsync(x => x.Id.Equals(request.AddressId));
            if (address is null)
            {
                return new ErrorResult(Messages.DataNotFound);
            }

            var district = await _districtRepository.GetAsync(x => x.Id.Equals(request.DistrictId));
            if (district is null)
            {
                return new ErrorResult(Messages.DataNotFound);
            }

            address = _mapper.Map(request, address);
            address.District = district;
            address.LastUpdatedBy = currentUser.Id;
            address.LastUpdatedDate = DateTime.Now;
            return new SuccessResult();
        }
    }
}