using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RookieEShopper.Application.Dto.Customer;
using RookieEShopper.Application.Dto.Product;
using RookieEShopper.Application.Repositories;
using RookieEShopper.Application.Service;
using RookieEShopper.Domain.Data.Entities;
using RookieEShopper.SharedLibrary.HelperClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RookieEShopper.Infrastructure.Persistent.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CustomerRepository(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public Task<ResponseCustomerDto> CreateCustomerAsync(CreateCustomerDto customer)
        {
            throw new NotImplementedException();
        }

        public async Task<PagedList<ResponseCustomerDto>> GetAllCustomerAsync(QueryParameters query)
        {
            var result = _context.BaseApplicationUsers
                .Include(u => u.Customer)
                .Select(c => new ResponseCustomerDto
                {
                    Id = c.Customer.Id,
                    Email = c.Email,
                    Name = c.UserName
                });

            return await PagedList<ResponseCustomerDto>.ToPagedList(result,
                    query.PageNumber,
                    query.PageSize);
        }

        public async Task<Customer?> GetDomainCustomerByIdAsync(int id)
        {
            return await _context.Customers
                .FindAsync(id);
        }

        public Task<ResponseCustomerDto> GetCustomerByNameAsync(string name)
        {
            throw new NotImplementedException();
        }

        public Task IsCustomerExistAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task LockCustomerAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<ResponseCustomerDto?> GetCustomerByIdAsync(Guid id)
        {
            return await _context.BaseApplicationUsers
                .Where(u => u.Customer.Id == id)
                .Select(c =>
                    new ResponseCustomerDto
                    {
                        Id = c.Customer.Id,
                        Email = c.Email,
                        Name = c.UserName
                    })
                .FirstOrDefaultAsync();
        }
    }
}