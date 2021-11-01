using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Catalog.Application.Constants;
using Catalog.Application.Features.Commands.CreateOptionCommand;
using Catalog.Application.Interfaces.Repositories;
using Catalog.Domain.Entities;
using Catalog.UnitTests.Helpers;
using FluentAssertions;
using Moq;
using Xunit;

namespace Catalog.UnitTests.Handlers.OptionTests
{
    public class CreateOptionTests
    {
        private readonly Mock<IOptionRepository> _optionRepository;

        public CreateOptionTests()
        {
            _optionRepository = new Mock<IOptionRepository>();
        }

        [Fact]
        private async Task CreateOption_WithOptionToCreate_ReturnSuccessResult()
        {
            // Arrange
            var command = new CreateOptionCommand
            {
                Name = "Test",
                Varianter = false,
                IsActive = true,
                IsRequired = true
            };
            _optionRepository.Setup(x => x.AddAsync(It.IsAny<Option>()))
                .ReturnsAsync(new Option())
                .Verifiable();
            var handler = new CreateOptionCommandHandler(MockHelper.CreateMapper(), _optionRepository.Object);

            // Act
            var result = await handler.Handle(command,
                new CancellationToken());

            // Assert
            result.Success.Should().BeTrue();
            result.Data.Should().BeEquivalentTo(command, options =>
                options.ExcludingMissingMembers());
        }

        [Fact]
        private async Task CreateOption_WithOptionToCreate_WhenOptionNameAlreadyExists_ReturnErrorResult()
        {
            // Arrange
            var command = new CreateOptionCommand
            {
                Name = "Test",
                Varianter = false,
                IsActive = true,
                IsRequired = true
            };
            _optionRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<Option, bool>>>()))
                .ReturnsAsync(new Option())
                .Verifiable();
            var handler = new CreateOptionCommandHandler(MockHelper.CreateMapper(), _optionRepository.Object);

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