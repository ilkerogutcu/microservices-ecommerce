using System.Threading;
using System.Threading.Tasks;
using Catalog.Application.Constants;
using Catalog.Application.Features.Commands.OptionValues.DeleteOptionValueCommand;
using Catalog.Application.Interfaces.Repositories;
using Catalog.Domain.Entities;
using FluentAssertions;
using MongoDB.Bson;
using Moq;
using Xunit;

namespace Catalog.UnitTests.Handlers.OptionValueTests
{
    public class DeleteOptionValueTests
    {
        private readonly Mock<IOptionValueRepository> _optionValueRepository;

        public DeleteOptionValueTests()
        {
            _optionValueRepository = new Mock<IOptionValueRepository>();
        }

        [Fact]
        private async Task DeleteOptionValue_WithExistingOption_ReturnSuccessResult()
        {
            // Arrange
            var command = new DeleteOptionValueCommand
            {
                Id = ObjectId.GenerateNewId().ToString()
            };
            _optionValueRepository.Setup(x => x.GetByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(new OptionValue())
                .Verifiable();
            var handler = new DeleteOptionValueCommandHandler(_optionValueRepository.Object);
            // Act
            var result = await handler.Handle(command,
                new CancellationToken());

            // Assert
            result.Success.Should().BeTrue();
        }

        [Fact]
        private async Task DeleteOptionValue_WithOptionValueIsNonexistent_ReturnErrorResult()
        {
            // Arrange
            var command = new DeleteOptionValueCommand
            {
                Id = ObjectId.GenerateNewId().ToString()
            };
            _optionValueRepository.Setup(x => x.GetByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(() => null)
                .Verifiable();
            var handler = new DeleteOptionValueCommandHandler(_optionValueRepository.Object);
            // Act
            var result = await handler.Handle(command,
                new CancellationToken());

            // Assert
            result.Success.Should().BeFalse();
            result.Message.Should().Be(Messages.DataNotFound);
        }
    }
}