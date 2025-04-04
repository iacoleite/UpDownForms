using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using System.Linq.Dynamic.Core;


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

        //public static async Task<Pageable<T>> ToPageable(IQueryable<T> items, int pageSize, int currentPage, string orderBy)
        public static async Task<Pageable<T>> ToPageable(IQueryable<T> items, PageParameters pageParameters)
        {
            var totalItems = items.Count();
            //var orderedItems = items.OrderBy(orderBy);
            //var pagedItems = orderedItems
            //    .Skip(currentPage * pageSize)
            //    .Take(pageSize)
            //    .ToArray();
            var pagedItems = await items
                //.OrderBy(orderBy)
                .Skip(pageParameters.Page * pageParameters.PageSize)
                .Take(pageParameters.PageSize)
                .ToListAsync();
            return new Pageable<T>(pagedItems.AsQueryable(), pageParameters.PageSize, pageParameters.Page, totalItems);
        }

    }
}
