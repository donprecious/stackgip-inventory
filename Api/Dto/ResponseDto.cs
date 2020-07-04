using Newtonsoft.Json;

namespace StackgipInventory.Dto
{
    public class ResponseDto
    {
        [JsonProperty("message")]
        public string Message { get; set; }
        [JsonProperty("status")]
        public string Status { get; set; }
        [JsonProperty("data")]
        public  object Data { get; set; }
        [JsonProperty("code")]
        public string Code { get; set; }
    }
}
