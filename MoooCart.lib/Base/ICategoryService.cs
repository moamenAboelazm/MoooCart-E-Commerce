using MoooCart.lib.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoooCart.lib.Base
{
    public interface ICategoryService
    {
        Task<IEnumerable<DtoGetCategory>> GetAllAsync();

        Task<DtoGetCategory> GetByIDAsync(Guid Id);
        Task<DtoResponse> AddAsync(DtoCategory entity);

        Task<DtoResponse> UpdateAsync(DtoUpdateCategory entity);

        Task<DtoResponse> DeleteAsync(Guid id);
    }
}
