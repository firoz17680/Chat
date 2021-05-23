using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chat.Server.Hubs
{
    public class ChatHub: Hub
    {

        public async Task SendMessage(string name, string message)
        {
            await Clients.All.SendAsync("SendMessage", name, message);
        }
    }
}
