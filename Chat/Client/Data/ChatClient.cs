using Chat.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chat.Client.Data
{
    public class ChatClient : IAsyncDisposable
    {
        public const string HUBURL = "/ChattingHub";
        private readonly NavigationManager navigationManager;
        private HubConnection hubconnection;
        private readonly string userName;
        private bool started = false;


        public ChatClient(string _userName, NavigationManager _navigationManager)
        {
            navigationManager = _navigationManager;
            userName = _userName;
        }

        public async ValueTask DisposeAsync()
        {
            await StopAsync();
        }

        public async Task StartAsync()
        {
            if (!started)
            {
                hubconnection = new HubConnectionBuilder()
                                    .WithUrl(navigationManager.ToAbsoluteUri(HUBURL))
                                    .Build();

                hubconnection.On<string, string>(Message.RECEIVE, (user, message) => {
                    HandleReceiveMessage(user,message);
                });

                await hubconnection.StartAsync();
                started = true;

                await hubconnection.SendAsync(Message.REGISTER, userName);

            }
        }

        private void HandleReceiveMessage(string user, string message)
        {
            MessageReceived?.Invoke(this, new MessageReceivedEventArgs(user, message));
        }

        public event MessageReceivedEventHandler MessageReceived;
        public delegate void MessageReceivedEventHandler(object sender, MessageReceivedEventArgs e);

        public async Task SendAsync(string message)
        {
            if (!started)
            {
                throw new InvalidOperationException("Client not started");
            }
            await hubconnection.SendAsync(Message.SEND, userName, message);
        }

        public async Task StopAsync()
        {
            if (started)
            {
                await hubconnection.StopAsync();
                await hubconnection.DisposeAsync();
                hubconnection = null;
                started = false;
            }
        }

    }

    public class MessageReceivedEventArgs: EventArgs
    {
        public string  UserName { get; set; }
        public string Message { get; set; }

        public MessageReceivedEventArgs(string _userName, string _message)
        {
            UserName = _userName;
            Message = _message;
        }




    }

}
