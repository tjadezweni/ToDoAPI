namespace ToDo.Contracts.Interfaces
{
    public interface IPagedList<T> : IEnumerable<T>
        where T : class
    {
        bool HasPreviousPage();

        bool HasNextPage();

        void ConvertToPagedList(IEnumerable<T> source);
    }
}
