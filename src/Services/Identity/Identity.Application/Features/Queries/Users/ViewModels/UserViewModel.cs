using System;
using System.Collections.Generic;

namespace Identity.Application.Features.Queries.ViewModels
{
    public class UserViewModel
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public DateTime BirthDate { get; set; }
        public List<AddressViewModel> Addresses { get; set; } = new List<AddressViewModel>();
    }
}