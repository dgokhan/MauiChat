using MauiChat.Models;
using Microsoft.AspNetCore.SignalR.Client;
using System.Text.Json;

namespace MauiChat.Services
{
    public interface ISignalRService
    {
        Task ConnectAsync();
        Task DisconnectAsync();
        Task<bool> SendMessageAsync(string userName, string message);
    }

    public class SignalRService : ISignalRService
    {
        public HubConnection hubConnection;
        public SignalRService()
        {
            hubConnection = new HubConnectionBuilder()
                .WithUrl($"http://35.225.238.4:8890/chatHub")
                .Build();
        }

        public async Task ConnectAsync()
            => await hubConnection.StartAsync();

        public async Task DisconnectAsync()
            => await hubConnection.StopAsync();

        public async Task<bool> SendMessageAsync(string userName, string message)
        {
            if (hubConnection.State != HubConnectionState.Connected)
                await ConnectAsync();

            var messageObj = new Message{Text = message, MessageTime = DateTime.Now};
            await hubConnection.InvokeAsync("SendMessage", userName, JsonSerializer.Serialize(messageObj));

            return true;
        }

    }
}
