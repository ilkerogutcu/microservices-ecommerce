using System.Threading;
using System.Threading.Tasks;
using Catalog.Application.Constants;
using Catalog.Application.Features.Commands.Brands.DeleteBrandCommand;
using Catalog.Application.Interfaces.Repositories;
using Catalog.Domain.Entities;
using FluentAssertions;
using Moq;
using Xunit;

namespace Catalog.UnitTests.Handlers.BrandTests
{
    public class DeleteBrandTests
    {
        private readonly Mock<IBrandRepository> _brandRepository;

        public DeleteBrandTests()
        {
            _brandRepository = new Mock<IBrandRepository>();
        }

        [Fact]
        private async Task DeleteBrand_WithExistingBrand_ReturnSuccessResult()
        {
            // Arrange
            var command = new DeleteBrandCommand();

            _brandRepository.Setup(x => x.GetByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(new Brand())
                .Verifiable();

            var handler = new DeleteBrandCommandHandler(_brandRepository.Object);

            // Act
            var result = await handler.Handle(command,
                new CancellationToken());

            // Assert
            result.Success.Should().BeTrue();
        }

        [Fact]
        private async Task DeleteBrand_WithNonexistentBrand_ReturnErrorResult()
        {
            // Arrange
            var command = new DeleteBrandCommand();

            _brandRepository.Setup(x => x.GetByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(() => null)
                .Verifiable();

            var handler = new DeleteBrandCommandHandler(_brandRepository.Object);

            // Act
            var result = await handler.Handle(command,
                new CancellationToken());

            // Assert
            result.Success.Should().BeFalse();
            result.Message.Should().Be(Messages.DataNotFound);
        }
    }
}