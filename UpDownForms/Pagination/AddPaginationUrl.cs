using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Text.Json;

namespace UpDownForms.Pagination
{
    public static class AddPaginationUrl
    {
        public static void AddPaginationMetadata<T>(this ControllerBase controller, Pageable<T> pageableItems, PageParameters pageParameters)
        {
            if (pageableItems.HasPrevious)
            {
                pageableItems.PreviousPageUrl = controller.Url.Link(null, new
                {
                    page = pageParameters.Page - 1,
                    pageSize = pageParameters.PageSize
                });
            }

            if (pageableItems.HasNext)
            {
                pageableItems.NextPageUrl = controller.Url.Link(null, new
                {
                    page = pageParameters.Page + 1,
                    pageSize = pageParameters.PageSize
                });
            }

            // Add pagination metadata to header???

            //controller.Response.Headers.Append("X-Pagination", JsonSerializer.Serialize(
            //new
            //{
            //    pageableItems.HasNext,
            //    pageableItems.HasPrevious,
            //    pageableItems.TotalPages,
            //    pageableItems.TotalItems,
            //    pageableItems.CurrentPage,
            //    pageableItems.PageSize,
            //    pageableItems.PreviousPageUrl,
            //    pageableItems.NextPageUrl
            //}));


        }
    }
}
