namespace ChatWebApp.Domain.Entities
{
    public class User
    {
        public User(string username, string email)
        {
            Id = Guid.NewGuid();
            Username = username;
            Email = email;
        }

        public Guid Id { get; private set; }
        public string Username { get; private set; }
        public string Email { get; private set; }
    }
}