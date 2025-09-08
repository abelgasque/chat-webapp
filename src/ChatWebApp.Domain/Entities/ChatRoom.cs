namespace ChatWebApp.Domain.Entities
{
    public class ChatRoom
    {
        public ChatRoom()
        {
            Messages = new List<Message>();
            Participants = new List<User>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<User> Participants { get; set; }
        public ICollection<Message> Messages { get; set; }
    }
}