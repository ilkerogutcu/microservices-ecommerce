using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Catalog.Application.Constants;
using Catalog.Application.Dtos;
using Catalog.Application.Features.Queries.Brands.GetActiveBrandsQuery;
using Catalog.Application.Features.Queries.Brands.GetAllBrandsQuery;
using Catalog.Application.Features.Queries.Brands.GetBrandByIdQuery;
using Catalog.Application.Features.Queries.Brands.GetNotActiveBrandsQuery;
using Catalog.Application.Interfaces.Repositories;
using Catalog.Domain.Entities;
using Catalog.UnitTests.Helpers;
using FluentAssertions;
using MongoDB.Bson;
using Moq;
using Xunit;

namespace Catalog.UnitTests.Handlers.BrandTests
{
    public class GetBrandTests
    {
        private readonly Mock<IBrandRepository> _brandRepository;

        public GetBrandTests()
        {
            _brandRepository = new Mock<IBrandRepository>();
        }

        #region Get brand by id

        [Fact]
        private async Task GetBrandById_WhenExistingBrand_ReturnSuccessResult()
        {
            // Arrange
            var query = new GetBrandByIdQuery();

            var brand = new Brand
            {
                Id = query.Id,
                Name = "Test",
                IsActive = true,
                CreatedBy = "test user",
                CreatedDate = DateTime.Now,
                NormalizedName = "test",
            };

            _brandRepository.Setup(x => x.GetByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(brand)
                .Verifiable();
            var handler = new GetBrandyByIdQueryHandler(_brandRepository.Object, MockHelper.CreateMapper());

            // Act
            var result = await handler.Handle(query,
                new CancellationToken());

            // Assert
            result.Success.Should().BeTrue();
            result.Data.Should()
                .BeEquivalentTo(query);
        }


        [Fact]
        private async Task GetBrandById_WhenNonexistentBrand_ReturnErrorResult()
        {
            // Arrange
            var query = new GetBrandByIdQuery();

            _brandRepository.Setup(x => x.GetByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(() => null)
                .Verifiable();
            var handler = new GetBrandyByIdQueryHandler(_brandRepository.Object, MockHelper.CreateMapper());

            // Act
            var result = await handler.Handle(query,
                new CancellationToken());

            // Assert
            result.Success.Should().BeFalse();
            result.Message.Should().Be(Messages.DataNotFound);
            result.Data.Should().BeNull();
        }

        #endregion


        #region Get all brands

        [Fact]
        private async Task GetAllBrands_WhenExistingBrands_ReturnSuccessResult()
        {
            // Arrange
            var query = new GetAllBrandsQuery()
            {
                PageIndex = 0,
                PageSize = 10
            };

            var brandList = new List<Brand>()
            {
                new()
                {
                    Id = ObjectId.GenerateNewId().ToString(),
                    Name = "Test",
                    IsActive = true,
                    CreatedBy = "test user",
                    CreatedDate = DateTime.Now,
                    NormalizedName = "test",
                },
                new()
                {
                    Id = ObjectId.GenerateNewId().ToString(),
                    Name = "Test",
                    IsActive = true,
                    CreatedBy = "test user",
                    CreatedDate = DateTime.Now,
                    NormalizedName = "test",
                }
            }.AsQueryable();


            _brandRepository.Setup(x => x.GetListAsync(It.IsAny<Expression<Func<Brand, bool>>>()))
                .ReturnsAsync(brandList)
                .Verifiable();
            var handler = new GetAllBrandsQueryHandler(_brandRepository.Object, MockHelper.CreateMapper());

            // Act
            var result = await handler.Handle(query,
                new CancellationToken());

            // Assert
            result.Success.Should().BeTrue();
            result.Data.Should().NotBeNull();
            result.Data.Should().BeEquivalentTo(brandList, options =>
                options.ExcludingMissingMembers());
        }

        [Fact]
        private async Task GetAllBrands_WhenNonexistentBrands_ReturnSuccessResult()
        {
            // Arrange
            var query = new GetAllBrandsQuery();

            var brandList = new List<Brand>().AsQueryable();


            _brandRepository.Setup(x => x.GetListAsync(It.IsAny<Expression<Func<Brand, bool>>>()))
                .ReturnsAsync(brandList)
                .Verifiable();
            var handler = new GetAllBrandsQueryHandler(_brandRepository.Object, MockHelper.CreateMapper());

            // Act
            var result = await handler.Handle(query,
                new CancellationToken());

            // Assert
            result.Data.Should().HaveCount(0);
            result.Success.Should().BeTrue();
        }

        #endregion


        #region Get active brands

        [Fact]
        private async Task GetActiveBrands_WhenExistingBrands_ReturnSuccessResult()
        {
            // Arrange
            var query = new GetActiveBrandsQuery
            {
                PageIndex = 0,
                PageSize = 1
            };

            var brandList = new List<Brand>()
            {
                new()
                {
                    Id = ObjectId.GenerateNewId().ToString(),
                    Name = "Test",
                    IsActive = true,
                    CreatedBy = "test user",
                    CreatedDate = DateTime.Now,
                    NormalizedName = "test",
                }
            }.AsQueryable();


            _brandRepository.Setup(x => x.GetListAsync(It.IsAny<Expression<Func<Brand, bool>>>()))
                .ReturnsAsync(brandList)
                .Verifiable();
            var handler = new GetActiveBrandsQueryHandler(_brandRepository.Object, MockHelper.CreateMapper());

            // Act
            var result = await handler.Handle(query,
                new CancellationToken());

            // Assert
            result.Success.Should().BeTrue();
            result.Data.Should().NotBeNull();
            result.Data.Should().BeEquivalentTo(brandList, options =>
                options.ExcludingMissingMembers());
        }

        [Fact]
        private async Task GetActiveBrands_WhenNonexistentBrands_ReturnSuccessResult()
        {
            // Arrange
            var query = new GetActiveBrandsQuery()
            {
                PageIndex = 0,
                PageSize = 1
            };

            var brandList = new List<Brand>().AsQueryable();


            _brandRepository.Setup(x => x.GetListAsync(It.IsAny<Expression<Func<Brand, bool>>>()))
                .ReturnsAsync(brandList)
                .Verifiable();
            var handler = new GetActiveBrandsQueryHandler(_brandRepository.Object, MockHelper.CreateMapper());

            // Act
            var result = await handler.Handle(query,
                new CancellationToken());

            // Assert
            result.Success.Should().BeTrue();
            result.Data.Should().HaveCount(0);
        }

        #endregion


        #region Get not active brands

        [Fact]
        private async Task GetNotActiveBrands_WhenExistingBrands_ReturnSuccessResult()
        {
            // Arrange
            var query = new GetNotActiveBrandsQuery
            {
                PageIndex = 0,
                PageSize = 1
            };

            var brandList = new List<Brand>()
            {
                new()
                {
                    Id = ObjectId.GenerateNewId().ToString(),
                    Name = "Test",
                    IsActive = false,
                    CreatedBy = "test user",
                    CreatedDate = DateTime.Now,
                    NormalizedName = "test",
                }
            }.AsQueryable();


            _brandRepository.Setup(x => x.GetListAsync(It.IsAny<Expression<Func<Brand, bool>>>()))
                .ReturnsAsync(brandList)
                .Verifiable();
            var handler = new GetNotActiveBrandsQueryHandler(_brandRepository.Object, MockHelper.CreateMapper());

            // Act
            var result = await handler.Handle(query,
                new CancellationToken());

            // Assert
            result.Success.Should().BeTrue();
            result.Data.Should().NotBeNull();
            result.Data.Should().BeEquivalentTo(brandList, options =>
                options.ExcludingMissingMembers());
        }

        [Fact]
        private async Task GetNotActiveBrands_WhenNonexistentBrands_ReturnSuccessResult()
        {
            // Arrange
            var query = new GetNotActiveBrandsQuery
            {
                PageIndex = 0,
                PageSize = 1
            };

            var brandList = new List<Brand>().AsQueryable();


            _brandRepository.Setup(x => x.GetListAsync(It.IsAny<Expression<Func<Brand, bool>>>()))
                .ReturnsAsync(brandList)
                .Verifiable();
            var handler = new GetNotActiveBrandsQueryHandler(_brandRepository.Object, MockHelper.CreateMapper());

            // Act
            var result = await handler.Handle(query,
                new CancellationToken());

            // Assert
            result.Success.Should().BeTrue();
            result.Data.Should().HaveCount(0);
        }

        #endregion
    }
}