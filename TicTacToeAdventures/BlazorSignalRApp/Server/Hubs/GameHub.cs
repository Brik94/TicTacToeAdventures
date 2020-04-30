using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorSignalRApp.Server.Hubs
{
    //TODO: test...
    //TODO: Look into handling lost connections https://docs.microsoft.com/en-us/aspnet/core/signalr/dotnet-client?view=aspnetcore-3.1&tabs=visual-studio#handle-lost-connection
    public class GameHub : Hub
    {
        public static Dictionary<string, Game> _games = new Dictionary<string, Game>();

        public override async Task OnConnectedAsync()
        {
            var game = await JoinGameIfAvailable();

            if(game.P2 != null)
            {
                await Clients.Groups(game.ID).SendAsync("ClientLog", $"Player 2 has joined. The game will now begin.");
                await Clients.Groups(game.ID).SendAsync("SetGameID", game.ID);

                await Clients.Client(game.P1.ConnectionID).SendAsync("SetPlayer", game.P1.GamePiece, false);
                await Clients.Client(game.P2.ConnectionID).SendAsync("SetPlayer", game.P2.GamePiece, true);
            }
            else
            {
                await Clients.Groups(game.ID).SendAsync("ClientLog", $"Player 1 has joined");
                await Clients.Groups(game.ID).SendAsync("ClientLog", $"Waiting for Player 2...");
            }

            await base.OnConnectedAsync();
        }

        public async Task<Game> JoinGameIfAvailable()
        {
            var waitingGame = _games.Where(x => x.Value.P1 != null && x.Value.P2 == null).Select(x => x.Value).FirstOrDefault();
            if(waitingGame != null)
            {
                waitingGame.P2 = new Player()
                {
                    ConnectionID = Context.ConnectionId,
                    GamePiece = 'O'
                };
                await Groups.AddToGroupAsync(waitingGame.P2.ConnectionID, waitingGame.ID);

                return waitingGame;
            }
            else
            {
                var newGame = CreateNewGame();
                await Groups.AddToGroupAsync(newGame.P1.ConnectionID, newGame.ID);

                _games.Add(newGame.ID, newGame);
                return newGame;
            }
        }

        private Game CreateNewGame()
        {
            return new Game()
            {
                ID = "game-" + Guid.NewGuid().ToString(),
                P1 = new Player()
                {
                    ConnectionID = Context.ConnectionId,
                    GamePiece = 'X'
                }
            };
        }

        //TODO: On disconnected send an update to the player still in game
        //TODO: This entire method is a buggy POS.
        public async override Task OnDisconnectedAsync(Exception exception)
        {
            var inSessionGamesExists = _games.Where(x => x.Value.P1 != null && x.Value.P2 != null).Any();
            if (inSessionGamesExists)
            {
                var associatedGame = _games.Where(x => x.Value.P1.ConnectionID == Context.ConnectionId || x.Value.P2.ConnectionID == Context.ConnectionId)
                    .Select(x => x.Value).FirstOrDefault();

                if (associatedGame != null)
                {

                    var opponent = associatedGame.P1.ConnectionID == Context.ConnectionId ? associatedGame.P1 : associatedGame.P2;
                    await Clients.Client(opponent.ConnectionID).SendAsync("ClientLog", $"{opponent.GamePiece} has left");
                    _games.Remove(associatedGame.ID);
                }
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
                await Clients.Group(game.ID).SendAsync("ReceiveEndGameUpdate", endGameUpdate);
            }
        }

        public async Task SendRematchRequest(string gameId, char player, bool accepted)
        {
            var game = _games[gameId];

            if (game != null)
            {
                if (player == game.P1.GamePiece)
                {
                    await Clients.Client(game.P2.ConnectionID).SendAsync("ReceiveRematchRequest", accepted);
                }
                else if (player == game.P2.GamePiece)
                {
                    await Clients.Client(game.P1.ConnectionID).SendAsync("ReceiveRematchRequest", accepted);
                }

            }
        }
    }
}
