﻿using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Catalog.Application.Constants;
using Catalog.Application.Features.Commands.Brands.CreateBrandCommand;
using Catalog.Application.Interfaces.Repositories;
using Catalog.Domain.Entities;
using Catalog.UnitTests.Helpers;
using FluentAssertions;
using Moq;
using Xunit;

namespace Catalog.UnitTests.Handlers.BrandTests
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
            _brandRepository.Setup(x => x.AnyAsync(It.IsAny<Expression<Func<Brand, bool>>>()))
                .ReturnsAsync(() => false)
                .Verifiable();
            _brandRepository.Setup(x => x.AddAsync(It.IsAny<Brand>()))
                .ReturnsAsync(new Brand())
                .Verifiable();
            var handler = new CreateBrandCommandHandler(MockHelper.CreateMapper(), _brandRepository.Object);

            // Act
            var result = await handler.Handle(command,
                new CancellationToken());

            // Assert
            result.Success.Should().BeTrue();
            result.Data.Should().BeEquivalentTo(command, options =>
                options.ExcludingMissingMembers());
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
            _brandRepository.Setup(x => x.AnyAsync(It.IsAny<Expression<Func<Brand, bool>>>()))
                .ReturnsAsync(() => true)
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