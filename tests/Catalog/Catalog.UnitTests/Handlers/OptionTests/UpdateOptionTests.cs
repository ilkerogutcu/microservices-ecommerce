using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Catalog.Application.Constants;
using Catalog.Application.Features.Commands.Options.UpdateOptionCommand;
using Catalog.Application.Interfaces.Repositories;
using Catalog.Domain.Entities;
using Catalog.UnitTests.Helpers;
using FluentAssertions;
using MongoDB.Bson;
using Moq;
using Xunit;

namespace Catalog.UnitTests.Handlers.OptionTests
{
    public class UpdateOptionTests
    {
        private readonly Mock<IOptionRepository> _optionRepository;

        public UpdateOptionTests()
        {
            _optionRepository = new Mock<IOptionRepository>();
        }

        [Fact]
        private async Task UpdateOption_WithOptionToUpdate_ReturnSuccessResult()
        {
            // Arrange
            var command = new UpdateOptionCommand
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Name = "Test",
                IsActive = true,
                Varianter = true,
                IsRequired = true
            };
            _optionRepository.Setup(x => x.GetByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(new Option())
                .Verifiable();
            _optionRepository.Setup(x => x.AnyAsync(It.IsAny<Expression<Func<Option, bool>>>()))
                .ReturnsAsync(() => false)
                .Verifiable();
            var handler = new UpdateOptionCommandHandler(MockHelper.CreateMapper(), _optionRepository.Object);

            // Act
            var result = await handler.Handle(command,
                new CancellationToken());

            // Assert
            result.Success.Should().BeTrue();
            result.Data.Should().BeEquivalentTo(command, options =>
                options.ExcludingMissingMembers());
        }

        [Fact]
        private async Task UpdateOption_WithOptionToUpdate_WhenOptionNameAlreadyExists_ReturnsErrorResult()
        {
            // Arrange
            var command = new UpdateOptionCommand
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Name = "Test",
                IsActive = true,
                Varianter = true,
                IsRequired = true
            };
            var option = new Option
            {
                Id = command.Id,
                Name = "Test",
                IsActive = true,
                CreatedBy = "test user",
                CreatedDate = DateTime.Now,
                NormalizedName = "test",
            };

            _optionRepository.Setup(x => x.GetByIdAsync(It.IsAny<string>())).ReturnsAsync(option)
                .Verifiable();
            _optionRepository.Setup(x => x.AnyAsync(It.IsAny<Expression<Func<Option, bool>>>()))
                .ReturnsAsync(() => true)
                .Verifiable();
            var handler = new UpdateOptionCommandHandler(MockHelper.CreateMapper(), _optionRepository.Object);

            // Act
            var result = await handler.Handle(command,
                new CancellationToken());

            // Assert
            result.Message.Should().Be(Messages.DataAlreadyExist);
            result.Data.Should().BeNull();
            result.Success.Should().BeFalse();
        }

        [Fact]
        private async Task UpdateOption_WithOptionToUpdate_WhenDataNotFound_ReturnsErrorResult()
        {
            // Arrange
            var command = new UpdateOptionCommand
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Name = "Test",
                IsActive = true,
                Varianter = true,
                IsRequired = true
            };

            _optionRepository.Setup(x => x.GetByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(() => null)
                .Verifiable();
            var handler = new UpdateOptionCommandHandler(MockHelper.CreateMapper(), _optionRepository.Object);

            // Act
            var result = await handler.Handle(command,
                new CancellationToken());

            // Assert
            result.Message.Should().Be(Messages.DataNotFound);
            result.Data.Should().BeNull();
            result.Success.Should().BeFalse();
        }
    }
}