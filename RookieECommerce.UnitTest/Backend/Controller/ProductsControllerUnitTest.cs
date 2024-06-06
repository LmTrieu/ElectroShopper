using AutoFixture;
using AutoFixture.Xunit2;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RookieEShopper.Api.Controllers;
using RookieEShopper.Application.Dto.Product;
using RookieEShopper.Application.Repositories;
using RookieEShopper.Application.Service;
using RookieEShopper.Domain.Data.Entities;
using RookieEShopper.SharedLibrary.HelperClasses;
using Microsoft.AspNetCore.Http;
using RookieEShopper.Api.Dto;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http.HttpResults;
using RookieEShopper.SharedLibrary.ViewModels;

namespace RookieECommerce.UnitTest.Backend.Controller
{
    public class ProductsControllerUnitTest : BaseUnitTest
    {
        private readonly Mock<IProductRepository> _productRepositoryMock;
        private readonly Mock<IValidator<ProductDto>> _productValidatorMock;
        private readonly ProductsController _controller;
        private readonly Mock<HttpResponse> _mockHttpResponse;
        private readonly Mock<HttpContext> _mockHttpContext;

        public ProductsControllerUnitTest()
        {
            _productRepositoryMock = _fixture.Freeze<Mock<IProductRepository>>();
            _productValidatorMock = _fixture.Freeze<Mock<IValidator<ProductDto>>>();
            _controller = new ProductsController(_productRepositoryMock.Object, _productValidatorMock.Object);

            _mockHttpContext = new Mock<HttpContext>();
            _mockHttpResponse = new Mock<HttpResponse>();
            _mockHttpResponse.Setup(r => r.Headers).Returns(new HeaderDictionary());

            _mockHttpContext.Setup(c => c.Response).Returns(_mockHttpResponse.Object);
            _controller.ControllerContext = new ControllerContext()
            {
                HttpContext = _mockHttpContext.Object
            };
        }

        [Theory, AutoData]
        public async Task GetProducts_WhenProductsExist_ReturnsOk(QueryParameters query, PagedList<ResponseProductDto> products)
        {
            // Arrange
            _productRepositoryMock.Setup(repo => repo.GetAllProductsAsync(query)).ReturnsAsync(products);

            // Act
            var result = await _controller.GetProducts(query);

            // Assert
            var okResult = Assert.IsType<Ok<ApiListObjectResponse<ResponseProductDto>>>(result.Result);
            var response = Assert.IsType<ApiListObjectResponse<ResponseProductDto>>(okResult.Value);
            Assert.Equal("Products fetched successfully", response.Message);
            Assert.Equal(products.Count, response.Total);
        }

        [Theory, AutoData]
        public async Task GetProducts_WhenNoProductsExist_ReturnsNotFound(QueryParameters query)
        {
            // Arrange
            _productRepositoryMock.Setup(repo => repo.GetAllProductsAsync(query)).ReturnsAsync(new PagedList<ResponseProductDto>(new List<ResponseProductDto>(),0,0,0));

            // Act
            var result = await _controller.GetProducts(query);

            // Assert
            var notFoundResult = Assert.IsType<NotFound<string>>(result.Result);
            Assert.Equal("No product is available at the moment, try again later", notFoundResult.Value);
        }

        [Fact]
        public async Task GetProductsByCategory_WhenProductsExist_ReturnsOk()
        {
            // Arrange
            List<ResponseProductDto> products = new List<ResponseProductDto>
            {
                new ResponseProductDto()
                {
                    id = 1,
                    category = new CategoryVM
                    {
                        Id = 12,
                        Name = "Category1"
                    },
                },
                new ResponseProductDto()
                {
                    id = 2,
                    category = new CategoryVM
                    {
                        Id = 22,
                        Name = "Product2"
                    },
                },
                new ResponseProductDto()
                {
                    id = 3,
                    category = new CategoryVM
                    {
                        Id = 32,
                        Name = "Product3"
                    },
                },
                new ResponseProductDto()
                {
                    id = 4,
                    category = new CategoryVM
                    {
                        Id = 12,
                        Name = "Category1"
                    },
                },
            };

            List<ResponseProductDto> expectedProducts = new List<ResponseProductDto>
            {
                new ResponseProductDto()
                {
                    id = 1,
                    category = new CategoryVM
                    {
                        Id = 12,
                        Name = "Product1"
                    },
                },
                new ResponseProductDto()
                {
                    id = 4,
                    category = new CategoryVM
                    {
                        Id = 12,
                        Name = "Category1"
                    },
                },
            };
            PagedList<ResponseProductDto> expectedProductsDto = 
                new PagedList<ResponseProductDto>(expectedProducts, expectedProducts.Count, 1, 10);

            var query = new QueryParameters { PageNumber = 1, PageSize = 10 };

            _productRepositoryMock.Setup(repo => 
                    repo.GetProductsByCategoryAsync(query, 12))
                        .ReturnsAsync(expectedProductsDto);

            // Act
            var result = await _controller.GetProductsByCategory(query, 12);

            // Assert
            var okResult = Assert.IsType<Ok<ApiListObjectResponse<ResponseProductDto>>>(result.Result);
            var response = Assert.IsType<ApiListObjectResponse<ResponseProductDto>>(okResult.Value);

            Assert.Equal("Products fetched successfully", response.Message);
            Assert.Equal(expectedProducts.Count, response.Total);

            Assert.True(
                response.Data.Select(p => p.category.Id)
                    .Distinct()
                    .Count() < 2
                );

            Assert.Equal(response.Data[0].category.Id, 12);
        }

