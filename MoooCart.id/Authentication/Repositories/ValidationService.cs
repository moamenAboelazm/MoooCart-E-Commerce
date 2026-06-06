using FluentValidation;
using MoooCart.id.Authentication.Base;
using MoooCart.id.Validation;
using MoooCart.lib.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoooCart.id.Authentication.Repositories
{
    public class ValidationService : IValidationService
    {
        public async Task<DtoResponse> ValidationAsync<T>(T model, IValidator<T> validator)
        {
            var _validator = await validator.ValidateAsync(model);
            if (!_validator.IsValid)
            {
                var errors = _validator.Errors.Select(e => e.ErrorMessage).ToList();
                string errorsToString = string.Join(", ", errors);
                return new DtoResponse { msg = errorsToString };
            }
            return new DtoResponse { Success = true };
        }
    }
}
