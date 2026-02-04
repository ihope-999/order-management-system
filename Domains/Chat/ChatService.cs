using System.Collections.Concurrent;
using System.Security.Cryptography.X509Certificates;

namespace project1.Domains.Chat
{
    public class ChatService
    {
        private readonly ILogger<ChatService> _logger;
        private readonly ConcurrentDictionary<string, List<ChatMessage>> _room = new ConcurrentDictionary<string, List<ChatMessage>>();
        private readonly ConcurrentDictionary<string,UserConnection> _usersConnected = new ConcurrentDictionary<string, UserConnection>();
        private readonly List<string> allUsersInTheRoom = new List<string>();   
        private readonly int MAX_ROOM_PER_MESSAGE = 10;

        public ChatService(ILogger<ChatService> logger) 
        {
            _logger = logger;
            CreateRoom("help");
            CreateRoom("general");
        }

        public void CreateRoom(string room)
        {
            _room.TryAdd(room, new List<ChatMessage>());

        }
        public bool userConnectionExists(string connectionId)
        {
            return _usersConnected.ContainsKey(connectionId);
        }
        public void ConnectUser(string connectionId)
        {
            if(!_usersConnected.TryGetValue(connectionId, out var user))
            {
                _usersConnected[connectionId] = new UserConnection {
                    ConnectionId = connectionId,
                    UserName = connectionId
                };
                _logger.LogCritical("USER CONNECTION CREATED");
                return; 
            }
            _logger.LogInformation("User connection already exists");
        }

        public List<string> GetAllUsersInTheRoom(string room)
            
        {
            
            foreach(var user in _usersConnected.Values)
            {
                if (user.RoomsAttended.Contains(room))
                {
                    allUsersInTheRoom.Add(user.ConnectionId);
                }


            }
            return allUsersInTheRoom;
        }
        public void AttendRoom(string connectionId, string room)
        {
      
            try
            {
                if (!userConnectionExists(connectionId))
                {
                    ConnectUser(connectionId);
                }
                _usersConnected[connectionId].RoomsAttended.Add(room);
                if (_usersConnected[connectionId].RoomsAttended.Contains(room))
                {
                    _logger.LogInformation($"you attended to the room {room} successfully");
                }

            }
            catch (Exception ex) 
            {
                _logger.LogCritical($"An unexpected error happened while ATTENDING ROOM {room}: {ex.Message}");
            }
        }
        public void LeaveRoom(string connectionId, string room)
        {
            try
            {
                _usersConnected[room].RoomsAttended.Remove(room);
                if (!_usersConnected[room].RoomsAttended.Contains(room))
                {
                    _logger.LogInformation($"You leaved, Room:{room}");
                }
                else
                {
                    _logger.LogCritical($"Room : {room} could not be deleted from your connection for some reason..LeaveRoom Function malfunction");
                }
            }
            catch(Exception ex)
            {
                _logger.LogCritical($"An unexpected error happened while LEAVING ROOM {room}: {ex.Message}");

            }
        }
        public void SendMessage(ChatMessage message)

        {
            if (_room.Keys.Contains(message.Room))
            {
                if (_room[message.Room].Count > MAX_ROOM_PER_MESSAGE) _room[message.Room].RemoveAt(0);
                _room[message.Room].Add(message);
                return;

            }
            _logger.LogCritical($"Could not found the rooom to send the message: {message.Room}, the error is cs.sendmessage");


            //cache should be done later dont forget

        }

        public List<string> GetAllRooms()
        {
            return _room.Keys.ToList();
        }
        public List<string> GetAllUsers()
        {
            return _usersConnected.Keys.ToList();
        }
        

    }
}