﻿namespace RookieEShopper.SharedViewModel
{
    public class ProductVM
    {
        public int id { get; set; }
        public string name { get; set; }
        public decimal price { get; set; }
        public string imagePath { get; set; } = string.Empty;
        public CategoryVM category { get; set; } = new CategoryVM();
    }
}