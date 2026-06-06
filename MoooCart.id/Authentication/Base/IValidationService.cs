using FluentValidation;
using MoooCart.lib.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoooCart.id.Authentication.Base
{
    public interface IValidationService
    {
        Task<DtoResponse> ValidationAsync<T>(T model, IValidator<T> validator);
    }
}
