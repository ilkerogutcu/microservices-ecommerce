using Olcsan.Boilerplate.Utilities.Results;

namespace Catalog.Application.Wrappers
{
    public class PaginatedResult<T> : DataResult<T> where T : class
    {
        public int PageIndex { get; }
        public int PageSize { get; }
        public long Count { get; }


        public PaginatedResult(T data, string message, int pageIndex, int pageSize, long count) : base(data, true,
            message)
        {
            PageIndex = pageIndex;
            PageSize = pageSize;
            Count = count;
        }

        public PaginatedResult(T data, int pageIndex, int pageSize, long count) : base(data, true)
        {
            PageIndex = pageIndex;
            PageSize = pageSize;
            Count = count;
        }

        public PaginatedResult(string message, int pageIndex, int pageSize, long count) : base(default, true, message)
        {
            PageIndex = pageIndex;
            PageSize = pageSize;
            Count = count;
        }

        public PaginatedResult(int pageIndex, int pageSize, long count) : base(default, true)
        {
            PageIndex = pageIndex;
            PageSize = pageSize;
            Count = count;
        }
    }
}