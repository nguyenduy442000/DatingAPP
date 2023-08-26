using Microsoft.EntityFrameworkCore;

namespace API.Helpers
{
    // Phân trang
    public class PagedList<T> : List<T>
    {
        public PagedList(IEnumerable<T> items, int count , int pageNumber , int pageSize){
            CurrentPage = pageNumber;
            TotalPages = (int) Math.Ceiling(count / (double) pageSize);
            PageSize =pageSize;
            TotalCount = count;
            AddRange(items); // AddRange Thêm nhiều phần tử vào cùng 1 lúc
        }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }

        public static async Task<PagedList<T>> CreateAsync(IQueryable<T> source, int pageNumber , int pageSize)
        {
                var count = await source.CountAsync(); // đếm số lượng phần tử
                var items = await source.Skip((pageNumber -1) * pageSize).Take(pageSize).ToListAsync();
                //bỏ qua các phần tử trước trang hiện tại bằng cách nhảy qua (pageNumber - 1) * pageSize phần tử.
                // Take(pageSize): lấy số lượng phần tử pageSize từ vị trí hiện tại.
                return new PagedList<T>(items, count, pageNumber , pageSize);
        }
    }
}