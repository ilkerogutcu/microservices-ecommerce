using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Catalog.Application.Dtos;
using Catalog.Application.Features.Queries.OptionValues.GetAllOptionValuesQuery;
using Catalog.Application.Interfaces.Repositories;
using FluentAssertions;
using MongoDB.Bson;
using Moq;
using Xunit;

namespace Catalog.UnitTests.Handlers.OptionValueTests
{
    public class GetOptionValueTests
    {
        private readonly Mock<IOptionValueRepository> _optionValueRepository;

        public GetOptionValueTests()
        {
            _optionValueRepository = new Mock<IOptionValueRepository>();
        }

        [Fact]
        private async Task GetAllOptionValues_WhenExistingOptionValues_ReturnSuccessResult()
        {
            // Arrange
            var query = new GetAllOptionValuesQuery();

            var optionValueDetails = new List<OptionValueDetailsDto>()
            {
                new()
                {
                    OptionId = ObjectId.GenerateNewId().ToString(),
                    OptionName = "Test",
                    OptionValues = new List<OptionValueDto>
                    {
                        new()
                        {
                            Id = ObjectId.GenerateNewId().ToString(),
                            Name = "test value"
                        }
                    }
                }
            };

            _optionValueRepository.Setup(x => x.GetAllDetailsAsync())
                .ReturnsAsync(optionValueDetails)
                .Verifiable();
            var handler = new GetAllOptionValuesQueryHandler(_optionValueRepository.Object);

            // Act
            var result = await handler.Handle(query,
                new CancellationToken());

            // Assert
            result.Success.Should().BeTrue();
            result.Data.Should().NotBeNull();
            result.Data.Should().BeEquivalentTo(optionValueDetails, options =>
                options.ExcludingMissingMembers());
        }

        [Fact]
        private async Task GetAllOptionValues_WhenNonexistentOptionValues_ReturnSuccessResult()
        {
            // Arrange
            var query = new GetAllOptionValuesQuery();
            var optionValueDetails = new List<OptionValueDetailsDto>();

            _optionValueRepository.Setup(x => x.GetAllDetailsAsync())
                .ReturnsAsync(optionValueDetails)
                .Verifiable();
            var handler = new GetAllOptionValuesQueryHandler(_optionValueRepository.Object);

            // Act
            var result = await handler.Handle(query,
                new CancellationToken());

            // Assert
            result.Success.Should().BeTrue();
            result.Data.Count.Should().Be(0);
        }
    }
}