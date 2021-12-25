using System;
using MediatR;
using Olcsan.Boilerplate.Utilities.Results;

namespace Identity.Application.Features.Commands.Users.DeleteAddressCommand
{
    public class DeleteAddressFromUserCommand : IRequest<IResult>
    {
        public DeleteAddressFromUserCommand(Guid addressId)
        {
            AddressId = addressId;
        }

        public Guid AddressId { get; set; }
    }
}