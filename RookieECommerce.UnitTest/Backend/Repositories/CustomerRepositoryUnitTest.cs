﻿using AutoFixture;
using AutoFixture.Xunit2;
using Moq;
using RookieEShopper.Application.Dto.Customer;
using RookieEShopper.Application.Repositories;
using RookieEShopper.Application.Service;
using RookieEShopper.Domain.Data.Entities;
using RookieEShopper.SharedLibrary.HelperClasses;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace RookieECommerce.UnitTest.Backend.Repositories
{
    public class CustomerRepositoryUnitTest : BaseUnitTest
    {
        private readonly Mock<ICustomerRepository> customerRepository;

        public CustomerRepositoryUnitTest()
        {
            customerRepository = _fixture.Freeze<Mock<ICustomerRepository>>();
        }

        [Fact]
        public async Task GetCustomerByIdAsync_WhenFound_ReturnsOneCustomer()
        {
            // Arrange
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            var user = _fixture.Create<BaseApplicationUser>();

            ResponseCustomerDto responseCustomerDtos = new()
            {
                Id = user.Customer.Id,
                Email = user.Email,
                Name = user.UserName
            };

            customerRepository.Setup(repo => repo.GetCustomerByIdAsync(user.Customer.Id))
                .ReturnsAsync(responseCustomerDtos);
            
            // Act
            var result = await customerRepository.Object.GetCustomerByIdAsync(user.Customer.Id);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<ResponseCustomerDto>(result);

            Assert.Equal(result, responseCustomerDtos);
            Assert.Equal(result.Id, responseCustomerDtos.Id);
            Assert.Equal(result.Name, responseCustomerDtos.Name);
            Assert.Equal(result.Email, responseCustomerDtos.Email);

            customerRepository.Verify(repo => repo.GetCustomerByIdAsync(user.Customer.Id), Times.Once);
            _fixture.Behaviors.Remove(new OmitOnRecursionBehavior());
        }

        [Theory]
        [AutoData]
        public async Task GetAllCustomerAsync_ReturnsCustomerPagedList(            
            QueryParameters query)
        {
            // Arrange
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            var users = _fixture.CreateMany<BaseApplicationUser>();

            List<ResponseCustomerDto> responseCustomerDtos = new List<ResponseCustomerDto>();

            users.ToList().ForEach(c => responseCustomerDtos.Add(new ResponseCustomerDto
            {
                Id = c.Customer.Id,
                Email = c.Email,
                Name = c.UserName
            }));

            var expectedCustomers = new PagedList<ResponseCustomerDto>(responseCustomerDtos, responseCustomerDtos.Count,query.PageNumber,query.PageSize);

            customerRepository.Setup(cr => cr.GetAllCustomerAsync(query))
                .ReturnsAsync(expectedCustomers);

            // Act
            var result = await customerRepository.Object.GetAllCustomerAsync(query);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<PagedList<ResponseCustomerDto>>(result);

            Assert.Equal(expectedCustomers, result);
            Assert.Equal(expectedCustomers.Count, result.Count);
            Assert.Equal(expectedCustomers.PageSize, result.PageSize);
            Assert.Equal(expectedCustomers.CurrentPage, result.CurrentPage);
            Assert.Equal(expectedCustomers.TotalCount, result.TotalCount);
            Assert.Equal(expectedCustomers.TotalPages, result.TotalPages);

            Assert.Equal(expectedCustomers.FirstOrDefault(), result.FirstOrDefault());
            Assert.Equal(expectedCustomers.LastOrDefault(), result.LastOrDefault());

            Assert.Equal(users.FirstOrDefault().Customer.Id, result.FirstOrDefault().Id);
            Assert.Equal(users.FirstOrDefault().Email, result.FirstOrDefault().Email);
            Assert.Equal(users.FirstOrDefault().UserName, result.FirstOrDefault().Name);


            customerRepository.Verify(cr => cr.GetAllCustomerAsync(query), Times.Once);
            _fixture.Behaviors.Remove(new OmitOnRecursionBehavior());
        }

        [Fact]
        public async Task GetCustomerByIdAsync_WhenFound_ReturnsExpectedCustomerDto()            
        {
            // Arrange
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            var user = _fixture.Create<BaseApplicationUser>();
            var expectedCustomer = new ResponseCustomerDto
            {
                Id = user.Customer.Id,
                Email = user.Email,
                Name = user.UserName
            };

            customerRepository.Setup(repo => repo.GetCustomerByIdAsync(user.Customer.Id))
                .ReturnsAsync(expectedCustomer);

            // Act
            var result = await customerRepository.Object.GetCustomerByIdAsync(user.Customer.Id);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<ResponseCustomerDto>(result);

            Assert.Equal(expectedCustomer, result);
            customerRepository.Verify(repo => repo.GetCustomerByIdAsync(user.Customer.Id), Times.Once);
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        }

        [Theory]
        [AutoData]
        public async Task GetCustomerByIdAsync_WhenNotFound_ReturnsNull(
            Guid id)
        {
            // Arrange
            customerRepository.Setup(repo => repo.GetCustomerByIdAsync(id))
                .ReturnsAsync(() => null);

            // Act
            var result = await customerRepository.Object.GetCustomerByIdAsync(id);

            // Assert
            Assert.Null(result);
            customerRepository.Verify(repo => repo.GetCustomerByIdAsync(id), Times.Once);
        }
    }
}
