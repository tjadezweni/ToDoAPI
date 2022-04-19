using System.Collections.Generic;
using System.Threading.Tasks;
using ToDo.Contracts.Common;
using Xunit;

namespace ToDo.Tests.Common
{
    public class PagedList_UnitTests
    {
        [Theory]
        [InlineData(1, 30)]
        [InlineData(-1, -6)]
        public async Task CreatePagedList_ShouldCreatePagedList(int pageNumber, int pageCount)
        {
            // Arrange
            var list = new List<string>()
            {
                "hello",
                "wooo",
                "bye"
            };

            pageNumber = pageNumber < 1 ? 1 : pageNumber;
            pageCount = pageCount < 20 ? 20 : pageCount;

            // Act
            var pagedList = PagedList<string>.CreatePagedList(list, pageNumber, pageCount);

            // Assert
            Assert.NotNull(pagedList);
            Assert.Equal(pageNumber, pagedList.Metadata.CurrentPage);
            Assert.Equal(list.Count, pagedList.Metadata.PageCount);
        }
    }
}
