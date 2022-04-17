using System;

namespace Identity.Application.Features.Queries.Cities.ViewModels
{
    public class DistrictViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid CityId { get; set; }
    }
}