﻿using RookieEShopper.SharedLibrary.ViewModels;

namespace RookieEShopper.Application.Dto.Product
{
    public class ResponseDomainProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; } = 0;
        public int NumOfProduct { get; set; }
        public string MainImagePath { get; set; } = string.Empty;
        public CategoryVM Category { get; set; } = new CategoryVM();

        public IList<string> ImageGallery { get; set; } = new List<string>();
        public IList<int> AppliableCoupons { get; set; } = new List<int>();
        public ICollection<int> ProductReviews { get; set; } = new List<int>();
    }
}