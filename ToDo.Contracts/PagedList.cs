using System.Collections.ObjectModel;
using ToDo.Contracts.Interfaces;

namespace ToDo.Contracts
{
    public class PagedList<T> : List<T>, IPagedList<T>
        where T : class
    {
        public int CurrentPage { get; private set; }
        public int TotalPages { get; private set; }
        public int PageCount { get; private set; }
        public int TotalCount { get; private set; }

        public PagedList(IEnumerable<T> source, int pageNumber, int pageCount)
        {
            CurrentPage = pageNumber;
            TotalCount = source.Count();
            PageCount = pageCount;
            TotalPages = (int)Math.Ceiling(TotalCount / (double)pageCount);

            ConvertToPagedList(source);
        }

        public bool HasPreviousPage()
        {
            return CurrentPage > 1;
        }

        public bool HasNextPage()
        {
            return CurrentPage < TotalPages;
        }

        public void ConvertToPagedList(IEnumerable<T> source)
        {
            var items = source.Skip((CurrentPage - 1) * PageCount)
                .Take(PageCount)
                .ToList();

            AddRange(items);
        }
    }
}
