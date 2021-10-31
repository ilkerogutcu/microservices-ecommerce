using AutoMapper;
using Catalog.Application.Mappings;

namespace Catalog.UnitTests.Helpers
{
    public static class MockHelper
    {
        public static IMapper CreateMapper()
        {
            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfile()); //your AutoMapper helper
            });
            return mockMapper.CreateMapper();
        }
    }
}