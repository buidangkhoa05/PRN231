using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BusinessObject.Dto
{
    public class AccountReq
    {
        public string? Email { get; set; }

        public string Username { get; set; } = null!;

        public string City { get; set; } = null!;

        public string Country { get; set; } = null!;

        public string Password { get; set; } = null!;

        public DateOnly? Birthday { get; set; }

        public byte? AccountStatus { get; set; }

        public string? Fullname { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public AccountReqRole Role { get; set; }
    }

    public enum AccountReqRole
    {
        Staff,
        User
    }

    public class AccountValidator : AbstractValidator<AccountReq>
    {
        public AccountValidator()
        {
            RuleFor(account => account.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email address.");

            RuleFor(account => account.Username)
                .NotEmpty().WithMessage("Username is required.")
                .MaximumLength(180).WithMessage("Username cannot exceed 180 characters.");

            RuleFor(account => account.City)
                .NotEmpty().WithMessage("City is required.")
                .MaximumLength(15).WithMessage("City cannot exceed 15 characters.");

            RuleFor(account => account.Country)
                .NotEmpty().WithMessage("Country is required.")
                .MaximumLength(15).WithMessage("Country cannot exceed 15 characters.");

            RuleFor(account => account.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MaximumLength(30).WithMessage("Password cannot exceed 30 characters.");

            RuleFor(account => account.Birthday)
                .NotNull().WithMessage("Birthday is required.");

            RuleFor(account => account.AccountStatus)
                .InclusiveBetween((byte)0, (byte)1).WithMessage("Invalid AccountStatus value.");

            RuleFor(account => account.Fullname)
                .MaximumLength(100).WithMessage("Fullname cannot exceed 100 characters.");
        }
    }
}
