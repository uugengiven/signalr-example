using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using signalr_server.Data;

namespace signalr_server
{
    public class ChatHub : Hub
    {
        private readonly LFGDataContext _context;
        private readonly IDictionary<int, string> _connections;

        // This is all of the commands a client can do that the server has to respond to

        public ChatHub(LFGDataContext context, IDictionary<int, string> connections)
        {
            _context = context;
            _connections = connections;
        }

        // This is the method that the client will call to send a message to all other clients
        public async Task SendMessage(string user, string message)
        {
            // This is the method that the client will call to send a message to all other clients
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        // This is the method that the client will call to identify itself to the server
        public async Task Login(string user)
        {
            // This is the method that the client will call to identify itself to the server
            // look up the computer id in the database using user
            var computer = await _context.Computer.Where(c => c.Name == user).FirstOrDefaultAsync();

            if (computer == null)
            {
                // send a message back to the client that the user is not found
                await Clients.Caller.SendAsync("LoginFail", "System", "User not found");
                return;
            }
            // Save connectionid into the _connections table
            _connections[computer.Id] = Context.ConnectionId;
            // send a message back to the client that the user is found
            await Clients.Caller.SendAsync("LoginSuccess", "System", computer);
        }

        // When clients connect send a request login message
        public override async Task OnConnectedAsync()
        {
            // When clients connect send a request login message
            await Clients.Caller.SendAsync("RequestLogin", "System", "Please login");
        }

        // When a client disconnects remove the connectionid from the _connections table
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            // When a client disconnects remove the connectionid from the _connections table
            var id = _connections.FirstOrDefault(c => c.Value == Context.ConnectionId).Key;
            if (id != 0)
            {
                _connections.Remove(id);
            }
            await base.OnDisconnectedAsync(exception);
        }

    }
}
