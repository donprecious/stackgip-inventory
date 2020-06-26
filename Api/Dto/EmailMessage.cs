namespace StackgipInventory.Dto
{
    public class EmailMessage
    {
        public string RecieverName { get; set; }
        public string To { get; set; }
        public string SenderName { get; set; }
        public string From { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }
}
