﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RookieEShopper.Domain.Data.Entities;
using RookieEShopper.Application.Repositories;
using RookieEShopper.Application.Dto;
using Microsoft.AspNetCore.Http;

namespace RookieEShopper.Infrastructure.Persistent.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly ICategoryRepository _categoryRepository;

        public ProductRepository(ApplicationDbContext context, IMapper mapper, ICategoryRepository categoryRepository)
        {
            _context = context;
            _mapper = mapper;
            _categoryRepository = categoryRepository;
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return await _context.Products
                .Include(p => p.Category)
                .ToListAsync();
        }

        public async Task<Product?> GetProductByIdAsync(int productId)
        {
            var product = await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Id == productId);
            return product;
        }

        public async Task<List<Product>> GetProductByNameAsync(string productName)
        {
            var product = await _context.Products
                .Include(p => p.Category)
                .Where(p => p.Name.Contains(productName))
                .ToListAsync();
            return product;
        }
        public async Task<Product?> CreateProductAsync(ProductRequestBodyDto productdto)
        {
            var product = _mapper.Map<ProductRequestBodyDto, Product>(productdto);
            product.Category = await _categoryRepository.GetCategoryByIdAsync(productdto.CategoryId);

            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();

            return product;
        }

        public async Task<Product?> UpdateProductAsync(int id, ProductRequestBodyDto productdto)
        {
            Product? result = await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Id == id);

            _mapper.Map(productdto, result);

            await _context.SaveChangesAsync();
            return result;
        }

        public async Task<bool> IsProductExist(int id)
        {
            return await _context.Products.AnyAsync(e => e.Id == id);
        }

        public async Task<bool> DeleteProductAsync(Product product)
        {
            try
            {
                _context.Products.Remove(product);
                return await _context.SaveChangesAsync() is 1;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UploadProductImage(IFormFile image)
        {
            string path = image.FileName + "_" + Guid.NewGuid().ToString();
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            using (var stream = File.Create(path))
            {
                await image.CopyToAsync(stream);
            }
            throw new NotImplementedException();
        }
    }
}
