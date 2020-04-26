using Microsoft.AspNetCore.SignalR;
using System;
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
        public static string _gameSessionID = null;

        public override async Task OnConnectedAsync()
        {
            if (string.IsNullOrEmpty(_playerOne))
            {
                _playerOne = Context.ConnectionId;

                await Clients.All.SendAsync("ClientLog", $"Player 1 has joined");
                await Clients.All.SendAsync("ClientLog", $"Waiting for Player 2...");
            }
            else
            {
                _playerTwo = Context.ConnectionId;
                _gameSessionID = "game-" + Guid.NewGuid().ToString();

                await Groups.AddToGroupAsync(_playerOne, _gameSessionID);
                await Groups.AddToGroupAsync(_playerTwo, _gameSessionID);

                await Clients.All.SendAsync("ClientLog", $"Player 2 has joined. The game will now begin.");
            }

            if (_gameSessionID != null)
            {
                await Clients.Client(_playerOne).SendAsync("SetPlayer", 'X');
                await Clients.Client(_playerTwo).SendAsync("SetPlayer", 'O');

                await Clients.Groups(_gameSessionID).SendAsync("RefreshGame");
            }

            await base.OnConnectedAsync();
        }

        //Make this method smarter.
        public async Task SendMove(int move, char player)
        {
            if (player == 'X') //Sent to Opponent O
            {
                await Clients.Client(_playerTwo).SendAsync("ReceiveOpponentMove", move, player);
            }
            else if (player == 'O') //Send to Opponent X
            {
                await Clients.Client(_playerOne).SendAsync("ReceiveOpponentMove", move, player);
            }
        }

        public async Task SendEndGameUpdate(string endGameUpdate)
        {
            await Clients.Group(_gameSessionID).SendAsync("ReceiveEndGameUpdate", endGameUpdate);
        }


        //Brainstorming Ideas
        public class Player
        {
            string ConnectionID { get; set; }
            char GamePiece { get; set; }
        }

        public class Game
        {
            string GameSessionID { get; set; }
            //GameStatus-> Starting, InProgress, Ending: (Win or Tie)}

            Player One { get; set; }
            Player Two { get; set; }
        }
    }
}
