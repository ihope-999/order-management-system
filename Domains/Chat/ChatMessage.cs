namespace project1.Domains.Chat
{
    public class ChatMessage
    {
        public ChatMessage() { }
        public string Room { get; set; }
        public string Message { get; set; }
        public string ConnectionId {  get; set; }

        public string Username { get; set; }
        
    }
}
