namespace ToDo.Contracts.Common
{
    public class PagedList<T>
        where T : class
    {
        public PageMetadata Metadata { get; private set; }

        public IEnumerable<T> Items { get; private set; }

        private PagedList(IEnumerable<T> source, int pageNumber = 1, int pageCount = 20)
        {
            Metadata = new PageMetadata
            {
                CurrentPage = pageNumber,
                TotalCount = source.Count(),
                PageCount = pageCount,
                TotalPages = (int)Math.Ceiling(source.Count() / (double)pageCount)
            };

            Items = source.Skip((Metadata.CurrentPage - 1) * Metadata.PageCount)
                .Take(Metadata.PageCount)
                .ToList();
        }

        public static PagedList<T> CreatePagedList(IEnumerable<T> source, int pageNumber, int pageCount)
        {
            var pagedList = new PagedList<T>(source, pageNumber, pageCount);
            return pagedList;
        }
    }
}
