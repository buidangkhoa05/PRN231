using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace BusinessObject.Dto
{
    public class SupplierRequest
    {
        public string? SupplierName { get; set; }

        public string? SupplierAddress { get; set; }

        public string? Telephone { get; set; }
    }

    public class SupplierValidator : AbstractValidator<SupplierRequest>
    {
        public SupplierValidator()
        {
            RuleFor(supplier => supplier.SupplierName)
                .NotEmpty().WithMessage("Supplier Name is required.")
                .MaximumLength(50).WithMessage("Supplier Name cannot exceed 50 characters.");

            RuleFor(supplier => supplier.SupplierAddress)
                .MaximumLength(150).WithMessage("Supplier Address cannot exceed 150 characters.");

            RuleFor(supplier => supplier.Telephone)
                .MaximumLength(15).WithMessage("Telephone cannot exceed 15 characters.");

            // You can add more validation rules as needed
        }
    }
}
