using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Catalog.Application.Constants;
using Catalog.Application.Features.Commands.CreateBrandCommand;
using Catalog.Application.Interfaces.Repositories;
using Catalog.Domain.Entities;
using Catalog.UnitTests.Helpers;
using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using Moq;
using Xunit;

namespace Catalog.UnitTests.Handlers
{
    public class CreateBrandTests
    {
        private readonly Mock<IBrandRepository> _brandRepository;

        public CreateBrandTests()
        {
            _brandRepository = new Mock<IBrandRepository>();
        }

        [Fact]
        private async Task CreateBrand_WithBrandToCreate_ReturnSuccessResult()
        {
            // Arrange
            var command = new CreateBrandCommand
            {
                Name = "Test",
                IsActive = true
            };
            _brandRepository.Setup(x => x.AddAsync(It.IsAny<Brand>()))
                .ReturnsAsync(new Brand())
                .Verifiable();
            var handler = new CreateBrandCommandHandler(MockHelper.CreateMapper(), _brandRepository.Object);

            // Act
            var result = await handler.Handle(command,
                new CancellationToken());

            // Assert
            result.Success.Should().BeTrue();
            result.Data.Should()
                .BeEquivalentTo(command,
                    cfg => cfg.ComparingByMembers<Brand>()
                        .ExcludingMissingMembers());
        }

        [Fact]
        private async Task CreateBrand_WithBrandToCreate_WhenBrandNameAlreadyExists_ReturnsErrorResult()
        {
            // Arrange
            var command = new CreateBrandCommand
            {
                Name = "Test",
                IsActive = true
            };
            _brandRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<Brand, bool>>>()))
                .ReturnsAsync(new Brand())
                .Verifiable();
            
            var handler = new CreateBrandCommandHandler(MockHelper.CreateMapper(), _brandRepository.Object);
            
            // Act
            var result = await handler.Handle(command,
                new CancellationToken());

            // Assert
            result.Message.Should().Be(Messages.DataAlreadyExist);
            result.Data.Should().BeNull();
            result.Success.Should().BeFalse();
        }
    }
}