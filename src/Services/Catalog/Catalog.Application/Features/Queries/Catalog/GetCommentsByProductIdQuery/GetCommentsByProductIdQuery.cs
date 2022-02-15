using System.Collections.Generic;
using Catalog.Application.Dtos;
using MediatR;
using Olcsan.Boilerplate.Utilities.Results;

namespace Catalog.Application.Features.Queries.Catalog.GetCommentsByProductIdQuery
{
    public class GetCommentsByProductIdQuery : IRequest<IDataResult<List<CommentDto>>>
    {
        public string ProductId { get; }

        public GetCommentsByProductIdQuery(string productId)
        {
            ProductId = productId;
        }
    }
}