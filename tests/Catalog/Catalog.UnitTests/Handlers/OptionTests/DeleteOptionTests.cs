using System.Threading;
using System.Threading.Tasks;
using Catalog.Application.Constants;
using Catalog.Application.Features.Commands.Options.DeleteOptionCommand;
using Catalog.Application.Interfaces.Repositories;
using Catalog.Domain.Entities;
using FluentAssertions;
using Moq;
using Xunit;

namespace Catalog.UnitTests.Handlers.OptionTests
{
    public class DeleteOptionTests
    {
        private readonly Mock<IOptionRepository> _optionRepository;
        private readonly Mock<IOptionValueRepository> _optionValueRepository;

        public DeleteOptionTests()
        {
            _optionRepository = new Mock<IOptionRepository>();
            _optionValueRepository = new Mock<IOptionValueRepository>();
        }

        [Fact]
        private async Task DeleteOption_WithExistingOption_ReturnSuccessResult()
        {
            // Arrange
            var command = new DeleteOptionCommand();

            _optionRepository.Setup(x => x.GetByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(new Option())
                .Verifiable();

            var handler = new DeleteOptionCommandHandler(_optionRepository.Object, _optionValueRepository.Object);

            // Act
            var result = await handler.Handle(command,
                new CancellationToken());

            // Assert
            result.Success.Should().BeTrue();
        }

        [Fact]
        private async Task DeleteOption_WithNonexistentOption_ReturnErrorResult()
        {
            // Arrange
            var command = new DeleteOptionCommand();

            _optionRepository.Setup(x => x.GetByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(() => null)
                .Verifiable();

            var handler = new DeleteOptionCommandHandler(_optionRepository.Object, _optionValueRepository.Object);

            // Act
            var result = await handler.Handle(command,
                new CancellationToken());

            // Assert
            result.Success.Should().BeFalse();
            result.Message.Should().Be(Messages.DataNotFound);
        }
    }
}