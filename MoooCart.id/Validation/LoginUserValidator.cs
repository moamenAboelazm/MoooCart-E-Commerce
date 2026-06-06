using FluentValidation;
using MoooCart.lib.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoooCart.id.Validation
{
    public class LoginUserValidator : AbstractValidator<DtoLoginUser>
    {
        public LoginUserValidator()
        {
            RuleFor(x => x.Email).NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Invalid Email format");

            RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required");

        }
    }
}
