using DCMLockerServidor.Shared;
using DCMLockerServidor.Shared.Models;
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

    public async Task UpdateLockerList()
    {
        if (Clients != null)
        {
            await Clients.All.SendAsync("UpdateLockerList");
        }
    }

    public async Task UpdateTokenList()
    {
        if (Clients != null)
        {
            await Clients.All.SendAsync("UpdateTokenList");
        }
    }


}

