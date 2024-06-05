using AutoFixture;
using AutoFixture.Xunit2;
using Moq;
using RookieEShopper.Application.Dto.Category;
using RookieEShopper.Application.Repositories;
using RookieEShopper.Application.Service;
using RookieEShopper.Domain.Data.Entities;
using RookieEShopper.SharedLibrary.HelperClasses;
using RookieEShopper.SharedLibrary.ViewModels;

namespace RookieECommerce.UnitTest.Backend.Repositories
{
    public class CategoryRepositoryUnitTest : BaseUnitTest
    {
        [Theory]
        [AutoData]
        public async Task GetCategoryByIdAsync_WhenFound_ReturnCategory(
            Category category)
        {
            //Arrange
            var categoryRepository = _fixture.Create<Mock<ICategoryRepository>>();
            categoryRepository.Setup(cr => cr.GetCategoryByIdAsync(category.Id)).ReturnsAsync(category);

            //Act
            var result = await categoryRepository.Object.GetCategoryByIdAsync(category.Id);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<Category>(result);

            Assert.Equal(category, result); 
            Assert.Equal(category.Id,result.Id);
            Assert.Equal(category.Name, result.Name);
            Assert.Equal(category.Description, result.Description);
            categoryRepository.Verify(cr => cr.GetCategoryByIdAsync(category.Id), Times.Once);
        }

        [Fact]
        public async Task GetAllCategoriesAsync_OnFirstPageSizeTen_ReturnsCategories(
            )
        {
            // Arrange
            var query = new QueryParameters
            {
                PageNumber = 1,
                PageSize = 10
            };

            var categories = _fixture.Create<PagedList<ResponseCategoryDto>>();

            var categoryRepositoryMock = new Mock<ICategoryRepository>();
            categoryRepositoryMock.Setup(cr => cr.GetAllCategoriesAsync(query))
                .ReturnsAsync(categories);

            // Act
            var result = await categoryRepositoryMock.Object.GetAllCategoriesAsync(query);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<PagedList<ResponseCategoryDto>>(result);

            Assert.Equal(categories, result);
            Assert.Equal(categories.Count, result.Count);
            Assert.Equal(categories.PageSize, result.PageSize);
            Assert.Equal(categories.CurrentPage, result.CurrentPage);
            Assert.Equal(categories.TotalCount, result.TotalCount);
            Assert.Equal(categories.TotalPages, result.TotalPages);

            Assert.Equal(categories.FirstOrDefault(), result.FirstOrDefault());
            Assert.Equal(categories.LastOrDefault(), result.LastOrDefault());

            categoryRepositoryMock.Verify(cr => cr.GetAllCategoriesAsync(query), Times.Once);
        }


        [Theory]
        [AutoData]
        public async Task CreateCategoryAsync_WhenFound_ReturnCategory(
            CategoryDto categoryDto)
        {
            //Arrange
            var category = new Category
            {
                Description = categoryDto.Description,
                Name = categoryDto.Name
            };
            var categoryRepository = _fixture.Create<Mock<ICategoryRepository>>();
            categoryRepository.Setup(cr => cr.CreateCategoryAsync(categoryDto))
                .ReturnsAsync(category);

            //Act
            var result = await categoryRepository.Object.CreateCategoryAsync(categoryDto);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<Category>(result);

            Assert.Equal(categoryDto.Name, result.Name);
            Assert.Equal(categoryDto.Description, result.Description);

            categoryRepository.Verify(cr => cr.CreateCategoryAsync(categoryDto), Times.Once);
        }

        [Theory]
        [AutoData]
        public async Task UpdateCategoryAsync_WhenFound_ReturnCategory(
            CategoryDto categoryDto,
            Category category)
        {
            //Arrange
            var updatedCategory = new Category
            {
                Id = category.Id,
                Name = categoryDto.Name,
                Description = categoryDto.Description
            };

            var categoryRepository = _fixture.Create<Mock<ICategoryRepository>>();
            categoryRepository.Setup(cr => cr.UpdateCategoryAsync(category.Id, categoryDto))
                .ReturnsAsync(updatedCategory);

            //Act
            var result = await categoryRepository.Object.UpdateCategoryAsync(category.Id, categoryDto);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<Category>(result);
            Assert.Equal(updatedCategory.Name, result.Name);
            Assert.Equal(updatedCategory.Description, result.Description);

            categoryRepository.Verify(cr => cr.UpdateCategoryAsync(category.Id, categoryDto), Times.Once);
        }
        
        [Theory]
        [AutoData]
        public async Task DeleteCategoryAsync_WhenFound_ReturnCategory(
            Category category)
        {
            //Arrange
            var categoryRepository = _fixture.Create<Mock<ICategoryRepository>>();
            categoryRepository.Setup(cr => cr.DeleteCategoryAsync(category.Id))
                .ReturnsAsync(true);

            //Act
            var result = await categoryRepository.Object.DeleteCategoryAsync(category.Id);

            //Assert 
            Assert.True(result);
            categoryRepository.Verify(cr => cr.DeleteCategoryAsync(category.Id), Times.Once);
        }
    }
}
