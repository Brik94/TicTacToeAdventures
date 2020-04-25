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
        public static string _playerOne = string.Empty;
        public static string _playerTwo = string.Empty;

        public override async Task OnConnectedAsync()
        {
            string gameSessionID = null;

            if (string.IsNullOrEmpty(_playerOne))
            {
                _playerOne = Context.ConnectionId;

                await Clients.All.SendAsync("ClientLog", $"Player 1 has joined");
                await Clients.All.SendAsync("ClientLog", $"Waiting for player two...");
            }
            else
            {
                gameSessionID = "game-" + Guid.NewGuid().ToString();
                _playerTwo = Context.ConnectionId;

                await Groups.AddToGroupAsync(_playerOne, gameSessionID);
                await Groups.AddToGroupAsync(_playerTwo, gameSessionID);

                await Clients.All.SendAsync("ClientLog", $"Player 2 has joined");
                await Clients.All.SendAsync("ClientLog", $"The game will now begin.");
            }

            if (gameSessionID != null)
            {
                //NOTE: SendAsync can also send back objects.
                await Clients.Client(_playerOne).SendAsync("SetPlayer", 'X');
                await Clients.Client(Context.ConnectionId).SendAsync("SetPlayer", 'O');
                await Clients.Groups(gameSessionID).SendAsync("RefreshGame");
            }

            await base.OnConnectedAsync();
        }

        //TODO: Send move only to Opponent, not ALL clients
        // await Clients.Client(opponentID).SendAsync()
        public async Task SendMove(int move, char player)
        {
            await Clients.All.SendAsync("ClientLog", $"Move:{move} Player:{player}");
            await Clients.All.SendAsync("RecieveMove", move, player);
        }
    }
}
