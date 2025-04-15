using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS2PlayersSettings.Data.Repository.Model
{
    public class PagedResult<T>
    {
        public List<T> Items { get; set; } = null!; // 当前页的数据
        public int TotalItems { get; set; } // 总记录数
        public int PageNumber { get; set; } // 当前页码
        public int PageSize { get; set; } // 每页大小
        public int TotalPages => (int)Math.Ceiling((double)TotalItems / PageSize); // 总页数
    }
}
