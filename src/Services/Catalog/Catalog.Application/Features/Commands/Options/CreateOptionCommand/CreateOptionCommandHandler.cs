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

namespace Catalog.Application.Features.Commands.Options.CreateOptionCommand
{
    public class CreateOptionCommandHandler : IRequestHandler<CreateOptionCommand, IDataResult<Option>>
    {
        private readonly IMapper _mapper;
        private readonly IOptionRepository _optionRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CreateOptionCommandHandler(IMapper mapper, IOptionRepository optionRepository,
            IHttpContextAccessor httpContextAccessor)
        {
            _mapper = mapper;
            _optionRepository = optionRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        [LogAspect(typeof(FileLogger), "Catalog-Application")]
        [ExceptionLogAspect(typeof(FileLogger), "Catalog-Application")]
        [ValidationAspect(typeof(CreateOptionCommandValidator))]
        public async Task<IDataResult<Option>> Handle(CreateOptionCommand request, CancellationToken cancellationToken)
        {
            var currentUserId = _httpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)
                ?.Value;
            if (string.IsNullOrEmpty(currentUserId))
            {
                return new ErrorDataResult<Option>(Messages.SignInFirst);
            }

            var isAlreadyExists =
                await _optionRepository.AnyAsync(x => x.NormalizedName.Equals(request.Name.ToLower()));

            if (isAlreadyExists)
            {
                return new ErrorDataResult<Option>(Messages.DataAlreadyExist);
            }

            var option = _mapper.Map<Option>(request);
            option.CreatedBy = currentUserId;
            option.CreatedDate = DateTime.Now;
            option.NormalizedName = request.Name.ToLower();

            await _optionRepository.AddAsync(option);
            return new SuccessDataResult<Option>(option);
        }
    }
}