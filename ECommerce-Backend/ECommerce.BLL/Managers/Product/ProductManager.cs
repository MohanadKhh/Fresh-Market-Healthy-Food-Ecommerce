using ECommerce.Common;
using ECommerce.Common.Pagination;
using ECommerce.DAL;
using FluentValidation;

namespace ECommerce.BLL
{
    public class ProductManager : IProductManager
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IImageManager _imageManager;
        private readonly IValidator<ProductWriteDto> _writeProductValidator;
        private readonly IValidator<PaginationParameters> _paginationParamValidator;
        private readonly IValidator<ProductFilter> _productFilterValidator;
        public ProductManager(IUnitOfWork unitOfWork
            , IImageManager imageManager
            , IValidator<ProductWriteDto> writeProductValidator
            , IValidator<PaginationParameters> paginationParamValidator
            , IValidator<ProductFilter> productFilterValidator)
        {
            _unitOfWork = unitOfWork;
            _imageManager = imageManager;
            _writeProductValidator = writeProductValidator;
            _paginationParamValidator = paginationParamValidator;
            _productFilterValidator = productFilterValidator;
        }

        public async Task<GeneralResult<IEnumerable<ProductReadDto>>> GetAllProductsAsync()
        {
            var products = await _unitOfWork.ProductRepository.GetAllWithCategoryAsync();

            var productsReadDto = products.Select(p => p.ToReadDto());

            return GeneralResult<IEnumerable<ProductReadDto>>.SuccessedResult(productsReadDto);
        }

        public async Task<GeneralResult<ProductReadDto?>> GetProductAsync(int id)
        {
            var product = await _unitOfWork.ProductRepository.GetByIdWithCategoryAsync(id);

            if (product == null)
                return GeneralResult<ProductReadDto?>.NotFound("This Product Not Found");

            var productReadDto = product.ToReadDto();

            return GeneralResult<ProductReadDto?>.SuccessedResult(productReadDto);
        }

        public async Task<GeneralResult<ProductReadDto?>> AddProductAsync(ProductWriteDto productWriteDto)
        {
            var validationResult = await _writeProductValidator.ValidateAsync(productWriteDto);
            if (!validationResult.IsValid)
            {
                Dictionary<string, List<Error>> errors = validationResult.ToError();
                return GeneralResult<ProductReadDto?>.FailedResult(errors);
            }

            Product product = productWriteDto.ToProductModel();

            _unitOfWork.ProductRepository.Add(product);
            await _unitOfWork.SaveAsync();

            var productHelper = await _unitOfWork.ProductRepository.GetByIdWithCategoryAsync(product.Id);
            ProductReadDto productReadDto = productHelper!.ToReadDto();

            return GeneralResult<ProductReadDto?>.SuccessedResult(productReadDto);
        }

        public async Task<GeneralResult<ProductReadDto?>> EditProductAsync(int id, ProductWriteDto productWriteDto)
        {
            var validationResult = await _writeProductValidator.ValidateAsync(productWriteDto);
            if (!validationResult.IsValid)
            {
                Dictionary<string, List<Error>> errors = validationResult.ToError();
                return GeneralResult<ProductReadDto?>.FailedResult(errors);
            }

            var productUpdated = await _unitOfWork.ProductRepository.GetByIdWithCategoryAsync(id);
            if (productUpdated == null)
            {
                return GeneralResult<ProductReadDto?>.NotFound("This Product Not Found");
            }

            productUpdated.Name = productWriteDto.Name;
            productUpdated.Description = productWriteDto.Description;
            productUpdated.Price = productWriteDto.Price;
            productUpdated.Stock = productWriteDto.Stock;
            productUpdated.Unit = productWriteDto.Unit;
            productUpdated.Reviews = productWriteDto.Reviews;
            productUpdated.IsOrganic = productWriteDto.IsOrganic;
            productUpdated.IsFeatured = productWriteDto.IsFeatured;
            productUpdated.CreatedAt = productWriteDto.CreatedAt == default
                ? productUpdated.CreatedAt
                : productWriteDto.CreatedAt;
            productUpdated.ExpiryDate = productWriteDto.ExpiryDate;
            productUpdated.ImageUrl = productWriteDto.ImageUrl;
            productUpdated.CategoryId = productWriteDto.CategoryId;

            await _unitOfWork.SaveAsync();

            var product = await _unitOfWork.ProductRepository.GetByIdWithCategoryAsync(productUpdated.Id);
            ProductReadDto productReadDto = product!.ToReadDto();

            return GeneralResult<ProductReadDto?>.SuccessedResult(productReadDto);
        }

