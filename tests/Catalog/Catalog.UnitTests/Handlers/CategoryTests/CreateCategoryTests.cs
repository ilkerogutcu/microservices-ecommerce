using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Catalog.Application.Constants;
using Catalog.Application.Features.Commands.Brands.CreateBrandCommand;
using Catalog.Application.Features.Commands.Categories.CreateCategoryCommand;
using Catalog.Application.Interfaces.Repositories;
using Catalog.Domain.Entities;
using Catalog.UnitTests.Helpers;
using FluentAssertions;
using MongoDB.Bson;
using Moq;
using Xunit;

namespace Catalog.UnitTests.Handlers.CategoryTests
{
    public class CreateCategoryTests
    {
        private readonly Mock<ICategoryRepository> _categoryRepository;

        public CreateCategoryTests()
        {
            _categoryRepository = new Mock<ICategoryRepository>();
        }

        [Fact]
        private async Task CreateMainCategory_WithCategoryToCreate_ReturnSuccessResult()
        {
            // Arrange
            var command = new CreateCategoryCommand()
            {
                Name = "Main Test Category",
                IsActive = true
            };
            _categoryRepository.Setup(x => x.AnyAsync(It.IsAny<Expression<Func<Category, bool>>>()))
                .ReturnsAsync(() => false)
                .Verifiable();
            _categoryRepository.Setup(x => x.AddAsync(It.IsAny<Category>()))
                .ReturnsAsync(new Category())
                .Verifiable();
            var handler = new CreateCategoryCommandHandler(MockHelper.CreateMapper(), _categoryRepository.Object);

            // Act
            var result = await handler.Handle(command,
                new CancellationToken());

            // Assert
            result.Success.Should().BeTrue();
            result.Data.Should().BeEquivalentTo(command, options =>
                options.ExcludingMissingMembers());
        }

        [Fact]
        private async Task CreateMainCategory_WithCategoryToCreate_WhenCategoryNameIsAlreadyExists_ReturnErrorResult()
        {
            // Arrange
            var command = new CreateCategoryCommand()
            {
                Name = "Main Test Category",
                IsActive = true
            };
            _categoryRepository.Setup(x => x.AnyAsync(It.IsAny<Expression<Func<Category, bool>>>()))
                .ReturnsAsync(() => true)
                .Verifiable();

            var handler = new CreateCategoryCommandHandler(MockHelper.CreateMapper(), _categoryRepository.Object);

            // Act
            var result = await handler.Handle(command,
                new CancellationToken());

            // Assert
            result.Success.Should().BeFalse();
            result.Data.Should().BeNull();
            result.Message.Should().Be(Messages.DataAlreadyExist);
        }
        [Fact]
        private async Task CreateSubCategory_WithCategoryToCreate_WhenMainCategoryIdIsNull_ReturnErrorResult()
        {
            // Arrange
            var command = new CreateCategoryCommand
            {
                SubCategoryId = ObjectId.GenerateNewId().ToString(),
                Name = "Sub Category",
                IsActive = true
            };
            var handler = new CreateCategoryCommandHandler(MockHelper.CreateMapper(), _categoryRepository.Object);

            // Act
            var result = await handler.Handle(command,
                new CancellationToken());

            // Assert
            result.Success.Should().BeFalse();
            result.Data.Should().BeNull();
            result.Message.Should().Be(Messages.DataNotFound);
        }
        [Fact]
        private async Task CreateSubCategory_WithCategoryToCreate_WhenSubCategoryIdIsNull_ReturnSuccessResult()
        {
            // Arrange
            var command = new CreateCategoryCommand
            {
                MainCategoryId = ObjectId.GenerateNewId().ToString(),
                Name = "Sub Category",
                IsActive = true
            };
            _categoryRepository.Setup(x => x.GetByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(new Category())
                .Verifiable();
            _categoryRepository.Setup(x => x.UpdateAsync(It.IsAny<string>(),It.IsAny<Category>()))
                .ReturnsAsync(new Category())
                .Verifiable();
            var handler = new CreateCategoryCommandHandler(MockHelper.CreateMapper(), _categoryRepository.Object);

            // Act
            var result = await handler.Handle(command,
                new CancellationToken());

            // Assert
            result.Success.Should().BeTrue();
            result.Data.Should().NotBeNull();
        }
        [Fact]
        private async Task CreateSubCategory_WithCategoryToCreate_WhenSubCategoryIdAndMainCategoryIdIsNotNull_ReturnSuccessResult()
        {
            // Arrange
            var command = new CreateCategoryCommand
            {
                SubCategoryId = ObjectId.GenerateNewId().ToString(),
                MainCategoryId = ObjectId.GenerateNewId().ToString(),
                Name = "Sub Category",
                IsActive = true
            };
            _categoryRepository.Setup(x => x.GetByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(new Category())
                .Verifiable();
            _categoryRepository.Setup(x => x.UpdateAsync(It.IsAny<string>(),It.IsAny<Category>()))
                .ReturnsAsync(new Category())
                .Verifiable();
            var handler = new CreateCategoryCommandHandler(MockHelper.CreateMapper(), _categoryRepository.Object);

            // Act
            var result = await handler.Handle(command,
                new CancellationToken());

            // Assert
            result.Success.Should().BeTrue();
            result.Data.Should().NotBeNull();
        }
    }
}