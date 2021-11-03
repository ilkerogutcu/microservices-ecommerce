using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Catalog.Application.Constants;
using Catalog.Application.Features.Commands.OptionValues.UpdateOptionValueCommand;
using Catalog.Application.Interfaces.Repositories;
using Catalog.Domain.Entities;
using Catalog.UnitTests.Helpers;
using FluentAssertions;
using MongoDB.Bson;
using Moq;
using Xunit;

namespace Catalog.UnitTests.Handlers.OptionValueTests
{
    public class UpdateOptionValueTests
    {
        private readonly Mock<IOptionValueRepository> _optionValueRepository;
        private readonly Mock<IOptionRepository> _optionRepository;

        public UpdateOptionValueTests()
        {
            _optionValueRepository = new Mock<IOptionValueRepository>();
            _optionRepository = new Mock<IOptionRepository>();
        }

        [Fact]
        private async Task UpdateOptionValue_WithOptionValueToUpdate_ReturnSuccessResult()
        {
            // Arrange
            var command = new UpdateOptionValueCommand()
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Name = "Test",
                OptionId = ObjectId.GenerateNewId().ToString(),
            };

            _optionRepository.Setup(x => x.GetByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(new Option())
                .Verifiable();
            _optionValueRepository.Setup(x => x.GetByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(new OptionValue())
                .Verifiable();
            _optionValueRepository.Setup(x => x.AnyAsync(It.IsAny<Expression<Func<OptionValue, bool>>>()))
                .ReturnsAsync(() => false)
                .Verifiable();

            var handler = new UpdateOptionValueCommandHandler(MockHelper.CreateMapper(), _optionValueRepository.Object,
                _optionRepository.Object);

            // Act
            var result = await handler.Handle(command,
                new CancellationToken());

            // Assert
            result.Success.Should().BeTrue();
            result.Data.Should().BeEquivalentTo(command, options =>
                options.ExcludingMissingMembers());
        }

        [Fact]
        private async Task UpdateOptionValue_WithOptionValueToUpdate_WhenOptionValueAlreadyExists_ReturnErrorResult()
        {
            // Arrange
            var command = new UpdateOptionValueCommand()
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Name = "Test",
                OptionId = ObjectId.GenerateNewId().ToString(),
            };

            _optionRepository.Setup(x => x.GetByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(new Option())
                .Verifiable();
            _optionValueRepository.Setup(x => x.GetByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(new OptionValue())
                .Verifiable();
            _optionValueRepository.Setup(x => x.AnyAsync(It.IsAny<Expression<Func<OptionValue, bool>>>()))
                .ReturnsAsync(() => true)
                .Verifiable();

            var handler = new UpdateOptionValueCommandHandler(MockHelper.CreateMapper(), _optionValueRepository.Object,
                _optionRepository.Object);

            // Act
            var result = await handler.Handle(command,
                new CancellationToken());

            // Assert
            result.Success.Should().BeFalse();
            result.Message.Should().Be(Messages.DataAlreadyExist);
            result.Data.Should().BeNull();
        }

        [Fact]
        private async Task UpdateOptionValue_WithOptionValueToUpdate_WhenOptionValueIsNonexistent_ReturnErrorResult()
        {
            // Arrange
            var command = new UpdateOptionValueCommand()
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Name = "Test",
                OptionId = ObjectId.GenerateNewId().ToString(),
            };

            _optionValueRepository.Setup(x => x.GetByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(() => null)
                .Verifiable();

            var handler = new UpdateOptionValueCommandHandler(MockHelper.CreateMapper(), _optionValueRepository.Object,
                _optionRepository.Object);

            // Act
            var result = await handler.Handle(command,
                new CancellationToken());

            // Assert
            result.Success.Should().BeFalse();
            result.Message.Should().Be(Messages.DataNotFound);
            result.Data.Should().BeNull();
        }

        [Fact]
        private async Task UpdateOptionValue_WithOptionValueToUpdate_WhenOptionIsNonexistent_ReturnErrorResult()
        {
            // Arrange
            var command = new UpdateOptionValueCommand()
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Name = "Test",
                OptionId = ObjectId.GenerateNewId().ToString(),
            };

            _optionValueRepository.Setup(x => x.GetByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(new OptionValue())
                .Verifiable();

            _optionRepository.Setup(x => x.GetByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(() => null)
                .Verifiable();

            var handler = new UpdateOptionValueCommandHandler(MockHelper.CreateMapper(), _optionValueRepository.Object,
                _optionRepository.Object);

            // Act
            var result = await handler.Handle(command,
                new CancellationToken());

            // Assert
            result.Success.Should().BeFalse();
            result.Message.Should().Be(Messages.DataNotFound);
            result.Data.Should().BeNull();
        }

        [Fact]
        private async Task
            UpdateOptionValue_WithOptionValueToUpdate_WhenOptionIsNonexistentAndOptionValueIsNotExistent_ReturnErrorResult()
        {
            // Arrange
            var command = new UpdateOptionValueCommand()
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Name = "Test",
                OptionId = ObjectId.GenerateNewId().ToString(),
            };

            _optionValueRepository.Setup(x => x.GetByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(() => null)
                .Verifiable();

            _optionRepository.Setup(x => x.GetByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(() => null)
                .Verifiable();

            var handler = new UpdateOptionValueCommandHandler(MockHelper.CreateMapper(), _optionValueRepository.Object,
                _optionRepository.Object);

            // Act
            var result = await handler.Handle(command,
                new CancellationToken());

            // Assert
            result.Success.Should().BeFalse();
            result.Message.Should().Be(Messages.DataNotFound);
            result.Data.Should().BeNull();
        }
    }
}