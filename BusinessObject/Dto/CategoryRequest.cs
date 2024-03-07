using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Dto
{
    public class CategoryRequest
    {
        public string CategoryName { get; set; } = null!;

        public string? CategoryDescription { get; set; }

        public string? CategoryNote { get; set; }
    }

    public class CategoryValidator : AbstractValidator<CategoryRequest>
    {
        public CategoryValidator()
        {
            RuleFor(category => category.CategoryName)
                .NotEmpty().WithMessage("Category Name is required.")
                .MaximumLength(40).WithMessage("Category Name cannot exceed 40 characters.");

            RuleFor(category => category.CategoryDescription)
                .MaximumLength(150).WithMessage("Category Description cannot exceed 150 characters.");

            RuleFor(category => category.CategoryNote)
                .MaximumLength(150).WithMessage("Category Note cannot exceed 150 characters.");

            // You can add more validation rules as needed
        }
    }
}
