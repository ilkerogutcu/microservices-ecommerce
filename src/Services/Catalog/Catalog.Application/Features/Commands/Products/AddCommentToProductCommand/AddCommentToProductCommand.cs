using System.Text.Json.Serialization;
using MediatR;
using Olcsan.Boilerplate.Utilities.Results;

namespace Catalog.Application.Features.Commands.Products.AddCommentToProductCommand
{
    public class AddCommentToProductCommand : IRequest<IResult>
    {
        [JsonIgnore]
        public string ProductId { get; set; }
        public string CommentContent { get; set; }
        public int ProductRating { get; set; }
    }
}