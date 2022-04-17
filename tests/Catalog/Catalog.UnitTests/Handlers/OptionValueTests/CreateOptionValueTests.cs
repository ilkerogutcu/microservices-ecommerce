using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Catalog.Application.Constants;
using Catalog.Application.Features.Commands.OptionValues.CreateOptionValueCommand;
using Catalog.Application.Interfaces.Repositories;
using Catalog.Domain.Entities;
using Catalog.UnitTests.Helpers;
using FluentAssertions;
using MongoDB.Bson;
using Moq;
using Xunit;

namespace Catalog.UnitTests.Handlers.OptionValueTests
{
    public class CreateOptionValueTests
    {
        private readonly Mock<IOptionValueRepository> _optionValueRepository;
        private readonly Mock<IOptionRepository> _optionRepository;

        public CreateOptionValueTests()
        {
            _optionValueRepository = new Mock<IOptionValueRepository>();
            _optionRepository = new Mock<IOptionRepository>();
        }

        [Fact]
        private async Task CreateOptionValue_WithOptionValueToCreate_ReturnSuccessResult()
        {
            // Arrange
            var command = new CreateOptionValueCommand
            {
                Name = "Test",
                OptionId = ObjectId.GenerateNewId().ToString()
            };
            _optionRepository.Setup(x => x.GetByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(new Option())
                .Verifiable();
            _optionValueRepository.Setup(x => x.AnyAsync(It.IsAny<Expression<Func<OptionValue, bool>>>()))
                .ReturnsAsync(() => false)
                .Verifiable();
            _optionValueRepository.Setup(x => x.AddAsync(It.IsAny<OptionValue>()))
                .ReturnsAsync(new OptionValue())
                .Verifiable();
            var handler = new CreateOptionValueCommandHandler(MockHelper.CreateMapper(), _optionValueRepository.Object,
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
        private async Task CreateOptionValue_WithOptionValueToCreate_WhenOptionIsNonexistent_ReturnErrorResult()
        {
            // Arrange
            var command = new CreateOptionValueCommand
            {
                Name = "Test",
                OptionId = ObjectId.GenerateNewId().ToString()
            };

            _optionRepository.Setup(x => x.GetByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(() => null)
                .Verifiable();

            var handler = new CreateOptionValueCommandHandler(MockHelper.CreateMapper(), _optionValueRepository.Object,
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
        private async Task CreateOptionValue_WithOptionValueToCreate_WhenOptionValueAlreadyExists_ReturnErrorResult()
        {
            // Arrange
            var command = new CreateOptionValueCommand
            {
                Name = "Test",
                OptionId = ObjectId.GenerateNewId().ToString()
            };
            _optionRepository.Setup(x => x.GetByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(new Option())
                .Verifiable();
            _optionValueRepository.Setup(x => x.AnyAsync(It.IsAny<Expression<Func<OptionValue, bool>>>()))
                .ReturnsAsync(() => true)
                .Verifiable();

            var handler = new CreateOptionValueCommandHandler(MockHelper.CreateMapper(), _optionValueRepository.Object,
                _optionRepository.Object);

            // Act
            var result = await handler.Handle(command,
                new CancellationToken());
            // Assert
            result.Success.Should().BeFalse();
            result.Message.Should().Be(Messages.DataAlreadyExist);
            result.Data.Should().BeNull();
        }
    }
}