using System;
using System.Text.Json.Serialization;
using MediatR;
using Olcsan.Boilerplate.Utilities.Results;

namespace Identity.Application.Features.Commands.Users.UpdateAddressFromUserCommand
{
    public class UpdateAddressFromUserCommand : IRequest<IResult>
    {
        [JsonIgnore]
        public Guid AddressId { get; set; }
        public Guid DistrictId { get; set; }
        public string Zip { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string AddressLine { get; set; }
        public string AddressTitle { get; set; }
    }
}