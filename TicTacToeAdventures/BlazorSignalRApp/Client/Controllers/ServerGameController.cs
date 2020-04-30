using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Threading.Tasks;

namespace BlazorSignalRApp.Client.Controllers
{
    //Responsible for talking to our GameHub. The middle man.
    public class ServerGameController
    {
        private HubConnection _hubConnection;
        private string _gameSessionID;
        public ClientState State { get; set; }

        public event EventHandler StartGameEvent;
        public event EventHandler OpponentUpdateEvent;
        public event EventHandler EndGameEvent;
        public event EventHandler RestartGameEvent;

        public async Task Initialize(Uri uri)
        {
            State = new ClientState();

            _hubConnection = new HubConnectionBuilder()
           .WithUrl(uri)
           .Build();

            _hubConnection.On<string>("ClientLog", ClientLog);
            _hubConnection.On<string>("SetGameID", SetGameID);
            _hubConnection.On<char, bool>("SetPlayer", SetPlayer);
            _hubConnection.On<int, char>("ReceiveOpponentMove", ReceiveOpponentMove);
            _hubConnection.On<string>("ReceiveEndGameUpdate", ReceiveEndGameUpdate);
            _hubConnection.On<bool>("ReceiveRematchRequest", ReceiveRematchRequest);

            await _hubConnection.StartAsync();
        }

        public async Task SendMoveToOpponent(int move, char player) =>
            await _hubConnection.SendAsync("SendMove", _gameSessionID, move, player);

        public async Task SendEndGameUpdate(string update) =>
            await _hubConnection.SendAsync("SendEndGameUpdate", _gameSessionID, update);

        public async Task SendRematchRequest(char player, bool accepted = false) =>
            await _hubConnection.SendAsync("SendRematchRequest", _gameSessionID, player, accepted);

        public bool IsConnected =>
            _hubConnection.State == HubConnectionState.Connected;

        private void ClientLog(string msg) => Console.WriteLine(msg.ToString());

        private void SetGameID(string gameID) => _gameSessionID = gameID;

        private void SetPlayer(char player, bool uiDisabled)
        {
            State.ClientPiece = player;
            State.UIDisabled = uiDisabled;
            State.GameStatus = GameStatus.InProgress;
            Console.WriteLine("Player Game Piece: " + State.ClientPiece);

            StartGameEvent?.Invoke(this, new EventArgs());
        }

        private void ReceiveEndGameUpdate(string update)
        {
            State.StatusMessage = update;
            State.GameStatus = GameStatus.Ending;
            EndGameEvent?.Invoke(this, new EventArgs());
        }

        private void ReceiveOpponentMove(int move, char opponent)
        {
            State.OpponentMove = move;
            State.OpponentPiece = opponent;
            OpponentUpdateEvent?.Invoke(this, new EventArgs());
        }

        public void ReceiveRematchRequest(bool accepted)
        {
            if (accepted)
            {
                State.GameStatus = GameStatus.Starting;
            }
            else
            {
                State.GameStatus = GameStatus.RematchRequest;
            }

            RestartGameEvent?.Invoke(this, new EventArgs());
        }

        public class ClientState
        {
            public bool UIDisabled { get; set; } = true;
            public GameStatus GameStatus { get; set; } = GameStatus.Starting;
            public string StatusMessage { get; set; }
            public char ClientPiece { get; set; }
            public char OpponentPiece { get; set; }
            public int OpponentMove { get; set; }
        }
    }

    public enum GameStatus
    {
        Starting,
        InProgress,
        Ending,
        RematchRequest
    }
}
