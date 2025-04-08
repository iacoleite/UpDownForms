namespace UpDownForms.Pagination
{
    public static class PageParamValidator
    {
        public static string SetSortOrder<T>(PageParameters pageParameters)
        {
            var properties = typeof(T).GetProperties();
            string[] propertiesStrings = properties.Select(p => p.Name.ToLower()).ToArray();

            var orderParam = propertiesStrings.Contains(pageParameters.OrderBy.ToLower()) ? pageParameters.OrderBy : "Id";
            if (pageParameters.Sort.ToLower().Equals("desc"))
            {
                orderParam += " desc";
            }
            return orderParam;
        }
    }
}
