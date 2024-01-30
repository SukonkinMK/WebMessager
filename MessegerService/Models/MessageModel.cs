namespace MessegerService.Models
{
    public class MessageModel
    {
        public int SenderId { get; set; }
        public int RecipientId { get; set; }
        public bool IsRead { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Text { get; set; }
    }
}
