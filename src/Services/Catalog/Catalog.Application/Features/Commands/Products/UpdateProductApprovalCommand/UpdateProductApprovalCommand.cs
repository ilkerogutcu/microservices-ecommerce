﻿using System.Text.Json.Serialization;
using MediatR;
using Olcsan.Boilerplate.Utilities.Results;

namespace Catalog.Application.Features.Commands.Products.UpdateProductApprovalCommand
{
    public class UpdateProductApprovalCommand : IRequest<IResult>
    {
        [JsonIgnore] public string Id { get; set; }
        public bool Approve { get; set; }
    }
}