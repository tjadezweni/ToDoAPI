using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
