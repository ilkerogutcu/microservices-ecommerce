using Olcsan.Boilerplate.Utilities.Results;

namespace Catalog.Application.ViewModels
{
    public class PaginatedItemsViewModel<T> : DataResult<T> where T : class
    {
        public int PageIndex { get; private set; }
        public int PageSize { get; private set; }
        public long Count { get; private set; }

        public PaginatedItemsViewModel(T data, string message) : base(data, true, message)
        {
        }

        public PaginatedItemsViewModel(T data) : base(data, true)
        {
        }

        public PaginatedItemsViewModel(string message) : base(default, true, message)
        {
        }

        public PaginatedItemsViewModel() : base(default, true)
        {
        }
    }
}