using DCMLockerServidor.Shared;
using Microsoft.AspNetCore.SignalR;
using System.Text.Json;

public partial class ServerHub : Hub
{


    public async Task SendMessage(string user, string message)
    {
        if (Clients != null)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }

    public async Task SendLockerList()
    {
        if (Clients != null)
        {
            await Clients.All.SendAsync("UpdateLockerList");
        }
    }

    
}

