namespace ChatWebApp.Domain.Entities
{
    public class User
    {
        public User() { }

        public Guid Id { get; private set; }
        public string Username { get; private set; }
        public string Email { get; private set; }
    }
}