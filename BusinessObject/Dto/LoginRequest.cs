using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Dto
{
    public class LoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }


    public class LoginRequestValidator : AbstractValidator<LoginRequest>
    {
        public LoginRequestValidator()
        {
            RuleFor(x => x.Username).NotEmpty().WithMessage("Username is required");
            RuleFor(x => x.Password)
                    .MinimumLength(8).WithMessage("Password must be at least 8 characters")
                    .NotEmpty().WithMessage("Password is required");
        }
    }
}
