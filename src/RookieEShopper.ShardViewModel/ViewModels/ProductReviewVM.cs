﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RookieEShopper.SharedLibrary.ViewModels
{
    public class ProductReviewVM
    {
        public int Id { get; set; }
        public string Feedback { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public int Rating { get; set; }
    }
}