using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RookieEShopper.Application.Dto.Customer;
using RookieEShopper.Application.Dto.Review;
using RookieEShopper.Application.Repositories;
using RookieEShopper.Domain.Data.Entities;
using RookieEShopper.SharedViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RookieEShopper.Infrastructure.Persistent.Repositories
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _context;
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productReposity;
        private readonly ICustomerRepository _customerRepository;
        public ReviewRepository(IMapper mapper, ApplicationDbContext context, IOrderRepository orderRepository,
                                IProductRepository productReposity, ICustomerRepository customerRepository) 
        {
            _mapper = mapper;
            _context = context;
            _orderRepository = orderRepository;
            _productReposity = productReposity;
            _customerRepository = customerRepository;
        }
        public async Task<ResponseProductReviewDto?> CreateReviewAsync(CreateProductReviewDto productReviewDto)
        {
            ProductReview productReview = new ProductReview();
            _mapper.Map(productReviewDto, productReview);           

            if (productReviewDto.ProductId != 0)            
                productReview.Product = 
                    await _productReposity.GetDomainProductByIdAsync(productReviewDto.CustomerId);
            
            if (productReviewDto.CustomerId != 0)
                productReview.Customer = 
                    await _context.Customers.FindAsync(productReviewDto.CustomerId);
            else
                productReview.Customer =
                    await _context.Customers.FindAsync(1);

            var entityEntry= await _context.ProductReviews.AddAsync(productReview);

            await _context.SaveChangesAsync();           

            throw new Exception();
        }

        public async Task<ICollection<ResponseProductReviewDto>?> GetAllReviewsAsync()
        {
            var productReviews = await _context.ProductReviews
                .Include(r => r.Customer)
                .Include(r => r.Product)
                .ToListAsync();

            ICollection<ResponseProductReviewDto> response = new HashSet<ResponseProductReviewDto>();

            foreach (var productReview in productReviews)
            {
                response.Add(_mapper.Map(productReview
                    , new ResponseProductReviewDto()
                    {
                        Product = await _productReposity.GetProductByIdAsync((int) productReview.ProductId),
                        Customer = await _customerRepository.GetCustomerByIdAsync((int) productReview.CustomerId)
                    }));
            }            

            return response;
        }

        public async Task<ResponseProductReviewDto?> GetReviewByIdAsync(int id)
        {
            var productReview = await _context.ProductReviews
                .Include(r => r.Customer)
                .Include(r => r.Product)
                .Where(r => r.Id == id)
                .FirstOrDefaultAsync();

            ResponseProductReviewDto response = new ResponseProductReviewDto();

            _mapper.Map(productReview
                , new ResponseProductReviewDto()
                {
                    Product = await _productReposity.GetProductByIdAsync((int)productReview.ProductId),
                    Customer = await _customerRepository.GetCustomerByIdAsync((int)productReview.CustomerId)
                });

            return response;
        }

        public async Task<ICollection<ResponseProductReviewDto>?> GetReviewsByCustomerAsync(int customerId)
        {
            var productReviews = await _context.ProductReviews
                .Include(r => r.Customer)
                .Include(r => r.Product)
                .Where(r => r.CustomerId == customerId)
                .ToListAsync();

            ICollection<ResponseProductReviewDto> response = new HashSet<ResponseProductReviewDto>();

            foreach (var productReview in productReviews)
            {
                response.Add(_mapper.Map(productReview
                    , new ResponseProductReviewDto()
                    {
                        Product = await _productReposity.GetProductByIdAsync((int)productReview.ProductId),
                        Customer = await _customerRepository.GetCustomerByIdAsync((int)productReview.CustomerId)
                    }));
            }

            return response;
        }

        public async Task<ICollection<ResponseProductReviewDto>?> GetReviewsByProductAsync(int productId)
        {
            var productReviews = await _context.ProductReviews
                .Include(r => r.Customer)
                .Include(r => r.Product)
                .Where(r => r.ProductId == productId)
                .ToListAsync();

            ICollection<ResponseProductReviewDto> response = new HashSet<ResponseProductReviewDto>();

            foreach (var productReview in productReviews)
            {
                response.Add(_mapper.Map(productReview
                    , new ResponseProductReviewDto()
                    {
                        Product = await _productReposity.GetProductByIdAsync((int)productReview.ProductId),
                        Customer = await _customerRepository.GetCustomerByIdAsync((int)productReview.CustomerId)
                    }));
            }

            return response;
        }

    }
}
