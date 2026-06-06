using MoooCart.lib.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoooCart.lib.Base
{
    public interface IProductService
    {
        Task<DtoPagedResponse<DtoGetProducts>> GetAllAsync(DtoProductParams productParams);
        Task<DtoGetProducts> GetByIDAsync(Guid Id);
        Task<DtoResponse> AddAsync(DtoProduct entity);

        Task<DtoResponse> UpdateAsync(DtoUpdateProduct entity);

        Task<DtoResponse> DeleteAsync(Guid id);
    }
}
