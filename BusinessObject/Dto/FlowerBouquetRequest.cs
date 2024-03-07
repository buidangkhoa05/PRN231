using Microsoft.EntityFrameworkCore;
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
    public class FlowerBouquetRequest
    {

        public int CategoryId { get; set; }

        [StringLength(40)]
        public string FlowerBouquetName { get; set; } = null!;

        [StringLength(220)]
        public string Description { get; set; } = null!;

        [Column(TypeName = "money")]
        public decimal UnitPrice { get; set; }

        public int UnitsInStock { get; set; }

        public byte? FlowerBouquetStatus { get; set; }

        public int? SupplierId { get; set; }

        public string? Morphology { get; set; }
    }

    public class FlowerBouquetValidator : AbstractValidator<FlowerBouquetRequest>
    {
        public FlowerBouquetValidator()
        {
            RuleFor(bouquet => bouquet.FlowerBouquetName)
                .NotEmpty().WithMessage("Flower Bouquet Name is required.")
                .MaximumLength(40).WithMessage("Flower Bouquet Name cannot exceed 40 characters.");

            RuleFor(bouquet => bouquet.Description)
                .NotEmpty().WithMessage("Description is required.")
                .MaximumLength(220).WithMessage("Description cannot exceed 220 characters.");

            RuleFor(bouquet => bouquet.UnitPrice)
                .GreaterThan(0).WithMessage("Unit Price must be greater than 0.");

            RuleFor(bouquet => bouquet.UnitsInStock)
                .GreaterThanOrEqualTo(0).WithMessage("Units In Stock must be greater than or equal to 0.");

            RuleFor(bouquet => bouquet.FlowerBouquetStatus)
                .InclusiveBetween((byte)0, (byte)1).WithMessage("Invalid Flower Bouquet Status value.");

            RuleFor(bouquet => bouquet.Morphology)
                .MaximumLength(250).WithMessage("Morphology cannot exceed 250 characters.");

            RuleFor(bouquet => bouquet.CategoryId)
                .GreaterThan(0).WithMessage("Category ID must be greater than 0.");

            RuleFor(bouquet => bouquet.SupplierId)
                .GreaterThan(0).When(bouquet => bouquet.SupplierId.HasValue)
                .WithMessage("Supplier ID must be greater than 0 when provided.");
        }
    }
}
