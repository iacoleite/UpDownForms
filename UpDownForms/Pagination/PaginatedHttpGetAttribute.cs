using Microsoft.AspNetCore.Mvc;

namespace UpDownForms.Pagination
{
    public class PaginatedHttpGetAttribute : HttpGetAttribute
    {
        public PaginatedHttpGetAttribute(string name)
        {
            Name = name;
        }
    }
}
