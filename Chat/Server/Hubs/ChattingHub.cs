using Chat.Shared;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chat.Server.Hubs
{
    public class ChattingHub: Hub
    {
        public static readonly Dictionary<string, string> userLookup = new Dictionary<string, string>();
        public async Task SendMessage(string name, string message)
        {
            await Clients.All.SendAsync(Message.RECEIVE, name, message);
        }

        public async Task Register(string UserName)
        {
            var currentId = Context.ConnectionId;
            if (!userLookup.ContainsKey(currentId))
            {
                userLookup.Add(currentId, UserName);
                await Clients.AllExcept(currentId).SendAsync(
                            Message.RECEIVE,
                            UserName,
                            $"{UserName} joined the chat");



            }
        }

        public override Task OnConnectedAsync()
        {
            return base.OnConnectedAsync();
        }
     
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            string id = Context.ConnectionId;
            if(!userLookup.TryGetValue(id, out string username))
            {
                username = "Unknown";
            }
            userLookup.Remove(id);
            await Clients.AllExcept(Context.ConnectionId).SendAsync(
                        Message.RECEIVE,
                        username,
                        $"{username} has left the chat");
            await base.OnDisconnectedAsync(exception);
        }

    }
}
