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
using Olcsan.Boilerplate.Aspects.Autofac.Exception;
using Olcsan.Boilerplate.Aspects.Autofac.Logger;
using Olcsan.Boilerplate.Aspects.Autofac.Validation;
using Olcsan.Boilerplate.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Olcsan.Boilerplate.Utilities.Results;

namespace Catalog.Application.Features.Commands.Options.UpdateOptionCommand
{
    public class UpdateOptionCommandHandler : IRequestHandler<UpdateOptionCommand, IDataResult<Option>>
    {
        private readonly IMapper _mapper;
        private readonly IOptionRepository _optionRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UpdateOptionCommandHandler(IMapper mapper, IOptionRepository optionRepository,
            IHttpContextAccessor httpContextAccessor)
        {
            _mapper = mapper;
            _optionRepository = optionRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        [LogAspect(typeof(FileLogger), "Catalog-Application")]
        [ExceptionLogAspect(typeof(FileLogger), "Catalog-Application")]
        [ValidationAspect(typeof(UpdateOptionCommandValidator))]
        public async Task<IDataResult<Option>> Handle(UpdateOptionCommand request, CancellationToken cancellationToken)
        {
            var currentUserId = _httpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)
                ?.Value;
            if (string.IsNullOrEmpty(currentUserId))
            {
                return new ErrorDataResult<Option>(Messages.SignInFirst);
            }

            var option = await _optionRepository.GetByIdAsync(request.Id);
            if (option is null)
            {
                return new ErrorDataResult<Option>(Messages.DataNotFound);
            }

            var isAlreadyExist = await _optionRepository.AnyAsync(x =>
                x.NormalizedName.Equals(request.Name.ToLower()) && !x.NormalizedName.Equals(option.NormalizedName));
            if (isAlreadyExist)
            {
                return new ErrorDataResult<Option>(Messages.DataAlreadyExist);
            }

            option = _mapper.Map(request, option);
            option.LastUpdatedBy = currentUserId;
            option.LastUpdatedDate = DateTime.Now;
            option.NormalizedName = request.Name.ToLower();
            await _optionRepository.UpdateAsync(option.Id, option);
            return new SuccessDataResult<Option>(option);
        }
    }
}