using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Catalog.Application.Constants;
using Catalog.Application.Dtos;
using Catalog.Application.Features.Commands.Brands.UpdateBrandCommand;
using Catalog.Application.Interfaces.Repositories;
using Catalog.Domain.Entities;
using Catalog.UnitTests.Helpers;
using FluentAssertions;
using MongoDB.Bson;
using Moq;
using Xunit;

namespace Catalog.UnitTests.Handlers.BrandTests
{
    public class UpdateBrandTests
    {
        private readonly Mock<IBrandRepository> _brandRepository;

        public UpdateBrandTests()
        {
            _brandRepository = new Mock<IBrandRepository>();
        }

        [Fact]
        private async Task UpdateBrand_WithBrandToUpdate_ReturnSuccessResult()
        {
            // Arrange
            var command = new UpdateBrandCommand
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Name = "Test",
                IsActive = true
            };
            _brandRepository.Setup(x => x.GetByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(new Brand())
                .Verifiable();
            var handler = new UpdateBrandCommandHandler(_brandRepository.Object, MockHelper.CreateMapper());

            // Act
            var result = await handler.Handle(command,
                new CancellationToken());

            // Assert
            result.Success.Should().BeTrue();
            result.Data.Should().BeEquivalentTo(command, options =>
                options.ExcludingMissingMembers());
        }
        
        [Fact]
        private async Task UpdateBrand_WithBrandToUpdate_WhenBrandNameAlreadyExists_ReturnsErrorResult()
        {
            // Arrange
            var command = new UpdateBrandCommand
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Name = "Test2",
                IsActive = true
            };
            var brand = new Brand
            {
                Id = command.Id,
                Name = "Test",
                IsActive = true,
                CreatedBy = "test user",
                CreatedDate = DateTime.Now,
                NormalizedName = "test",
            };
            var brand2 = new Brand
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Name = "Test2",
                IsActive = true,
                CreatedBy = "test user",
                CreatedDate = DateTime.Now,
                NormalizedName = "test2",
            };

            _brandRepository.Setup(x => x.GetByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(brand)
                .Verifiable();
            _brandRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<Brand, bool>>>()))
                .ReturnsAsync(brand2)
                .Verifiable();
            var handler = new UpdateBrandCommandHandler(_brandRepository.Object, MockHelper.CreateMapper());

            // Act
            var result = await handler.Handle(command,
                new CancellationToken());

            // Assert
            result.Message.Should().Be(Messages.DataAlreadyExist);
            result.Data.Should().BeNull();
            result.Success.Should().BeFalse();
        }
        
        [Fact]
        private async Task UpdateBrand_WithBrandToUpdate_WhenDataNotFound_ReturnsErrorResult()
        {
            // Arrange
            var command = new UpdateBrandCommand
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Name = "Test",
                IsActive = true
            };
        
            _brandRepository.Setup(x => x.GetByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(()=>null)
                .Verifiable();
            var handler = new UpdateBrandCommandHandler(_brandRepository.Object, MockHelper.CreateMapper());

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