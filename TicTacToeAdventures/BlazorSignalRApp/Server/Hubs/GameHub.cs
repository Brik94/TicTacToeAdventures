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
        public static Dictionary<string, Game> _games = new Dictionary<string, Game>();
        public static string _playerOne = string.Empty;

        public override async Task OnConnectedAsync()
        {
            Game game = null;

            if (string.IsNullOrEmpty(_playerOne))
            {
                _playerOne = Context.ConnectionId;

                await Clients.All.SendAsync("ClientLog", $"Player 1 has joined");
                await Clients.All.SendAsync("ClientLog", $"Waiting for Player 2...");
            }
            else
            {
                game = CreateNewGame();
                _games.Add(game.SessionID, game);


                await Groups.AddToGroupAsync(game.P1.ConnectionID, game.SessionID);
                await Groups.AddToGroupAsync(game.P2.ConnectionID, game.SessionID);

                await Clients.All.SendAsync("ClientLog", $"Player 2 has joined. The game will now begin.");
            }

            if (game != null)
            {
                await Clients.Client(game.P1.ConnectionID).SendAsync("SetPlayer", game.P1.GamePiece, false);
                await Clients.Client(game.P2.ConnectionID).SendAsync("SetPlayer", game.P2.GamePiece, true);

                await Clients.Groups(game.SessionID).SendAsync("SetGameID", game.SessionID); //Does sending this ID create a security risk?
            }

            await base.OnConnectedAsync();
        }

        private Game CreateNewGame()
        {
            return new Game()
            {
                SessionID = "game-" + Guid.NewGuid().ToString(),
                P1 = new Player()
                {
                    ConnectionID = _playerOne,
                    GamePiece = 'X'
                },
                P2 = new Player()
                {
                    ConnectionID = Context.ConnectionId,
                    GamePiece = 'O'
                }
            };
        }

        public async override Task OnDisconnectedAsync(Exception exception)
        {
            if(_playerOne == Context.ConnectionId) _playerOne = string.Empty;

            var game = _games.Where(x => x.Value.P1.ConnectionID == Context.ConnectionId || x.Value.P2.ConnectionID == Context.ConnectionId)
                .Select(x => x.Value).FirstOrDefault();

            if(game != null)
            {
                var opponent = game.P1.ConnectionID == Context.ConnectionId ? game.P1 : game.P2;
                await Clients.Client(opponent.ConnectionID).SendAsync("ClientLog", $"{opponent.GamePiece} has left");
                _games.Remove(game.SessionID);
            }
        }

        public async Task SendMove(string gameId, int move, char player)
        {
            var game = _games[gameId];

            if(game != null)
            {
                if (player == game.P1.GamePiece)
                {
                    await Clients.Client(game.P2.ConnectionID).SendAsync("ReceiveOpponentMove", move, player);
                }
                else if (player == game.P2.GamePiece)
                {
                    await Clients.Client(game.P1.ConnectionID).SendAsync("ReceiveOpponentMove", move, player);
                }
            }
        }

        public async Task SendEndGameUpdate(string gameId, string endGameUpdate)
        {
            var game = _games[gameId];

            if(game != null)
            {
                await Clients.Group(game.SessionID).SendAsync("ReceiveEndGameUpdate", endGameUpdate);
            }
        }

        public class Player
        {
            public string ConnectionID { get; set; }
            public char GamePiece { get; set; }
        }

        public class Game
        {
            public string SessionID { get; set; }
            //GameStatus-> Starting, InProgress, Ending: (Win or Tie)}

            public Player P1 { get; set; }
            public Player P2 { get; set; }
        }
    }
}
