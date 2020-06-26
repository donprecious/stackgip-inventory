namespace StackgipInventory.Dto
{
    public class PaginationResponse
    {
        public int PageNumber{get;set;}
        public int PageCount{get;set;}
        public int PageSize{get;set;}
        public bool HasNextPage{get;set;}
        public bool HasPreviousPage{get;set;}

        public int FirstItemOnPage {get;set;}
        public int LastItemOnPage {get;set;}
        public int TotalItemCount {get;set;}
      
    }

}