        [Theory, AutoData]
        public async Task GetProduct_WhenProductExists_ReturnsOk(int id, ResponseDomainProductDto product)
        {
            // Arrange
            _productRepositoryMock.Setup(repo => repo.GetProductDetailByIdAsync(id)).ReturnsAsync(product);

            // Act
            var result = await _controller.GetProduct(id);

            // Assert
            var okResult = Assert.IsType<Ok<ApiSingleObjectResponse<ResponseDomainProductDto>>>(result.Result);
            var response = Assert.IsType<ApiSingleObjectResponse<ResponseDomainProductDto>>(okResult.Value);
            Assert.Equal("Products fetched successfully", response.Message);
            Assert.Equal(product, response.Data);
        }

        [Theory, AutoData]
        public async Task GetProduct_WhenProductDoesNotExist_ReturnsNotFound(int id)
        {
            // Arrange
            _productRepositoryMock.Setup(repo => repo.GetProductDetailByIdAsync(id)).ReturnsAsync((ResponseDomainProductDto)null);

            // Act
            var result = await _controller.GetProduct(id);

            // Assert
            var notFoundResult = Assert.IsType<NotFound<string>>(result.Result);
            Assert.Contains("No product is available at the moment, try again later", notFoundResult.Value);
        }

        [Theory, AutoData]
        public async Task PatchProduct_WhenProductExists_ReturnsOk(int id, Product updatedProduct)
        {
            // Arrange
            _fixture.Customize<ProductDto>(c => c
                .Without(p => p.ProductImage)
            );
            var productDto = _fixture.Create<ProductDto>();

            _productRepositoryMock.Setup(repo => repo.IsProductExistAsync(id)).ReturnsAsync(true);
            _productRepositoryMock.Setup(repo => repo.UpdateProductAsync(id, productDto)).ReturnsAsync(updatedProduct);

            // Act
            var result = await _controller.PatchProduct(id, productDto);

            // Assert
            var okResult = Assert.IsType<Ok<ApiSingleObjectResponse<Product>>>(result.Result);
            var response = Assert.IsType<ApiSingleObjectResponse<Product>>(okResult.Value);
            Assert.Equal("Product updated successfully", response.Message);
            Assert.Equal(updatedProduct, response.Data);
        }

        [Theory, AutoData]
        public async Task PatchProduct_WhenProductDoesNotExist_ReturnsBadRequest(int id)
        {
            // Arrange
            _fixture.Customize<ProductDto>(c => c
                .Without(p => p.ProductImage)
            );
            var productDto = _fixture.Create<ProductDto>();

            _productRepositoryMock.Setup(repo => repo.IsProductExistAsync(id)).ReturnsAsync(false);

            // Act
            var result = await _controller.PatchProduct(id, productDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequest<string>>(result.Result);
            Assert.Contains("Product not found", badRequestResult.Value);
        }

        [Theory, AutoData]
        public async Task PostProduct_WhenValidationPasses_ReturnsOk(Product product)
        {
            // Arrange
            _fixture.Customize<ProductDto>(c => c
                .Without(p => p.ProductImage)
            );
            var productDto = _fixture.Create<ProductDto>();
            var validationResult = new ValidationResult();
            _productValidatorMock.Setup(v => v.Validate(productDto)).Returns(validationResult);
            _productRepositoryMock.Setup(repo => repo.CreateProductAsync(productDto, It.IsAny<IFormFileCollection>())).ReturnsAsync(product);

            // Act
            var result = await _controller.PostProduct(productDto, null);

            // Assert
            var okResult = Assert.IsType<Ok<ApiSingleObjectResponse<Product>>>(result.Result);
            var response = Assert.IsType<ApiSingleObjectResponse<Product>>(okResult.Value);
            Assert.Equal("Product posted successfully", response.Message);
            Assert.Equal(product, response.Data);
        }

        [Theory, AutoData]
        public async Task PostProduct_WhenValidationFails_ReturnsBadRequest(List<KeyValuePair<string, string[]>> errors)
        {
            // Arrange
            _fixture.Customize<ProductDto>(c => c
                .Without(p => p.ProductImage)
            );
            var productDto = _fixture.Create<ProductDto>();

            var validationResult = new ValidationResult(errors.Select(e => new ValidationFailure(e.Key, e.Value.First())).ToList());
            _productValidatorMock.Setup(v => v.Validate(productDto)).Returns(validationResult);

            // Act
            var result = await _controller.PostProduct(productDto, null);

            // Assert
            var badRequestResult = Assert.IsType<BadRequest<List<KeyValuePair<string, string[]>>>>(result.Result);
            var response = Assert.IsType<List<KeyValuePair<string, string[]>>>(badRequestResult.Value);
            Assert.Equal(errors.Select(e => e.Value[0]), response.Select(e => e.Value[0]));
        }

        [Theory, AutoData]
        public async Task DeleteProduct_WhenProductExists_ReturnsOk(int id)
        {
            // Arrange
            _productRepositoryMock.Setup(repo => repo.IsProductExistAsync(id)).ReturnsAsync(true);
            _productRepositoryMock.Setup(repo => repo.DeleteProductAsync(id)).ReturnsAsync(true);

            // Act
            var result = await _controller.DeleteProduct(id);
            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);

            Assert.Contains("Product deleted successfully", okResult.Value.ToString());
        }

        [Theory, AutoData]
        public async Task DeleteProduct_WhenProductDoesNotExist_ReturnsNotFound(int id)
        {
            // Arrange
            _productRepositoryMock.Setup(repo => repo.IsProductExistAsync(id)).ReturnsAsync(false);

            // Act
            var result = await _controller.DeleteProduct(id);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Product not found with the specified ID.", notFoundResult.Value.ToString());
        }
    }
}
