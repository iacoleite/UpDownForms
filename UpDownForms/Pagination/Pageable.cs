using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

namespace UpDownForms.Pagination
{
    public class Pageable<T> 
    {
        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
        public int TotalItems { get; set; }
        public int TotalPages { get; set; }
        [JsonIgnore]
        public bool HasNext => (CurrentPage < TotalPages);
        [JsonIgnore]
        public bool HasPrevious => (CurrentPage >= 1);
        public string? NextPageUrl = null;
        public string? PreviousPageUrl = null;

        public IQueryable<T> Items;

        public Pageable(IQueryable<T> items, int pageSize, int currentPage, int totalItems)
        {
            this.PageSize = pageSize;
            this.CurrentPage = currentPage;
            this.TotalItems = totalItems;
            this.TotalPages = (int)Math.Ceiling((double)(totalItems / pageSize));
            this.Items = items;
        }

        public static async Task<Pageable<T>> ToPageable(IQueryable<T> items, int pageSize, int currentPage)
        {
            var totalItems = await items.CountAsync();
            var pagedItems = await items.Skip(currentPage * pageSize).Take(pageSize).ToListAsync();
            return new Pageable<T>(pagedItems.AsQueryable(), pageSize, currentPage, totalItems);
        }

    }
}
