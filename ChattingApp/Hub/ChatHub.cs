namespace ChattingApp.Hub;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

[Authorize]
public class ChatHub : Hub
{
    public Task SendMessage1(string user, string message)               // Two parameters accepted
    {
        return Clients.All.SendAsync("ReceiveOne", user, message);    // Note this 'ReceiveOne' 
    }
}