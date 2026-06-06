using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
using MoooCart.DB.Base;
using MoooCart.DB.Models;
using MoooCart.lib.Base;
using MoooCart.lib.DTOs;

namespace MoooCart.Services
{
    public class CategoryServices(IGenericRepository<ClsCategory> category, IMapper mapper, IMemoryCache cache) : ICategoryService
    {
        private const string CacheKey = "AllCategoriesCache";

        public async Task<DtoResponse> AddAsync(DtoCategory entity)
        {
            try
            {
                var mappData = mapper.Map<ClsCategory>(entity);
                int result = await category.AddAsync(mappData);

                if (result > 0)
                {
                    cache.Remove(CacheKey);
                    return new DtoResponse(true, "Success !");
                }
            }
            catch (Exception ex)
            {
                return new DtoResponse(false, ex.Message);
            }
            return new DtoResponse(false, "Please check your server, Can't save new ?!");
        }

        public async Task<DtoResponse> DeleteAsync(Guid id)
        {
            try
            {
                int result = await category.DeleteAsync(id);

                if (result > 0)
                {
                    cache.Remove(CacheKey);
                    return new DtoResponse(true, "Success !");
                }
            }
            catch (Exception ex)
            {
                return new DtoResponse(false, ex.Message);
            }
            return new DtoResponse(false, "Please check your server, Can't delete ?!");
        }

        public async Task<IEnumerable<DtoGetCategory>> GetAllAsync()
        {
            try
            {
                if (cache.TryGetValue(CacheKey, out IEnumerable<DtoGetCategory>? cachedData))
                {
                    return cachedData!;
                }

                var data = await category.GetAllAsync("Products");

                if (data == null || !data.Any()) return [];

                var mappedCategories = mapper.Map<IEnumerable<DtoGetCategory>>(data);

                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromHours(2))
                    .SetSlidingExpiration(TimeSpan.FromMinutes(30));

                cache.Set(CacheKey, mappedCategories, cacheOptions);

                return mappedCategories;
            }
            catch
            {
                throw;
            }
        }

        public async Task<DtoGetCategory> GetByIDAsync(Guid id)
        {
            try
            {
                var data = await category.GetWithIncludesAsync(c => c.ID == id, "Products");

                if (data == null) return new DtoGetCategory();

                return mapper.Map<DtoGetCategory>(data);
            }
            catch
            {
                return new DtoGetCategory();
            }
        }

        public async Task<DtoResponse> UpdateAsync(DtoUpdateCategory entity)
        {
            try
            {
                var existingCategory = await category.GetByIDAsync(Guid.Parse(entity.ID));

                if (existingCategory == null)
                    return new DtoResponse(false, "Category not found!");

                mapper.Map(entity, existingCategory);

                int result = await category.UpdateAsync(existingCategory);

                if (result > 0)
                {
                    cache.Remove(CacheKey);
                    return new DtoResponse(true, "Success !");
                }
            }
            catch (Exception ex)
            {
                return new DtoResponse(false, ex.Message);
            }
            return new DtoResponse(false, "Please check your server, Can't update ?!");
        }
    }
}