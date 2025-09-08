namespace ChatWebApp.Domain.Entities
{
    public class Message
    {
        public Message() { }

        public Guid Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public DateTime SentAt { get; set; } = DateTime.UtcNow;

        public Guid UserId { get; set; }
        public User User { get; set; }

        public Guid ChatRoomId { get; set; }
        public ChatRoom ChatRoom { get; set; }
    }
}