        public async Task<GeneralResult> DeleteProductAsync(int id)
        {
            var productDeleted = await _unitOfWork.ProductRepository.GetByIdAsync(id);
            if (productDeleted == null)
                return GeneralResult.NotFound("This Product Not Found");

            await _unitOfWork.CartRepository.RemoveAllItemsWithProductIdAsync(id);
            productDeleted.IsDeleted = true;
            await _unitOfWork.SaveAsync();

            return GeneralResult.SuccessedResult($"Deleted the product with id '{id}' and all cart items of that product");
        }

        public async Task<GeneralResult<IEnumerable<ProductReadDto>>> GetProductsByCategoryIdAsync(int categoryId)
        {
            var isCategoryExist = await _unitOfWork.CategoryRepository.GetByIdAsync(categoryId);
            if (isCategoryExist == null)
            {
                return GeneralResult<IEnumerable<ProductReadDto>>.NotFound("This Category Not Found");
            }

            var products = await _unitOfWork.ProductRepository.GetAllByCategoryIdAsync(categoryId);

            var productsReadDto = products.Select(p => p.ToReadDto());

            return GeneralResult<IEnumerable<ProductReadDto>>.SuccessedResult(productsReadDto);
        }

        public async Task<GeneralResult<PaginationResult<Product>>> GetAllProductsByPaginationAsync(PaginationParameters paginationParameters, ProductFilter productFilter)
        {
            var validationPaginationRes = await _paginationParamValidator.ValidateAsync(paginationParameters);
            if (!validationPaginationRes.IsValid)
            {
                Dictionary<string, List<Error>> errors = validationPaginationRes.ToError();
                return GeneralResult<PaginationResult<Product>>.FailedResult(errors);
            }

            var validationFilterRes = await _productFilterValidator.ValidateAsync(productFilter);
            if (!validationFilterRes.IsValid)
            {
                Dictionary<string, List<Error>> errors = validationFilterRes.ToError();
                return GeneralResult<PaginationResult<Product>>.FailedResult(errors);
            }

            var productsPagination = await _unitOfWork.ProductRepository.GetAllByPagination(paginationParameters, productFilter);
            return GeneralResult<PaginationResult<Product>>.SuccessedResult(productsPagination);
        }

        public async Task<GeneralResult> UploadProductImageAsync(
            ImageUploadDto imageUploadDto,
            string? basePath,
            string? schema,
            string? host,
            int productId)
        {
            var result = await _imageManager.UploadImageAsync(imageUploadDto, basePath, schema, host, true, false);
            if (!result.Success)
            {
                return result;
            }

            var productUpdated = await _unitOfWork.ProductRepository.GetByIdAsync(productId);
            _imageManager.TryDeleteImageFromDisk(productUpdated.ImageUrl, basePath, true, false);

            productUpdated.ImageUrl = result.Data.ImageURL;
            await _unitOfWork.SaveAsync();

            var product = await _unitOfWork.ProductRepository.GetByIdWithCategoryAsync(productUpdated.Id);
            ProductReadDto productReadDto = product!.ToReadDto();

            return GeneralResult<ProductReadDto>.SuccessedResult(productReadDto);
        }
    }
}