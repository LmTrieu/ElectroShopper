﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RookieEShopper.Application.Dto.Review;
using RookieEShopper.Application.Repositories;
using RookieEShopper.Domain.Data.Entities;

namespace RookieEShopper.Infrastructure.Persistent.Repositories
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _context;
        private readonly IProductRepository _productReposity;
        private readonly ICustomerRepository _customerRepository;

        public ReviewRepository(IMapper mapper, ApplicationDbContext context,
                                IProductRepository productReposity, ICustomerRepository customerRepository)
        {
            _mapper = mapper;
            _context = context;
            _productReposity = productReposity;
            _customerRepository = customerRepository;
        }

        public async Task<ResponseProductReviewDto?> CreateReviewAsync(CreateProductReviewDto productReviewDto)
        {
            ProductReview productReview = new ProductReview();
            _mapper.Map(productReviewDto, productReview);

            if (productReviewDto.ProductId != 0)
                productReview.ProductId = (productReviewDto.ProductId);

            if (productReviewDto.CustomerId != Guid.Empty)
                productReview.Customer =
                    await _context.Customers.FindAsync(productReviewDto.CustomerId);
            else
            {
                //Anonymous user
                productReview.Customer =
                    await _context.Customers.FindAsync(1);
            }

            var entityEntry = await _context.ProductReviews.AddAsync(productReview);

            await _context.SaveChangesAsync();

            ResponseProductReviewDto response = _mapper.Map<ResponseProductReviewDto>(productReview);

            response.Customer = await _customerRepository.GetCustomerByIdAsync(entityEntry.Entity.CustomerId);

            return response;
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
                        Customer = await _customerRepository.GetCustomerByIdAsync(productReview.CustomerId)
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

            ResponseProductReviewDto response = new();

            response = _mapper.Map<ResponseProductReviewDto>(productReview);

            response.Customer = await _customerRepository.GetCustomerByIdAsync(productReview.CustomerId);

            return response;
        }

        public async Task<ICollection<ResponseProductReviewDto>?> GetReviewsByCustomerAsync(Guid customerId)
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
                        Customer = await _customerRepository.GetCustomerByIdAsync(productReview.CustomerId)
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
                        Customer = await _customerRepository.GetCustomerByIdAsync(productReview.CustomerId)
                    }));
            }

            return response;
        }
    }
}