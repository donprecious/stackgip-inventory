namespace StackgipInventory.ApiDto
{
    public class RefitApiResponse
    {
        public bool Success { get; set; } = false; 
        public string Message { get; set; } 

        public object Data { get; set; }
        
        public object Error { get; set; }
    }
}
