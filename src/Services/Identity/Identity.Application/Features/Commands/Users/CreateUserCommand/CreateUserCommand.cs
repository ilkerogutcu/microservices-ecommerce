using System;
using System.Collections.Generic;
using MediatR;
using Olcsan.Boilerplate.Utilities.Results;

namespace Identity.Application.Features.Commands.Users.CreateUserCommand
{
    public class CreateUserCommand : IRequest<IResult>
    {
        /// <summary>
        ///     First name
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        ///     Last name
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        ///     Email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        ///     Birth Date
        /// </summary>
        public DateTime BirthDate { get; set; }

        /// <summary>
        /// Roles
        /// </summary>
        public List<string> Roles { get; set; }

        /// <summary>
        ///     Password
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        ///     This variable validates for first password entered
        /// </summary>
        public string ConfirmPassword { get; set; }
    }
}