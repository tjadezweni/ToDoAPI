namespace ToDo.Contracts.Common
{
    public class PageMetadata
    {
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageCount { get; set; }
        public int TotalCount { get; set; }

        public bool HasPreviousPage()
        {
            return CurrentPage > 1;
        }

        public bool HasNextPage()
        {
            return CurrentPage < TotalPages;
        }
    }
}
