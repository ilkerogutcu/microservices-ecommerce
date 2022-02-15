using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Catalog.Application.Constants;
using Catalog.Application.Dtos;
using Catalog.Application.Interfaces;
using Catalog.Application.Interfaces.Repositories;
using MediatR;
using Olcsan.Boilerplate.Utilities.Results;

namespace Catalog.Application.Features.Queries.Catalog.GetCommentsByProductIdQuery
{
    public class GetCommentsByProductIdQueryHandler : IRequestHandler<GetCommentsByProductIdQuery, IDataResult<List<CommentDto>>>
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        private readonly IUserGrpcService _userGrpcService;

        public GetCommentsByProductIdQueryHandler(IProductRepository productRepository, IMapper mapper, IUserGrpcService userGrpcService)
        {
            _productRepository = productRepository;
            _mapper = mapper;
            _userGrpcService = userGrpcService;
        }

        public async Task<IDataResult<List<CommentDto>>> Handle(GetCommentsByProductIdQuery request, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetByIdAsync(request.ProductId);
            if (product == null)
            {
                return new ErrorDataResult<List<CommentDto>>(Messages.DataNotFound);
            }

            var commentsOfProduct = _mapper.Map<List<CommentDto>>(product.Comments);
            foreach (var comment in commentsOfProduct)
            {
                var userId = product.Comments.Where(x => x.Id.Equals(comment.Id)).Select(x => x.CreatedBy).FirstOrDefault();
                if (string.IsNullOrEmpty(userId))
                {
                    continue;
                }

                var user = await _userGrpcService.GetUserByIdAsync(userId);
                if (user is null)
                {
                    comment.FullName = "******* *****";
                }
                else
                {
                    comment.FullName = user.FirstName[..1] + "******" + " " + user.Lastname[..1] + "******";
                }
            }

            return new SuccessDataResult<List<CommentDto>>(commentsOfProduct);
        }
    }
}