using ECommerce.Common;
using ECommerce.DAL;
using FluentValidation;

namespace ECommerce.BLL
{
    public class CategoryManager : ICategoryManager
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<CategoryWriteDto> _writeCategoryValidator;
        private readonly IImageManager _imageManager;
        public CategoryManager(IUnitOfWork unitOfWork
            , IValidator<CategoryWriteDto> writeCategoryValidator
            , IImageManager imageManager)
        {
            _unitOfWork = unitOfWork;
            _writeCategoryValidator = writeCategoryValidator;
            _imageManager = imageManager;
        }
        public async Task<GeneralResult<IEnumerable<CategoryReadDto>>> GetAllCategoriesAsync()
        {
            var categories = await _unitOfWork.CategoryRepository.GetAllByProductsAsync();

            var categoriesDto = categories
                .Select(c => c.ToReadDto());

            return GeneralResult<IEnumerable<CategoryReadDto>>.SuccessedResult(categoriesDto);
        }

        public async Task<GeneralResult<CategoryReadDto>> GetCategoryAsync(int id)
        {
            var category = await _unitOfWork.CategoryRepository.GetByIdWithProductsAsync(id);

            if (category == null)
                return GeneralResult<CategoryReadDto>.NotFound("This Category Not Found");

            var categoryDTO = category.ToReadDto();

            return GeneralResult<CategoryReadDto>.SuccessedResult(categoryDTO);
        }

        public async Task<GeneralResult<CategoryReadDto>> AddCategoryAsync(CategoryWriteDto categoryWriteDto)
        {
            var validationResult = await _writeCategoryValidator.ValidateAsync(categoryWriteDto);
            if (!validationResult.IsValid)
            {
                Dictionary<string, List<Error>> errors = validationResult.ToError();
                return GeneralResult<CategoryReadDto>.FailedResult(errors);
            }

            var category = categoryWriteDto.ToCategoryModel();
            _unitOfWork.CategoryRepository.Add(category);
            await _unitOfWork.SaveAsync();

            var categoryHelper = await _unitOfWork.CategoryRepository.GetByIdWithProductsAsync(category.Id);
            CategoryReadDto categoryReadDto = categoryHelper!.ToReadDto();

            return GeneralResult<CategoryReadDto>.SuccessedResult(categoryReadDto);
        }

        public async Task<GeneralResult<CategoryReadDto>> EditCategoryAsync(int id, CategoryWriteDto categoryWriteDto)
        {
            var categoryUpdated = await _unitOfWork.CategoryRepository.GetByIdAsync(id);
            if (categoryUpdated == null)
                return GeneralResult<CategoryReadDto>.NotFound("This Category Not Found");

            var validationResult = await _writeCategoryValidator.ValidateAsync(categoryWriteDto);
            if (!validationResult.IsValid)
            {
                Dictionary<string, List<Error>> errors = validationResult.ToError();
                return GeneralResult<CategoryReadDto>.FailedResult(errors);
            }

            categoryUpdated.Name = categoryWriteDto.Name;
            categoryUpdated.Description = categoryWriteDto.Description;
            categoryUpdated.ImageUrl = categoryWriteDto.ImageUrl;
            await _unitOfWork.SaveAsync();

            var categoryHelper = await _unitOfWork.CategoryRepository.GetByIdWithProductsAsync(categoryUpdated.Id);
            CategoryReadDto categoryReadDto = categoryHelper!.ToReadDto();

            return GeneralResult<CategoryReadDto>.SuccessedResult(categoryReadDto);
        }

        public async Task<GeneralResult> DeleteCategoryAsync(int id)
        {
            var category = await _unitOfWork.CategoryRepository.GetByIdWithProductsAsync(id);
            if (category == null)
                return GeneralResult.NotFound("This Category Not Found");

            category.IsDeleted = true;
            foreach (var product in category.Products!)
            {
                product.IsDeleted = true;
            }

            await _unitOfWork.SaveAsync();

            return GeneralResult.SuccessedResult();
        }

        public async Task<GeneralResult> UploadCategoryImageAsync(
            ImageUploadDto imageUploadDto,
            string? basePath, string? schema,
            string? host, int categoryId)
        {
            var result = await _imageManager.UploadImageAsync(imageUploadDto, basePath, schema, host, false, true);
            if (!result.Success)
            {
                return result;
            }


            var categoryUpdated = await _unitOfWork.CategoryRepository.GetByIdAsync(categoryId);

            _imageManager.TryDeleteImageFromDisk(categoryUpdated.ImageUrl, basePath, true, false);


            categoryUpdated.ImageUrl = result.Data.ImageURL;
            await _unitOfWork.SaveAsync();

            var categoryHelper = await _unitOfWork.CategoryRepository.GetByIdWithProductsAsync(categoryUpdated.Id);
            CategoryReadDto categoryReadDto = categoryHelper!.ToReadDto();

            return GeneralResult<CategoryReadDto>.SuccessedResult(categoryReadDto);
        }
    }
}
