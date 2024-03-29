﻿using System;

namespace Identity.Application.Features.Queries.Users.ViewModels
{
    public class AddressViewModel
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string AddressLine { get; set; }
        public string AddressTitle { get; set; }
        public Guid CityId { get; set; }
        public string City { get; set; }
        public Guid DistrictId { get; set; }
        public string District { get; set; }
        public string Zip { get; set; }
    }
}