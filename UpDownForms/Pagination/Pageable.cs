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

        public List<T> Items = new List<T>();

        public Pageable(List<T> items, int pageSize, int currentPage, int totalItems)
        {
            this.PageSize = pageSize;
            this.CurrentPage = currentPage;
            this.TotalItems = totalItems;
            this.TotalPages = (int)Math.Ceiling((double)(totalItems / pageSize));
            this.Items.AddRange(items);
        }

        public static async Task<Pageable<T>> ToPageable(List<T> items, int pageSize, int currentPage)
        {
            var totalItems = items.Count();
            var pagedItems = items.Skip(currentPage * pageSize).Take(pageSize).ToList();
            return await Task.FromResult(new Pageable<T>(pagedItems, pageSize, currentPage, totalItems));
        }

    }
}
