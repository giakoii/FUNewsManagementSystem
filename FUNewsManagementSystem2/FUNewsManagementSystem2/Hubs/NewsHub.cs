using Microsoft.AspNetCore.SignalR;

namespace FUNewsManagementSystem.Hubs;

public class NewsHub : Hub
{
    public async Task SendNewsUpdate(string message)
    {
        await Clients.All.SendAsync("ReceiveNewsUpdate", message);
    }
}