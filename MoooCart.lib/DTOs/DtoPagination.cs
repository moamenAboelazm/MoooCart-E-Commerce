using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoooCart.lib.DTOs
{
    public class DtoProductParams
    {
        private const int MaxPageSize = 50;
        public int Page { get; set; } = 1;

        private int _pageSize = 10;
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
        }

        public string? Search { get; set; }
        public Guid? CategoryId { get; set; }
        public string? Sort { get; set; }
    }
    public class DtoPagedResponse<T>
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
        public IEnumerable<T> Data { get; set; } = new List<T>();
    }
}
