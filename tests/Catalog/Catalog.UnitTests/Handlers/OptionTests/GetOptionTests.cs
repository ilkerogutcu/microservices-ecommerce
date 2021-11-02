using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Catalog.Application.Features.Queries.Brands.GetAllBrandsQuery;
using Catalog.Application.Features.Queries.Options.GetAllOptionsQuery;
using Catalog.Application.Interfaces.Repositories;
using Catalog.Domain.Entities;
using Catalog.UnitTests.Helpers;
using FluentAssertions;
using MongoDB.Bson;
using Moq;
using Xunit;

namespace Catalog.UnitTests.Handlers.OptionTests
{
    public class GetOptionTests
    {
        private readonly Mock<IOptionRepository> _optionRepository;

        public GetOptionTests()
        {
            _optionRepository = new Mock<IOptionRepository>();
        }

        [Fact]
        private async Task GetAllOptions_WithExistingOptions_ReturnSuccessResult()
        {
            // Arrange
            var query = new GetAllOptionsQuery
            {
                PageIndex = 0,
                PageSize = 10,
                IsActive = true,
                IsRequired = true,
                Varianter = true,
            };

            var optionList = new List<Option>()
            {
                new()
                {
                    Id = ObjectId.GenerateNewId().ToString(),
                    Name = "Test",
                    Varianter = true,
                    NormalizedName = "test",
                    CreatedBy = "admin",
                    CreatedDate = DateTime.Now,
                    IsActive = true,
                    IsRequired = true,
                },
                new()
                {
                    Id = ObjectId.GenerateNewId().ToString(),
                    Name = "Test",
                    Varianter = true,
                    NormalizedName = "test",
                    CreatedBy = "admin",
                    CreatedDate = DateTime.Now,
                    IsActive = true,
                    IsRequired = true
                }
            }.AsQueryable();


            _optionRepository.Setup(x => x.GetListAsync(It.IsAny<Expression<Func<Option, bool>>>()))
                .ReturnsAsync(optionList)
                .Verifiable();
            var handler = new GetAllOptionsQueryHandler(MockHelper.CreateMapper(), _optionRepository.Object);

            // Act
            var result = await handler.Handle(query,
                new CancellationToken());

            // Assert
            result.Success.Should().BeTrue();
            result.Data.Should().NotBeEmpty();
            result.Data.Should().BeEquivalentTo(optionList, options =>
                options.ExcludingMissingMembers());
        }

        [Fact]
        private async Task GetAllOptions_WithNonexistentOptions_ReturnSuccessResult()
        {
            // Arrange
            var query = new GetAllOptionsQuery
            {
                PageIndex = 0,
                PageSize = 10,
                Varianter = true,
                IsActive = true,
                IsRequired = false
            };
            var optionList = new List<Option>().AsQueryable();

            _optionRepository.Setup(x => x.GetListAsync(It.IsAny<Expression<Func<Option, bool>>>()))
                .ReturnsAsync(optionList)
                .Verifiable();
            var handler = new GetAllOptionsQueryHandler(MockHelper.CreateMapper(), _optionRepository.Object);

            // Act
            var result = await handler.Handle(query,
                new CancellationToken());

            // Assert
            result.Success.Should().BeTrue();
            result.Data.Should().HaveCount(0);
        }
    }
}