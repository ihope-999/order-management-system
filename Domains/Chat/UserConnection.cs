namespace project1.Domains.Chat
{
    public class UserConnection
    {
        public UserConnection() { }
        public string ConnectionId { get; set; }
        public string UserName { get; set; }
        public List<string> RoomsAttended { get; set; }

        public DateTime StartedConnection { get; set; } = DateTime.Now;

        public TimeSpan ConnectionSpan => DateTime.Now - StartedConnection;

    }
}
