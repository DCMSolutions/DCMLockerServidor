using Microsoft.AspNetCore.SignalR;


public partial class ServerHub : Hub
{


    public async Task SendMessage(string user, string message)
    {
        if (Clients != null)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }

   
}

