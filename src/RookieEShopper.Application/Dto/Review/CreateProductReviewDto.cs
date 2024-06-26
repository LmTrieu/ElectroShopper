﻿using FluentValidation;

namespace RookieEShopper.Application.Dto.Review
{
    public class CreateProductReviewDto
    {
        public int ProductId { get; set; }
        public Guid CustomerId { get; set; }
        public string Feedback { get; set; } = string.Empty;
        public int Rating { get; set; }
    }

    public class CreateProductReviewDtoValidator : AbstractValidator<CreateProductReviewDto>
    {
        public CreateProductReviewDtoValidator()
        {
            RuleFor(x => x.ProductId).NotEmpty().GreaterThan(0);
            RuleFor(x => x.Rating).InclusiveBetween(1, 5);
            RuleFor(x => x.Feedback).MaximumLength(200);
        }
    }
}