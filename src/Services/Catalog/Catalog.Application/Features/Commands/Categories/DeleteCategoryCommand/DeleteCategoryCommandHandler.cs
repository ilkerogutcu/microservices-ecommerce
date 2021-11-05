using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Catalog.Application.Constants;
using Catalog.Application.Extensions;
using Catalog.Application.Interfaces.Repositories;
using MediatR;
using Olcsan.Boilerplate.Aspects.Autofac.Exception;
using Olcsan.Boilerplate.Aspects.Autofac.Logger;
using Olcsan.Boilerplate.Aspects.Autofac.Validation;
using Olcsan.Boilerplate.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Olcsan.Boilerplate.Utilities.Results;

namespace Catalog.Application.Features.Commands.Categories.DeleteCategoryCommand
{
    public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand, IResult>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IProductRepository _productRepository;


        public DeleteCategoryCommandHandler(ICategoryRepository categoryRepository,
            IProductRepository productRepository)
        {
            _categoryRepository = categoryRepository;
            _productRepository = productRepository;
        }


        [LogAspect(typeof(FileLogger), "Catalog-Application")]
        [ExceptionLogAspect(typeof(FileLogger), "Catalog-Application")]
        [ValidationAspect(typeof(DeleteCategoryCommandValidator))]
        public async Task<IResult> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            var mainCategory = await _categoryRepository.GetByIdAsync(request.MainCategoryId);
            if (mainCategory is null) return new ErrorResult(Messages.DataNotFound);

            if (string.IsNullOrEmpty(request.SubCategoryId) && string.IsNullOrEmpty(request.ParentId))
            {
                var productCategoryIds = (await _productRepository.GetListAsync()).Select(x => x.Category.Id);
                foreach (var productCategoryId in productCategoryIds)
                {
                    var categoryHasProduct = mainCategory.SubCategories
                        .Map(p => p.Id.Equals(productCategoryId), n => n.SubCategories)
                        .Any();
                    if (categoryHasProduct)
                    {
                        return new ErrorResult(Messages.DataAssociated);
                    }
                }

                await _categoryRepository.DeleteByIdAsync(mainCategory.Id);
                return new SuccessResult();
            }

            var deleteCategory = mainCategory.SubCategories
                .Map(p => p.ParentId.Equals(request.ParentId), n => n.SubCategories)
                .Map(p => p.Id.Equals(request.SubCategoryId), n => n.SubCategories).FirstOrDefault();

            mainCategory.SubCategories
                .Map(p => p.Id.Equals(request.ParentId), n => n.SubCategories).FirstOrDefault()
                ?.DeleteSubCategory(deleteCategory, "admin");
            await _categoryRepository.UpdateAsync(mainCategory.Id, mainCategory);
            return new SuccessResult();
        }
    }
}