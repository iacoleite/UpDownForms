namespace UpDownForms.Pagination
{
    public record PageParameters (int Page = 0, int PageSize = 5, string OrderBy = "Id");
}
