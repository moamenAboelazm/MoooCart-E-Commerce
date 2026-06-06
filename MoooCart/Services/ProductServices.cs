using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MoooCart.DB.Base;
using MoooCart.DB.Models;
using MoooCart.lib.Base;
using MoooCart.lib.DTOs;


namespace MoooCart.lib.Services
{
    class ProductServices(IGenericRepository<ClsProduct> product , IMapper _mapper) : IProductService
    {


        public async Task<DtoPagedResponse<DtoGetProducts>> GetAllAsync(DtoProductParams productParams)
        {
            var query = product.GetQueryable().Include(p => p.Category).AsQueryable();

            if (!string.IsNullOrEmpty(productParams.Search))
            {
                var lowerSearch = productParams.Search.ToLower();
                query = query.Where(p => p.Name.ToLower().Contains(lowerSearch));
            }

            if (productParams.CategoryId.HasValue)
            {
                query = query.Where(p => p.CategoryID == productParams.CategoryId.Value);
            }

            if (!string.IsNullOrEmpty(productParams.Sort))
            {
                query = productParams.Sort switch
                {
                    "priceAsc" => query.OrderBy(p => p.Price),
                    "priceDesc" => query.OrderByDescending(p => p.Price),
                    "nameDesc" => query.OrderByDescending(p => p.Name),
                    _ => query.OrderBy(p => p.Name)
                };
            }

            var totalCount = await query.CountAsync();

            var products = await query
                .Skip((productParams.Page - 1) * productParams.PageSize)
                .Take(productParams.PageSize)
                .ToListAsync();

            var mappedData = _mapper.Map<List<DtoGetProducts>>(products);

            return new DtoPagedResponse<DtoGetProducts>
            {
                Page = productParams.Page,
                PageSize = productParams.PageSize,
                TotalCount = totalCount,
                Data = mappedData
            };
        }

        public async Task<DtoResponse> AddAsync(DtoProduct entity)
        {
            try
            {
                var mapdata = _mapper.Map<ClsProduct>(entity);
                int result = await product.AddAsync(mapdata);
                if (result > 0) return new DtoResponse(true, "Successed (:");
            }
            catch (Exception ex)
            {
                return new DtoResponse(false, ex.Message);
            }
            return new DtoResponse(false, "Can't Save , Please check your service");

        }
        public async Task<DtoResponse> DeleteAsync(Guid id)
        {
            try
            {
                int result = await product.DeleteAsync(id);
                if (result > 0) return new DtoResponse(true, "Successed (:");
            }
            catch (Exception ex)
            {
                return new DtoResponse(false, ex.Message);
            }
            return new DtoResponse(false, "Can't delete , Please check your service");
        }
        public async Task<DtoGetProducts> GetByIDAsync(Guid id)
        {
            try
            {
                var data = await product.GetWithIncludesAsync(p => p.ID == id, "Category");

                if (data == null) return new DtoGetProducts();

                return _mapper.Map<DtoGetProducts>(data);
            }
            catch
            {
                return new DtoGetProducts();
            }
        }
        public async Task<DtoResponse> UpdateAsync(DtoUpdateProduct entity)
        {
            try
            {
                var existingProduct = await product.GetByIDAsync(Guid.Parse(entity.ID));

                if (existingProduct == null)
                    return new DtoResponse(false, "Product not found!");

                _mapper.Map(entity, existingProduct);

                int result = await product.UpdateAsync(existingProduct);

                if (result > 0)
                    return new DtoResponse(true, "Successed (:");
            }
            catch (Exception ex)
            {
                return new DtoResponse(false, ex.Message);
            }
            return new DtoResponse(false, "Can't Save , Please check your service");
        }
    }
}
