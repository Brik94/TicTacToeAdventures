using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorSignalRApp.Server.Hubs
{
    //TODO: test...
    //TODO: Eventually keep track of Games, Sessions, Connections, etc.
    //TODO: Look into handling lost connections https://docs.microsoft.com/en-us/aspnet/core/signalr/dotnet-client?view=aspnetcore-3.1&tabs=visual-studio#handle-lost-connection
    public class GameHub : Hub
    {
        public static string _waitingPlayer = string.Empty;

        public override async Task OnConnectedAsync()
        {
            string gameSessionID = null;

            if (String.IsNullOrEmpty(_waitingPlayer))
            {
                _waitingPlayer = Context.ConnectionId;

                await Clients.All.SendAsync("ClientLog", $"Player 1 has joined");
                await Clients.All.SendAsync("ClientLog", $"Waiting for player two...");
            }
            else
            {
                gameSessionID = "game-" + Guid.NewGuid().ToString();

                await Groups.AddToGroupAsync(_waitingPlayer, gameSessionID);
                await Groups.AddToGroupAsync(Context.ConnectionId, gameSessionID);

                await Clients.All.SendAsync("ClientLog", $"Player 2 has joined");
                await Clients.All.SendAsync("ClientLog", $"The game will now begin.");
            }

            if (gameSessionID != null)
            {
                //NOTE: SendAsync can also send back objects.
                await Clients.Client("connection-" + Guid.NewGuid().ToString()).SendAsync("SetPlayer");
                await Clients.Client("connection-" + Guid.NewGuid().ToString()).SendAsync("SetPlayer");
                await Clients.Groups(gameSessionID).SendAsync("RefreshGame");
            }

            await base.OnConnectedAsync();
        }
    }
}
