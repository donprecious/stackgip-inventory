namespace StackgipInventory.Dto
{
    public class ResponseAuth
    {
        public string Status { get; set; }
        public string Message { get; set; }
        public AuthTokenResponse Data { get; set; }
    }
}
