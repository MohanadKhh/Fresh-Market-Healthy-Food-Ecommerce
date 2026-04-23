using ECommerce.Common;
using ECommerce.DAL;
using FluentValidation;

namespace ECommerce.BLL
{
    public class CategoryManager : ICategoryManager
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<CategoryWriteDto> _writeCategoryValidator;
        public CategoryManager(IUnitOfWork unitOfWork, IValidator<CategoryWriteDto> writeCategoryValidator)
        {
            _unitOfWork = unitOfWork;
            _writeCategoryValidator = writeCategoryValidator;
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

            //Mapping to Category Model
            var category = new Category { Name = categoryWriteDto.Name };
            _unitOfWork.CategoryRepository.Add(category);
            await _unitOfWork.SaveAsync();

            //Mapping to DTO
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
            await _unitOfWork.SaveAsync();

            //Mapping to DTO
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
    }
}
