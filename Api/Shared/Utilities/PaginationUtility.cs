using StackgipInventory.Dto;
using X.PagedList;

namespace StackgipInventory.Shared.Utilities
{
    public static class  PaginationUtility
    {
        public static PaginationResponse GetPaginationResponse(IPagedList<object> obj)
        {
           
            return new PaginationResponse()
            {
                PageNumber = obj.PageNumber,
                FirstItemOnPage = obj.FirstItemOnPage,
                HasNextPage = obj.HasNextPage,
                HasPreviousPage = obj.HasPreviousPage,
                LastItemOnPage = obj.LastItemOnPage,
                PageCount = obj.PageCount,
                PageSize = obj.PageSize,
                TotalItemCount = obj.TotalItemCount
            };
        }
    }
}
