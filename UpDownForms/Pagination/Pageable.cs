using Microsoft.EntityFrameworkCore;

namespace UpDownForms.Pagination
{
    public class Pageable<T> : List<T>
    {
        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
        public int TotalItems { get; set; }
        public int TotalPages { get; set; }
        public bool HasNext => (CurrentPage < TotalPages);
        public bool HasPrevious => (CurrentPage > 1);

        public Pageable(List<T> items, int pageSize, int currentPage, int totalItems)
        {
            this.PageSize = pageSize;
            this.CurrentPage = currentPage;
            this.TotalItems = totalItems;
            this.TotalPages = (int)Math.Ceiling((double)(totalItems / pageSize));
            this.AddRange(items);
        }

        public static async Task<Pageable<T>> ToPageable(List<T> items, int pageSize, int currentPage)
        {
            var totalItems = items.Count();
            var pagedItems = items.Skip(currentPage * pageSize).Take(pageSize).ToList();
            return new Pageable<T>(pagedItems, pageSize, currentPage, totalItems);
        }

    }
}
