using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Catalog.Application.Constants;
using Catalog.Application.Interfaces.Repositories;
using Catalog.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Olcsan.Boilerplate.Utilities.Results;

namespace Catalog.Application.Features.Commands.OptionValues.UpdateOptionValueCommand
{
    public class UpdateOptionValueCommandHandler : IRequestHandler<UpdateOptionValueCommand, IDataResult<OptionValue>>
    {
        private readonly IMapper _mapper;
        private readonly IOptionValueRepository _optionValueRepository;
        private readonly IOptionRepository _optionRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UpdateOptionValueCommandHandler(IMapper mapper, IOptionValueRepository optionValueRepository,
            IOptionRepository optionRepository, IHttpContextAccessor httpContextAccessor)
        {
            _mapper = mapper;
            _optionValueRepository = optionValueRepository;
            _optionRepository = optionRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IDataResult<OptionValue>> Handle(UpdateOptionValueCommand request,
            CancellationToken cancellationToken)
        {
            var currentUserId = _httpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)
                ?.Value;
            if (string.IsNullOrEmpty(currentUserId))
            {
                return new ErrorDataResult<OptionValue>(Messages.SignInFirst);
            }

            var optionValue = await _optionValueRepository.GetByIdAsync(request.Id);
            var option = await _optionRepository.GetByIdAsync(request.OptionId);

            if (optionValue is null || option is null)
            {
                return new ErrorDataResult<OptionValue>(Messages.DataNotFound);
            }

            var isAlreadyExist = await _optionValueRepository.AnyAsync(x =>
                x.OptionId.Equals(request.OptionId) && x.NormalizedName.Equals(request.Name.ToLower()) &&
                !x.NormalizedName.Equals(option.NormalizedName));
            if (isAlreadyExist)
            {
                return new ErrorDataResult<OptionValue>(Messages.DataAlreadyExist);
            }

            optionValue = _mapper.Map(request, optionValue);
            optionValue.LastUpdatedBy = currentUserId;
            optionValue.LastUpdatedDate = DateTime.Now;
            optionValue.NormalizedName = request.Name.ToLower();

            await _optionValueRepository.UpdateAsync(request.Id, optionValue);
            return new SuccessDataResult<OptionValue>(optionValue);
        }
    }
}