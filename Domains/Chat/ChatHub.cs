using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.SignalR;

namespace project1.Domains.Chat
{
    public class ChatHub : Hub
    {
        private readonly ChatService _chatService;
        private readonly ILogger<ChatHub> _logger;
        public ChatHub(ChatService chatService, ILogger<ChatHub> logger) 
        {

            _chatService = chatService;
            _logger = logger;

        }

        public async Task ConnectToServerGeneralRoom()
        {
            await base.OnConnectedAsync();

            var connectionId = Context.ConnectionId;
            var currentUsersInTheRoom = _chatService.GetAllUsersInTheRoom("general");
            var userConnection = new UserConnection
            {
                ConnectionId = connectionId,
                UserName = Context.User.Identity.Name
            };
            _chatService.ConnectUser(connectionId);
            _chatService.AttendRoom(connectionId, "general");

            await Groups.AddToGroupAsync(connectionId, "general"); // officially in group
            await Clients.Group("general").SendAsync("UserJoinedMessage",userConnection); // send to others
            await Clients.Caller.SendAsync("YouConnectedMessage", new {DateTime.UtcNow}); // send to me 
            await Clients.Caller.SendAsync("SendUserTheCurrentUsers", currentUsersInTheRoom); // send to me 



        }




    }
}
