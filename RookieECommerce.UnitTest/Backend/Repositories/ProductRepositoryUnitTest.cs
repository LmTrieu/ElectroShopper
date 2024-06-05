using AutoFixture;
using AutoFixture.Xunit2;
using Microsoft.AspNetCore.Http;
using Moq;
using RookieEShopper.Application.Dto.Product;
using RookieEShopper.Application.Repositories;
using RookieEShopper.Application.Service;
using RookieEShopper.Domain.Data.Entities;
using RookieEShopper.SharedLibrary.HelperClasses;
using RookieEShopper.SharedLibrary.ViewModels;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace RookieECommerce.UnitTest.Backend.Repositories
{
    public class ProductRepositoryUnitTest : BaseUnitTest
    {
        [Theory]
        [AutoData]
        public async Task GetAllProductsAsync_ReturnsProductPageList(
            QueryParameters query)
        {
            //Arrange
            var products = _fixture.CreateMany<Product>();

            List<ResponseProductDto> responseProductDtos = new List<ResponseProductDto>();

            products.ToList().ForEach(c => responseProductDtos.Add(new ResponseProductDto
            {
                id = c.Id,
                description = c.Description,
                name = c.Name,
                category = new CategoryVM { Id = c.Category.Id, Name = c.Category.Name , Description = c.Category.Description},
                price = c.Price,
                mainImagePath = c.MainImagePath,
                imageGallery = c.ImageGallery
            }));

            var expectedProducts = new PagedList<ResponseProductDto>(responseProductDtos, responseProductDtos.Count, query.PageNumber, query.PageSize);

            var productRepository = _fixture.Create<Mock<IProductRepository>>();
            productRepository.Setup(cr => cr.GetAllProductsAsync(query))
                .ReturnsAsync(expectedProducts);

            // Act
            var result = await productRepository.Object.GetAllProductsAsync(query);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<PagedList<ResponseProductDto>>(result);

            Assert.Equal(expectedProducts.Count, result.Count);
            Assert.Equal(expectedProducts.PageSize, result.PageSize);
            Assert.Equal(expectedProducts.TotalCount, result.TotalCount);
            Assert.Equal(expectedProducts.TotalPages, result.TotalPages);
            Assert.Equal(expectedProducts.CurrentPage, result.CurrentPage);

            Assert.Equal(expectedProducts, result);
            Assert.Equal(expectedProducts.FirstOrDefault(), result.FirstOrDefault());
            Assert.Equal(expectedProducts.LastOrDefault(), result.LastOrDefault());

            Assert.Equal(products.FirstOrDefault().Id, result.FirstOrDefault().id);
            Assert.Equal(products.LastOrDefault().Id, result.LastOrDefault().id);

            Assert.Equal(products.FirstOrDefault().Category.Id, result.FirstOrDefault().category.Id);
            Assert.Equal(products.LastOrDefault().Category.Id, result.LastOrDefault().category.Id);
        }

        [Theory]
        [AutoData]
        public async Task GetProductDetailByIdAsync_WhenFound_ReturnsResponseDomainProductDto(
            ResponseDomainProductDto product)
        {
            // Arrange
            var productRepository = _fixture.Create<Mock<IProductRepository>>();
            productRepository.Setup(repo => repo.GetProductDetailByIdAsync(product.Id))
                .ReturnsAsync(product);

            // Act
            var result = await productRepository.Object.GetProductDetailByIdAsync(product.Id);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<ResponseDomainProductDto>(result);

            Assert.Equal(product, result);
        }

        [Fact]
        public async Task CreateProductAsync_WithNoImageGallery_ReturnProduct()
        {
            // Arrange
            _fixture.Customize<ProductDto>(c => c
                .Without(p => p.ProductImage)
            );

            var productDto = _fixture.Create<ProductDto>();
            productDto.ProductImage = new FormFile(new MemoryStream(), 0, 0, null, "test.jpg");

            var expectedProduct = new Product
            {
                Description = productDto.Description,
                Name = productDto.Name,
                Category = new  Category { Id = productDto.CategoryId },
                Price = productDto.Price,
                MainImagePath = productDto.ProductImage.FileName
            };

            var productRepository = _fixture.Create<Mock<IProductRepository>>();

            productRepository.Setup(repo => repo.CreateProductAsync(productDto, null))
                .ReturnsAsync(expectedProduct);

            // Act
            var result = await productRepository.Object.CreateProductAsync(productDto, null);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<Product>(result);

            Assert.Equal(expectedProduct, result);
            Assert.Equal(expectedProduct.Id, result.Id);
            Assert.Equal(expectedProduct.Name, result.Name);
            Assert.Equal(expectedProduct.Price, result.Price);
            Assert.Equal(expectedProduct.Category, result.Category);
            Assert.Equal(expectedProduct.Description, result.Description);
            Assert.Equal(expectedProduct.MainImagePath, result.MainImagePath);
            Assert.Equal(expectedProduct.ProductReviews, result.ProductReviews);

            Assert.Equal(productDto.Name, result.Name);
            Assert.Equal(productDto.Price, result.Price);
            Assert.Equal(productDto.CategoryId, result.Category.Id);
            Assert.Equal(productDto.Description, result.Description);
            Assert.Equal(productDto.ProductImage.FileName, result.MainImagePath);
        }

        [Fact]
        public async Task UpdateProductAsync_ReturnsListProduct()
        {
            // Arrange
            _fixture.Customize<ProductDto>(c => c
                .Without(p => p.ProductImage)
            );

            var productDto = _fixture.Create<ProductDto>();
            productDto.ProductImage = new FormFile(new MemoryStream(), 0, 0, null, "test.jpg");

            var expectedProduct = new Product
            {
                Description = productDto.Description,
                Name = productDto.Name,
                Category = new Category { Id = productDto.CategoryId },
                Price = productDto.Price,
                MainImagePath = productDto.ProductImage.FileName
            };

            var productRepository = _fixture.Create<Mock<IProductRepository>>();

            productRepository.Setup(repo => repo.UpdateProductAsync(expectedProduct.Id,productDto))
                .ReturnsAsync(expectedProduct);

            // Act
            var result = await productRepository.Object.UpdateProductAsync(expectedProduct.Id, productDto);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<Product>(result);

            Assert.Equal(result, expectedProduct);
            Assert.Equal(result.Id, expectedProduct.Id);
            Assert.Equal(result.Name, expectedProduct.Name);
            Assert.Equal(result.Price, expectedProduct.Price);
            Assert.Equal(result.Category, expectedProduct.Category);
            Assert.Equal(result.Description, expectedProduct.Description);
            Assert.Equal(result.MainImagePath, expectedProduct.MainImagePath);
            Assert.Equal(result.ProductReviews, expectedProduct.ProductReviews);

            Assert.Equal(result.Name, productDto.Name);
            Assert.Equal(result.Price, productDto.Price);
            Assert.Equal(result.Category.Id, productDto.CategoryId);
            Assert.Equal(result.Description, productDto.Description);
            Assert.Equal(result.MainImagePath, productDto.ProductImage.FileName);
        }

        [Theory]
        [AutoData]
        public async Task DeleteProductAsync_WhenFound_ReturnCategory(
            Product product)
        {
            //Arrange
            var categoryRepository = _fixture.Create<Mock<IProductRepository>>();
            categoryRepository.Setup(cr => cr.DeleteProductAsync(product.Id))
                .ReturnsAsync(true);

            //Act
            var result = await categoryRepository.Object.DeleteProductAsync(product.Id);

            //Assert 
            Assert.True(result);
            categoryRepository.Verify(cr => cr.DeleteProductAsync(product.Id), Times.Once);
        }

        [Theory]
        [AutoData]
        public async Task IsProductExistAsync_WhenFound_ReturnCategory(
            Product product)
        {
            //Arrange
            var categoryRepository = _fixture.Create<Mock<IProductRepository>>();
            categoryRepository.Setup(cr => cr.IsProductExistAsync(product.Id))
                .ReturnsAsync(true);

            //Act
            var result = await categoryRepository.Object.IsProductExistAsync(product.Id);

            //Assert 
            Assert.True(result);
            categoryRepository.Verify(cr => cr.IsProductExistAsync(product.Id), Times.Once);
        }

        [Theory]
        [AutoData]
        public async Task GetProductsByCategoryAsync_ReturnsProductPageList(
            QueryParameters query)
        {
            //Arrange
            var products = _fixture.CreateMany<Product>();

            List<ResponseProductDto> responseProductDtos = new List<ResponseProductDto>();

            products.ToList().ForEach(c => responseProductDtos.Add(new ResponseProductDto
            {
                id = c.Id,
                description = c.Description,
                name = c.Name,
                category = new CategoryVM { Id = c.Category.Id, Name = c.Category.Name, Description = c.Category.Description },
                price = c.Price,
                mainImagePath = c.MainImagePath,
                imageGallery = c.ImageGallery
            }));

            var expectedProducts = new PagedList<ResponseProductDto>(responseProductDtos, responseProductDtos.Count, query.PageNumber, query.PageSize);

            var productRepository = _fixture.Create<Mock<IProductRepository>>();
            productRepository.Setup(cr => cr.GetProductsByCategoryAsync(query, products.FirstOrDefault().Category.Id))
                .ReturnsAsync(expectedProducts);

            // Act
            var result = await productRepository.Object.GetProductsByCategoryAsync(query, products.FirstOrDefault().Category.Id);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<PagedList<ResponseProductDto>>(result);

            Assert.Equal(expectedProducts.Count, result.Count);
            Assert.Equal(expectedProducts.PageSize, result.PageSize);
            Assert.Equal(expectedProducts.TotalCount, result.TotalCount);
            Assert.Equal(expectedProducts.TotalPages, result.TotalPages);
            Assert.Equal(expectedProducts.CurrentPage, result.CurrentPage);

            Assert.Equal(expectedProducts, result);
            Assert.Equal(expectedProducts.FirstOrDefault(), result.FirstOrDefault());
            Assert.Equal(expectedProducts.LastOrDefault(), result.LastOrDefault());

            Assert.Equal(products.FirstOrDefault().Id, result.FirstOrDefault().id);
            Assert.Equal(products.LastOrDefault().Id, result.LastOrDefault().id);

            Assert.Equal(products.FirstOrDefault().Category.Id, result.FirstOrDefault().category.Id);
            Assert.Equal(products.LastOrDefault().Category.Id, result.LastOrDefault().category.Id);
        }

        [Theory]
        [AutoData]
        public async Task UploadProductImageAsync_WhenFound_ReturnCategory(
            Product product)
        {
            // Arrange
            IFormFile image = new FormFile(new MemoryStream(), 0, 0, null, "test.jpg");

            var expectedImagePath = Guid.NewGuid() + "_" + image.FileName;

            var productRepository = _fixture.Create<Mock<IProductRepository>>();
            productRepository.Setup(cr => cr.UploadProductImageAsync(product, image))
                .ReturnsAsync(expectedImagePath);

            // Act
            var result = await productRepository.Object.UploadProductImageAsync(product, image);

            // Assert

            Assert.NotNull(result);
            Assert.IsType<string>(result);

            Assert.Contains(expectedImagePath, result);
        }

        [Fact]
        public async Task UploadOnlyProductImageAsync_WhenFound_ReturnCategory()
        {
            // Arrange
            IFormFile image = new FormFile(new MemoryStream(), 0, 0, "testimage", "test.jpg");

            var imagePath = $"{Guid.NewGuid()}_{image.FileName}";

            var expectedProductImageDto = new ProductImageDto
            {
                Uid = imagePath,
                Name = image.Name,
                Url = "https:\\localhost:7265\\ProductImages\\PlaceHolderPDID\\"+ imagePath,
                Status = "done"
            };

            var productRepository = _fixture.Create<Mock<IProductRepository>>();
            productRepository.Setup(cr => cr.UploadOnlyProductImageAsync(image))
                .ReturnsAsync(expectedProductImageDto);

            // Act
            var result = await productRepository.Object.UploadOnlyProductImageAsync(image);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<ProductImageDto>(result);

            Assert.Equal(expectedProductImageDto, result);
            Assert.Equal(expectedProductImageDto.Url, result.Url);
        }


    }
}
